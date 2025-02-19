using DanaTweaks.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.GroundStorageImmersiveCrafting))]
public static class GroundStorage_ImmersiveCrafting
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockGroundStorage), nameof(BlockGroundStorage.OnBlockInteractStart))]
    public static bool Prefix(ref bool __result, IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel)
    {
        if (blockSel == null || world.BlockAccessor.GetBlockEntity(blockSel.Position) is not BlockEntityGroundStorage begs)
        {
            return true;
        }

        if (!byPlayer.Entity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
        {
            world.BlockAccessor.MarkBlockDirty(blockSel.Position.AddCopy(blockSel.Face));
            byPlayer.InventoryManager.ActiveHotbarSlot.MarkDirty();
            return true;
        }

        ItemSlot firstSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
        ItemSlot secondSlot = begs.GetSlotAt(blockSel);

        if (firstSlot.Empty || secondSlot == null || secondSlot.Empty)
        {
            return true;
        }

        if (!GetMatchingRecipe(firstSlot, secondSlot, out GridRecipe matchingRecipe)
            || !AnySatisfies(firstSlot, secondSlot, matchingRecipe)
            || HasSameIngredients(firstSlot, secondSlot, matchingRecipe)
            || HasSealedOrEmptyCrock(firstSlot, secondSlot)
            || HasFullBackpack(firstSlot, secondSlot)
            || Extensions.AnyCrate(firstSlot, secondSlot) )
        {
            return true;
        }

        try
        {
            DummySlot dummySlot = new DummySlot();
            matchingRecipe.GenerateOutputStack(new ItemSlot[] { firstSlot, secondSlot }, dummySlot);
            if (!TryConsumeInput(byPlayer, firstSlot, secondSlot, matchingRecipe))
            {
                return true;
            }

            switch (dummySlot.Itemstack.StackSize)
            {
                case 1 when secondSlot.Empty && dummySlot.Itemstack.Collectible.HasBehavior<CollectibleBehaviorGroundStorable>():
                    dummySlot.TryPutInto(world, secondSlot);
                    break;
                default:
                    if (!byPlayer.InventoryManager.TryGiveItemstack(dummySlot.Itemstack))
                    {
                        world.SpawnItemEntity(dummySlot.Itemstack, begs.Pos.ToVec3d().AddCopy(0.5f, 0.5f, 0.5f));
                    }
                    break;
            }

            firstSlot.MarkDirty();
            secondSlot.MarkDirty();
            begs.MarkDirty(true);

            if (begs.Inventory.Empty)
            {
                BlockPos pos = begs.Pos.Copy();
                world.BlockAccessor.SetBlock(0, pos);
                world.BlockAccessor.TriggerNeighbourBlockUpdate(pos);
            }
        }
        catch (System.ObjectDisposedException)
        {
            return true;
        }

        __result = true;
        return false;
    }

    private static bool AnySatisfies(ItemSlot firstSlot, ItemSlot secondSlot, GridRecipe recipe)
    {
        GridRecipeIngredient firstIngredient = recipe.resolvedIngredients[0];
        GridRecipeIngredient secondIngredient = recipe.resolvedIngredients[1];
        return firstIngredient.SatisfiesAsIngredient(firstSlot.Itemstack) && secondIngredient.SatisfiesAsIngredient(secondSlot.Itemstack)
                    || secondIngredient.SatisfiesAsIngredient(firstSlot.Itemstack) && firstIngredient.SatisfiesAsIngredient(secondSlot.Itemstack);
    }

    /// <summary>
    /// Temporary solution until "ignoreImmersiveCrafting" recipe attribute is added
    /// </summary>
    private static bool HasSameIngredients(ItemSlot firstSlot, ItemSlot secondSlot, GridRecipe recipe)
    {
        return WildcardUtil.Match(recipe.resolvedIngredients[0].Code, recipe.resolvedIngredients[1].Code);
    }

    private static bool TryConsumeInput(IPlayer byPlayer, ItemSlot firstSlot, ItemSlot secondSlot, GridRecipe recipe)
    {
        ItemSlot[] slots = { firstSlot, secondSlot };
        ItemSlot[] slotsReversed = { secondSlot, firstSlot };

        return recipe.ConsumeInput(byPlayer, slots, 1)
            || recipe.ConsumeInput(byPlayer, slots, 2)
            || recipe.ConsumeInput(byPlayer, slotsReversed, 1)
            || recipe.ConsumeInput(byPlayer, slotsReversed, 2);
    }

    private static bool HasSealedOrEmptyCrock(ItemSlot firstSlot, ItemSlot secondSlot) => firstSlot.Itemstack.Collectible switch
    {
        BlockCrock crock when secondSlot.Itemstack.Collectible is not BlockCrock => crock.IsEmpty(firstSlot.Itemstack) || firstSlot.Itemstack.Attributes.TryGetBool("sealed") == true,
        not BlockCrock when secondSlot.Itemstack.Collectible is BlockCrock crock => crock.IsEmpty(secondSlot.Itemstack) || secondSlot.Itemstack.Attributes.TryGetBool("sealed") == true,
        _ => false,
    };

    private static bool HasFullBackpack(ItemSlot firstSlot, ItemSlot secondSlot)
    {
        bool isBackpackAndFull1 = firstSlot?.Itemstack?.Collectible?.GetBehavior<CollectibleBehaviorHeldBag>()?.IsEmpty(firstSlot.Itemstack) == false;
        bool isBackpackAndFull2 = secondSlot?.Itemstack?.Collectible?.GetBehavior<CollectibleBehaviorHeldBag>()?.IsEmpty(secondSlot.Itemstack) == false;
        return isBackpackAndFull1 || isBackpackAndFull2;
    }

    private static bool GetMatchingRecipe(ItemSlot firstSlot, ItemSlot secondSlot, out GridRecipe matchingRecipe)
    {
        List<GridRecipe> recipes = Recipes.GroundStorableRecipes;
        for (int i = 0; i < recipes.Count; i++)
        {
            GridRecipe _recipe = recipes[i];

            CraftingRecipeIngredient ingredient1 = _recipe.resolvedIngredients[0];
            CraftingRecipeIngredient ingredient2 = _recipe.resolvedIngredients[1];

            bool firstMatchingFirst = firstSlot.Itemstack.Collectible.WildCardMatch(ingredient1.Code) && firstSlot.Itemstack.Collectible.MatchesForCrafting(firstSlot.Itemstack, _recipe, ingredient1);
            bool secondMatchingSecond = secondSlot.Itemstack.Collectible.WildCardMatch(ingredient2.Code) && secondSlot.Itemstack.Collectible.MatchesForCrafting(secondSlot.Itemstack, _recipe, ingredient2);

            bool firstMatchingSecond = firstSlot.Itemstack.Collectible.WildCardMatch(ingredient2.Code) && firstSlot.Itemstack.Collectible.MatchesForCrafting(firstSlot.Itemstack, _recipe, ingredient2);
            bool secondMatchingFirst = secondSlot.Itemstack.Collectible.WildCardMatch(ingredient1.Code) && secondSlot.Itemstack.Collectible.MatchesForCrafting(secondSlot.Itemstack, _recipe, ingredient1);

            if (firstMatchingFirst && secondMatchingSecond || firstMatchingSecond && secondMatchingFirst)
            {
                matchingRecipe = _recipe.Clone();
                return true;
            }
        }

        matchingRecipe = null;
        return false;
    }
}
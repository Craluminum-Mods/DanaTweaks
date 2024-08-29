using System.Collections.Generic;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockGroundStorage_OnBlockInteractStart_Patch2
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockGroundStorage).GetMethod(nameof(BlockGroundStorage.OnBlockInteractStart));
    }

    public static MethodInfo GetPrefix() => typeof(BlockGroundStorage_OnBlockInteractStart_Patch2).GetMethod(nameof(Prefix));

    /// <summary>
    /// Handle immersive crafting using ground storage
    /// </summary>
    /// <returns>Return false to skip original method</returns>
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

        if (firstSlot.Empty || secondSlot.Empty)
        {
            return true;
        }

        if (world.Side.IsClient())
        {
            __result = true;
            return false;
        }

        if (!GetMatchingRecipe(firstSlot, secondSlot, out var matchingRecipe) || !AnySatisfies(firstSlot, secondSlot, matchingRecipe) || IsSealedOrEmptyCrock(firstSlot, secondSlot))
        {
            return true;
        }

        DummySlot dummySlot = new DummySlot();
        matchingRecipe.GenerateOutputStack(new ItemSlot[] { firstSlot, secondSlot }, dummySlot);
        if (!TryConsumeInput(byPlayer, firstSlot, secondSlot, matchingRecipe))
        {
            return true;
        }

        if (!byPlayer.InventoryManager.TryGiveItemstack(dummySlot.Itemstack))
        {
            world.SpawnItemEntity(dummySlot.Itemstack, begs.Pos.ToVec3d().AddCopy(0.5f, 0.5f, 0.5f));
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
        __result = true;
        return false;
    }

    private static bool AnySatisfies(ItemSlot firstSlot, ItemSlot secondSlot, GridRecipe recipe)
    {
        GridRecipeIngredient firstIngredient = recipe.resolvedIngredients[0];
        GridRecipeIngredient secondIngredient = recipe.resolvedIngredients[1];
        return (firstIngredient.SatisfiesAsIngredient(firstSlot.Itemstack) && secondIngredient.SatisfiesAsIngredient(secondSlot.Itemstack))
                    || (secondIngredient.SatisfiesAsIngredient(firstSlot.Itemstack) && firstIngredient.SatisfiesAsIngredient(secondSlot.Itemstack));
    }

    private static bool TryConsumeInput(IPlayer byPlayer, ItemSlot firstSlot, ItemSlot secondSlot, GridRecipe recipe) =>
            recipe.ConsumeInput(byPlayer, new ItemSlot[] { firstSlot, secondSlot }, 1)
            || recipe.ConsumeInput(byPlayer, new ItemSlot[] { firstSlot, secondSlot }, 2)
            || recipe.ConsumeInput(byPlayer, new ItemSlot[] { secondSlot, firstSlot }, 1)
            || recipe.ConsumeInput(byPlayer, new ItemSlot[] { secondSlot, firstSlot }, 2)
        ;

    private static bool IsSealedOrEmptyCrock(ItemSlot firstSlot, ItemSlot secondSlot) => (firstSlot.Itemstack.Collectible) switch
    {
        BlockCrock crock when secondSlot.Itemstack.Collectible is not BlockCrock => crock.IsEmpty(firstSlot.Itemstack) || firstSlot.Itemstack.Attributes.TryGetBool("sealed") == true,
        not BlockCrock when secondSlot.Itemstack.Collectible is BlockCrock crock => crock.IsEmpty(secondSlot.Itemstack) || secondSlot.Itemstack.Attributes.TryGetBool("sealed") == true,
        _ => false,
    };

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

            if ((firstMatchingFirst && secondMatchingSecond) || (firstMatchingSecond && secondMatchingFirst))
            {
                matchingRecipe = _recipe.Clone();
                return true;
            }
        }

        matchingRecipe = null;
        return false;
    }
}
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class Extensions
{
    public static void EnsureAttributesNotNull(this CollectibleObject obj) => obj.Attributes ??= new JsonObject(new JObject());
    public static void EnsureAttributesNotNull(this EntityProperties obj) => obj.Attributes ??= new JsonObject(new JObject());

    public static T GetBlockEntityExt<T>(this IBlockAccessor blockAccessor, BlockPos pos) where T : BlockEntity
    {
        if (blockAccessor.GetBlockEntity<T>(pos) is T blockEntity)
        {
            return blockEntity;
        }

        if (blockAccessor.GetBlock(pos) is BlockMultiblock multiblock)
        {
            BlockPos multiblockPos = new BlockPos(pos.X + multiblock.OffsetInv.X, pos.Y + multiblock.OffsetInv.Y, pos.Z + multiblock.OffsetInv.Z, pos.dimension);

            return blockAccessor.GetBlockEntity<T>(multiblockPos);
        }

        return null;
    }

    public static bool IsCrate(this ICoreClientAPI api)
    {
        BlockPos pos = api.World.Player?.CurrentBlockSelection?.Position;
        return pos != null && api.World.BlockAccessor.GetBlockEntityExt<BlockEntityCrate>(pos) != null;
    }

    public static bool IsCorrectLabel(this ItemSlot activeSlot, ItemStack DefaultLabelStack)
    {
        return activeSlot?.Itemstack?.Collectible?.Code == DefaultLabelStack.Collectible.Code;
    }

    public static string GetHotkeyCodes(this ICoreClientAPI capi, string hotkeyCode)
    {
        return "(" + capi.Input.HotKeys.Get(hotkeyCode).CurrentMapping + ")";
    }

    public static bool IsPlank(this CraftingRecipeIngredient ingredient)
    {
        return ingredient.Code.ToString().StartsWith("game:plank-");
    }

    public static bool HasLogAsIngredient(this GridRecipe recipe)
    {
        return recipe.Ingredients.Values.Any(ingredient => ingredient.Code.ToString().StartsWith("game:log"));
    }

    public static bool IsShears(this ItemSlot slot)
    {
        return slot?.Itemstack?.Collectible is ItemShears;
    }

    public static bool IsCrockEmpty(this ItemStack stack)
    {
        ICoreAPI api = stack.Collectible.GetField<ICoreAPI>("api");
        ItemStack[] nonEmptyContents = (stack.Collectible as BlockCrock)?.GetNonEmptyContents(api.World, stack);
        return nonEmptyContents == null || nonEmptyContents.Length == 0;
    }

    public static bool IsPlayerInventory(this IInventory inventory)
    {
        return inventory.ClassName is GlobalConstants.hotBarInvClassName or GlobalConstants.backpackInvClassName;
    }

    public static void CoolWithWater(this BlockEntityToolMold mold)
    {
        ItemStack stack = mold?.metalContent;
        if (stack != null)
        {
            float temperature = stack.Collectible.GetTemperature(mold.Api.World, stack);
            stack.Collectible.SetTemperature(mold.Api.World, stack, Math.Max(20f, temperature - Core.Config.CoolMoldsWithWateringCanSpeed));
        }
    }

    public static void CoolWithWater(this BlockEntityIngotMold mold)
    {
        ItemStack rightStack = mold.contentsRight;
        ItemStack leftStack = mold.contentsLeft;
        if (rightStack != null)
        {
            float temperature = rightStack.Collectible.GetTemperature(mold.Api.World, rightStack);
            rightStack.Collectible.SetTemperature(mold.Api.World, rightStack, Math.Max(20f, temperature - Core.Config.CoolMoldsWithWateringCanSpeed));
        }
        if (leftStack != null)
        {
            float temperature = leftStack.Collectible.GetTemperature(mold.Api.World, leftStack);
            leftStack.Collectible.SetTemperature(mold.Api.World, leftStack, Math.Max(20f, temperature - Core.Config.CoolMoldsWithWateringCanSpeed));
        }
    }
}
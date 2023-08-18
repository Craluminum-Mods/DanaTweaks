using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class Extensions
{
    public static T GetBlockEntityExt<T>(this IBlockAccessor blockAccessor, BlockPos pos) where T : BlockEntity
    {
        if (blockAccessor.GetBlockEntity<T>(pos) is T blockEntity)
        {
            return blockEntity;
        }

        if (blockAccessor.GetBlock(pos) is BlockMultiblock multiblock)
        {
            BlockPos multiblockPos = new(pos.X + multiblock.OffsetInv.X, pos.Y + multiblock.OffsetInv.Y, pos.Z + multiblock.OffsetInv.Z);

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
}
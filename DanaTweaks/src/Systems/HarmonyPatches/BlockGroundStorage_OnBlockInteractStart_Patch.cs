using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockGroundStorage_OnBlockInteractStart_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockGroundStorage).GetMethod(nameof(BlockGroundStorage.OnBlockInteractStart));
    }

    public static MethodInfo GetPrefix() => typeof(BlockGroundStorage_OnBlockInteractStart_Patch).GetMethod(nameof(Prefix));

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

        ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
        ItemSlot targetSlot = begs.GetSlotAt(blockSel);

        if (targetSlot.Empty
            || targetSlot.Itemstack.Collectible is not ILiquidInterface
            || hotbarSlot.Empty
            || hotbarSlot.Itemstack.Collectible is not ILiquidInterface)
        {
            return true;
        }

        BlockLiquidContainerBase _toLiquidContainer = hotbarSlot.Itemstack.Collectible as BlockLiquidContainerBase;

        CollectibleObject obj = hotbarSlot.Itemstack.Collectible;
        bool singleTake = byPlayer.WorldData.EntityControls.ShiftKey;
        bool singlePut = byPlayer.WorldData.EntityControls.CtrlKey;
        if (obj is ILiquidSource liquidSource && !singleTake)
        {
            if (!liquidSource.AllowHeldLiquidTransfer)
            {
                __result = false;
                return false;
            }
            ItemStack contentStackToMove = liquidSource.GetContent(hotbarSlot.Itemstack);

            int moved = _toLiquidContainer.TryPutLiquid(
                containerStack: targetSlot.Itemstack,
                liquidStack: contentStackToMove,
                desiredLitres: singlePut ? liquidSource.TransferSizeLitres : liquidSource.CapacityLitres);

            if (moved > 0)
            {
                _toLiquidContainer.CallMethod<int>("splitStackAndPerformAction", byPlayer.Entity, hotbarSlot, delegate (ItemStack stack)
                {
                    liquidSource.TryTakeContent(stack, moved);
                    return moved;
                });
                _toLiquidContainer.DoLiquidMovedEffects(byPlayer, contentStackToMove, moved, BlockLiquidContainerBase.EnumLiquidDirection.Pour);

                hotbarSlot.MarkDirty();
                targetSlot.MarkDirty();
                begs.MarkDirty(true);
                __result = true;
                return false;
            }
        }

        if (obj is ILiquidSink liquidSink && !singlePut)
        {
            if (!liquidSink.AllowHeldLiquidTransfer)
            {
                __result = false;
                return false;
            }
            ItemStack owncontentStack = _toLiquidContainer.GetContent(targetSlot.Itemstack);
            if (owncontentStack == null)
            {
                __result = false;
                return false;
            }
            ItemStack liquidStackForParticles = owncontentStack.Clone();
            float litres = (singleTake ? liquidSink.TransferSizeLitres : liquidSink.CapacityLitres);
            int moved = _toLiquidContainer.CallMethod<int>("splitStackAndPerformAction", byPlayer.Entity, hotbarSlot, (ItemStack stack) => liquidSink.TryPutLiquid(stack, owncontentStack, litres));
            if (moved > 0)
            {
                _toLiquidContainer.TryTakeContent(targetSlot.Itemstack, moved);
                _toLiquidContainer.DoLiquidMovedEffects(byPlayer, liquidStackForParticles, moved, BlockLiquidContainerBase.EnumLiquidDirection.Fill);

                hotbarSlot.MarkDirty();
                targetSlot.MarkDirty();
                begs.MarkDirty(true);
                __result = true;
                return false;
            }
        }
        __result = false;
        return false;
    }
}
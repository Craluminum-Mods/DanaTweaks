using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

/// <summary>
/// Handle ground storage liquid interactions
/// </summary>
[HarmonyPatchCategory(nameof(ConfigServer.GroundStorageLiquidInteraction))]
public static class BlockGroundStorage_OnBlockInteractStart_Patch
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

        ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
        ItemSlot targetSlot = begs.GetSlotAt(blockSel);

        if (targetSlot?.Itemstack?.Collectible?.GetCollectibleInterface<ILiquidInterface>() is ILiquidInterface cnt1
            && hotbarSlot?.Itemstack?.Collectible?.GetCollectibleInterface<ILiquidInterface>() is ILiquidInterface cnt2
            && !BothContainersFullOrEmpty(cnt1, targetSlot.Itemstack, cnt2, hotbarSlot.Itemstack))
        {
            return HandleGroundStorageLiquidInteraction(out __result, byPlayer, begs, hotbarSlot, targetSlot);
        }

        return true;
    }

    private static bool BothContainersFullOrEmpty(ILiquidInterface cnt1, ItemStack cnt1stack, ILiquidInterface cnt2, ItemStack cnt2stack)
    {
        return cnt1.IsFull(cnt1stack) && cnt2.IsFull(cnt2stack) || cnt1.GetCurrentLitres(cnt1stack) == 0 && cnt2.GetCurrentLitres(cnt2stack) == 0;
    }

    private static bool HandleGroundStorageLiquidInteraction(out bool __result, IPlayer byPlayer, BlockEntityGroundStorage begs, ItemSlot hotbarSlot, ItemSlot targetSlot)
    {
        BlockLiquidContainerBase _toLiquidContainer = hotbarSlot.Itemstack.Collectible as BlockLiquidContainerBase;

        CollectibleObject obj = hotbarSlot.Itemstack.Collectible;
        bool singleTake = byPlayer.WorldData.EntityControls.ShiftKey;
        bool singlePut = byPlayer.WorldData.EntityControls.CtrlKey;
        if (obj.GetCollectibleInterface<ILiquidSource>() is ILiquidSource liquidSource && !singleTake)
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
                _toLiquidContainer.SplitStackAndPerformAction(byPlayer.Entity, hotbarSlot, delegate (ItemStack stack)
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

        if (obj.GetCollectibleInterface<ILiquidSink>() is ILiquidSink liquidSink && !singlePut)
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
            float litres = singleTake ? liquidSink.TransferSizeLitres : liquidSink.CapacityLitres;
            int moved = _toLiquidContainer.SplitStackAndPerformAction(byPlayer.Entity, hotbarSlot, (ItemStack stack) => liquidSink.TryPutLiquid(stack, owncontentStack, litres));
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
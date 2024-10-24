using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

/// <summary>
/// Skip default "Shift" behavior to be able to move portions between liquid containers
/// </summary>
public static class BlockLiquidContainerBase_OnHeldInteractStart_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockLiquidContainerBase).GetMethod(nameof(BlockLiquidContainerBase.OnHeldInteractStart));
    }

    public static MethodInfo GetPrefix() => typeof(BlockLiquidContainerBase_OnHeldInteractStart_Patch).GetMethod(nameof(Prefix));

    public static bool Prefix(ItemSlot itemslot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling)
    {
        if (blockSel == null || byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is not BlockEntityGroundStorage begs)
        {
            return true;
        }

        IPlayer byPlayer = (byEntity as EntityPlayer)?.Player;
        if (!byEntity.World.Claims.TryAccess(byPlayer, blockSel.Position, EnumBlockAccessFlags.Use))
        {
            byEntity.World.BlockAccessor.MarkBlockDirty(blockSel.Position.AddCopy(blockSel.Face));
            byPlayer?.InventoryManager.ActiveHotbarSlot.MarkDirty();
            return true;
        }

        ItemSlot hotbarSlot = byPlayer.InventoryManager.ActiveHotbarSlot;
        ItemSlot targetSlot = begs.GetSlotAt(blockSel);

        if (targetSlot.Empty
            || targetSlot.Itemstack.Collectible is not ILiquidInterface
            || hotbarSlot.Empty
            || hotbarSlot.Itemstack.Collectible is not ILiquidInterface fromContainer)
        {
            return true;
        }

        if (fromContainer.AllowHeldLiquidTransfer)
        {
            return false;
        }
        return true;
    }
}
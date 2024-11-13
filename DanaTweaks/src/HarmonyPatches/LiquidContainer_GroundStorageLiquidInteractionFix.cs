using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

/// <summary>
/// Skip default "Shift" behavior to be able to move portions between liquid containers
/// </summary>
[HarmonyPatchCategory(nameof(ConfigServer.GroundStorageLiquidInteraction))]
public static class LiquidContainer_GroundStorageLiquidInteractionFix
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockLiquidContainerBase), nameof(BlockLiquidContainerBase.OnHeldInteractStart))]
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
            || targetSlot.Itemstack.Collectible.GetCollectibleInterface<ILiquidInterface>() is null
            || hotbarSlot.Empty
            || hotbarSlot.Itemstack.Collectible.GetCollectibleInterface<ILiquidInterface>() is not ILiquidInterface fromContainer)
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
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockCookedContainerBase_OnContainedInteractStart_Patch
{
    public static void Postfix(ref bool __result, ItemSlot slot, IPlayer byPlayer)
    {
        // if (__result == true)
        // {
        //     return;
        // }

        ItemSlot targetSlot = byPlayer.InventoryManager.ActiveHotbarSlot;

        if (slot.Itemstack.Collectible is BlockCrock
            && !slot.Itemstack.IsCrockEmpty()
            && !targetSlot.Empty
            && targetSlot.Itemstack != null
            && targetSlot.StackSize > 0
            && !slot.Itemstack.Attributes.HasAttribute("sealed")
            && targetSlot.Itemstack.Collectible.Attributes.KeyExists("canSealCrock")
            && targetSlot.Itemstack.Collectible.Attributes["canSealCrock"].AsBool())
        {
            slot.Itemstack.Attributes.SetBool("sealed", true);
            slot.MarkDirty();
            targetSlot.TakeOut(1);
            targetSlot.MarkDirty();
            __result = true;
        }
        return;
    }
}
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Config;

namespace DanaTweaks;

public class EntityBehaviorDropHotSlots : EntityBehavior
{
    public EntityBehaviorDropHotSlots(Entity entity) : base(entity) { }

    public override void OnGameTick(float deltaTime)
    {
        if (entity is not EntityPlayer player)
        {
            return;
        }

        player.WalkInventory(CheckSlot);
    }

    private bool CheckSlot(ItemSlot slot)
    {
        if (entity is not EntityPlayer entityPlayer)
        {
            return true;
        }

        if (!slot.Inventory.IsPlayerInventory())
        {
            return true;
        }

        slot?.Inventory?.DropSlotIfHot(slot, entityPlayer.Player);
        return true;
    }

    public override string PropertyName() => "danatweaks:dropallhotslots";
}
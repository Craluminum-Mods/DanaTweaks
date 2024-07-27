using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class EntityBehaviorExtinctSubmergedTorchInEverySlot : EntityBehavior
{
    public EntityBehaviorExtinctSubmergedTorchInEverySlot(Entity entity) : base(entity) { }

    public override void OnGameTick(float deltaTime)
    {
        if (entity is not EntityPlayer entityPlayer)
        {
            return;
        }

        entityPlayer.WalkInventory(CheckSlot);
    }

    private bool CheckSlot(ItemSlot slot)
    {
        if (slot?.Empty == true || slot?.Itemstack?.Collectible is not BlockTorch torch)
        {
            return true;
        }

        if (!entity.Swimming || torch.IsExtinct || torch.ExtinctVariant == null)
        {
            return true;
        }

        entity.Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), entity.Pos.X + 0.5, entity.Pos.Y + 0.75, entity.Pos.Z + 0.5, null, randomizePitch: false, 16f);

        slot.Itemstack.SetFrom(new ItemStack(
            torch.ExtinctVariant.Id,
            torch.ExtinctVariant.ItemClass,
            slot.StackSize,
            slot.Itemstack.Attributes.Clone() as TreeAttribute,
            entity.World));

        slot.MarkDirty();

        return true;
    }

    public override string PropertyName() => "danatweaks:extinctSubmergedTorchInEverySlot";
}
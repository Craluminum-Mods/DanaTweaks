using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BEBehaviorExtinctSubmergedTorchInEverySlot : BlockEntityBehavior
{
    public BEBehaviorExtinctSubmergedTorchInEverySlot(BlockEntity blockentity) : base(blockentity) { }

    public override void Initialize(ICoreAPI api, JsonObject properties)
    {
        Blockentity.RegisterGameTickListener(CheckSlots, Core.ConfigServer.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds);
        base.Initialize(api, properties);
    }

    private void CheckSlots(float dt)
    {
        if (!Api.Side.IsServer() || Api.World.BlockAccessor.GetBlock(Pos, BlockLayersAccess.Fluid)?.Code?.ToString().Contains("water") == false)
        {
            return;
        }

        IBlockEntityContainer container = Block.GetInterface<IBlockEntityContainer>(Api.World, Pos);
        if (container?.Inventory?.Empty == true)
        {
            return;
        }

        foreach (ItemSlot slot in container.Inventory)
        {
            if (slot?.Empty == true || slot.Itemstack.Collectible is not BlockTorch torch || torch.IsExtinct || torch.ExtinctVariant == null)
            {
                continue;
            }

            Api.World.PlaySoundAt(new AssetLocation("sounds/effect/extinguish"), Pos.X + 0.5, Pos.Y + 0.75, Pos.Z + 0.5, null, randomizePitch: false, 16f);

            slot.Itemstack.SetFrom(new ItemStack(
                torch.ExtinctVariant.Id,
                torch.ExtinctVariant.ItemClass,
                slot.StackSize,
                slot.Itemstack.Attributes.Clone() as TreeAttribute,
                Api.World));

            slot.MarkDirty();
        }
    }
}
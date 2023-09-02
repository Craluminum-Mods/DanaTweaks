using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

public class BlockBehaviorBranchCutter : BlockBehavior
{
    public BlockBehaviorBranchCutter(Block block) : base(block) { }

    public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;

        ItemSlot slot = byPlayer?.InventoryManager?.ActiveHotbarSlot;
        int? toolMode = slot?.Itemstack?.Collectible?.GetToolMode(slot, byPlayer, byPlayer?.CurrentBlockSelection);

        if (slot.IsShears() && toolMode == 1 && block.BlockMaterial == EnumBlockMaterial.Leaves)
        {
            return new[] { new ItemStack(block) };
        }

        return base.GetDrops(world, pos, byPlayer, ref dropChanceMultiplier, ref handling);
    }
}
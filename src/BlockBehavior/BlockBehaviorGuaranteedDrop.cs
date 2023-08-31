using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

public class BlockBehaviorGuaranteedDrop : BlockBehavior
{
    public BlockBehaviorGuaranteedDrop(Block block) : base(block)
    {
    }

    public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;
        return new ItemStack[] { block.OnPickBlock(world, pos) };
    }
}
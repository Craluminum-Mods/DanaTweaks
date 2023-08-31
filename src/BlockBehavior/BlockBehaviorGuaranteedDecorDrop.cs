using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

public class BlockBehaviorGuaranteedDecorDrop : BlockBehavior
{
    public BlockBehaviorGuaranteedDecorDrop(Block block) : base(block)
    {
    }

    public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;

        BlockDropItemStack drop = new() { Code = block.Code, Type = EnumItemClass.Block, Quantity = NatFloat.One };
        drop.Resolve(world, "", null);
        ItemStack resolvedStack = drop.ResolvedItemstack;
        return new ItemStack[] { resolvedStack };
    }
}
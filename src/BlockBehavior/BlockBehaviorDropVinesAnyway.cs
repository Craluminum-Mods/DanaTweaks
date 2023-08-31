using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

public class BlockBehaviorDropVinesAnyway : BlockBehavior
{
    private bool brokenByPlayer;

    public BlockBehaviorDropVinesAnyway(Block block) : base(block) { }

    public override bool IsReplacableBy(Block block, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;
        return false;
    }

    public override void OnBlockBroken(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;

        brokenByPlayer = byPlayer != null;

        base.OnBlockBroken(world, pos, byPlayer, ref handling);
    }

    public override void OnBlockRemoved(IWorldAccessor world, BlockPos pos, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;

        if (!brokenByPlayer)
        {
            block.OnBlockBroken(world, pos, null);
        }

        brokenByPlayer = false;

        base.OnBlockRemoved(world, pos, ref handling);
    }
}
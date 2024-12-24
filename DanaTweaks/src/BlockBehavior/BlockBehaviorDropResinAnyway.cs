using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorDropResinAnyway : BlockBehavior
{
    public BlockBehaviorDropResinAnyway(Block block) : base(block) { }

    public override bool ClientSideOptional => true;

    public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
    {
        BlockDropItemStack[] drops = block.Drops.Append(block.GetBehavior<BlockBehaviorHarvestable>().harvestedStacks);

        handling = EnumHandling.PreventDefault;
        ItemStack[] newDrops = System.Array.Empty<ItemStack>();
        for (int i = 0; i < drops.Length; i++)
        {
            newDrops = newDrops.Append(drops[i].GetNextItemStack());
        }
        return newDrops;
    }
}
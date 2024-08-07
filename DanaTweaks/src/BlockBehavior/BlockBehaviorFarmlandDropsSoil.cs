using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorFarmlandDropsSoil : BlockBehavior
{
    public BlockBehaviorFarmlandDropsSoil(Block block) : base(block) { }

    public override ItemStack[] GetDrops(IWorldAccessor world, BlockPos pos, IPlayer byPlayer, ref float dropChanceMultiplier, ref EnumHandling handling)
    {
        if (!block.Code.PathStartsWith("farmland") && block.Code.Domain != "game")
        {
            return Array.Empty<ItemStack>();
        }

        if ((byPlayer?.WorldData.CurrentGameMode == EnumGameMode.Creative) || (world.BlockAccessor.GetBlockEntity(pos) is not BlockEntityFarmland farmland))
        {
            return Array.Empty<ItemStack>();
        }

        handling = EnumHandling.PreventSubsequent;

        float nutrients = farmland.Nutrients.Zip(farmland.OriginalFertility, (current, original) => current / original).Min();
        if ((nutrients < 0.95) && (world.Rand.NextDouble() > nutrients))
        {
            return Array.Empty<ItemStack>();
        }

        string fertility = block.Variant["fertility"];
        if (fertility == null)
        {
            return Array.Empty<ItemStack>();
        }

        Block newBlock = world.GetBlock(new AssetLocation("game", $"soil-{fertility}-none"));
        return new ItemStack[] { new ItemStack(newBlock) };
    }
}

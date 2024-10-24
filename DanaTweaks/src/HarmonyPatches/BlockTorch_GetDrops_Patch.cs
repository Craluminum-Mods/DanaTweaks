using System.Linq;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockTorch_GetDrops_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockTorch).GetMethod(nameof(BlockTorch.GetDrops));
    }

    public static MethodInfo GetPostfix() => typeof(BlockTorch_GetDrops_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(ref ItemStack[] __result, IWorldAccessor world, BlockPos pos)
    {
        if (world.BlockAccessor.GetBlockEntity(pos) is not BlockEntityTorch betorch || __result.Length == 0)
        {
            return;
        }

        foreach (ItemStack stack in __result.Where(x => x.Collectible is BlockTorch))
        {
            stack.Attributes.SetDouble("transitionHoursLeft", betorch.GetField<double>("transitionHoursLeft"));
        }
    }
}

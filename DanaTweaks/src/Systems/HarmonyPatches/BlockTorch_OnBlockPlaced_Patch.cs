using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockTorch_OnBlockPlaced_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockTorch).GetMethod(nameof(BlockTorch.OnBlockPlaced));
    }

    public static MethodInfo GetPostfix() => typeof(BlockTorch_OnBlockPlaced_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack = null)
    {
        if (byItemStack == null || world.BlockAccessor.GetBlockEntity(blockPos) is not BlockEntityTorch betorch || !byItemStack.Attributes.HasAttribute("transitionHoursLeft"))
        {
            return;
        }

        betorch.SetField("transitionHoursLeft", byItemStack.Attributes.GetDouble("transitionHoursLeft"));
    }
}
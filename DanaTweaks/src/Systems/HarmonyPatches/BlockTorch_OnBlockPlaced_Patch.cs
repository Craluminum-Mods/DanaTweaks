using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockTorch_OnBlockPlaced_Patch
{
    public static void Postfix(IWorldAccessor world, BlockPos blockPos, ItemStack byItemStack = null)
    {
        if (byItemStack == null || world.BlockAccessor.GetBlockEntity(blockPos) is not BlockEntityTorch betorch)
        {
            return;
        }

        if (byItemStack.Attributes.HasAttribute("transitionHoursLeft"))
        {
            betorch.SetField("transitionHoursLeft", byItemStack.Attributes.GetDouble("transitionHoursLeft"));
        }
    }
}
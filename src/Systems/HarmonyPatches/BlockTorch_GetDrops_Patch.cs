using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockTorch_GetDrops_Patch
{
    public static void Postfix(ref ItemStack[] __result, IWorldAccessor world, BlockPos pos)
    {
        if (world.BlockAccessor.GetBlockEntity(pos) is not BlockEntityTorch betorch)
        {
            return;
        }
        if (__result.Length != 0)
        {
            foreach (ItemStack stack in __result)
            {
                stack.Attributes.SetDouble("transitionHoursLeft", betorch.GetField<double>("transitionHoursLeft"));
            }
        }
    }
}

using HarmonyLib;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public partial class HarmonyPatches
{
    [HarmonyPatch(typeof(BlockClutter), nameof(BlockClutter.GetDrops))]
    public static class BlockClutter_GetDrops_Patch
    {
        public static bool Prefix(BlockClutter __instance, BlockPos pos)
        {
            BEBehaviorShapeFromAttributes bec = __instance.GetBEBehavior<BEBehaviorShapeFromAttributes>(pos);
            bec.Collected = true;
            return true;
        }
    }
}
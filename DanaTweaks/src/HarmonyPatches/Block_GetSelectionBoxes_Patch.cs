using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory("Unsorted")]
public static class Block_GetSelectionBoxes_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.GetSelectionBoxes))]
    public static bool Prefix(Block __instance, Cuboidf[] __result, IBlockAccessor blockAccessor, BlockPos pos, ICoreAPI ___api)
    {
        if (!ButcheringFix.Enabled) return true;

        Entity nearestEntity = ___api.World.GetNearestEntity(pos.ToVec3d(), 2, 2, Matches);
        if (nearestEntity != null)
        {
            __result = null;
            return false;
        }

        return true;
    }

    private static bool Matches(Entity entity)
    {
        return entity?.HasBehavior<EntityBehaviorHarvestable>() == true && entity?.Alive == false;
    }
}
using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class Block_GetSelectionBoxes_Patch
{
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(Block), nameof(Block.GetSelectionBoxes), new[] { typeof(IBlockAccessor), typeof(BlockPos) });
    }

    public static MethodInfo GetPrefix() => typeof(Block_GetSelectionBoxes_Patch).GetMethod(nameof(Prefix));

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
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(Core.ConfigClient.OverrideWaypointColors))]
public static class WaypointMapLayer_WaypointColors_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(WaypointMapLayer), nameof(WaypointMapLayer.WaypointColors), methodType: MethodType.Getter)]
    public static void Postfix(ref List<int> __result)
    {
        __result = Core.ConfigClient.ExtraWaypointColors.Select(ColorUtil.Hex2Int).ToList();
    }
}
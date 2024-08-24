using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class WaypointMapLayer_WaypointColors_Patch
{
    public static MethodInfo TargetMethod()
    {
        return AccessTools.PropertyGetter(typeof(WaypointMapLayer), nameof(WaypointMapLayer.WaypointColors));
    }

    public static MethodInfo GetPostfix() => typeof(WaypointMapLayer_WaypointColors_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(ref List<int> __result)
    {
        __result = Core.ConfigClient.ExtraWaypointColors.Select(ColorUtil.Hex2Int).ToList();
    }
}
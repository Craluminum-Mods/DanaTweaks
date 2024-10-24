using HarmonyLib;
using System.Reflection;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class ItemChisel_carvingTime_Patch
{
    public static MethodBase TargetMethod() => AccessTools.PropertyGetter(typeof(ItemChisel), nameof(ItemChisel.carvingTime));
    public static MethodInfo GetPostfix() => typeof(ItemChisel_carvingTime_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(ref bool __result)
    {
        __result = Core.ConfigServer.HalloweenEveryDay || __result;
    }
}
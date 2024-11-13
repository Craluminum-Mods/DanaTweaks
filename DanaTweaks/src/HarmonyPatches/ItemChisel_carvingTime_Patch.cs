using HarmonyLib;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory("UnsortedServer")]
public static class ItemChisel_carvingTime_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(ItemChisel), nameof(ItemChisel.carvingTime), methodType: MethodType.Getter)]
    public static void Postfix(ref bool __result)
    {
        __result = Core.ConfigServer.HalloweenEveryDay || __result;
    }
}
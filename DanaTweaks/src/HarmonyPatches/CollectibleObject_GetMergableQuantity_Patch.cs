using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.SealCrockExtraInteractions))]
public static class CollectibleObject_GetMergableQuantity_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CollectibleObject), nameof(CollectibleObject.GetMergableQuantity))]
    public static bool Prefix(CollectibleObject __instance, ref int __result, ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
    {
        if (sinkStack == null || sourceStack?.ItemAttributes == null) return true;

        if (__instance is BlockCrock && priority == EnumMergePriority.DirectMerge)
        {
            if (!sinkStack.IsCrockEmpty()
            && !sinkStack.Attributes.GetAsBool("sealed")
            && sourceStack.ItemAttributes.IsTrue("canSealCrock"))
            {
                __result = 1;
                return false;
            }
        }

        return true;
    }
}
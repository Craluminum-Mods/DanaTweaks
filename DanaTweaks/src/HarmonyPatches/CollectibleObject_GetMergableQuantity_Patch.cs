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
        if (__instance is BlockCrock
            && priority == EnumMergePriority.DirectMerge
            && !sinkStack.IsCrockEmpty()
            && !sinkStack.Attributes.GetAsBool("sealed")
            && sourceStack.Collectible.Attributes.KeyExists("canSealCrock")
            && sourceStack.Collectible.Attributes["canSealCrock"].AsBool())
        {
            __result = 1;
            return false;
        }

        return true;
    }
}
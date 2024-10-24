using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class CollectibleObject_GetMergableQuantity_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.GetMergableQuantity));
    }

    public static MethodInfo GetPrefix() => typeof(CollectibleObject_GetMergableQuantity_Patch).GetMethod(nameof(Prefix));

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
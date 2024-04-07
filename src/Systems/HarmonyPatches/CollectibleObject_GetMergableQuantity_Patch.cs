using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class CollectibleObject_GetMergableQuantity_Patch
{
    public static bool Prefix(CollectibleObject __instance, ref int __result, ItemStack sinkStack, ItemStack sourceStack, EnumMergePriority priority)
    {
        if (__instance is not BlockCrock)
        {
            return true;
        }

        if (priority == EnumMergePriority.DirectMerge)
        {
            if (!sinkStack.IsCrockEmpty()
                && !sinkStack.Attributes.GetAsBool("sealed")
                && sourceStack.Collectible.Attributes.KeyExists("canSealCrock")
                && sourceStack.Collectible.Attributes["canSealCrock"].AsBool())
            {
                __result = 1;
                return false;
            }
        }

        return true;
    }
}
using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.SealCrockExtraInteractions))]
public static class CollectibleObject_TryMergeStacks_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(CollectibleObject), nameof(CollectibleObject.TryMergeStacks))]
    public static bool Prefix(CollectibleObject __instance, ItemStackMergeOperation op, ICoreAPI ___api)
    {
        if (__instance is BlockCrock && op.CurrentPriority == EnumMergePriority.DirectMerge)
        {
            if (!op.SinkSlot.Itemstack.IsCrockEmpty()
                && !op.SinkSlot.Itemstack.Attributes.GetAsBool("sealed")
                && op.SourceSlot.Itemstack.Collectible.Attributes.KeyExists("canSealCrock")
                && op.SourceSlot.Itemstack.Collectible.Attributes["canSealCrock"].AsBool())
            {
                op.SinkSlot.Itemstack.Attributes.SetBool("sealed", true);
                op.MovedQuantity = 1;
                op.SourceSlot.TakeOut(1);
                op.SinkSlot.MarkDirty();
                return false;
            }
            else
            {
                if (op.World.Api.Side == EnumAppSide.Client)
                {
                    (___api as ICoreClientAPI)?.TriggerIngameError(__instance, "crockemptyorsealed", Lang.Get("ingameerror-crock-empty-or-sealed"));
                }
            }
        }

        return true;
    }
}
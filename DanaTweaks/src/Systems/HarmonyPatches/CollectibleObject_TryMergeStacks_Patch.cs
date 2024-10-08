using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class CollectibleObject_TryMergeStacks_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.TryMergeStacks));
    }

    public static MethodInfo GetPrefix() => typeof(CollectibleObject_TryMergeStacks_Patch).GetMethod(nameof(Prefix));

    public static bool Prefix(CollectibleObject __instance, ItemStackMergeOperation op)
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
                    ICoreAPI api = __instance.GetField<ICoreAPI>("api");
                    (api as ICoreClientAPI)?.TriggerIngameError(__instance, "crockemptyorsealed", Lang.Get("ingameerror-crock-empty-or-sealed"));
                }
            }
        }

        return true;
    }
}
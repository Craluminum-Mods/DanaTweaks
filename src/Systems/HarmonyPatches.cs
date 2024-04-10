using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class HarmonyPatches : ModSystem
{
    private Harmony HarmonyInstance => new Harmony(Mod.Info.ModID);

    public override void Start(ICoreAPI api)
    {
        if (Core.Config.SealCrockExtraInteractions)
        {
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.GetMergableQuantity)), prefix: typeof(CollectibleObject_GetMergableQuantity_Patch).GetMethod(nameof(CollectibleObject_GetMergableQuantity_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.TryMergeStacks)), prefix: typeof(CollectibleObject_TryMergeStacks_Patch).GetMethod(nameof(CollectibleObject_TryMergeStacks_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockCookedContainerBase).GetMethod(nameof(BlockCookedContainerBase.OnContainedInteractStart)), postfix: typeof(BlockCookedContainerBase_OnContainedInteractStart_Patch).GetMethod(nameof(BlockCookedContainerBase_OnContainedInteractStart_Patch.Postfix)));
        }
        if (Core.Config.FirepitHeatsOven)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityFirepit).GetMethod("OnBurnTick", AccessTools.all), postfix: typeof(BlockEntityFirepit_OnBurnTick_Patch).GetMethod(nameof(BlockEntityFirepit_OnBurnTick_Patch.Postfix)));
        }
        if (Core.Config.AlwaysSwitchToBestTool)
        {
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.OnHeldUseStart)), prefix: typeof(CollectibleObject_OnHeldUseStart_Patch).GetMethod(nameof(CollectibleObject_OnHeldUseStart_Patch.Prefix)));
        }
        if (Core.Config.CreativeMiddleClickEntity)
        {
            HarmonyInstance.Patch(original: typeof(SystemMouseInWorldInteractions).GetMethod("HandleMouseInteractionsNoBlockSelected", AccessTools.all), postfix: typeof(MiddleClickEntityPatch).GetMethod(nameof(MiddleClickEntityPatch.Postfix)));
        }
        if (Core.Config.CoolMoldsWithWateringCan)
        {
            HarmonyInstance.Patch(original: typeof(BlockWateringCan).GetMethod(nameof(BlockWateringCan.OnHeldInteractStep)), postfix: typeof(BlockWateringCan_OnHeldInteractStep_Patch).GetMethod(nameof(BlockWateringCan_OnHeldInteractStep_Patch.Postfix)));
        }
        if (Core.Config.PreventTorchTimerReset)
        {
            HarmonyInstance.Patch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.GetDrops)), postfix: typeof(BlockTorch_GetDrops_Patch).GetMethod(nameof(BlockTorch_GetDrops_Patch.Postfix)));
            HarmonyInstance.Patch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.OnBlockPlaced)), postfix: typeof(BlockTorch_OnBlockPlaced_Patch).GetMethod(nameof(BlockTorch_OnBlockPlaced_Patch.Postfix)));
        }
        if (Core.Config.FixOvenFuelRendering)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityOven).GetMethod("getOrCreateMesh", AccessTools.all), prefix: typeof(BlockEntityOven_getOrCreateMesh_Patch).GetMethod(nameof(BlockEntityOven_getOrCreateMesh_Patch.Prefix)));
        }
    }

    public override void Dispose()
    {
        if (Core.Config.SealCrockExtraInteractions)
        {
            HarmonyInstance.Unpatch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.GetMergableQuantity)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.TryMergeStacks)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockCookedContainerBase).GetMethod(nameof(BlockCookedContainerBase.OnContainedInteractStart)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.FirepitHeatsOven)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockEntityFirepit).GetMethod("OnBurnTick", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.AlwaysSwitchToBestTool)
        {
            HarmonyInstance.Unpatch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.OnHeldUseStart)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.CreativeMiddleClickEntity)
        {
            HarmonyInstance.Unpatch(original: typeof(SystemMouseInWorldInteractions).GetMethod("HandleMouseInteractionsNoBlockSelected", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.CoolMoldsWithWateringCan)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockWateringCan).GetMethod(nameof(BlockWateringCan.OnHeldInteractStep)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.PreventTorchTimerReset)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.GetDrops)), HarmonyPatchType.All, HarmonyInstance.Id);
            HarmonyInstance.Unpatch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.OnBlockPlaced)), HarmonyPatchType.All, HarmonyInstance.Id);
        }
        if (Core.Config.FixOvenFuelRendering)
        {
            HarmonyInstance.Unpatch(original: typeof(BlockEntityOven).GetMethod("getOrCreateMesh", AccessTools.all), HarmonyPatchType.All, HarmonyInstance.Id);
        }
    }
}
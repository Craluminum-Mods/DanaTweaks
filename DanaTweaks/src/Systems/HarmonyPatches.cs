using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class HarmonyPatches : ModSystem
{
    private ICoreAPI api;
    private Harmony HarmonyInstance => new Harmony(Mod.Info.ModID);

    public override void Start(ICoreAPI api)
    {
        this.api = api;
        if (Core.ConfigServer.SealCrockExtraInteractions)
        {
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.GetMergableQuantity)), prefix: typeof(CollectibleObject_GetMergableQuantity_Patch).GetMethod(nameof(CollectibleObject_GetMergableQuantity_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.TryMergeStacks)), prefix: typeof(CollectibleObject_TryMergeStacks_Patch).GetMethod(nameof(CollectibleObject_TryMergeStacks_Patch.Prefix)));
            HarmonyInstance.Patch(original: typeof(BlockCookedContainerBase).GetMethod(nameof(BlockCookedContainerBase.OnContainedInteractStart)), postfix: typeof(BlockCookedContainerBase_OnContainedInteractStart_Patch).GetMethod(nameof(BlockCookedContainerBase_OnContainedInteractStart_Patch.Postfix)));
        }
        if (Core.ConfigServer.FirepitHeatsOven)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityFirepit).GetMethod("OnBurnTick", AccessTools.all), postfix: typeof(BlockEntityFirepit_OnBurnTick_Patch).GetMethod(nameof(BlockEntityFirepit_OnBurnTick_Patch.Postfix)));
        }
        if (api.Side.IsClient() && Core.ConfigClient.AlwaysSwitchToBestTool)
        {
            HarmonyInstance.Patch(original: typeof(CollectibleObject).GetMethod(nameof(CollectibleObject.OnHeldUseStart)), prefix: typeof(CollectibleObject_OnHeldUseStart_Patch).GetMethod(nameof(CollectibleObject_OnHeldUseStart_Patch.Prefix)));
        }
        if (Core.ConfigServer.CreativeMiddleClickEntity)
        {
            HarmonyInstance.Patch(original: typeof(SystemMouseInWorldInteractions).GetMethod("HandleMouseInteractionsNoBlockSelected", AccessTools.all), postfix: typeof(MiddleClickEntityPatch).GetMethod(nameof(MiddleClickEntityPatch.Postfix)));
        }
        if (Core.ConfigServer.CoolMoldsWithWateringCan)
        {
            HarmonyInstance.Patch(original: typeof(BlockWateringCan).GetMethod(nameof(BlockWateringCan.OnHeldInteractStep)), postfix: typeof(BlockWateringCan_OnHeldInteractStep_Patch).GetMethod(nameof(BlockWateringCan_OnHeldInteractStep_Patch.Postfix)));
        }
        if (Core.ConfigServer.PreventTorchTimerReset)
        {
            HarmonyInstance.Patch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.GetDrops)), postfix: typeof(BlockTorch_GetDrops_Patch).GetMethod(nameof(BlockTorch_GetDrops_Patch.Postfix)));
            HarmonyInstance.Patch(original: typeof(BlockTorch).GetMethod(nameof(BlockTorch.OnBlockPlaced)), postfix: typeof(BlockTorch_OnBlockPlaced_Patch).GetMethod(nameof(BlockTorch_OnBlockPlaced_Patch.Postfix)));
        }
        if (Core.ConfigServer.FixOvenFuelRendering)
        {
            HarmonyInstance.Patch(original: typeof(BlockEntityOven).GetMethod("getOrCreateMesh", AccessTools.all), prefix: typeof(BlockEntityOven_getOrCreateMesh_Patch).GetMethod(nameof(BlockEntityOven_getOrCreateMesh_Patch.Prefix)));
        }
        if (Core.ConfigServer.RegrowResin)
        {
            HarmonyInstance.Patch(original: BlockEntitySapling_CheckGrow_Patch.TargetMethod(), transpiler: BlockEntitySapling_CheckGrow_Patch.GetTranspiler());
        }
        if (api.Side.IsClient() && Core.ConfigClient.ZoomMapWithKey)
        {
            HarmonyInstance.Patch(original: GuiElementMap_OnKeyDown_Patch.TargetMethod(), postfix: GuiElementMap_OnKeyDown_Patch.GetPostfix());
        }
        if (api.Side.IsClient() && Core.ConfigClient.ModesPerRowForVoxelRecipesEnabled)
        {
            HarmonyInstance.Patch(original: GuiDialogBlockEntityRecipeSelector_SetupDialog_Patch.TargetMethod(), transpiler: GuiDialogBlockEntityRecipeSelector_SetupDialog_Patch.GetTranspiler());
        }
        if (api.Side.IsClient() && Core.ConfigClient.ColorsPerRowForWaypointWindowEnabled)
        {
            HarmonyInstance.Patch(original: GuiComposerHelpers_AddColorListPicker_Patch.TargetMethod(), prefix: GuiComposerHelpers_AddColorListPicker_Patch.GetPrefix());
        }
        if (api.Side.IsClient() && Core.ConfigClient.IconsPerRowForWaypointWindowEnabled)
        {
            HarmonyInstance.Patch(original: GuiComposerHelpers_AddIconListPicker_Patch.TargetMethod(), prefix: GuiComposerHelpers_AddIconListPicker_Patch.GetPrefix());
        }
        if (api.Side.IsClient() && Core.ConfigClient.OverrideWaypointColors)
        {
            HarmonyInstance.Patch(original: WaypointMapLayer_WaypointColors_Patch.TargetMethod(), postfix: WaypointMapLayer_WaypointColors_Patch.GetPostfix());
        }
        HarmonyInstance.Patch(original: Block_GetSelectionBoxes_Patch.TargetMethod(), prefix: Block_GetSelectionBoxes_Patch.GetPrefix());
    }

    public override void Dispose()
    {
        HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
    }
}
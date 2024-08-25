using HarmonyLib;
using Vintagestory.API.Common;

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
            HarmonyInstance.Patch(original: CollectibleObject_GetMergableQuantity_Patch.TargetMethod(), prefix: CollectibleObject_GetMergableQuantity_Patch.GetPrefix());
            HarmonyInstance.Patch(original: CollectibleObject_TryMergeStacks_Patch.TargetMethod(), prefix: CollectibleObject_TryMergeStacks_Patch.GetPrefix());
            HarmonyInstance.Patch(original: BlockCookedContainerBase_OnContainedInteractStart_Patch.TargetMethod(), postfix: BlockCookedContainerBase_OnContainedInteractStart_Patch.GetPostfix());
        }
        if (Core.ConfigServer.FirepitHeatsOven)
        {
            HarmonyInstance.Patch(original: BlockEntityFirepit_OnBurnTick_Patch.TargetMethod(), postfix: BlockEntityFirepit_OnBurnTick_Patch.GetPostfix());
        }
        if (Core.ConfigServer.CreativeMiddleClickEntity)
        {
            HarmonyInstance.Patch(original: SystemMouseInWorldInteractions_HandleMouseInteractionsNoBlockSelected_Patch.TargetMethod(), postfix: SystemMouseInWorldInteractions_HandleMouseInteractionsNoBlockSelected_Patch.GetPostfix());
        }
        if (Core.ConfigServer.CoolMoldsWithWateringCan)
        {
            HarmonyInstance.Patch(original: BlockWateringCan_OnHeldInteractStep_Patch.TargetMethod(), postfix: BlockWateringCan_OnHeldInteractStep_Patch.GetPostfix());
        }
        if (Core.ConfigServer.PreventTorchTimerReset)
        {
            HarmonyInstance.Patch(original: BlockTorch_GetDrops_Patch.TargetMethod(), postfix: BlockTorch_GetDrops_Patch.GetPostfix());
            HarmonyInstance.Patch(original: BlockTorch_OnBlockPlaced_Patch.TargetMethod(), postfix: BlockTorch_OnBlockPlaced_Patch.GetPostfix());
        }
        if (Core.ConfigServer.FixOvenFuelRendering)
        {
            HarmonyInstance.Patch(original: BlockEntityOven_getOrCreateMesh_Patch.TargetMethod(), prefix: BlockEntityOven_getOrCreateMesh_Patch.GetPrefix());
        }
        if (Core.ConfigServer.RegrowResin)
        {
            HarmonyInstance.Patch(original: BlockEntitySapling_CheckGrow_Patch.TargetMethod(), transpiler: BlockEntitySapling_CheckGrow_Patch.GetTranspiler());
        }
        if (Core.ConfigServer.LiquidInteractionGroundStorage)
        {
            HarmonyInstance.Patch(original: BlockGroundStorage_OnBlockInteractStart_Patch.TargetMethod(), prefix: BlockGroundStorage_OnBlockInteractStart_Patch.GetPrefix());
            HarmonyInstance.Patch(original: BlockLiquidContainerBase_OnHeldInteractStart_Patch.TargetMethod(), prefix: BlockLiquidContainerBase_OnHeldInteractStart_Patch.GetPrefix());
        }
        if (api.Side.IsClient() && Core.ConfigClient.AlwaysSwitchToBestTool)
        {
            HarmonyInstance.Patch(original: CollectibleObject_OnHeldUseStart_Patch.TargetMethod(), prefix: CollectibleObject_OnHeldUseStart_Patch.GetPrefix());
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
using HarmonyLib;
using Vintagestory.API.Common;

namespace DanaTweaks;

public class HarmonyPatches : ModSystem
{
    private Harmony HarmonyInstance => new Harmony(Mod.Info.ModID);

    public override void Start(ICoreAPI api)
    {
        switch (api.Side)
        {
            case EnumAppSide.Server:
                ApplyServerPatches();
                break;
            case EnumAppSide.Client:
                ApplyClientPatches();
                break;
        }
        HarmonyInstance.PatchCategory("Unsorted");
    }

    private void ApplyServerPatches()
    {
        PatchCategoryIfTrue(nameof(Core.ConfigServer.SlabToolModes), value: Core.ConfigServer.SlabToolModes);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.SealCrockExtraInteractions), value: Core.ConfigServer.SealCrockExtraInteractions);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.FirepitHeatsOven), value: Core.ConfigServer.FirepitHeatsOven);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.CreativeMiddleClickEntity), value: Core.ConfigServer.CreativeMiddleClickEntity);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.FixOvenFuelRendering), value: Core.ConfigServer.FixOvenFuelRendering);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.RegrowResin), value: Core.ConfigServer.RegrowResin);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.GroundStorageLiquidInteraction), value: Core.ConfigServer.GroundStorageLiquidInteraction);
        PatchCategoryIfTrue(nameof(Core.ConfigServer.GroundStorageImmersiveCrafting), value: Core.ConfigServer.GroundStorageImmersiveCrafting);
        HarmonyInstance.PatchCategory("UnsortedServer");
    }

    private void ApplyClientPatches()
    {
        PatchCategoryIfTrue(nameof(Core.ConfigClient.AlwaysSwitchToBestTool), value: Core.ConfigClient.AlwaysSwitchToBestTool);
        PatchCategoryIfTrue(nameof(Core.ConfigClient.ModesPerRowForVoxelRecipesEnabled), value: Core.ConfigClient.ModesPerRowForVoxelRecipesEnabled);
        PatchCategoryIfTrue(nameof(Core.ConfigClient.ColorsPerRowForWaypointWindowEnabled), value: Core.ConfigClient.ColorsPerRowForWaypointWindowEnabled);
        PatchCategoryIfTrue(nameof(Core.ConfigClient.IconsPerRowForWaypointWindowEnabled), value: Core.ConfigClient.IconsPerRowForWaypointWindowEnabled);
        PatchCategoryIfTrue(nameof(Core.ConfigClient.OverrideWaypointColors), value: Core.ConfigClient.OverrideWaypointColors);
        HarmonyInstance.PatchCategory("UnsortedClient");
    }

    private void PatchCategoryIfTrue(string category, bool value)
    {
        if (value)
        {
            HarmonyInstance.PatchCategory(category);
        }
    }

    public override void Dispose()
    {
        HarmonyInstance.UnpatchAll(HarmonyInstance.Id);
    }
}
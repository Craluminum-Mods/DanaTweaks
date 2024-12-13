using Newtonsoft.Json;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace DanaTweaks.Configuration;

public class ConfigClient : IModConfig
{
    public bool AlwaysSwitchToBestTool { get; set; }

    [JsonConverter(typeof(StringArrayEnumConverter<EnumTool>))]
    public EnumTool[] AlwaysSwitchToBestToolIgnoredTools { get; set; }

    public bool OverrideWaypointColors { get; set; } = false;
    public List<string> ExtraWaypointColors { get; set; } = new();

    public bool ModesPerRowForVoxelRecipesEnabled { get; set; } = true;
    public int ModesPerRowForVoxelRecipes { get; set; } = 10;

    public bool ColorsPerRowForWaypointWindowEnabled { get; set; } = true;
    public float ColorsPerRowForWaypointWindowRatio { get; set; } = 1;

    public bool IconsPerRowForWaypointWindowEnabled { get; set; } = true;
    public float IconsPerRowForWaypointWindowRatio { get; set; } = 1;

    public bool GlowingProjectiles { get; set; }
    public bool ResinOnAllSides { get; set; } = true;

    public ConfigClient(ICoreAPI api, ConfigClient previousConfig = null)
    {
        if (previousConfig == null)
        {
            AlwaysSwitchToBestToolIgnoredTools ??= DefaultIgnoredTools();
            return;
        }

        AlwaysSwitchToBestToolIgnoredTools = previousConfig?.AlwaysSwitchToBestToolIgnoredTools ?? DefaultIgnoredTools();
        AlwaysSwitchToBestTool = previousConfig.AlwaysSwitchToBestTool;

        OverrideWaypointColors = previousConfig.OverrideWaypointColors;
        ExtraWaypointColors.AddRange(previousConfig.ExtraWaypointColors);

        ModesPerRowForVoxelRecipesEnabled = previousConfig.ModesPerRowForVoxelRecipesEnabled;
        ModesPerRowForVoxelRecipes = previousConfig.ModesPerRowForVoxelRecipes;

        ColorsPerRowForWaypointWindowEnabled = previousConfig.ColorsPerRowForWaypointWindowEnabled;
        ColorsPerRowForWaypointWindowRatio = previousConfig.ColorsPerRowForWaypointWindowRatio;

        IconsPerRowForWaypointWindowEnabled = previousConfig.IconsPerRowForWaypointWindowEnabled;
        IconsPerRowForWaypointWindowRatio = previousConfig.IconsPerRowForWaypointWindowRatio;

        GlowingProjectiles = previousConfig.GlowingProjectiles;
        ResinOnAllSides = previousConfig.ResinOnAllSides;
    }

    private static EnumTool[] DefaultIgnoredTools() => new[]
    {
        EnumTool.Bow,
        EnumTool.Chisel,
        EnumTool.Hammer,
        EnumTool.Hoe,
        EnumTool.Meter,
        EnumTool.Probe,
        EnumTool.Saw,
        EnumTool.Sickle,
        EnumTool.Sling,
        EnumTool.Spear,
        EnumTool.Sword,
        EnumTool.Wrench
    };
}
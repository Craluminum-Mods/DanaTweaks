using System.Collections.Generic;
using Vintagestory.API.Common;

namespace DanaTweaks.Configuration;

public class ConfigClient : IModConfig
{
    public bool OverrideWaypointColors { get; set; } = false;
    public List<string> ExtraWaypointColors { get; set; } = new();

    public bool ModesPerRowForVoxelRecipesEnabled { get; set; } = true;
    public int ModesPerRowForVoxelRecipes { get; set; } = 10;

    public bool ColorsPerRowForWaypointWindowEnabled { get; set; } = true;
    public float ColorsPerRowForWaypointWindowRatio { get; set; } = 1;

    public bool IconsPerRowForWaypointWindowEnabled { get; set; } = true;
    public float IconsPerRowForWaypointWindowRatio { get; set; } = 1;

    public ConfigClient(ICoreAPI api, ConfigClient previousConfig = null)
    {
        if (previousConfig == null)
        {
            return;
        }

        OverrideWaypointColors = previousConfig.OverrideWaypointColors;
        ExtraWaypointColors.AddRange(previousConfig.ExtraWaypointColors);

        ModesPerRowForVoxelRecipesEnabled = previousConfig.ModesPerRowForVoxelRecipesEnabled;
        ModesPerRowForVoxelRecipes = previousConfig.ModesPerRowForVoxelRecipes;

        ColorsPerRowForWaypointWindowEnabled = previousConfig.ColorsPerRowForWaypointWindowEnabled;
        ColorsPerRowForWaypointWindowRatio = previousConfig.ColorsPerRowForWaypointWindowRatio;

        IconsPerRowForWaypointWindowEnabled = previousConfig.IconsPerRowForWaypointWindowEnabled;
        IconsPerRowForWaypointWindowRatio = previousConfig.IconsPerRowForWaypointWindowRatio;
    }
}
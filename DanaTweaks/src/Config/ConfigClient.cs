using Newtonsoft.Json;
using Vintagestory.API.Common;

namespace DanaTweaks.Configuration;

public class ConfigClient : IModConfig
{
    public bool AlwaysSwitchToBestTool { get; set; }

    [JsonConverter(typeof(StringArrayEnumConverter<EnumTool>))]
    public EnumTool[] AlwaysSwitchToBestToolIgnoredTools { get; set; }

    public bool GlowingProjectiles { get; set; }
    public bool ZoomMapWithKey { get; set; } = true;
    
    public ConfigClient(ICoreAPI api, ConfigClient previousConfig = null)
    {
        if (previousConfig == null)
        {
            AlwaysSwitchToBestToolIgnoredTools ??= DefaultIgnoredTools();
            return;
        }

        AlwaysSwitchToBestToolIgnoredTools = previousConfig?.AlwaysSwitchToBestToolIgnoredTools ?? DefaultIgnoredTools();
        AlwaysSwitchToBestTool = previousConfig.AlwaysSwitchToBestTool;
        GlowingProjectiles = previousConfig.GlowingProjectiles;
        ZoomMapWithKey = previousConfig.ZoomMapWithKey;
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
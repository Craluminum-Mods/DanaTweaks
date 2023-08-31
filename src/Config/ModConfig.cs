using Vintagestory.API.Common;
using static DanaTweaks.Constants;

namespace DanaTweaks;

public static class ModConfig
{
    public static Config ReadConfig(ICoreAPI api)
    {
        Config config;

        try
        {
            config = LoadConfig(api);

            if (config == null)
            {
                GenerateConfig(api);
                config = LoadConfig(api);
            }
            else
            {
                GenerateConfig(api, config);
            }
        }
        catch
        {
            GenerateConfig(api);
            config = LoadConfig(api);
        }

        if (api.ModLoader.IsModEnabled("useplanksinpitkiln"))
        {
            api.World.Config.SetBool("DanaTweaks_PlanksInPitKiln_Enabled", false);
            config.PlanksInPitKiln = false;
        }
        else
        {
            api.World.Config.SetBool("DanaTweaks_PlanksInPitKiln_Enabled", config.PlanksInPitKiln);
        }

        return config;
    }
    private static Config LoadConfig(ICoreAPI api)
    {
        return api.LoadModConfig<Config>(ConfigName);
    }
    private static void GenerateConfig(ICoreAPI api)
    {
        api.StoreModConfig(new Config(), ConfigName);
    }
    private static void GenerateConfig(ICoreAPI api, Config previousConfig)
    {
        api.StoreModConfig(new Config(previousConfig), ConfigName);
    }
}
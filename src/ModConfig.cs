using CraluTweaks.Config;
using Vintagestory.API.Common;

namespace CraluTweaks.Utility
{
  static class ModConfig
  {
    private static CraluTweaksConfig config;

    public static void ReadConfig(ICoreAPI api)
    {
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

      api.World.Config.SetBool("AdjustWithRightClickEnabled", config.AdjustWithRightClickEnabled);
      api.World.Config.SetBool("CarryableBunchOCandlesEnabled", config.CarryableBunchOCandlesEnabled);
      api.World.Config.SetBool("CarryableChandelierEnabled", config.CarryableChandelierEnabled);
      api.World.Config.SetBool("CarryableFlowerpotEnabled", config.CarryableFlowerpotEnabled);
      api.World.Config.SetBool("CarryableForgeEnabled", config.CarryableForgeEnabled);
      api.World.Config.SetBool("CarryableLogWithResinEnabled", config.CarryableLogWithResinEnabled);
      api.World.Config.SetBool("CarryableMoldrackEnabled", config.CarryableMoldrackEnabled);
      api.World.Config.SetBool("CarryableMoldsEnabled", config.CarryableMoldsEnabled);
      api.World.Config.SetBool("CarryableOvenEnabled", config.CarryableOvenEnabled);
      api.World.Config.SetBool("CarryableShelfEnabled", config.CarryableShelfEnabled);
      api.World.Config.SetBool("CarryableToolrackEnabled", config.CarryableToolrackEnabled);
      api.World.Config.SetBool("CarryableTorchholderEnabled", config.CarryableTorchholderEnabled);
      api.World.Config.SetBool("Compost2xEnabled", config.Compost2xEnabled);
      api.World.Config.SetBool("DropWallpapersEnabled", config.DropWallpapersEnabled);
      api.World.Config.SetBool("InfiniteTraderMoneyEnabled", config.InfiniteTraderMoneyEnabled);
      api.World.Config.SetBool("LazyKnappingClayformingEnabled", config.LazyKnappingClayformingEnabled);
      api.World.Config.SetBool("PickUpBonesEnabled", config.PickUpBonesEnabled);
      api.World.Config.SetBool("PlacePieOnShelfEnabled", config.PlacePieOnShelfEnabled);
      api.World.Config.SetBool("PlacePotOnShelfEnabled", config.PlacePotOnShelfEnabled);
      api.World.Config.SetBool("RackableFirestarterEnabled", config.RackableFirestarterEnabled);
      api.World.Config.SetBool("ScrapRecipesEnabled", config.ScrapRecipesEnabled);
      api.World.Config.SetBool("UsePlanksInPitKilnEnabled", config.UsePlanksInPitKilnEnabled);
      api.World.Config.SetBool("VisuallyGlowingArrowsAndSpearsEnabled", config.VisuallyGlowingArrowsAndSpearsEnabled);
    }
    private static CraluTweaksConfig LoadConfig(ICoreAPI api)
    {
      return api.LoadModConfig<CraluTweaksConfig>("CraluTweaksConfig.json");
    }
    private static void GenerateConfig(ICoreAPI api)
    {
      api.StoreModConfig<CraluTweaksConfig>(new CraluTweaksConfig(), "CraluTweaksConfig.json");
    }
    private static void GenerateConfig(ICoreAPI api, CraluTweaksConfig previousConfig)
    {
      api.StoreModConfig<CraluTweaksConfig>(new CraluTweaksConfig(previousConfig), "CraluTweaksConfig.json");
    }
  }
}
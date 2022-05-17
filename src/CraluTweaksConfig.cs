namespace CraluTweaks.Config
{
  class CraluTweaksConfig
  {
    public bool AdjustWithRightClickEnabled = false;
    public bool CarryableBunchOCandlesEnabled = true;
    public bool CarryableChandelierEnabled = true;
    public bool CarryableFlowerpotEnabled = true;
    public bool CarryableForgeEnabled = true;
    public bool CarryableLogWithResinEnabled = false;
    public bool CarryableMoldrackEnabled = true;
    public bool CarryableMoldsEnabled = true;
    public bool CarryableOvenEnabled = true;
    public bool CarryableShelfEnabled = true;
    public bool CarryableToolrackEnabled = true;
    public bool CarryableTorchholderEnabled = true;
    public bool Compost2xEnabled = true;
    public bool DropWallpapersEnabled = true;
    public bool InfiniteTraderMoneyEnabled = true;
    public bool LazyKnappingClayformingEnabled = false;
    public bool PickUpBonesEnabled = true;
    public bool PlacePieOnShelfEnabled = true;
    public bool PlacePotOnShelfEnabled = true;
    public bool RackableFirestarterEnabled = true;
    public bool ScrapRecipesEnabled = true;
    public bool UsePlanksInPitKilnEnabled = true;
    public bool VisuallyGlowingArrowsAndSpearsEnabled = false;

    public CraluTweaksConfig()
    {

    }
    public CraluTweaksConfig(CraluTweaksConfig previousConfig)
    {
      AdjustWithRightClickEnabled = previousConfig.AdjustWithRightClickEnabled;
      CarryableBunchOCandlesEnabled = previousConfig.CarryableBunchOCandlesEnabled;
      CarryableChandelierEnabled = previousConfig.CarryableChandelierEnabled;
      CarryableFlowerpotEnabled = previousConfig.CarryableFlowerpotEnabled;
      CarryableForgeEnabled = previousConfig.CarryableForgeEnabled;
      CarryableLogWithResinEnabled = previousConfig.CarryableLogWithResinEnabled;
      CarryableMoldrackEnabled = previousConfig.CarryableMoldrackEnabled;
      CarryableMoldsEnabled = previousConfig.CarryableMoldsEnabled;
      CarryableOvenEnabled = previousConfig.CarryableOvenEnabled;
      CarryableShelfEnabled = previousConfig.CarryableShelfEnabled;
      CarryableToolrackEnabled = previousConfig.CarryableToolrackEnabled;
      CarryableTorchholderEnabled = previousConfig.CarryableTorchholderEnabled;
      Compost2xEnabled = previousConfig.Compost2xEnabled;
      DropWallpapersEnabled = previousConfig.DropWallpapersEnabled;
      InfiniteTraderMoneyEnabled = previousConfig.InfiniteTraderMoneyEnabled;
      LazyKnappingClayformingEnabled = previousConfig.LazyKnappingClayformingEnabled;
      PickUpBonesEnabled = previousConfig.PickUpBonesEnabled;
      PlacePieOnShelfEnabled = previousConfig.PlacePieOnShelfEnabled;
      PlacePotOnShelfEnabled = previousConfig.PlacePotOnShelfEnabled;
      RackableFirestarterEnabled = previousConfig.RackableFirestarterEnabled;
      ScrapRecipesEnabled = previousConfig.ScrapRecipesEnabled;
      UsePlanksInPitKilnEnabled = previousConfig.UsePlanksInPitKilnEnabled;
      VisuallyGlowingArrowsAndSpearsEnabled = previousConfig.VisuallyGlowingArrowsAndSpearsEnabled;
    }
  }
}
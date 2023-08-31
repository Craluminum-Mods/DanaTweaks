namespace DanaTweaks;

public class Config
{
    public bool CrateOpenCloseLid = true;
    public bool CrateRemoveOrAddLabel = true;

    public bool BranchCutterEnabled = true;
    public bool DropResinAnywayEnabled = true;
    public bool DropVinesAnywayEnabled = true;
    public bool DropWallpapersEnabled = true;
    public bool GlowingProjectilesEnabled;
    public bool PickUpBonesEnabled;
    public bool PlanksInPitKilnEnabled = true;
    public bool RackableFirestarterEnabled = true;
    public bool RemoveBookSignatureEnabled = true;
    public bool RichTradersEnabled;
    public bool ScrapRecipesEnabled = true;
    public bool ShelvablePieEnabled = true;
    public bool ShelvablePotEnabled = true;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        CrateOpenCloseLid = previousConfig.CrateOpenCloseLid;
        CrateRemoveOrAddLabel = previousConfig.CrateRemoveOrAddLabel;

        BranchCutterEnabled = previousConfig.BranchCutterEnabled;
        DropResinAnywayEnabled = previousConfig.DropResinAnywayEnabled;
        DropVinesAnywayEnabled = previousConfig.DropVinesAnywayEnabled;
        DropWallpapersEnabled = previousConfig.DropWallpapersEnabled;
        GlowingProjectilesEnabled = previousConfig.GlowingProjectilesEnabled;
        PickUpBonesEnabled = previousConfig.PickUpBonesEnabled;
        PlanksInPitKilnEnabled = previousConfig.PlanksInPitKilnEnabled;
        RackableFirestarterEnabled = previousConfig.RackableFirestarterEnabled;
        RemoveBookSignatureEnabled = previousConfig.RemoveBookSignatureEnabled;
        RichTradersEnabled = previousConfig.RichTradersEnabled;
        ScrapRecipesEnabled = previousConfig.ScrapRecipesEnabled;
        ShelvablePieEnabled = previousConfig.ShelvablePieEnabled;
        ShelvablePotEnabled = previousConfig.ShelvablePotEnabled;
    }
}
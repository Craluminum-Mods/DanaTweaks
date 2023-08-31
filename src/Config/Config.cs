namespace DanaTweaks;

public class Config
{
    public bool CrateOpenCloseLid = true;
    public bool CrateRemoveOrAddLabel = true;

    public bool DropWallpapersEnabled = true;
    public bool GlowingProjectilesEnabled;
    public bool PickUpBonesEnabled;
    public bool RackableFirestarterEnabled = true;
    public bool RichTradersEnabled;
    public bool ScrapRecipesEnabled = true;
    public bool ShelvablePieEnabled = true;
    public bool ShelvablePotEnabled = true;
    public bool PlanksInPitKilnEnabled = true;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        CrateOpenCloseLid = previousConfig.CrateOpenCloseLid;
        CrateRemoveOrAddLabel = previousConfig.CrateRemoveOrAddLabel;

        DropWallpapersEnabled = previousConfig.DropWallpapersEnabled;
        GlowingProjectilesEnabled = previousConfig.GlowingProjectilesEnabled;
        PickUpBonesEnabled = previousConfig.PickUpBonesEnabled;
        RackableFirestarterEnabled = previousConfig.RackableFirestarterEnabled;
        RichTradersEnabled = previousConfig.RichTradersEnabled;
        ScrapRecipesEnabled = previousConfig.ScrapRecipesEnabled;
        ShelvablePieEnabled = previousConfig.ShelvablePieEnabled;
        ShelvablePotEnabled = previousConfig.ShelvablePotEnabled;
        PlanksInPitKilnEnabled = previousConfig.PlanksInPitKilnEnabled;
    }
}
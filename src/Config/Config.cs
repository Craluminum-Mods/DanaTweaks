namespace DanaTweaks;

public class Config
{
    public bool CrateOpenRemoveLid = true;
    public bool CrateRemoveLabel = true;

    public bool ShelvablePieEnabled = true;
    public bool ShelvablePotEnabled = true;
    public bool RackableFirestarterEnabled = true;

    public bool GlowingProjectilesEnabled;
    public bool RichTradersEnabled;

    public bool DropWallpapersEnabled = true;
    public bool PickUpBonesEnabled;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        CrateOpenRemoveLid = previousConfig.CrateOpenRemoveLid;
        CrateRemoveLabel = previousConfig.CrateRemoveLabel;

        ShelvablePieEnabled = previousConfig.ShelvablePieEnabled;
        ShelvablePotEnabled = previousConfig.ShelvablePotEnabled;
        RackableFirestarterEnabled = previousConfig.RackableFirestarterEnabled;

        GlowingProjectilesEnabled = previousConfig.GlowingProjectilesEnabled;
        RichTradersEnabled = previousConfig.RichTradersEnabled;

        DropWallpapersEnabled = previousConfig.DropWallpapersEnabled;
        PickUpBonesEnabled = previousConfig.PickUpBonesEnabled;
    }
}
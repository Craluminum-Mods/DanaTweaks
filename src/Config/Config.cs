namespace DanaTweaks;

public class Config
{
    public bool CrateOpenCloseLid = true;
    public bool CrateRemoveOrAddLabel = true;

    public bool BranchCutter = true;
    public bool DropClutterAnyway = true;
    public bool DropResinAnyway = true;
    public bool DropVinesAnyway = true;
    public bool DropWallpapers = true;
    public bool FourPlanksFromLog;
    public bool GlowingProjectiles;
    public bool PickUpBones;
    public bool PlanksInPitKiln = true;
    public bool RackableFirestarter = true;
    public bool RemoveBookSignature = true;
    public bool RichTraders;
    public bool ScrapRecipes = true;
    public bool ShelvablePie = true;
    public bool ShelvablePot = true;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        CrateOpenCloseLid = previousConfig.CrateOpenCloseLid;
        CrateRemoveOrAddLabel = previousConfig.CrateRemoveOrAddLabel;

        BranchCutter = previousConfig.BranchCutter;
        DropClutterAnyway = previousConfig.DropClutterAnyway;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        DropWallpapers = previousConfig.DropWallpapers;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        GlowingProjectiles = previousConfig.GlowingProjectiles;
        PickUpBones = previousConfig.PickUpBones;
        PlanksInPitKiln = previousConfig.PlanksInPitKiln;
        RackableFirestarter = previousConfig.RackableFirestarter;
        RemoveBookSignature = previousConfig.RemoveBookSignature;
        RichTraders = previousConfig.RichTraders;
        ScrapRecipes = previousConfig.ScrapRecipes;
        ShelvablePie = previousConfig.ShelvablePie;
        ShelvablePot = previousConfig.ShelvablePot;
    }
}
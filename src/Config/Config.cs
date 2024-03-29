using System.Linq;
using System.Collections.Generic;

namespace DanaTweaks;

public class Config
{
    public Command Command { get; set; } = new();
    public OvenFuel OvenFuel { get; set; } = new();
    public RainCollector RainCollector { get; set; } = new();

    public bool BranchCutter { get; set; } = true;
    public bool DropClutterAnyway { get; set; } = true;
    public bool DropResinAnyway { get; set; } = true;
    public bool DropVinesAnyway { get; set; } = true;
    public bool DropWallpapers { get; set; } = true;
    public bool FourPlanksFromLog { get; set; }
    public bool FragileBones { get; set; }
    public bool GlowingProjectiles { get; set; }
    public bool PickUpBones { get; set; }
    public bool PlanksInPitKiln { get; set; } = true;
    public bool RackableFirestarter { get; set; } = true;
    public bool RemoveBookSignature { get; set; } = true;
    public bool RichTraders { get; set; }
    public bool ScrapRecipes { get; set; } = true;
    public bool ShelvablePie { get; set; } = true;
    public bool ShelvablePot { get; set; } = true;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        Command = previousConfig.Command;
        RainCollector = previousConfig.RainCollector;

        foreach (KeyValuePair<string, bool> keyVal in previousConfig.OvenFuel.Items.Where(keyVal => !OvenFuel.Items.ContainsKey(keyVal.Key)))
        {
            OvenFuel.Items.Add(keyVal.Key, keyVal.Value);
        }

        foreach (KeyValuePair<string, bool> keyVal in previousConfig.OvenFuel.Blocks.Where(keyVal => !OvenFuel.Blocks.ContainsKey(keyVal.Key)))
        {
            OvenFuel.Blocks.Add(keyVal.Key, keyVal.Value);
        }

        BranchCutter = previousConfig.BranchCutter;
        DropClutterAnyway = previousConfig.DropClutterAnyway;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        DropWallpapers = previousConfig.DropWallpapers;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        FragileBones = previousConfig.FragileBones;
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
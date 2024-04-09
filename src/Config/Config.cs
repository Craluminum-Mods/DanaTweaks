using System.Collections.Generic;
using System.Linq;

namespace DanaTweaks;

public class Config
{
    public Command Command { get; set; } = new();

    public Dictionary<string, CreatureOpenDoors> CreaturesOpenDoors { get; set; } = new()
    {
        ["drifter-*"] = new CreatureOpenDoors() { Enabled = true, Cooldown = 5, Range = 1 }
    };

    public Dictionary<string, OvenFuel> OvenFuelItems { get; set; } = new()
    {
        ["plank-*"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/plankpile" }
    };

    public Dictionary<string, OvenFuel> OvenFuelBlocks { get; set; } = new()
    {
        ["peatbrick"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/peatpile" }
    };

    public RainCollector RainCollector { get; set; } = new();

    public bool ExtinctSubmergedTorchInEverySlot { get; set; }
    public int ExtinctSubmergedTorchInEverySlotEveryMs { get; set; } = 5000;

    public bool AlwaysSwitchToBestTool { get; set; } = true;
    public bool BranchCutter { get; set; } = true;
    public bool DropResinAnyway { get; set; } = true;
    public bool DropVinesAnyway { get; set; } = true;
    public bool DropWallpapers { get; set; } = true;
    public bool FirepitHeatsOven { get; set; } = true;
    public bool FourPlanksFromLog { get; set; }
    public bool FragileBones { get; set; }
    public bool GlowingProjectiles { get; set; }
    public bool GroundStorageParticles { get; set; } = true;
    public bool PickUpBones { get; set; }
    public bool PlanksInPitKiln { get; set; } = true;
    public bool PlayerDropsHotSlots { get; set; }
    public bool PlayerWakesUpWhenHungry { get; set; }
    public bool RackableFirestarter { get; set; } = true;
    public bool RemoveBookSignature { get; set; } = true;
    public bool RichTraders { get; set; }
    public bool ScrapRecipes { get; set; } = true;
    public bool SealCrockExtraInteractions { get; set; } = true;
    public bool ShelvablePie { get; set; } = true;
    public bool SlabToolModes { get; set; } = true;

    public Config()
    {
    }
    public Config(Config previousConfig)
    {
        Command = previousConfig.Command;
        RainCollector = previousConfig.RainCollector;

        foreach ((string key, CreatureOpenDoors val) in previousConfig.CreaturesOpenDoors.Where(keyVal => !CreaturesOpenDoors.ContainsKey(keyVal.Key)))
        {
            CreaturesOpenDoors.Add(key, val);
        }

        foreach ((string key, OvenFuel val) in previousConfig.OvenFuelItems.Where(keyVal => !OvenFuelItems.ContainsKey(keyVal.Key)))
        {
            OvenFuelItems.Add(key, val);
        }

        foreach ((string key, OvenFuel val) in previousConfig.OvenFuelBlocks.Where(keyVal => !OvenFuelBlocks.ContainsKey(keyVal.Key)))
        {
            OvenFuelBlocks.Add(key, val);
        }

        ExtinctSubmergedTorchInEverySlot = previousConfig.ExtinctSubmergedTorchInEverySlot;
        ExtinctSubmergedTorchInEverySlotEveryMs = previousConfig.ExtinctSubmergedTorchInEverySlotEveryMs;

        AlwaysSwitchToBestTool = previousConfig.AlwaysSwitchToBestTool;
        BranchCutter = previousConfig.BranchCutter;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        DropWallpapers = previousConfig.DropWallpapers;
        FirepitHeatsOven = previousConfig.FirepitHeatsOven;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        FragileBones = previousConfig.FragileBones;
        GlowingProjectiles = previousConfig.GlowingProjectiles;
        GroundStorageParticles = previousConfig.GroundStorageParticles;
        PickUpBones = previousConfig.PickUpBones;
        PlanksInPitKiln = previousConfig.PlanksInPitKiln;
        PlayerDropsHotSlots = previousConfig.PlayerDropsHotSlots;
        PlayerWakesUpWhenHungry = previousConfig.PlayerWakesUpWhenHungry;
        RackableFirestarter = previousConfig.RackableFirestarter;
        RemoveBookSignature = previousConfig.RemoveBookSignature;
        RichTraders = previousConfig.RichTraders;
        ScrapRecipes = previousConfig.ScrapRecipes;
        SealCrockExtraInteractions = previousConfig.SealCrockExtraInteractions;
        ShelvablePie = previousConfig.ShelvablePie;
        SlabToolModes = previousConfig.SlabToolModes;
    }
}
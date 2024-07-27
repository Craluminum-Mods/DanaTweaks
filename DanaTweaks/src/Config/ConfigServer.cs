using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace DanaTweaks.Configuration;

public class ConfigServer : IModConfig
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
    public ScytheMore ScytheMore { get; set; } = new();

    public bool ExtinctSubmergedTorchInEverySlot { get; set; }
    public int ExtinctSubmergedTorchInEverySlotUpdateMilliseconds { get; set; } = 5000;

    public bool CoolMoldsWithWateringCan { get; set; } = true;
    public int CoolMoldsWithWateringCanSpeed { get; set; } = 3;

    public bool BranchCutter { get; set; } = true;
    public bool CreativeMiddleClickEntity { get; set; } = true;
    public bool DropResinAnyway { get; set; } = true;
    public bool DropVinesAnyway { get; set; } = true;
    public bool DropWallpapers { get; set; } = true;
    public bool EverySoilUnstable { get; set; }
    public bool FirepitHeatsOven { get; set; } = true;
    public bool FixOvenFuelRendering { get; set; } = true;
    public bool FourPlanksFromLog { get; set; }
    public bool FragileBones { get; set; }
    public bool GroundStorageParticles { get; set; } = true;
    public bool HalloweenEveryDay { get; set; } = true;
    public bool PickUpBones { get; set; }
    public bool PlanksInPitKiln { get; set; } = true;
    public bool PlayerDropsHotSlots { get; set; }
    public bool PlayerWakesUpWhenHungry { get; set; }
    public bool PreventTorchTimerReset { get; set; } = true;
    public bool RackableFirestarter { get; set; } = true;
    public bool RemoveBookSignature { get; set; } = true;
    public bool RichTraders { get; set; }
    public bool ScrapRecipes { get; set; } = true;
    public bool SealCrockExtraInteractions { get; set; } = true;
    public bool ShelvablePie { get; set; } = true;
    public bool SlabToolModes { get; set; } = true;

    public ConfigServer(ICoreAPI api, ConfigServer previousConfig = null)
    {
        if (previousConfig == null)
        {
            ScytheMore ??= new ScytheMore();
            ScytheMore.DisallowedParts ??= ScytheMore.DefaultDisallowedParts();
            ScytheMore.DisallowedSuffixes ??= ScytheMore.DefaultDisallowedSuffixes();
            return;
        }

        ScytheMore ??= previousConfig.ScytheMore ?? new ScytheMore();
        ScytheMore.Enabled = previousConfig.ScytheMore.Enabled;
        ScytheMore.DisallowedParts ??= previousConfig.ScytheMore.DisallowedParts ?? ScytheMore.DefaultDisallowedParts();
        ScytheMore.DisallowedSuffixes ??= previousConfig.ScytheMore.DisallowedSuffixes ?? ScytheMore.DefaultDisallowedSuffixes();

        Command = previousConfig.Command;
        RainCollector = previousConfig.RainCollector;

        CreaturesOpenDoors.AddRange(previousConfig.CreaturesOpenDoors);
        OvenFuelItems.AddRange(previousConfig.OvenFuelItems);
        OvenFuelBlocks.AddRange(previousConfig.OvenFuelBlocks);

        ExtinctSubmergedTorchInEverySlot = previousConfig.ExtinctSubmergedTorchInEverySlot;
        ExtinctSubmergedTorchInEverySlotUpdateMilliseconds = previousConfig.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds;

        CoolMoldsWithWateringCan = previousConfig.CoolMoldsWithWateringCan;
        CoolMoldsWithWateringCanSpeed = previousConfig.CoolMoldsWithWateringCanSpeed;

        BranchCutter = previousConfig.BranchCutter;
        CreativeMiddleClickEntity = previousConfig.CreativeMiddleClickEntity;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        DropWallpapers = previousConfig.DropWallpapers;
        EverySoilUnstable = previousConfig.EverySoilUnstable;
        FirepitHeatsOven = previousConfig.FirepitHeatsOven;
        FixOvenFuelRendering = previousConfig.FixOvenFuelRendering;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        FragileBones = previousConfig.FragileBones;
        GroundStorageParticles = previousConfig.GroundStorageParticles;
        HalloweenEveryDay = previousConfig.HalloweenEveryDay;
        PickUpBones = previousConfig.PickUpBones;
        PlanksInPitKiln = previousConfig.PlanksInPitKiln;
        PlayerDropsHotSlots = previousConfig.PlayerDropsHotSlots;
        PlayerWakesUpWhenHungry = previousConfig.PlayerWakesUpWhenHungry;
        PreventTorchTimerReset = previousConfig.PreventTorchTimerReset;
        RackableFirestarter = previousConfig.RackableFirestarter;
        RemoveBookSignature = previousConfig.RemoveBookSignature;
        RichTraders = previousConfig.RichTraders;
        ScrapRecipes = previousConfig.ScrapRecipes;
        SealCrockExtraInteractions = previousConfig.SealCrockExtraInteractions;
        ShelvablePie = previousConfig.ShelvablePie;
        SlabToolModes = previousConfig.SlabToolModes;
    }
}
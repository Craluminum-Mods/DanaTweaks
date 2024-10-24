using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace DanaTweaks.Configuration;

public class ConfigServer : IModConfig
{
    public readonly int AutoCloseDefaultDelay = 3000;

    public bool AutoClose { get; set; } = true;
    public Dictionary<string, int> AutoCloseDelays { get; set; } = new();

    public bool AutoPlantDroppedTreeSeeds { get; set; } = true;
    public int AutoPlantDroppedTreeSeedsDelay { get; set; } = 5000;
    public Command Command { get; set; } = new();
    public Dictionary<string, CreatureOpenDoors> CreaturesOpenDoors { get; set; } = new() { ["drifter-*"] = new CreatureOpenDoors() { Enabled = true, Cooldown = 5, Range = 1 } };
    public Dictionary<string, OvenFuel> OvenFuelItems { get; set; } = new() { ["plank-*"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/plankpile" } };
    public Dictionary<string, OvenFuel> OvenFuelBlocks { get; set; } = new() { ["peatbrick"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/peatpile" } };
    public RainCollector RainCollector { get; set; } = new();
    public ScytheMore ScytheMore { get; set; } = new();
    
    public bool ChuteAutoHarvesting { get; set; }
    public int ChuteAutoHarvestingUpdateMilliseconds { get; set; } = 5000;

    public bool DropDecor { get; set; } = true;
    public Dictionary<string, bool> DropDecorBlocks { get; set; } = new();

    public bool ExtinctSubmergedTorchInEverySlot { get; set; }
    public int ExtinctSubmergedTorchInEverySlotUpdateMilliseconds { get; set; } = 5000;

    public bool OpenConnectedTrapdoors { get; set; } = true;
    public int OpenConnectedTrapdoorsMaxBlocksDistance { get; set; } = 10;

    public bool RichTraders { get; set; }
    public Dictionary<string, NatFloat> RichTradersList { get; set; } = new();

    public bool BranchCutter { get; set; } = true;
    public bool CreativeMiddleClickEntity { get; set; } = true;
    public bool CreativeTapestries { get; set; } = true;
    public bool DropResinAnyway { get; set; } = true;
    public bool DropVinesAnyway { get; set; } = true;
    public bool EverySoilUnstable { get; set; }
    public bool ExtraClayforming { get; set; } = true;
    public bool FarmlandDropsSoil { get; set; } = true;
    public bool FirepitHeatsOven { get; set; } = true;
    public bool FixOvenFuelRendering { get; set; } = true;
    public bool FourPlanksFromLog { get; set; }
    public bool FragileBones { get; set; }
    public bool GroundStorageImmersiveCrafting { get; set; } = true;
    public bool GroundStorageLiquidInteraction { get; set; } = true;
    public bool GroundStorageParticles { get; set; } = true;
    public bool HalloweenEveryDay { get; set; } = true;
    public bool PickUpBones { get; set; }
    public bool PlanksInPitKiln { get; set; } = true;
    public bool PlayerDropsHotSlots { get; set; }
    public bool PlayerWakesUpWhenHungry { get; set; }
    public bool RackableFirestarter { get; set; } = true;
    public bool RecycleBags { get; set; }
    public bool RecycleClothes { get; set; }
    public bool RegrowResin { get; set; } = true;
    public bool RemoveBookSignature { get; set; } = true;
    public bool ScrapRecipes { get; set; } = true;
    public bool SealCrockExtraInteractions { get; set; } = true;
    public bool ShelvablePie { get; set; } = true;
    public bool SlabToolModes { get; set; } = true;
    public bool WaxCheeseOnGround { get; set; } = true;

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

        AutoClose = previousConfig.AutoClose;
        AutoCloseDelays.AddRange(previousConfig.AutoCloseDelays);

        AutoPlantDroppedTreeSeeds = previousConfig.AutoPlantDroppedTreeSeeds;
        AutoPlantDroppedTreeSeedsDelay = previousConfig.AutoPlantDroppedTreeSeedsDelay;

        Command = previousConfig.Command;
        RainCollector = previousConfig.RainCollector;

        CreaturesOpenDoors.AddRange(previousConfig.CreaturesOpenDoors);
        OvenFuelItems.AddRange(previousConfig.OvenFuelItems);
        OvenFuelBlocks.AddRange(previousConfig.OvenFuelBlocks);

        ChuteAutoHarvesting = previousConfig.ChuteAutoHarvesting;
        ChuteAutoHarvestingUpdateMilliseconds = previousConfig.ChuteAutoHarvestingUpdateMilliseconds;

        DropDecor = previousConfig.DropDecor;
        DropDecorBlocks.AddRange(previousConfig.DropDecorBlocks);

        ExtinctSubmergedTorchInEverySlot = previousConfig.ExtinctSubmergedTorchInEverySlot;
        ExtinctSubmergedTorchInEverySlotUpdateMilliseconds = previousConfig.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds;

        OpenConnectedTrapdoors = previousConfig.OpenConnectedTrapdoors;
        OpenConnectedTrapdoorsMaxBlocksDistance = previousConfig.OpenConnectedTrapdoorsMaxBlocksDistance;

        RichTraders = previousConfig.RichTraders;
        RichTradersList.AddRange(previousConfig.RichTradersList);

        BranchCutter = previousConfig.BranchCutter;
        CreativeMiddleClickEntity = previousConfig.CreativeMiddleClickEntity;
        CreativeTapestries = previousConfig.CreativeTapestries;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        EverySoilUnstable = previousConfig.EverySoilUnstable;
        ExtraClayforming = previousConfig.ExtraClayforming;
        FarmlandDropsSoil = previousConfig.FarmlandDropsSoil;
        FirepitHeatsOven = previousConfig.FirepitHeatsOven;
        FixOvenFuelRendering = previousConfig.FixOvenFuelRendering;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        FragileBones = previousConfig.FragileBones;
        GroundStorageImmersiveCrafting = previousConfig.GroundStorageImmersiveCrafting;
        GroundStorageLiquidInteraction = previousConfig.GroundStorageLiquidInteraction;
        GroundStorageParticles = previousConfig.GroundStorageParticles;
        HalloweenEveryDay = previousConfig.HalloweenEveryDay;
        PickUpBones = previousConfig.PickUpBones;
        PlanksInPitKiln = previousConfig.PlanksInPitKiln;
        PlayerDropsHotSlots = previousConfig.PlayerDropsHotSlots;
        PlayerWakesUpWhenHungry = previousConfig.PlayerWakesUpWhenHungry;
        RackableFirestarter = previousConfig.RackableFirestarter;
        RecycleBags = previousConfig.RecycleBags;
        RecycleClothes = previousConfig.RecycleClothes;
        RegrowResin = previousConfig.RegrowResin;
        RemoveBookSignature = previousConfig.RemoveBookSignature;
        ScrapRecipes = previousConfig.ScrapRecipes;
        SealCrockExtraInteractions = previousConfig.SealCrockExtraInteractions;
        ShelvablePie = previousConfig.ShelvablePie;
        SlabToolModes = previousConfig.SlabToolModes;
        WaxCheeseOnGround = previousConfig.WaxCheeseOnGround;
    }
}
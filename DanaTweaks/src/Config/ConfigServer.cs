using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace DanaTweaks.Configuration;

public class ConfigServer : IModConfig
{
    public readonly int AutoCloseDefaultDelay = 3000;

    public bool AutoClose { get; set; } = true;
    public Dictionary<string, int> AutoCloseDelays { get; set; } = new();

    public bool AutoPlantDroppedTreeSeeds { get; set; } = true;
    public int AutoPlantDroppedTreeSeedsDelay { get; set; } = 5000;
    public TweakHotkeys Hotkeys { get; set; } = new();
    public Dictionary<string, CreatureOpenDoors> CreaturesOpenDoors { get; set; } = new();
    public Dictionary<string, OvenFuel> OvenFuelItems { get; set; } = new();
    public Dictionary<string, OvenFuel> OvenFuelBlocks { get; set; } = new();
    public RainCollector RainCollector { get; set; } = new();
    public ScytheMore ScytheMore { get; set; } = new();

    public bool EverySoilUnstable { get; set; }
    public List<string> EverySoilUnstableBlacklist { get; set; } = new();
    
    public bool DropDecor { get; set; } = true;
    public Dictionary<string, bool> DropDecorBlocks { get; set; } = new();

    public bool ExtinctSubmergedTorchInEverySlot { get; set; }
    public int ExtinctSubmergedTorchInEverySlotUpdateMilliseconds { get; set; } = 5000;

    public bool OpenConnectedTrapdoors { get; set; } = true;
    public int OpenConnectedTrapdoorsMaxBlocksDistance { get; set; } = 10;

    public bool BranchCutter { get; set; } = true;
    public bool CreativeMiddleClickEntity { get; set; } = true;
    public bool DropResinAnyway { get; set; } = true;
    public bool DropVinesAnyway { get; set; } = true;
    public bool ExtraClayforming { get; set; } = true;
    public bool FirepitHeatsOven { get; set; } = true;
    public bool FourPlanksFromLog { get; set; }
    public bool HalloweenEveryDay { get; set; } = true;
    public bool PlanksInPitKiln { get; set; } = true;
    public bool PlayerDropsHotSlots { get; set; }
    public bool PlayerWakesUpWhenHungry { get; set; }
    public bool RackableFirestarter { get; set; } = true;
    public bool RegrowResin { get; set; } = true;
    public bool RemoveBookSignature { get; set; } = true;
    public bool SlabToolModes { get; set; } = true;
    public bool WaxCheeseOnGround { get; set; } = true;

    public ConfigServer(ICoreAPI api, ConfigServer previousConfig = null)
    {
        if (previousConfig == null)
        {
            ScytheMore ??= new ScytheMore();
            ScytheMore.DisallowedParts ??= ScytheMore.DefaultDisallowedParts();
            ScytheMore.DisallowedSuffixes ??= ScytheMore.DefaultDisallowedSuffixes();

            CreaturesOpenDoors = new() { ["drifter-*"] = new CreatureOpenDoors() { Enabled = true, Cooldown = 5, Range = 1 } };
            OvenFuelItems = new() { ["plank-*"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/plankpile" } };
            OvenFuelBlocks = new() { ["peatbrick"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/peatpile" } };
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

        Hotkeys = previousConfig.Hotkeys;
        RainCollector = previousConfig.RainCollector;

        CreaturesOpenDoors.AddRange(previousConfig.CreaturesOpenDoors);
        OvenFuelItems.AddRange(previousConfig.OvenFuelItems);
        OvenFuelBlocks.AddRange(previousConfig.OvenFuelBlocks);

        EverySoilUnstable = previousConfig.EverySoilUnstable;
        EverySoilUnstableBlacklist.AddRange(previousConfig.EverySoilUnstableBlacklist);

        DropDecor = previousConfig.DropDecor;
        DropDecorBlocks.AddRange(previousConfig.DropDecorBlocks);

        ExtinctSubmergedTorchInEverySlot = previousConfig.ExtinctSubmergedTorchInEverySlot;
        ExtinctSubmergedTorchInEverySlotUpdateMilliseconds = previousConfig.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds;

        OpenConnectedTrapdoors = previousConfig.OpenConnectedTrapdoors;
        OpenConnectedTrapdoorsMaxBlocksDistance = previousConfig.OpenConnectedTrapdoorsMaxBlocksDistance;

        BranchCutter = previousConfig.BranchCutter;
        CreativeMiddleClickEntity = previousConfig.CreativeMiddleClickEntity;
        DropResinAnyway = previousConfig.DropResinAnyway;
        DropVinesAnyway = previousConfig.DropVinesAnyway;
        ExtraClayforming = previousConfig.ExtraClayforming;
        FirepitHeatsOven = previousConfig.FirepitHeatsOven;
        FourPlanksFromLog = previousConfig.FourPlanksFromLog;
        HalloweenEveryDay = previousConfig.HalloweenEveryDay;
        PlanksInPitKiln = previousConfig.PlanksInPitKiln;
        PlayerDropsHotSlots = previousConfig.PlayerDropsHotSlots;
        PlayerWakesUpWhenHungry = previousConfig.PlayerWakesUpWhenHungry;
        RackableFirestarter = previousConfig.RackableFirestarter;
        RegrowResin = previousConfig.RegrowResin;
        RemoveBookSignature = previousConfig.RemoveBookSignature;
        SlabToolModes = previousConfig.SlabToolModes;
        WaxCheeseOnGround = previousConfig.WaxCheeseOnGround;
    }
}
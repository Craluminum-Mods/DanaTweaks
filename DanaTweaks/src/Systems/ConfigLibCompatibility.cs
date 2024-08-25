using ConfigLib;
using ImGuiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;

namespace DanaTweaks.Configuration;

public class ConfigLibCompatibility
{
    private const string settingPrefix = "danatweaks:Config.Setting.";

    private const string settingsAdvanced = "danatweaks:Config.SettingsAdvanced";
    private const string settingsSimple = "danatweaks:Config.SettingsSimple";

    private const string textEnabled = "worldconfig-seasons-Enabled";
    private const string textItems = "tabname-items";
    private const string textBlocks = "Blocks";
    private const string textModel = "Model";
    private const string textSupportsWildcard = "danatweaks:Config.Text.SupportsWildcard";
    private const string textAutoClose = "danatweaks:Config.Setting.AutoClose.Description";

    public ConfigLibCompatibility(ICoreAPI api)
    {
        api.ModLoader.GetModSystem<ConfigLibModSystem>().RegisterCustomConfig(Lang.Get("danatweaks:danatweaks-server"), (id, buttons) => EditConfigServer(id, buttons, api));
        api.ModLoader.GetModSystem<ConfigLibModSystem>().RegisterCustomConfig(Lang.Get("danatweaks:danatweaks-client"), (id, buttons) => EditConfigClient(id, buttons, api));
    }

    private void EditConfigServer(string id, ControlButtons buttons, ICoreAPI api)
    {
        if (buttons.Save) ModConfig.WriteConfig(api, Constants.ConfigServerName, Core.ConfigServer);
        if (buttons.Restore) Core.ConfigServer = ModConfig.ReadConfig<ConfigServer>(api, Constants.ConfigServerName);
        if (buttons.Defaults) Core.ConfigServer = new(api);

        BuildSettingsServer(Core.ConfigServer, id);
    }

    private void EditConfigClient(string id, ControlButtons buttons, ICoreAPI api)
    {
        if (buttons.Save) ModConfig.WriteConfig(api, Constants.ConfigClientName, Core.ConfigClient);
        if (buttons.Restore) Core.ConfigClient = ModConfig.ReadConfig<ConfigClient>(api, Constants.ConfigClientName);
        if (buttons.Defaults) Core.ConfigClient = new(api);

        BuildSettingsClient(Core.ConfigClient, id);
    }

    private void BuildSettingsServer(ConfigServer config, string id)
    {
        if (ImGui.CollapsingHeader(Lang.Get(settingsSimple) + $"##server-settingSimple-{id}"))
        {
            ImGui.Indent();
            config.CreativeMiddleClickEntity = OnCheckBox(id, config.CreativeMiddleClickEntity, nameof(config.CreativeMiddleClickEntity));
            config.CreativeTapestries = OnCheckBox(id, config.CreativeTapestries, nameof(config.CreativeTapestries));
            config.EverySoilUnstable = OnCheckBox(id, config.EverySoilUnstable, nameof(config.EverySoilUnstable));
            config.ExtraClayforming = OnCheckBox(id, config.ExtraClayforming, nameof(config.ExtraClayforming));
            config.GroundStorageParticles = OnCheckBox(id, config.GroundStorageParticles, nameof(config.GroundStorageParticles));
            config.ScrapRecipes = OnCheckBox(id, config.ScrapRecipes, nameof(config.ScrapRecipes));
            config.SealCrockExtraInteractions = OnCheckBox(id, config.SealCrockExtraInteractions, nameof(config.SealCrockExtraInteractions));
            config.SlabToolModes = OnCheckBox(id, config.SlabToolModes, nameof(config.SlabToolModes));

            config.BranchCutter = OnCheckBox(id, config.BranchCutter, nameof(config.BranchCutter));
            config.DropResinAnyway = OnCheckBox(id, config.DropResinAnyway, nameof(config.DropResinAnyway));
            config.DropVinesAnyway = OnCheckBox(id, config.DropVinesAnyway, nameof(config.DropVinesAnyway));
            config.FarmlandDropsSoil = OnCheckBox(id, config.FarmlandDropsSoil, nameof(config.FarmlandDropsSoil));
            config.FirepitHeatsOven = OnCheckBox(id, config.FirepitHeatsOven, nameof(config.FirepitHeatsOven));
            config.FixOvenFuelRendering = OnCheckBox(id, config.FixOvenFuelRendering, nameof(config.FixOvenFuelRendering));
            config.FourPlanksFromLog = OnCheckBox(id, config.FourPlanksFromLog, nameof(config.FourPlanksFromLog));
            config.FragileBones = OnCheckBox(id, config.FragileBones, nameof(config.FragileBones));
            config.HalloweenEveryDay = OnCheckBox(id, config.HalloweenEveryDay, nameof(config.HalloweenEveryDay));
            config.LiquidInteractionGroundStorage = OnCheckBox(id, config.LiquidInteractionGroundStorage, nameof(config.LiquidInteractionGroundStorage));
            config.PickUpBones = OnCheckBox(id, config.PickUpBones, nameof(config.PickUpBones));
            config.PlanksInPitKiln = OnCheckBox(id, config.PlanksInPitKiln, nameof(config.PlanksInPitKiln));
            config.PlayerDropsHotSlots = OnCheckBox(id, config.PlayerDropsHotSlots, nameof(config.PlayerDropsHotSlots));
            config.PlayerWakesUpWhenHungry = OnCheckBox(id, config.PlayerWakesUpWhenHungry, nameof(config.PlayerWakesUpWhenHungry));
            config.PreventTorchTimerReset = OnCheckBox(id, config.PreventTorchTimerReset, nameof(config.PreventTorchTimerReset));
            config.RackableFirestarter = OnCheckBox(id, config.RackableFirestarter, nameof(config.RackableFirestarter));
            config.RecycleBags = OnCheckBox(id, config.RecycleBags, nameof(config.RecycleBags));
            config.RecycleClothes = OnCheckBox(id, config.RecycleClothes, nameof(config.RecycleClothes));
            config.RegrowResin = OnCheckBox(id, config.RegrowResin, nameof(config.RegrowResin));
            config.RemoveBookSignature = OnCheckBox(id, config.RemoveBookSignature, nameof(config.RemoveBookSignature));
            config.ShelvablePie = OnCheckBox(id, config.ShelvablePie, nameof(config.ShelvablePie));
            ImGui.Unindent();
            ImGui.Indent();
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.AutoPlantDroppedTreeSeeds)) + $"##settingAutoPlantDroppedTreeSeeds-{id}"))
            {
                ImGui.Indent();
                config.AutoPlantDroppedTreeSeeds = OnCheckBoxWithoutTranslation($"##boolean-AutoPlantDroppedTreeSeeds-{id}", config.AutoPlantDroppedTreeSeeds, Lang.Get(textEnabled));
                config.AutoPlantDroppedTreeSeedsDelay = OnInputInt(id, config.AutoPlantDroppedTreeSeedsDelay, nameof(config.AutoPlantDroppedTreeSeedsDelay), minValue: 1);
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.Command)) + $"##settingCommand-{id}"))
            {
                ImGui.Indent();
                config.Command.CrateOpenCloseLid = OnCheckBox(id, config.Command.CrateOpenCloseLid, nameof(config.Command.CrateOpenCloseLid));
                config.Command.CrateRemoveOrAddLabel = OnCheckBox(id, config.Command.CrateRemoveOrAddLabel, nameof(config.Command.CrateRemoveOrAddLabel));
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.RainCollector)) + $"##settingRainCollector-{id}"))
            {
                ImGui.Indent();
                config.RainCollector.Enabled = OnCheckBoxWithoutTranslation($"##boolean-RainCollector-{id}", config.RainCollector.Enabled, Lang.Get(textEnabled));
                config.RainCollector.LitresPerUpdate = OnInputFloat(id, config.RainCollector.LitresPerUpdate, nameof(config.RainCollector.LitresPerUpdate), 0.01f);
                config.RainCollector.MinPrecipitation = OnInputFloat(id, config.RainCollector.MinPrecipitation, nameof(config.RainCollector.MinPrecipitation));
                config.RainCollector.UpdateMilliseconds = OnInputInt(id, config.RainCollector.UpdateMilliseconds, nameof(config.RainCollector.UpdateMilliseconds), 1);
                config.RainCollector.LiquidCode = OnInputText(id, config.RainCollector.LiquidCode, nameof(config.RainCollector.LiquidCode));
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.CoolMoldsWithWateringCan)) + $"##settingCoolMolds-{id}"))
            {
                ImGui.Indent();
                config.CoolMoldsWithWateringCan = OnCheckBoxWithoutTranslation($"##boolean-CoolMoldsWithWateringCan-{id}", config.CoolMoldsWithWateringCan, Lang.Get(textEnabled));
                config.CoolMoldsWithWateringCanSpeed = OnInputInt(id, config.CoolMoldsWithWateringCanSpeed, nameof(config.CoolMoldsWithWateringCanSpeed), 1);
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.ExtinctSubmergedTorchInEverySlot)) + $"##settingExtinct-{id}"))
            {
                ImGui.Indent();
                config.ExtinctSubmergedTorchInEverySlot = OnCheckBoxWithoutTranslation($"##boolean-Extinct-{id}", config.ExtinctSubmergedTorchInEverySlot, Lang.Get(textEnabled));
                config.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds = OnInputInt(id, config.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds, nameof(config.ExtinctSubmergedTorchInEverySlotUpdateMilliseconds), 1);
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.OpenConnectedTrapdoors)) + $"##settingOpenConnectedTrapdoors-{id}"))
            {
                ImGui.Indent();
                config.OpenConnectedTrapdoors = OnCheckBoxWithoutTranslation($"##boolean-OpenConnectedTrapdoors-{id}", config.OpenConnectedTrapdoors, Lang.Get(textEnabled));
                config.OpenConnectedTrapdoorsMaxBlocksDistance = OnInputInt(id, config.OpenConnectedTrapdoorsMaxBlocksDistance, nameof(config.OpenConnectedTrapdoorsMaxBlocksDistance));
                ImGui.Unindent();
            }
            ImGui.Unindent();
        }
        if (ImGui.CollapsingHeader(Lang.Get(settingsAdvanced) + $"##server-settingAdvanced-{id}"))
        {
            ImGui.Indent();
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + "AutoClose") + $"##settingAutoClose-{id}"))
            {
                ImGui.Indent();
                config.AutoClose = OnCheckBoxWithoutTranslation($"##boolean-AutoClose-{id}", config.AutoClose, Lang.Get(textEnabled));
                ImGui.TextWrapped(Lang.Get(textAutoClose));
                DictionaryEditor(config.AutoCloseDelays, new());
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + "DropDecor") + $"##settingDropDecor-{id}"))
            {
                ImGui.Indent();
                config.DropDecor = OnCheckBoxWithoutTranslation($"##boolean-DropDecor-{id}", config.DropDecor, Lang.Get(textEnabled));
                ImGui.TextWrapped(Lang.Get(textBlocks));
                DictionaryEditor(config.DropDecorBlocks, new());
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(ScytheMore)) + $"##settingScytheMore-{id}"))
            {
                ImGui.Indent();
                config.ScytheMore.Enabled = OnCheckBoxWithoutTranslation($"##boolean-ScytheMore-{id}", config.ScytheMore.Enabled, Lang.Get(textEnabled));
                config.ScytheMore.DisallowedParts = OnInputTextMultiline(id, config.ScytheMore.DisallowedParts, nameof(config.ScytheMore.DisallowedParts)).ToList();
                config.ScytheMore.DisallowedSuffixes = OnInputTextMultiline(id, config.ScytheMore.DisallowedSuffixes, nameof(config.ScytheMore.DisallowedSuffixes)).ToList();
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(OvenFuel)) + $"##settingOvenFuel-{id}"))
            {
                ImGui.Indent();
                ImGui.TextWrapped(Lang.Get(textItems));
                DictionaryEditor(config.OvenFuelItems, new OvenFuel(), Lang.Get(textSupportsWildcard));
                ImGui.TextWrapped(Lang.Get(textBlocks));
                DictionaryEditor(config.OvenFuelBlocks, new OvenFuel(), Lang.Get(textSupportsWildcard));
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.CreaturesOpenDoors)) + $"##settingCreatureOpenDoors-{id}"))
            {
                ImGui.Indent();
                DictionaryEditor(config.CreaturesOpenDoors, new CreatureOpenDoors(), Lang.Get(textSupportsWildcard));
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.RichTraders)) + $"##settingRichTraders-{id}"))
            {
                ImGui.Indent();
                config.RichTraders = OnCheckBoxWithoutTranslation($"##boolean-RichTraders-{id}", config.RichTraders, Lang.Get(textEnabled));
                DictionaryEditor(config.RichTradersList);
                ImGui.Unindent();
            }
            ImGui.Unindent();
        }
    }

    private void BuildSettingsClient(ConfigClient config, string id)
    {
        if (ImGui.CollapsingHeader(Lang.Get(settingsSimple) + $"##client-settingSimple-{id}"))
        {
            ImGui.Indent();
            config.GlowingProjectiles = OnCheckBox(id, config.GlowingProjectiles, nameof(config.GlowingProjectiles));
            config.ShakeSlotsInsideBags = OnCheckBox(id, config.ShakeSlotsInsideBags, nameof(config.ShakeSlotsInsideBags));
            config.ShakeSlotsWithBags = OnCheckBox(id, config.ShakeSlotsWithBags, nameof(config.ShakeSlotsWithBags));
            config.ShakeSlotsWithSuitableClothes = OnCheckBox(id, config.ShakeSlotsWithSuitableClothes, nameof(config.ShakeSlotsWithSuitableClothes));
            ImGui.Unindent();
        }
        if (ImGui.CollapsingHeader(Lang.Get(settingsAdvanced) + $"##client-settingAdvanced-{id}"))
        {
            ImGui.Indent();
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + "UI") + $"##settingUI-{id}"))
            {
                ImGui.Indent();
                config.OverrideWaypointColors = OnCheckBox(id, config.OverrideWaypointColors, nameof(config.OverrideWaypointColors));
                config.ExtraWaypointColors = OnInputTextMultiline(id, config.ExtraWaypointColors, nameof(config.ExtraWaypointColors)).ToList();
                ImGui.NewLine();
                config.ModesPerRowForVoxelRecipesEnabled = OnCheckBox(id, config.ModesPerRowForVoxelRecipesEnabled, nameof(config.ModesPerRowForVoxelRecipesEnabled));
                config.ModesPerRowForVoxelRecipes = OnInputInt(id, config.ModesPerRowForVoxelRecipes, nameof(config.ModesPerRowForVoxelRecipes), 1);
                ImGui.NewLine();
                config.ColorsPerRowForWaypointWindowEnabled = OnCheckBox(id, config.ColorsPerRowForWaypointWindowEnabled, nameof(config.ColorsPerRowForWaypointWindowEnabled));
                config.ColorsPerRowForWaypointWindowRatio = OnInputFloat(id, config.ColorsPerRowForWaypointWindowRatio, nameof(config.ColorsPerRowForWaypointWindowRatio), 0.1f);
                ImGui.NewLine();
                config.IconsPerRowForWaypointWindowEnabled = OnCheckBox(id, config.IconsPerRowForWaypointWindowEnabled, nameof(config.IconsPerRowForWaypointWindowEnabled));
                config.IconsPerRowForWaypointWindowRatio = OnInputFloat(id, config.IconsPerRowForWaypointWindowRatio, nameof(config.IconsPerRowForWaypointWindowRatio), 0.1f);
                ImGui.NewLine();
                config.ZoomMapWithKey = OnCheckBox(id, config.ZoomMapWithKey, nameof(config.ZoomMapWithKey));
                ImGui.Unindent();
            }
            if (ImGui.CollapsingHeader(Lang.Get(settingPrefix + nameof(config.AlwaysSwitchToBestTool)) + $"##settingAlwaysSwitchToBestTool-{id}"))
            {
                ImGui.Indent();
                config.AlwaysSwitchToBestTool = OnCheckBoxWithoutTranslation($"##boolean-AlwaysSwitchToBestTool-{id}", config.AlwaysSwitchToBestTool, Lang.Get(textEnabled));
                ImGui.BeginListBox(Lang.Get(settingPrefix + nameof(config.AlwaysSwitchToBestToolIgnoredTools)) + $"##settingAlwaysSwitchToBestToolIgnoredTools-{id}");
                IOrderedEnumerable<KeyValuePair<EnumTool, bool>> ignoredTools = Enum.GetValues<EnumTool>().ToDictionary(x => x, x => config.AlwaysSwitchToBestToolIgnoredTools.Contains(x)).OrderBy(x => Enum.GetName(x.Key));
                Dictionary<EnumTool, bool> ignoredToolsNew = ignoredTools.ToDictionary(x => x.Key, x => x.Value);

                foreach ((EnumTool key, bool val) in ignoredTools)
                {
                    ignoredToolsNew[key] = OnCheckBoxWithoutTranslation(id, val, Enum.GetName(key));
                }

                config.AlwaysSwitchToBestToolIgnoredTools = ignoredToolsNew.Where(x => x.Value).Select(x => x.Key).ToArray();
                ImGui.EndListBox();
                ImGui.Unindent();
            }
            ImGui.Unindent();
        }
    }

    private bool OnCheckBox(string id, bool value, string name)
    {
        bool newValue = value;
        ImGui.Checkbox(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue);
        return newValue;
    }

    private bool OnCheckBoxWithoutTranslation(string id, bool value, string name)
    {
        bool newValue = value;
        ImGui.Checkbox(name + $"##{name}-{id}", ref newValue);
        return newValue;
    }

    private int OnInputInt(string id, int value, string name, int minValue = default)
    {
        int newValue = value;
        ImGui.InputInt(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue, step: 1, step_fast: 10);
        return newValue < minValue ? minValue : newValue;
    }

    private float OnInputFloat(string id, float value, string name, float minValue = default)
    {
        float newValue = value;
        ImGui.InputFloat(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue, step: 0.01f, step_fast: 1.0f);
        return newValue < minValue ? minValue : newValue;
    }

    private string OnInputText(string id, string value, string name)
    {
        string newValue = value;
        ImGui.InputText(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue, 64);
        return newValue;
    }

    private IEnumerable<string> OnInputTextMultiline(string id, IEnumerable<string> values, string name)
    {
        string newValue = values.Any() ? values.Aggregate((first, second) => $"{first}\n{second}") : "";
        ImGui.InputTextMultiline(Lang.Get(settingPrefix + name) + $"##{name}-{id}", ref newValue, 256, new(0, 0));
        return newValue.Split('\n', StringSplitOptions.RemoveEmptyEntries).AsEnumerable();
    }

    private void DictionaryEditor<T>(Dictionary<string, T> dict, T defaultValue = default, string hint = "", string[] possibleValues = null)
    {
        if (ImGui.BeginTable("dict", 3, ImGuiTableFlags.BordersOuter))
        {
            for (int row = 0; row < dict.Count; row++)
            {
                ImGui.TableNextRow();
                string key = dict.Keys.ElementAt(row);
                string prevKey = (string)key.Clone();
                T value = dict.Values.ElementAt(row);
                ImGui.TableNextColumn();
                ImGui.InputTextWithHint($"##text-{row}", hint, ref key, 300);
                if (prevKey != key)
                {
                    dict.Remove(prevKey);
                    dict.TryAdd(key, value);
                    value = dict.Values.ElementAt(row);
                }
                ImGui.TableNextColumn();
                if (typeof(T) == typeof(int))
                {
                    int intValue = Convert.ToInt32(value);
                    ImGui.InputInt($"##int-{row}" + key, ref intValue);
                    value = (T)Convert.ChangeType(intValue, typeof(T));
                }
                else if (typeof(T) == typeof(float))
                {
                    float floatValue = Convert.ToSingle(value);
                    ImGui.InputFloat($"##float-{row}" + key, ref floatValue);
                    value = (T)Convert.ChangeType(floatValue, typeof(T));
                }
                else if (typeof(T) == typeof(bool))
                {
                    bool boolValue = Convert.ToBoolean(value);
                    ImGui.Checkbox($"##boolean-{row}" + key, ref boolValue);
                    value = (T)Convert.ChangeType(boolValue, typeof(T));
                }
                else if (typeof(T) == typeof(OvenFuel))
                {
                    OvenFuel customValue = value as OvenFuel;
                    customValue.Enabled = OnCheckBoxWithoutTranslation($"##boolean-{row}" + key, customValue.Enabled, Lang.Get(textEnabled));
                    customValue.Model = OnInputText($"##model-{row}" + key, customValue.Model, textModel);
                    value = (T)Convert.ChangeType(customValue, typeof(OvenFuel));
                }
                else if (typeof(T) == typeof(CreatureOpenDoors))
                {
                    CreatureOpenDoors customValue = value as CreatureOpenDoors;
                    customValue.Enabled = OnCheckBoxWithoutTranslation($"##boolean-{row}" + key, customValue.Enabled, Lang.Get(textEnabled));
                    customValue.Cooldown = OnInputFloat($"##cooldown-{row}" + key, customValue.Cooldown, nameof(CreatureOpenDoors.Cooldown));
                    customValue.Range = OnInputFloat($"##range-{row}" + key, customValue.Range, nameof(CreatureOpenDoors.Range));
                    value = (T)Convert.ChangeType(customValue, typeof(CreatureOpenDoors));
                }
                else if (typeof(T) == typeof(NatFloat))
                {
                    NatFloat customValue = value as NatFloat;
                    customValue.avg = OnInputFloat($"##avg-{row}" + key, customValue.avg, "avg");
                    customValue.var = OnInputFloat($"##var-{row}" + key, customValue.var, "var");
                    value = (T)Convert.ChangeType(customValue, typeof(NatFloat));
                }
                dict[key] = value;
                ImGui.TableNextColumn();
                if (ImGui.Button($"Remove##row-value-{row}"))
                {
                    dict.Remove(key);
                }
            }
            ImGui.TableNextRow();
            ImGui.TableNextColumn();
            if (ImGui.Button("Add"))
            {
                int id = dict.Count;
                string newKey = possibleValues?.FirstOrDefault(x => !dict.ContainsKey(x)) ?? $"row {id}";
                while (dict.ContainsKey(newKey)) newKey = $"row {++id}";
                dict.TryAdd(newKey, defaultValue);
            }
            ImGui.TableNextColumn();
            ImGui.TableNextColumn();
            ImGui.EndTable();
        }
    }
}
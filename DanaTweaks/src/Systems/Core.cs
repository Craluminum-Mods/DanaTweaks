global using static DanaTweaks.Constants;
using DanaTweaks.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;

[assembly: ModInfo(name: "Dana Tweaks", modID: "danatweaks", Side = "Universal")]

namespace DanaTweaks;

public class Core : ModSystem
{
    public static ConfigServer ConfigServer { get; set; }
    public static ConfigClient ConfigClient { get; set; }

    public override void StartPre(ICoreAPI api)
    {
        if (api.Side.IsServer())
        {
            ConfigServer = ModConfig.ReadConfig<ConfigServer>(api, ConfigServerName);
            api.World.Config.SetBool("DanaTweaks.ExtraClayforming", ConfigServer.ExtraClayforming);
            api.World.Config.SetBool("DanaTweaks.CreativeTapestries", ConfigServer.CreativeTapestries);
            api.World.Config.SetBool("DanaTweaks.RecycleBags", ConfigServer.RecycleBags);
            api.World.Config.SetBool("DanaTweaks.RecycleClothes", ConfigServer.RecycleClothes);
            api.World.Config.SetBool("DanaTweaks.WaxCheeseOnGround", ConfigServer.WaxCheeseOnGround);
        }
        if (api.Side.IsClient())
        {
            ConfigClient = ModConfig.ReadConfig<ConfigClient>(api, ConfigClientName);
        }

        if (api.ModLoader.IsModEnabled("configlib"))
        {
            _ = new ConfigLibCompatibility(api);
        }
    }

    public override void Start(ICoreAPI api)
    {
        api.RegisterBlockBehaviorClass("DanaTweaks:SelectSlabToolMode", typeof(BlockBehaviorSelectSlabToolMode));
        api.RegisterBlockBehaviorClass("DanaTweaks:BranchCutter", typeof(BlockBehaviorBranchCutter));
        api.RegisterBlockBehaviorClass("DanaTweaks:CrateInteractionHelp", typeof(BlockBehaviorCrateInteractionHelp));
        api.RegisterBlockBehaviorClass("DanaTweaks:DropResinAnyway", typeof(BlockBehaviorDropResinAnyway));
        api.RegisterBlockBehaviorClass("DanaTweaks:DropVinesAnyway", typeof(BlockBehaviorDropVinesAnyway));
        api.RegisterBlockBehaviorClass("DanaTweaks:GuaranteedDecorDrop", typeof(BlockBehaviorGuaranteedDecorDrop));
        api.RegisterBlockBehaviorClass("DanaTweaks:GroundStorageParticles", typeof(BlockBehaviorGroundStorageParticles));
        api.RegisterBlockBehaviorClass("DanaTweaks:FarmlandDropsSoil", typeof(BlockBehaviorFarmlandDropsSoil));
        api.RegisterBlockBehaviorClass("DanaTweaks:AutoClose", typeof(BlockBehaviorAutoClose));
        //api.RegisterBlockBehaviorClass("DanaTweaks:OpenConnectedTrapdoors", typeof(BlockBehaviorOpenConnectedTrapdoors));
        api.RegisterBlockBehaviorClass("DanaTweaks:WaxCheeseOnGroundInteractions", typeof(BlockBehaviorWaxCheeseOnGroundInteractions));

        api.RegisterBlockEntityBehaviorClass("DanaTweaks:RainCollector", typeof(BEBehaviorRainCollector));
        api.RegisterBlockEntityBehaviorClass("DanaTweaks:ExtinctSubmergedTorchInEverySlot", typeof(BEBehaviorExtinctSubmergedTorchInEverySlot));
        api.RegisterBlockEntityBehaviorClass("DanaTweaks:ChuteAutoHarvesting", typeof(BEBehaviorChuteAutoHarvesting));

        api.RegisterCollectibleBehaviorClass("DanaTweaks:BranchCutter", typeof(CollectibleBehaviorBranchCutter));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:RemoveBookSignature", typeof(CollectibleBehaviorRemoveBookSignature));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:SealCrockWithToolMode", typeof(CollectibleBehaviorSealCrockWithToolMode));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:WaxCheeseOnGround", typeof(CollectibleBehaviorWaxCheeseOnGround));

        api.RegisterEntityBehaviorClass("danatweaks:autoPlantDroppedTreeSeeds", typeof(EntityBehaviorAutoPlantDroppedTreeSeeds));
        api.RegisterEntityBehaviorClass("danatweaks:dropallhotslots", typeof(EntityBehaviorDropHotSlots));
        api.RegisterEntityBehaviorClass("danatweaks:extinctSubmergedTorchInEverySlot", typeof(EntityBehaviorExtinctSubmergedTorchInEverySlot));
        api.RegisterEntityBehaviorClass("danatweaks:hungrywakeup", typeof(EntityBehaviorHungryWakeUp));
        api.RegisterEntityBehaviorClass("danatweaks:opendoors", typeof(EntityBehaviorOpenDoors));
    }

    public override void StartServerSide(ICoreServerAPI api)
    {
        api.Event.OnEntitySpawn += AddEntityBehaviors;
        api.Event.OnEntityLoaded += AddEntityBehaviors;
    }

    private void AddEntityBehaviors(Entity entity)
    {
        if (entity is EntityPlayer)
        {
            if (ConfigServer.PlayerWakesUpWhenHungry)
            {
                entity.AddBehavior(new EntityBehaviorHungryWakeUp(entity));
            }

            if (ConfigServer.PlayerDropsHotSlots)
            {
                entity.AddBehavior(new EntityBehaviorDropHotSlots(entity));
            }

            if (ConfigServer.ExtinctSubmergedTorchInEverySlot)
            {
                entity.AddBehavior(new EntityBehaviorExtinctSubmergedTorchInEverySlot(entity));
            }
        }
        if (entity is EntityItem)
        {
            if (ConfigServer.AutoPlantDroppedTreeSeeds)
            {
                entity.AddBehavior(new EntityBehaviorAutoPlantDroppedTreeSeeds(entity));
            }
        }
        CreatureOpenDoors creatureOpenDoors = ConfigServer.CreaturesOpenDoors.FirstOrDefault(keyVal => entity.WildCardMatch(keyVal.Key) && keyVal.Value.Enabled).Value;
        if (creatureOpenDoors != null)
        {
            JsonObject jsonAttributes = creatureOpenDoors.GetAsAttributes();
            EntityBehaviorOpenDoors behavior = new EntityBehaviorOpenDoors(entity);
            behavior.Initialize(entity.Properties, jsonAttributes);
            entity.AddBehavior(behavior);
        }
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        if (ConfigClient.GlowingProjectiles)
        {
            api.Event.OnEntitySpawn += SetGlowLevel;
            api.Event.OnEntityLoaded += SetGlowLevel;
        }
    }

    public static void SetGlowLevel(Entity entity)
    {
        if (entity is EntityProjectile || entity.Class.Contains("projectile", StringComparison.OrdinalIgnoreCase))
        {
            entity.Properties.Client.GlowLevel = 255;
        }
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        if (!api.Side.IsServer())
        {
            return;
        }

        bool any = false;

        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code == null) continue;

            if (block.IsAutoCloseCompatible() && !ConfigServer.AutoCloseDelays.ContainsKey(block.Code.ToString()))
            {
                any = true;
                bool enabled = !block.Code.ToString().Contains("heavy") && !block.Code.ToString().Contains("ruined");
                ConfigServer.AutoCloseDelays.Add(block.Code.ToString(), enabled ? ConfigServer.AutoCloseDefaultDelay : -1);
            }
            if (block.HasBehavior<BlockBehaviorDecor>())
            {
                string code = block.Code.ToString();

                if (!code.Contains("caveart") && !code.Contains("hotspring") && !ConfigServer.DropDecorBlocks.ContainsKey(code))
                {
                    any = true;
                    bool enabled = code.Contains("wallpaper") ? true : (block.decorBehaviorFlags & 16) != 0;
                    ConfigServer.DropDecorBlocks.Add(code, enabled);
                }
            }
        }

        foreach (EntityProperties entityType in api.World.EntityTypes.Where(entityType => entityType.Code.ToString().Contains("trader") || entityType.Class.Contains("trader")))
        {
            if (!ConfigServer.RichTradersList.ContainsKey(entityType.Code.ToString()))
            {
                any = true;
                ConfigServer.RichTradersList.Add(entityType.Code.ToString(), new NatFloat(averagevalue: 9999f, variance: 0, EnumDistribution.UNIFORM));
            }
        }

        if (any)
        {
            ModConfig.WriteConfig(api, ConfigServerName, ConfigServer);
            ConfigServer = ModConfig.ReadConfig<ConfigServer>(api, ConfigServerName);
        }

        List<string> scytheMorePrefixes = new List<string>();

        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code == null)
            {
                continue;
            }
            if (ConfigServer.ScytheMore.Enabled && block.BlockMaterial == EnumBlockMaterial.Plant && !ConfigServer.ScytheMore.DisallowedParts.Any(x => block.Code.ToString().Contains(x)))
            {
                if (!scytheMorePrefixes.Contains(block.Code.FirstCodePart()))
                {
                    scytheMorePrefixes.Add(block.Code.FirstCodePart());
                }
            }
            if (ConfigServer.SlabToolModes && block.HasBehavior<BlockBehaviorOmniRotatable>())
            {
                block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorSelectSlabToolMode(block));
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorSelectSlabToolMode(block));
            }
            if (ConfigServer.SealCrockExtraInteractions && block is BlockCrock)
            {
                block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new CollectibleBehaviorSealCrockWithToolMode(block));
            }
            if (block is BlockCrate)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorCrateInteractionHelp(block));
            }
            if (ConfigServer.DropDecor && block.HasBehavior<BlockBehaviorDecor>() && block.WildCardMatch(ConfigServer.DropDecorBlocks.Where(x => x.Value).Select(x => x.Key).ToArray()))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGuaranteedDecorDrop(block));
            }
            if (block.Code.ToString().Contains("carcass"))
            {
                if (ConfigServer.PickUpBones)
                {
                    block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorRightClickPickup(block));
                }
                if (ConfigServer.FragileBones)
                {
                    block.Resistance = 0.15f;
                }
            }
            if (ConfigServer.ShelvablePie && block is BlockPie)
            {
                block.EnsureAttributesNotNull();
                block.Attributes.Token["shelvable"] = JToken.FromObject(true);
                block.Attributes.Token["onDisplayTransform"] = JToken.FromObject(new ModelTransform()
                {
                    Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                    Scale = 0.65f
                });
            }
            if (ConfigServer.BranchCutter && block.BlockMaterial == EnumBlockMaterial.Leaves)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorBranchCutter(block));
            }
            if (ConfigServer.DropResinAnyway && block.GetBehavior<BlockBehaviorHarvestable>()?.harvestedStack.Code == new AssetLocation(ResinCode))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropResinAnyway(block));
            }
            if (ConfigServer.DropVinesAnyway && block is BlockVines)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropVinesAnyway(block));
            }
            if (ConfigServer.RainCollector.Enabled && block is BlockLiquidContainerBase or BlockGroundStorage)
            {
                block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
                {
                    Name = "DanaTweaks:RainCollector",
                    properties = null
                });
            }
            if (ConfigServer.ExtinctSubmergedTorchInEverySlot && (block is IBlockEntityContainer || block.HasBehavior<BlockBehaviorContainer>() || block is BlockContainer))
            {
                block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
                {
                    Name = "DanaTweaks:ExtinctSubmergedTorchInEverySlot",
                    properties = null
                });
            }
            if (ConfigServer.GroundStorageParticles && block is BlockGroundStorage)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGroundStorageParticles(block));
            }
            OvenFuel ovenFuel = ConfigServer.OvenFuelBlocks.FirstOrDefault(keyVal => block.WildCardMatch(keyVal.Key) && keyVal.Value.Enabled).Value;
            if (ovenFuel != null)
            {
                block.EnsureAttributesNotNull();
                block.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);

                string model = ovenFuel.Model;
                if (!string.IsNullOrEmpty(model))
                {
                    block.Attributes.Token["ovenFuelShape"] = JToken.FromObject(model);
                }
            }
            if (ConfigServer.EverySoilUnstable && block.BlockMaterial is EnumBlockMaterial.Soil or EnumBlockMaterial.Gravel or EnumBlockMaterial.Sand && !block.HasBehavior<BlockBehaviorUnstableFalling>())
            {
                var properties = new { fallSound = "effect/rockslide", fallSideways = true, dustIntensity = 0.25 };
                BlockBehaviorUnstableFalling behavior = new BlockBehaviorUnstableFalling(block);
                behavior.Initialize(new JsonObject(JToken.FromObject(properties)));
                behavior.block.BlockBehaviors = behavior.block.BlockBehaviors.Append(behavior);

                block.EnsureAttributesNotNull();
                block.Attributes.Token["allowUnstablePlacement"] = JToken.FromObject(true);
            }
            if (ConfigServer.FarmlandDropsSoil && block is BlockFarmland)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorFarmlandDropsSoil(block));
            }
            if (ConfigServer.AutoClose && block.IsAutoCloseCompatible())
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorAutoClose(block));
                //if (block.CreativeInventoryTabs.Length != 0) block.CreativeInventoryTabs = block.CreativeInventoryTabs.Append("autoclose");
            }
            //if (ConfigServer.OpenConnectedTrapdoors && (block is BlockTrapdoor || block.HasBehavior<BlockBehaviorTrapDoor>()))
            //{
            //    block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorOpenConnectedTrapdoors(block));
            //}
            if (ConfigServer.WaxCheeseOnGround && block is BlockCheese)
            {
                block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorWaxCheeseOnGroundInteractions(block));
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorWaxCheeseOnGroundInteractions(block));
            }
            if (ConfigServer.ChuteAutoHarvesting && block is BlockChute)
            {
                block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
                {
                    Name = "DanaTweaks:ChuteAutoHarvesting",
                    properties = null
                });
            }
        }

        foreach (Item item in api.World.Items)
        {
            if (ConfigServer.ScytheMore.Enabled && item is ItemScythe)
            {
                PatchScytheAttributes(item, scytheMorePrefixes, ConfigServer.ScytheMore.DisallowedSuffixes);
            }
            if (ConfigServer.SealCrockExtraInteractions && item.WildCardMatch("@(beeswax|fat)"))
            {
                item.EnsureAttributesNotNull();
                item.Attributes.Token["canSealCrock"] = JToken.FromObject(true);
            }
            if (ConfigServer.RackableFirestarter && item is ItemFirestarter)
            {
                item.EnsureAttributesNotNull();
                item.Attributes.Token["rackable"] = JToken.FromObject(true);
                item.Attributes.Token["toolrackTransform"] = JToken.FromObject(new ModelTransform()
                {
                    Translation = new() { X = 0.25f, Y = 0.55f, Z = 0.0275f },
                    Rotation = new() { X = 180, Y = -135, Z = 0 },
                    Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                    Scale = 0.7f
                });
            }
            if (ConfigServer.BranchCutter && item is ItemShears)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorBranchCutter(item));
            }
            if (ConfigServer.RemoveBookSignature && item is ItemBook)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorRemoveBookSignature(item));
            }
            OvenFuel ovenFuel = ConfigServer.OvenFuelItems.FirstOrDefault(keyVal => item.WildCardMatch(keyVal.Key) && keyVal.Value.Enabled).Value;
            if (ovenFuel != null)
            {
                item.EnsureAttributesNotNull();
                item.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);

                string model = ovenFuel.Model;
                if (!string.IsNullOrEmpty(model))
                {
                    item.Attributes.Token["ovenFuelShape"] = JToken.FromObject(model);
                }
            }
            if (ConfigServer.WaxCheeseOnGround && item?.Attributes?["waxCheeseOnGround"]?.AsBool() == true)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorWaxCheeseOnGround(item));
            }
        }

        foreach (EntityProperties entityType in api.World.EntityTypes)
        {
            if (ConfigServer.RichTraders && ConfigServer.RichTradersList.TryGetValue(entityType.Code.ToString(), out NatFloat richTraderValue))
            {
                entityType.EnsureAttributesNotNull();
                entityType.Attributes.Token["tradeProps"]["money"]["avg"] = JToken.FromObject(richTraderValue.avg);
                entityType.Attributes.Token["tradeProps"]["money"]["var"] = JToken.FromObject(richTraderValue.var);
            }
        }

        if (ConfigServer.PlanksInPitKiln)
        {
            Block blockPitKiln = api.World.GetBlock(new AssetLocation(PitkilnCode));
            blockPitKiln.PatchBuildMats(api);
            blockPitKiln.PatchModelConfigs();
        }

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }

    private static void PatchScytheAttributes(Item item, List<string> newPrefixes, List<string> newSuffixes)
    {
        item.EnsureAttributesNotNull();

        List<string> codePrefixes = item?.Attributes?["codePrefixes"]?.AsObject<List<string>>();
        List<string> disallowedSuffixes = item?.Attributes?["disallowedSuffixes"]?.AsObject<List<string>>();

        if (codePrefixes?.Any() == true)
        {
            codePrefixes.AddRange(newPrefixes.Except(codePrefixes));
            item.Attributes.Token["codePrefixes"] = JToken.FromObject(codePrefixes);
        }
        if (disallowedSuffixes?.Any() == true)
        {
            disallowedSuffixes.AddRange(newSuffixes.Except(disallowedSuffixes));
            item.Attributes.Token["disallowedSuffixes"] = JToken.FromObject(disallowedSuffixes);
        }
    }
}
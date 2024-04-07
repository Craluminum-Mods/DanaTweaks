using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.API.Common.Entities;
using Newtonsoft.Json.Linq;
using System;

[assembly: ModInfo(name: "Dana Tweaks", modID: "danatweaks", Side = "Universal")]

namespace DanaTweaks;

public class Core : ModSystem
{
    public static Config Config { get; set; }

    public override void StartPre(ICoreAPI api)
    {
        base.StartPre(api);
        Config = ModConfig.ReadConfig(api);
    }

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        api.RegisterBlockBehaviorClass("DanaTweaks:BranchCutter", typeof(BlockBehaviorBranchCutter));
        api.RegisterBlockBehaviorClass("DanaTweaks:CrateInteractionHelp", typeof(BlockBehaviorCrateInteractionHelp));
        api.RegisterBlockBehaviorClass("DanaTweaks:DropResinAnyway", typeof(BlockBehaviorDropResinAnyway));
        api.RegisterBlockBehaviorClass("DanaTweaks:DropVinesAnyway", typeof(BlockBehaviorDropVinesAnyway));
        api.RegisterBlockBehaviorClass("DanaTweaks:GuaranteedDecorDrop", typeof(BlockBehaviorGuaranteedDecorDrop));
        api.RegisterBlockBehaviorClass("DanaTweaks:GuaranteedDrop", typeof(BlockBehaviorGuaranteedDrop));
        api.RegisterBlockEntityBehaviorClass("DanaTweaks:RainCollector", typeof(BEBehaviorRainCollector));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:BranchCutter", typeof(CollectibleBehaviorBranchCutter));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:RemoveBookSignature", typeof(CollectibleBehaviorRemoveBookSignature));
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);

        if (Config.GlowingProjectiles)
        {
            api.Event.OnEntitySpawn += SetGlowLevel;
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
        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code == null)
            {
                continue;
            }
            if (block is BlockCrate)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorCrateInteractionHelp(block));
            }
            if (Config.DropWallpapers && block.HasBehavior<BlockBehaviorDecor>() && block.Code.ToString().Contains("wallpaper"))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGuaranteedDecorDrop(block));
            }
            if (block.Code.ToString().Contains("carcass"))
            {
                if (Config.PickUpBones)
                {
                    block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorRightClickPickup(block));
                }
                if (Config.FragileBones)
                {
                    block.Resistance = 0.15f;
                }
            }
            if (Config.ShelvablePie && block is BlockPie)
            {
                block.EnsureAttributesNotNull();
                block.MakeShelvable();
                block.Attributes.Token["onDisplayTransform"] = JToken.FromObject(Constants.PieShelfTransform);
            }
            if (Config.BranchCutter && block.BlockMaterial == EnumBlockMaterial.Leaves)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorBranchCutter(block));
            }
            if (Config.DropResinAnyway && block.GetBehavior<BlockBehaviorHarvestable>()?.harvestedStack.Code == new AssetLocation(Constants.ResinCode))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropResinAnyway(block));
            }
            if (Config.DropVinesAnyway && block is BlockVines)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropVinesAnyway(block));
            }
            if (Config.RainCollector.Enabled && block is BlockLiquidContainerBase or BlockGroundStorage)
            {
                block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(Constants.RainCollectorBehavior);
            }
            if (Config.GroundStorageParticles &&block is BlockGroundStorage)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGroundStorageParticles(block));
            }
            if (Config.OvenFuel.Blocks.Any(keyVal => block.WildCardMatch(keyVal.Key) && keyVal.Value))
            {
                block.EnsureAttributesNotNull();
                block.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);
            }
        }

        foreach (Item item in api.World.Items)
        {
            if (Config.RackableFirestarter && item is ItemFirestarter)
            {
                item.EnsureAttributesNotNull();
                item.MakeRackable();
                item.Attributes.Token["toolrackTransform"] = JToken.FromObject(Constants.FirestarterToolrackTransform);
            }
            if (Config.BranchCutter && item is ItemShears)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorBranchCutter(item));
            }
            if (Config.RemoveBookSignature && item is ItemBook)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorRemoveBookSignature(item));
            }
            if (Config.OvenFuel.Items.Any(keyVal => item.WildCardMatch(keyVal.Key) && keyVal.Value))
            {
                item.EnsureAttributesNotNull();
                item.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);
            }
        }

        foreach (EntityProperties entityType in api.World.EntityTypes)
        {
            if (Config.RichTraders && entityType.Code.ToString().Contains("trader"))
            {
                entityType.EnsureAttributesNotNull();
                entityType.Attributes.Token["tradeProps"]["money"]["avg"] = JToken.FromObject(999999);
                entityType.Attributes.Token["tradeProps"]["money"]["var"] = JToken.FromObject(0);
            }
        }

        if (Config.PlanksInPitKiln)
        {
            Block blockPitKiln = api.World.GetBlock(new AssetLocation(Constants.PitkilnCode));
            blockPitKiln.PatchBuildMats(api);
            blockPitKiln.PatchModelConfigs();
        }

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}
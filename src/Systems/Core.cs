using System.Linq;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Newtonsoft.Json.Linq;

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
        api.RegisterBlockBehaviorClass("DanaTweaks:CrateInteractionHelp", typeof(BlockBehaviorCrateInteractionHelp));
        api.RegisterBlockBehaviorClass("DanaTweaks:WallpaperDrops", typeof(BlockBehaviorWallpaperDrops));
        api.RegisterBlockBehaviorClass("DanaTweaks:BranchCutter", typeof(BlockBehaviorBranchCutter));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:BranchCutter", typeof(CollectibleBehaviorBranchCutter));
        api.RegisterCollectibleBehaviorClass("DanaTweaks:RemoveBookSignature", typeof(CollectibleBehaviorRemoveBookSignature));
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
            if (Config.DropWallpapersEnabled && block.HasBehavior<BlockBehaviorDecor>() && block.Code.ToString().Contains("wallpaper"))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorWallpaperDrops(block));
            }
            if (Config.PickUpBonesEnabled && block.Code.ToString().Contains("carcass"))
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorRightClickPickup(block));
            }
            if (Config.ShelvablePieEnabled && block is BlockPie)
            {
                ModelTransform transform = new()
                {
                    Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                    Scale = 0.65f
                };

                block.Attributes ??= new JsonObject(new JObject());
                block.Attributes.Token["shelvable"] = JToken.FromObject(true);
                block.Attributes.Token["onDisplayTransform"] = JToken.FromObject(transform);
            }
            if (Config.ShelvablePotEnabled && block.Code.ToString().Contains("claypot"))
            {
                ModelTransform transform = new()
                {
                    Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                    Scale = 0.8f
                };

                block.Attributes ??= new JsonObject(new JObject());
                block.Attributes.Token["shelvable"] = JToken.FromObject(true);
                block.Attributes.Token["onDisplayTransform"] = JToken.FromObject(transform);
            }
            if (Config.BranchCutterEnabled && block.BlockMaterial == EnumBlockMaterial.Leaves)
            {
                block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorBranchCutter(block));
            }
        }

        foreach (Item item in api.World.Items)
        {
            if (Config.RackableFirestarterEnabled && item is ItemFirestarter)
            {
                ModelTransform transform = new()
                {
                    Translation = new() { X = 0.25f, Y = 0.55f, Z = 0.0275f },
                    Rotation = new() { X = 180, Y = -135, Z = 0 },
                    Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                    Scale = 0.7f
                };

                item.Attributes ??= new JsonObject(new JObject());
                item.Attributes.Token["rackable"] = JToken.FromObject(true);
                item.Attributes.Token["toolrackTransform"] = JToken.FromObject(transform);
            }
            if (Config.BranchCutterEnabled && item is ItemShears)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorBranchCutter(item));
            }
            if (Config.RemoveBookSignatureEnabled && item is ItemBook)
            {
                item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorRemoveBookSignature(item));
            }
        }

        foreach (EntityProperties entityType in api.World.EntityTypes)
        {
            if (Config.GlowingProjectilesEnabled && api is ICoreClientAPI capi)
            {
                List<string> projectileCodes = new() { "arrow", "spear" };
                if (projectileCodes.Any(entityType.Code.ToString().Contains))
                {
                    entityType.Client.GlowLevel = 255;
                }
            }
            if (Config.RichTradersEnabled && entityType.Code.ToString().Contains("trader"))
            {
                entityType.Attributes ??= new JsonObject(new JObject());
                entityType.Attributes.Token["tradeProps"]["money"]["avg"] = JToken.FromObject(999999);
                entityType.Attributes.Token["tradeProps"]["money"]["var"] = JToken.FromObject(0);
            }
        }

        if (Config.PlanksInPitKilnEnabled)
        {
            Block blockPitKiln = api.World.GetBlock(new AssetLocation("pitkiln"));
            blockPitKiln.PatchBuildMats(api);
            blockPitKiln.PatchModelConfigs();
        }

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}
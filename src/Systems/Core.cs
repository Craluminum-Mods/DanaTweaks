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

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}
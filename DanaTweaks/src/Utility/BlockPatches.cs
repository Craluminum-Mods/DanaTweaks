using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;
using Vintagestory.ServerMods;

namespace DanaTweaks;

public static class BlockPatches
{
    public static void FillAutoCloseList(this Block block, ref bool any)
    {
        if (!block.IsAutoCloseCompatible())
        {
            return;
        }

        string code = block.Code.ToString();

        string wood = block.Variant["wood"];
        string metal = block.Variant["metal"];
        string material = block.Variant["material"];

        if (!string.IsNullOrEmpty(wood)) code = code.Replace(wood, "*");
        if (!string.IsNullOrEmpty(metal)) code = code.Replace(metal, "*");
        if (!string.IsNullOrEmpty(material)) code = code.Replace(material, "*");

        if (code.Contains("irondoor"))
        {
            code = "game:irondoor-*";
        }

        code = code.RemoveAfterSymbol('*');

        if (code == "game:door-*")
        {
            return;
        }

        if (!Core.ConfigServer.AutoCloseDelays.ContainsKey(code))
        {
            any = true;
            bool enabled = !block.Code.ToString().Contains("heavy") && !block.Code.ToString().Contains("ruined");
            Core.ConfigServer.AutoCloseDelays.Add(code, enabled ? Core.ConfigServer.AutoCloseDefaultDelay : -1);
        }
    }

    public static void FillDecorList(this Block block, ref bool any)
    {
        if (!block.HasBehavior<BlockBehaviorDecor>())
        {
            return;
        }

        string code = block.Code.GetCompactCode();

        if (!code.Contains("caveart") && !code.Contains("hotspring") && !Core.ConfigServer.DropDecorBlocks.ContainsKey(code))
        {
            any = true;
            bool enabled = code.Contains("wallpaper") ? true : (block.decorBehaviorFlags & 16) != 0;
            Core.ConfigServer.DropDecorBlocks.Add(code, enabled);
        }
    }

    public static void FillScytheList(this Block block, ref List<string> scytheMorePrefixes)
    {
        if (!Core.ConfigServer.ScytheMore.Enabled || block.BlockMaterial != EnumBlockMaterial.Plant || Core.ConfigServer.ScytheMore.DisallowedParts.Any(x => block.Code.ToString().Contains(x)))
        {
            return;
        }

        if (scytheMorePrefixes.Contains(block.Code.FirstCodePart()))
        {
            return;
        }

        scytheMorePrefixes.Add(block.Code.FirstCodePart());
    }

    public static void PatchOmniRotatable(this Block block)
    {
        if (Core.ConfigServer.SlabToolModes && block.HasBehavior<BlockBehaviorOmniRotatable>())
        {
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorSelectSlabToolMode(block));
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorSelectSlabToolMode(block));
        }
    }

    public static void PatchCrock(this Block block)
    {
        if (Core.ConfigServer.SealCrockExtraInteractions && block is BlockCrock)
        {
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new CollectibleBehaviorSealCrockWithToolMode(block));
        }
    }

    public static void PatchCrate(this Block block)
    {
        if (block is BlockCrate)
        {
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorCrateInteractionHelp(block));
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorCrateInteractionHelp(block));
        }
    }

    public static void PatchDecor(this Block block)
    {
        if (Core.ConfigServer.DropDecor && block.HasBehavior<BlockBehaviorDecor>() && Core.ConfigServer.DropDecorBlocks.Any(x => block.WildCardMatchExt(x.Key) && x.Value))
        {
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorGuaranteedDecorDrop(block));
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGuaranteedDecorDrop(block));
        }
    }

    public static void PatchPie(this Block block)
    {
        if (Core.ConfigServer.ShelvablePie && block is BlockPie)
        {
            block.EnsureAttributesNotNull();
            block.Attributes.Token["shelvable"] = JToken.FromObject(true);
            block.Attributes.Token["onDisplayTransform"] = JToken.FromObject(new ModelTransform()
            {
                Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                Scale = 0.65f
            });
        }
    }

    public static void PatchLeaves(this Block block)
    {
        if (Core.ConfigServer.BranchCutter && block.BlockMaterial == EnumBlockMaterial.Leaves)
        {
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorBranchCutter(block));
        }
    }

    public static void PatchResinLog(this Block block)
    {
        if (Core.ConfigServer.DropResinAnyway && block.GetBehavior<BlockBehaviorHarvestable>()?.harvestedStack.Code == new AssetLocation(ResinCode))
        {
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropResinAnyway(block));
        }
    }

    public static void PatchVine(this Block block)
    {
        if (Core.ConfigServer.DropVinesAnyway && block is BlockVines)
        {
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorDropVinesAnyway(block));
        }
    }

    public static void PatchRainCollector(this Block block)
    {
        if (Core.ConfigServer.RainCollector.Enabled && block is BlockLiquidContainerBase or BlockGroundStorage)
        {
            block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
            {
                Name = "DanaTweaks:RainCollector",
                properties = null
            });
        }
    }

    public static void PatchSubmersibleContainers(this Block block)
    {
        if (Core.ConfigServer.ExtinctSubmergedTorchInEverySlot && (block is IBlockEntityContainer || block.HasBehavior<BlockBehaviorContainer>() || block is BlockContainer))
        {
            block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
            {
                Name = "DanaTweaks:ExtinctSubmergedTorchInEverySlot",
                properties = null
            });
        }
    }

    public static void PatchGroundStorageParticles(this Block block)
    {
        if (Core.ConfigServer.GroundStorageParticles && block is BlockGroundStorage)
        {
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorGroundStorageParticles(block));
        }
    }

    public static void PatchOvenFuel(this Block block)
    {
        OvenFuel ovenFuel = Core.ConfigServer.OvenFuelBlocks.FirstOrDefault(keyVal => block.WildCardMatchExt(keyVal.Key) && keyVal.Value.Enabled).Value;
        if (ovenFuel == null)
        {
            return;
        }
        block.EnsureAttributesNotNull();
        block.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);
        string model = ovenFuel.Model;
        if (!string.IsNullOrEmpty(model))
        {
            block.Attributes.Token["ovenFuelShape"] = JToken.FromObject(model);
        }
    }

    public static void PatchEverySoilUnstable(this Block block)
    {
        if (Core.ConfigServer.EverySoilUnstable && block.BlockMaterial is EnumBlockMaterial.Soil or EnumBlockMaterial.Gravel or EnumBlockMaterial.Sand && !block.HasBehavior<BlockBehaviorUnstableFalling>())
        {
            var properties = new { fallSound = "effect/rockslide", fallSideways = true, dustIntensity = 0.25 };
            BlockBehaviorUnstableFalling behavior = new BlockBehaviorUnstableFalling(block);
            behavior.Initialize(new JsonObject(JToken.FromObject(properties)));
            behavior.block.BlockBehaviors = behavior.block.BlockBehaviors.Append(behavior);
            block.EnsureAttributesNotNull();
            block.Attributes.Token["allowUnstablePlacement"] = JToken.FromObject(true);
        }
    }

    public static void PatchAutoClose(this Block block)
    {
        if (Core.ConfigServer.AutoClose && block.IsAutoCloseCompatible())
        {
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorAutoClose(block));
            //if (block.CreativeInventoryTabs.Length != 0) block.CreativeInventoryTabs = block.CreativeInventoryTabs.Append("autoclose");
        }
    }

    public static void PatchOpenConnectedTrapdoors(this Block block)
    {
        //if (Core.ConfigServer.OpenConnectedTrapdoors && (block is BlockTrapdoor || block.HasBehavior<BlockBehaviorTrapDoor>()))
        //{
        //    block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorOpenConnectedTrapdoors(block));
        //}
    }

    public static void PatchWaxeableCheese(this Block block)
    {
        if (Core.ConfigServer.WaxCheeseOnGround && block is BlockCheese)
        {
            block.CollectibleBehaviors = block.CollectibleBehaviors.Append(new BlockBehaviorWaxCheeseOnGroundInteractions(block));
            block.BlockBehaviors = block.BlockBehaviors.Append(new BlockBehaviorWaxCheeseOnGroundInteractions(block));
        }
    }

    public static void PatchChute(this Block block)
    {
        if (Core.ConfigServer.ChuteAutoHarvesting && block is BlockChute)
        {
            block.BlockEntityBehaviors = block.BlockEntityBehaviors.Append(new BlockEntityBehaviorType()
            {
                Name = "DanaTweaks:ChuteAutoHarvesting",
                properties = null
            });
        }
    }

    public static void PatchPitKiln(this Block block, ICoreAPI api)
    {
        if (Core.ConfigServer.PlanksInPitKiln && block is BlockPitkiln)
        {
            block.PatchBuildMats(api);
            block.PatchModelConfigs();
        }
    }
}
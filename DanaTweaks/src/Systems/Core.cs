global using static DanaTweaks.Constants;
using DanaTweaks.Configuration;
using System.Collections.Generic;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

[assembly: ModInfo(name: "Dana Tweaks", modID: "danatweaks", Side = "Universal")]

namespace DanaTweaks;

public class Core : ModSystem
{
    public static ConfigServer ConfigServer { get; set; }
    public static ConfigClient ConfigClient { get; set; }

    public override void StartPre(ICoreAPI api)
    {
        switch (api.Side)
        {
            case EnumAppSide.Server:
                ConfigServer = ModConfig.ReadConfig<ConfigServer>(api, ConfigServerName);
                api.World.Config.SetBool("DanaTweaks.CreativeTapestries", ConfigServer.CreativeTapestries);
                api.World.Config.SetBool("DanaTweaks.ExtraClayforming", ConfigServer.ExtraClayforming);
                api.World.Config.SetBool("DanaTweaks.RecycleBags", ConfigServer.RecycleBags);
                api.World.Config.SetBool("DanaTweaks.RecycleClothes", ConfigServer.RecycleClothes);
                api.World.Config.SetBool("DanaTweaks.ScrapRecipes", ConfigServer.ScrapRecipes);
                api.World.Config.SetBool("DanaTweaks.WaxCheeseOnGround", ConfigServer.WaxCheeseOnGround);
                break;
            case EnumAppSide.Client:
                ConfigClient = ModConfig.ReadConfig<ConfigClient>(api, ConfigClientName);
                break;
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
        api.Event.OnEntitySpawn += (entity) => entity.AddEntityBehaviors();
        api.Event.OnEntityLoaded += (entity) => entity.AddEntityBehaviors();
    }

    public override void StartClientSide(ICoreClientAPI api)
    {
        api.Event.OnEntitySpawn += (entity) => entity.SetGlowLevel();
        api.Event.OnEntityLoaded += (entity) => entity.SetGlowLevel();
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        if (!api.Side.IsServer())
        {
            return;
        }

        bool any = false;
        List<string> scytheMorePrefixes = new List<string>();

        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code == null) continue;
            block.FillAutoCloseList(ref any);
            block.FillDecorList(ref any);
            block.FillScytheList(ref scytheMorePrefixes);
        }

        if (any)
        {
            ModConfig.WriteConfig(api, ConfigServerName, ConfigServer);
            ConfigServer = ModConfig.ReadConfig<ConfigServer>(api, ConfigServerName);
        }

        foreach (Block block in api.World.Blocks)
        {
            if (block?.Code == null) continue;
            block.PatchAutoClose();
            block.PatchCarcass();
            block.PatchChute();
            block.PatchCrate();
            block.PatchCrock();
            block.PatchDecor();
            block.PatchEverySoilUnstable();
            block.PatchGroundStorageParticles();
            block.PatchLeaves();
            block.PatchOmniRotatable();
            block.PatchOpenConnectedTrapdoors();
            block.PatchOvenFuel();
            block.PatchPie();
            block.PatchPitKiln(api);
            block.PatchRainCollector();
            block.PatchResinLog();
            block.PatchSubmersibleContainers();
            block.PatchVine();
            block.PatchWaxeableCheese();
        }

        foreach (Item item in api.World.Items)
        {
            if (item?.Code == null) continue;
            item.PatchBook();
            item.PatchBranchCutter();
            item.PatchCrockSealant();
            item.PatchFirestarter();
            item.PatchOvenFuel();
            item.PatchScythe(scytheMorePrefixes);
            item.PatchWaxForCheese();
        }

        api.World.Logger.Event("started '{0}' mod", Mod.Info.Name);
    }
}
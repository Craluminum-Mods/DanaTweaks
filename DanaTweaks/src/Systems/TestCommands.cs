using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Common.CommandAbbr;
using System.Linq;
using System;

namespace DanaTweaks;

public class TestCommands : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartServerSide(ICoreServerAPI api)
    {
        IChatCommand command = api.ChatCommands.GetOrCreate("danatweakstest").WithAlias("dtt").RequiresPlayer().RequiresPrivilege("root");

        command.BeginSub("crucible")
            .WithAlias("cr")
            .WithDesc("Gives hot crucible full of copper")
            .HandleWith(GiveCrucible)
        .EndSub();

        command.BeginSub("pot")
            .WithDesc("Gives pot with meatystew meal")
            .HandleWith(GivePotWithMeal)
        .EndSub();
    }

    private TextCommandResult GiveCrucible(TextCommandCallingArgs args)
    {
        IServerPlayer player = args.Caller.Player as IServerPlayer;
        ICoreServerAPI api = player.Entity.Api as ICoreServerAPI;

        if (!player.InventoryManager.GetHotbarInventory().Any(x => x.Empty))
        {
            return TextCommandResult.Deferred;
        }

        ItemStack outputStack = new ItemStack(api.World.GetItem(new AssetLocation("ingot-copper")));
        Block block = api.World.GetBlock(new AssetLocation("crucible-smelted"));
        ItemStack crucibleStack = new ItemStack(block);
        block.CallMethod("SetContents", crucibleStack, outputStack, 1000);
        crucibleStack.Collectible.SetTemperature(api.World, crucibleStack, 1500, true);

        player.InventoryManager.TryGiveItemstack(crucibleStack, true);

        return TextCommandResult.Deferred;
    }

    private TextCommandResult GivePotWithMeal(TextCommandCallingArgs args)
    {
        IServerPlayer player = args.Caller.Player as IServerPlayer;
        ICoreServerAPI api = player.Entity.Api as ICoreServerAPI;

        if (!player.InventoryManager.GetHotbarInventory().Any(x => x.Empty))
        {
            return TextCommandResult.Deferred;
        }

        string recipeCode = "meatystew";

        JsonItemStack jsonStack = new JsonItemStack()
        {
            Type = EnumItemClass.Item,
            Code = new AssetLocation("redmeat-raw")
        };

        ItemStack[] stacks = new[]
        {
            jsonStack,
            jsonStack,
            jsonStack,
            jsonStack
        }.Where(x => x.Resolve(api.World, "")).Select(x => x.ResolvedItemstack).ToArray();

        float quantityServings = stacks.Length;
        Block block = api.World.GetBlock(new AssetLocation("claypot-cooked"));
        ItemStack containerStack = new ItemStack(block);

        block.CallMethodWithTypeArgs(
            "SetContents",
            new Type[] { typeof(string), typeof(ItemStack), typeof(ItemStack[]), typeof(float) },
            recipeCode,
            containerStack,
            stacks,
            quantityServings);

        containerStack.Collectible.SetTemperature(api.World, containerStack, 500, true);

        player.InventoryManager.TryGiveItemstack(containerStack, true);

        return TextCommandResult.Deferred;
    }
}

// {type: "Block", code: "game:claypot-cooked", attributes: { "contents": { "0": { "type": "item", "code": "redmeat-raw", "1": { "type": "item", "code": "redmeat-raw" }, "2": { "type": "item", "code": "redmeat-raw", }, "3": { "type": "item", "code": "redmeat-raw"} }, "quantityServings": 1, "recipeCode": "meatystew" } }
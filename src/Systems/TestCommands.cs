using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.API.Common.CommandAbbr;
using System.Linq;

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
}
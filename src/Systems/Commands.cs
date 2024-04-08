using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common.CommandAbbr;

namespace DanaTweaks;

public class Commands : ModSystem
{
    private ItemStack LabelStack { get; set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartServerSide(ICoreServerAPI api)
    {
        LabelStack = new(api.World.GetItem(new AssetLocation(Constants.ParchmentCode)));

        IChatCommand command = api.ChatCommands.GetOrCreate("danatweaks").RequiresPlayer().RequiresPrivilege("useblock");

        if (Core.Config.Command.CrateRemoveOrAddLabel)
        {
            command.BeginSub("removeoraddlabel")
                .WithAlias("ral")
                .WithDesc(Constants.RemoveOrAddLabelName)
                .HandleWith(RemoveOrAddLabel)
            .EndSub();
        }
        if (Core.Config.Command.CrateOpenCloseLid)
        {
            command.BeginSub("openclose")
                .WithAlias("oc")
                .WithDesc(Constants.OpenCloseLidName)
                .HandleWith(OpenCloseLid)
            .EndSub();
        }
    }

    private TextCommandResult OpenCloseLid(TextCommandCallingArgs args)
    {
        IServerPlayer player = args.Caller.Player as IServerPlayer;
        BlockPos pos = player?.CurrentBlockSelection?.Position;

        if (pos == null || player.Entity.World.BlockAccessor.GetBlockEntityExt<BlockEntityCrate>(pos) is not BlockEntityCrate becrate)
        {
            return TextCommandResult.Error(Constants.NoCrate);
        }

        if (!player.Entity.Api.World.Claims.TryAccess(player, pos, EnumBlockAccessFlags.Use))
        {
            return TextCommandResult.Deferred;
        }

        becrate.preferredLidState = becrate.preferredLidState switch
        {
            "opened" => "closed",
            "closed" => "opened",
            _ => "opened"
        };

        becrate.MarkDirty(redrawOnClient: true);
        return TextCommandResult.Deferred;
    }

    public TextCommandResult RemoveOrAddLabel(TextCommandCallingArgs args)
    {
        IServerPlayer player = args.Caller.Player as IServerPlayer;
        BlockPos pos = player?.CurrentBlockSelection?.Position;

        if (pos == null)
        {
            return TextCommandResult.Error(Constants.NoCrate);
        }

        ItemSlot activeSlot = player.InventoryManager.ActiveHotbarSlot;

        BlockEntityCrate becrate = player.Entity.World.BlockAccessor.GetBlockEntityExt<BlockEntityCrate>(pos);

        if (becrate == null)
        {
            return TextCommandResult.Error(Constants.NoCrate);
        }

        if (!player.Entity.Api.World.Claims.TryAccess(player, pos, EnumBlockAccessFlags.Use))
        {
            return TextCommandResult.Deferred;
        }

        return becrate.label switch
        {
            Constants.DefaultLabel => RemoveLabel(player, becrate),
            not null and not "" => TextCommandResult.Error(Constants.HasDiffLabel),
            _ => activeSlot.IsCorrectLabel(LabelStack)
                                ? AddLabel(player, becrate)
                                : TextCommandResult.Error(Constants.NoLabel),
        };
    }

    public TextCommandResult RemoveLabel(IServerPlayer player, BlockEntityCrate bect)
    {
        if (!player.InventoryManager.TryGiveItemstack(LabelStack.Clone(), true))
        {
            bect.Api.World.SpawnItemEntity(LabelStack.Clone(), player.Entity.ServerPos.XYZ);
        }

        bect.label = null;
        bect.MarkDirty(redrawOnClient: true);
        return TextCommandResult.Deferred;
    }

    public static TextCommandResult AddLabel(IServerPlayer player, BlockEntityCrate bect)
    {
        player.Entity.ActiveHandItemSlot.TakeOut(1);
        player.Entity.ActiveHandItemSlot.MarkDirty();

        bect.label = Constants.DefaultLabel;
        bect.MarkDirty(redrawOnClient: true);
        return TextCommandResult.Deferred;
    }
}
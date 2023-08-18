using Vintagestory.API.Common;
using Vintagestory.API.Server;
using Vintagestory.GameContent;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common.CommandAbbr;
using static DanaTweaks.Constants;

namespace DanaTweaks;

public class Commands : ModSystem
{
    private ItemStack LabelStack { get; set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override void StartServerSide(ICoreServerAPI api)
    {
        LabelStack = new(api.World.GetItem(new AssetLocation("paper-parchment")));

        api.ChatCommands.GetOrCreate("danatweaks")
            .WithAlias("crtw")
            .RequiresPlayer()
            .RequiresPrivilege("useblock")
            .BeginSub("removeoraddlabel")
                .WithAlias("ral")
                .WithDesc(RemoveOrAddLabelName)
                .HandleWith(RemoveOrAddLabel)
            .EndSub()
            .BeginSub("openclose")
                .WithAlias("oc")
                .WithDesc(OpenCloseLidName)
                .HandleWith(OpenCloseLid)
            .EndSub();
    }

    private TextCommandResult OpenCloseLid(TextCommandCallingArgs args)
    {
        IServerPlayer player = args.Caller.Player as IServerPlayer;
        BlockPos pos = player?.CurrentBlockSelection?.Position;

        if (pos == null || player.Entity.World.BlockAccessor.GetBlockEntityExt<BlockEntityCrate>(pos) is not BlockEntityCrate becrate)
        {
            return TextCommandResult.Error(NoCrate);
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
            return TextCommandResult.Error(NoCrate);
        }

        ItemSlot activeSlot = player.InventoryManager.ActiveHotbarSlot;

        BlockEntityCrate becrate = player.Entity.World.BlockAccessor.GetBlockEntityExt<BlockEntityCrate>(pos);

        if (becrate == null)
        {
            return TextCommandResult.Error(NoCrate);
        }

        return becrate.label switch
        {
            DefaultLabel => RemoveLabel(player, becrate),
            not null and not "" => TextCommandResult.Error(HasDiffLabel),
            _ => activeSlot.IsCorrectLabel(LabelStack)
                                ? AddLabel(player, becrate)
                                : TextCommandResult.Error(NoLabel),
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

        bect.label = "paper-empty";
        bect.MarkDirty(redrawOnClient: true);
        return TextCommandResult.Deferred;
    }
}
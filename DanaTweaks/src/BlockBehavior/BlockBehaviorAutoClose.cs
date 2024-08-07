using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorAutoClose : BlockBehavior
{
    public BlockBehaviorAutoClose(Block block) : base(block) { }

    public static int GetDelay(Block block)
    {
        return Core.ConfigServer.AutoCloseDelays.ContainsKey(block.Code.ToString())
            ? Core.ConfigServer.AutoCloseDelays[block.Code.ToString()]
            : Core.ConfigServer.AutoCloseDefaultDelay;
    }

    public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
    {
        if (byPlayer?.Entity?.Controls.CtrlKey == true)
        {
            return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
        }
        handling = EnumHandling.PassThrough;
        world.RegisterCallbackUnique((world, pos, time) => TryAutoClose(world, blockSel, time), blockSel?.Position, GetDelay(block));
        return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
    }

    public override void Activate(IWorldAccessor world, Caller caller, BlockSelection blockSel, ITreeAttribute activationArgs, ref EnumHandling handling)
    {
        handling = EnumHandling.PassThrough;
        if (activationArgs?.GetAsBool("isAutoClose") == true)
        {
            return;
        }
        if (caller?.Player?.Entity?.Controls.CtrlKey == true)
        {
            return;
        }

        int delay = GetDelay(block);
        if (delay > 0)
        {
            world.RegisterCallbackUnique((world, pos, time) => TryAutoClose(world, blockSel, time), blockSel?.Position, delay);
        }
    }

    private void TryAutoClose(IWorldAccessor world, BlockSelection blockSel, float time)
    {
        Caller caller = new Caller()
        {
            Player = world.NearestPlayer(blockSel.Position.X, blockSel.Position.Y, blockSel.Position.Z)
        };

        TreeAttribute activationArgs = new();
        activationArgs.SetBool("opened", false);
        activationArgs.SetBool("isAutoClose", true);

        BEBehaviorDoor behavior = world.BlockAccessor.GetBlockEntity(blockSel?.Position)?.GetBehavior<BEBehaviorDoor>();
        if (behavior != null && behavior.Opened == true)
        {
            block.Activate(world, caller, blockSel, activationArgs);
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorAutoClose : BlockBehavior
{
    public BlockBehaviorAutoClose(Block block) : base(block) { }

    public static int GetDelay(Block block)
    {
        if (block == null)
        {
            return -1;
        }

        KeyValuePair<string, int> keyAndDelay = Core.ConfigServer.AutoCloseDelays.FirstOrDefault(x => WildcardUtil.Match(AssetLocation.Create(x.Key), block.Code));

        if (string.IsNullOrEmpty(keyAndDelay.Key))
        {
            return -1;
        }

        return keyAndDelay.Value;
    }

    public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
    {
        if (byPlayer?.Entity?.Controls.CtrlKey == true)
        {
            return base.OnBlockInteractStart(world, byPlayer, blockSel, ref handling);
        }

        Caller caller = new Caller()
        {
            Player = byPlayer
        };

        handling = EnumHandling.PassThrough;

        int delay = GetDelay(block);
        if (delay > 0)
        {
            world.RegisterCallbackUnique((world, pos, time) => TryAutoClose(world, caller, blockSel, time), blockSel?.Position, delay);
        }
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
            world.RegisterCallbackUnique((world, pos, time) => TryAutoClose(world, caller, blockSel, time), blockSel?.Position, delay);
        }
    }

    private void TryAutoClose(IWorldAccessor world, Caller caller, BlockSelection blockSel, float time)
    {
        TreeAttribute activationArgs = new();
        activationArgs.SetBool("opened", false);
        activationArgs.SetBool("isAutoClose", true);

        Block _block = world.BlockAccessor.GetBlock(blockSel.Position);
        if (_block is BlockBaseDoor blockBaseDoor && blockBaseDoor.IsOpened())
        {
            blockBaseDoor.CallMethod("Open", world, null, blockSel.Position);
            world.PlaySoundAt(AssetLocation.Create(_block.Attributes["triggerSound"].AsString("sounds/block/door"), _block.Code.Domain), blockSel.Position, 0.0);
            if (_block.FirstCodePart() != "roughhewnfencegate")
            {
                blockBaseDoor.CallMethod("TryOpenConnectedDoor", world, null, blockSel.Position);
            }
        }
        else if (IsOpened(world, blockSel))
        {
            try { block.Activate(world, caller, blockSel, activationArgs); } catch { }
        }
    }

    private bool IsOpened(IWorldAccessor world, BlockSelection blockSel)
    {
        BlockEntity blockEntity = world.BlockAccessor.GetBlockEntity(blockSel?.Position);
        if (blockEntity?.GetBehavior<BEBehaviorDoor>() is BEBehaviorDoor door)
        {
            return door.Opened;
        }
        if (blockEntity?.GetBehavior<BEBehaviorTrapDoor>() is BEBehaviorTrapDoor trapdoor)
        {
            return trapdoor.Opened;
        }
        return true;
    }
}

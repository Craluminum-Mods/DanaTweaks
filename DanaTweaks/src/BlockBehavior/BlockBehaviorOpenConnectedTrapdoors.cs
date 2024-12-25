using System;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorOpenConnectedTrapdoors : BlockBehavior
{
    public override bool ClientSideOptional => true;

    public BlockBehaviorOpenConnectedTrapdoors(Block block) : base(block) { }

    public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
    {
        BlockPos originalPos = blockSel.Position;
        BEBehaviorTrapDoor originalTrapdoor = GetTrapdoorAtPos(world, originalPos);
        if (byPlayer == null || originalTrapdoor == null)
        {
            return false;
        }

        BlockPos oppositePos = originalPos.AddCopy(originalTrapdoor.facingWhenOpened.Opposite);
        BEBehaviorTrapDoor oppositeTrapDoor = GetTrapdoorAtPos(world, oppositePos);

        bool openOpposite = oppositeTrapDoor != null && originalTrapdoor.facingWhenClosed == oppositeTrapDoor.facingWhenClosed;
        if (openOpposite)
        {
            Open(world, byPlayer, oppositePos, blockSel.Face, oppositeTrapDoor);
        }

        bool openConnected = !byPlayer.Entity.ServerControls.Sneak;
        if (openConnected)
        {
            OpenConnected(originalTrapdoor, oppositeTrapDoor, world, byPlayer, blockSel.Face);
        }
        return true;
    }

    private static BEBehaviorTrapDoor GetTrapdoorAtPos(IWorldAccessor world, BlockPos pos)
    {
        return world.BlockAccessor.GetBlockEntity(pos)?.GetBehavior<BEBehaviorTrapDoor>();
    }

    private static void Open(IWorldAccessor world, IPlayer byPlayer, BlockPos pos, BlockFacing facing, BEBehaviorTrapDoor trapdoor)
    {
        Caller caller = new Caller() { Player = byPlayer };
        BlockSelection _blockSel = new BlockSelection(pos, facing, trapdoor.Block);
        TreeAttribute tree = new TreeAttribute();
        tree.SetBool("opened", true);
        trapdoor.Block.Activate(world, caller, _blockSel, tree);
    }

    private static void OpenConnected(BEBehaviorTrapDoor originalTrapdoor, BEBehaviorTrapDoor oppositeTrapdoor, IWorldAccessor world, IPlayer player, BlockFacing facing)
    {
        EnumAxis axis = originalTrapdoor.facingWhenOpened.Axis switch
        {
            EnumAxis.X when originalTrapdoor.facingWhenClosed.Axis == EnumAxis.Y => EnumAxis.Z,
            EnumAxis.Z when originalTrapdoor.facingWhenClosed.Axis == EnumAxis.Y => EnumAxis.X,
            _ => EnumAxis.Y,
        };

        OpenConnectedRow(originalTrapdoor, world, player, facing, axis);
        OpenConnectedRow(originalTrapdoor, world, player, facing, axis, negative: true);

        OpenConnectedRow(oppositeTrapdoor, world, player, facing, axis);
        OpenConnectedRow(oppositeTrapdoor, world, player, facing, axis, negative: true);
    }

    private static void OpenConnectedRow(BEBehaviorTrapDoor originalTrapdoor, IWorldAccessor world, IPlayer player, BlockFacing facing, EnumAxis axis = EnumAxis.Y, bool negative = false)
    {
        if (originalTrapdoor == null) return;

        int currentAdditive = negative ? -1 : 1;
        bool moveNext = true;
        while (SatisfiesDistance(GetCoordinateByAxis(originalTrapdoor.Pos, axis), GetCoordinateByAxis(AddPosCopy(originalTrapdoor, currentAdditive, axis), axis)) && moveNext)
        {
            BlockPos _pos = AddPosCopy(originalTrapdoor, currentAdditive, axis);
            BEBehaviorTrapDoor _trapdoor = GetTrapdoorAtPos(world, _pos);
            if (_trapdoor != null && originalTrapdoor.facingWhenClosed == _trapdoor.facingWhenClosed)
            {
                Open(world, player, _pos, facing, _trapdoor);
                _ = negative ? currentAdditive-- : currentAdditive++;
                continue;
            }
            moveNext = false;
        }
    }

    private static BlockPos AddPosCopy(BEBehaviorTrapDoor originalTrapdoor, int currentAdditive, EnumAxis axis) => axis switch
    {
        EnumAxis.X => originalTrapdoor.Pos.AddCopy(currentAdditive, 0, 0),
        EnumAxis.Y => originalTrapdoor.Pos.AddCopy(0, currentAdditive, 0),
        EnumAxis.Z => originalTrapdoor.Pos.AddCopy(0, 0, currentAdditive),
        _ => originalTrapdoor.Pos.AddCopy(0, currentAdditive, 0),
    };

    private static int GetCoordinateByAxis(BlockPos pos, EnumAxis axis) => axis switch
    {
        EnumAxis.X => pos.X,
        EnumAxis.Y => pos.Y,
        EnumAxis.Z => pos.Z,
        _ => pos.Y,
    };

    private static bool SatisfiesDistance(int originalCoordinate, int currentCoordinate)
    {
        return Math.Abs(originalCoordinate - currentCoordinate) <= Core.ConfigServer.OpenConnectedTrapdoorsMaxBlocksDistance;
    }
}
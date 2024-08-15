using System;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorOpenConnectedTrapdoors : BlockBehavior
{
    public string State => block.Variant["state"];
    public string V => block.Variant["v"];
    public string Rot => block.Variant["rot"];

    public override bool ClientSideOptional => true;

    public BlockBehaviorOpenConnectedTrapdoors(Block block) : base(block) { }

    public override bool OnBlockInteractStart(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling)
    {
        if (byPlayer == null) return false;

        BlockPos originalPos = blockSel.Position;
        BlockPos sidePos = Rot switch
        {
            "north" => originalPos.SouthCopy(),
            "east" => originalPos.WestCopy(),
            "south" => originalPos.NorthCopy(),
            "west" => originalPos.EastCopy(),
            _ => originalPos
        };

        if (!originalPos.Equals(sidePos)) HandleSideBlock(world, byPlayer, blockSel, ref handling, sidePos);

        if (byPlayer.Entity.ServerControls.Sneak) return true;

        HandleVerticals(world, byPlayer, blockSel, ref handling, originalPos, V, Rot, EnumAxis.Y);
        HandleVerticals(world, byPlayer, blockSel, ref handling, sidePos, V, Rot, EnumAxis.Y);

        if (V is "up" or "down")
        {
            HandleHorizontalz(world, byPlayer, blockSel, ref handling, originalPos, Rot);
            HandleHorizontalz(world, byPlayer, blockSel, ref handling, sidePos, Rot);
        }
        return true;
    }

    private void HandleVerticals(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, BlockPos pos, string V, string Rot, EnumAxis axis)
    {
        HandleDirection(world, byPlayer, blockSel, ref handling, pos, axis, 1);
        HandleDirection(world, byPlayer, blockSel, ref handling, pos, axis, -1);
    }

    private void HandleHorizontalz(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, BlockPos pos, string rot)
    {
        EnumAxis axis = (rot is "east" or "west") ? EnumAxis.Z : EnumAxis.X;
        HandleDirection(world, byPlayer, blockSel, ref handling, pos, axis, 1);
        HandleDirection(world, byPlayer, blockSel, ref handling, pos, axis, -1);
    }

    private bool HandleSideBlock(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, BlockPos sidePos)
    {
        Block sideBlock = world.BlockAccessor.GetBlock(sidePos);
        if (sideBlock?.Variant["v"] == V && sideBlock?.Variant["state"] == State)
        {
            sideBlock?.GetBehavior<BlockBehaviorExchangeOnInteract>()?.OnBlockInteractStart(world, byPlayer, GetBlockSelection(blockSel, sidePos, sideBlock), ref handling);
            return true;
        }
        return false;
    }

    private void HandleDirection(IWorldAccessor world, IPlayer byPlayer, BlockSelection blockSel, ref EnumHandling handling, BlockPos pos, EnumAxis axis, int direction)
    {
        int coordinate = axis switch
        {
            EnumAxis.X => pos.X,
            EnumAxis.Y => pos.Y,
            EnumAxis.Z => pos.Z
        };

        int original = coordinate;
        Block block;
        while (Math.Abs(original - coordinate) < Core.ConfigServer.OpenConnectedTrapdoorsMaxBlocksDistance && (block = GetNewBlock(world, pos, coordinate += direction, axis))?.GetBlockBehavior<BlockBehaviorOpenConnectedTrapdoors>() != null)
        {
            if ((block?.Variant["state"]) != State)
            {
                continue;
            }

            BlockPos newPos = axis switch
            {
                EnumAxis.X => new BlockPos(coordinate, pos.Y, pos.Z, pos.dimension),
                EnumAxis.Y => new BlockPos(pos.X, coordinate, pos.Z, pos.dimension),
                EnumAxis.Z => new BlockPos(pos.X, pos.Y, coordinate, pos.dimension),
            };

            block.GetBehavior<BlockBehaviorExchangeOnInteract>()?.OnBlockInteractStart(world, byPlayer, GetBlockSelection(blockSel, newPos, block), ref handling);
        }
    }

    private Block GetNewBlock(IWorldAccessor world, BlockPos pos, int coordinate, EnumAxis axis) => axis switch
    {
        EnumAxis.X => world.BlockAccessor.GetBlock(new BlockPos(coordinate, pos.Y, pos.Z, pos.dimension)),
        EnumAxis.Y => world.BlockAccessor.GetBlock(new BlockPos(pos.X, coordinate, pos.Z, pos.dimension)),
        EnumAxis.Z => world.BlockAccessor.GetBlock(new BlockPos(pos.X, pos.Y, coordinate, pos.dimension))
    };

    private BlockSelection GetBlockSelection(BlockSelection old, BlockPos pos, Block nextBlock)
    {
        BlockSelection newSelection = old.Clone();
        newSelection.Block = nextBlock;
        newSelection.Position = pos;
        return newSelection;
    }
}
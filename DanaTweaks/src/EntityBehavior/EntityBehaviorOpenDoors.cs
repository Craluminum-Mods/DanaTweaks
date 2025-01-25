using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class EntityBehaviorOpenDoors : EntityBehavior
{
    private int cooldown;
    private int range;

    private float secondsSinceLastUpdate;

    public EntityBehaviorOpenDoors(Entity entity) : base(entity) { }

    public override void Initialize(EntityProperties properties, JsonObject attributes)
    {
        cooldown = attributes["cooldown"].AsInt(1);
        range = attributes["range"].AsInt(1);
    }

    public override void OnGameTick(float deltaTime)
    {
        secondsSinceLastUpdate += deltaTime;
        if (secondsSinceLastUpdate < cooldown) return;
        secondsSinceLastUpdate = 0;

        if (entity.Alive)
        {
            TryOpenDoors();
        }
    }

    private void TryOpenDoors()
    {
        entity.World.BlockAccessor.WalkBlocks(entity.SidedPos.AsBlockPos.AddCopy(-range, -range, -range), entity.SidedPos.AsBlockPos.AddCopy(range, range, range), (block, x, y, z) =>
        {
            BlockPos pos = new(x, y, z, entity.SidedPos.Dimension);
            TryOpen(pos);
        });
    }

    private bool TryOpen(BlockPos pos)
    {
        Block block = entity.World.BlockAccessor.GetBlock(pos);

        if (IsLocked(pos))
        {
            return false;
        }

        Caller caller = new() { Type = EnumCallerType.Entity, Entity = entity, };
        BlockSelection blockSelection = new(pos, BlockFacing.DOWN, block);
        TreeAttribute activationArgs = new();
        activationArgs.SetBool("opened", true);
        block.Activate(entity.World, caller, blockSelection, activationArgs);
        return true;
    }

    private bool IsLocked(BlockPos pos)
    {
        ModSystemBlockReinforcement blockReinforcementSys = entity.Api.ModLoader.GetModSystem<ModSystemBlockReinforcement>();

        if (entity.Api.World.BlockAccessor.GetBlock(pos) is BlockMultiblock multiblock)
        {
            BlockPos multiblockPos = new BlockPos(pos.X + multiblock.OffsetInv.X, pos.Y + multiblock.OffsetInv.Y, pos.Z + multiblock.OffsetInv.Z, pos.dimension);
            return blockReinforcementSys.GetReinforcment(multiblockPos)?.Locked == true;
        }
        return blockReinforcementSys.GetReinforcment(pos)?.Locked == true;
    }

    public override string PropertyName() => "danatweaks:opendoors";
}
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.API.Common.Entities;

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

        TryOpenDoors();
    }

    private void TryOpenDoors()
    {
        entity.World.BlockAccessor.WalkBlocks(entity.SidedPos.AsBlockPos.AddCopy(-range, -range, -range), entity.SidedPos.AsBlockPos.AddCopy(range, range, range), (block, x, y, z) =>
        {
            BlockPos blockPos = new BlockPos(x, y, z, entity.SidedPos.Dimension);
            TryOpen(blockPos);
        });
    }

    private bool TryOpen(BlockPos blockPos)
    {
        Caller caller = new Caller() { Type = EnumCallerType.Entity, Entity = entity, };
        Block block = entity.World.BlockAccessor.GetBlock(blockPos);
        BlockSelection blockSelection = new BlockSelection(blockPos, BlockFacing.DOWN, block);
        TreeAttribute activationArgs = new TreeAttribute();
        activationArgs.SetBool("opened", true);
        block.Activate(entity.World, caller, blockSelection, activationArgs);
        return true;
    }

    public override string PropertyName() => "danatweaks:opendoors";
}
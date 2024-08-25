using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;
using static DanaTweaks.Constants;

namespace DanaTweaks;

public class EntityBehaviorAutoPlantDroppedTreeSeeds : EntityBehavior
{
    public EntityBehaviorAutoPlantDroppedTreeSeeds(Entity entity) : base(entity) { }

    public override void OnGameTick(float deltaTime)
    {
        if (Core.ConfigServer?.AutoPlantDroppedTreeSeeds == false || entity is not EntityItem entityItem || entity.Api.Side.IsClient())
        {
            return;
        }

        if (entityItem.Itemstack?.Collectible is not ItemTreeSeed item
            || entityItem.World.ElapsedMilliseconds - entityItem.itemSpawnedMilliseconds < Core.ConfigServer.AutoPlantDroppedTreeSeedsDelay)
        {
            return;
        }

        BlockPos pos = entityItem.ServerPos.AsBlockPos;
        Block _block = entityItem.World.BlockAccessor.GetBlock(pos);
        BlockSelection blockSelection = new BlockSelection(pos, BlockFacing.DOWN, _block);
        if (_block == null)
        {
            return;
        }

        string treetype = item.Variant["type"];
        Block saplBlock = entityItem.World.GetBlock(AssetLocation.Create("sapling-" + treetype + "-free", item.Code.Domain));
        if (saplBlock is not BlockSapling blockSapling)
        {
            return;
        }

        string failureCode = "";
        if (!saplBlock.TryPlaceBlock(entityItem.World, null, entityItem.Itemstack, blockSelection, ref failureCode))
        {
            return;
        }

        entityItem.Slot.TakeOut(1);
        entityItem.Slot.MarkDirty();
        entityItem.World.BlockAccessor.GetBlockEntity(pos)?.MarkDirty(true);

        if (entityItem.Slot.StackSize == 0)
        {
            entityItem.Itemstack = null;
            entityItem.WatchedAttributes.MarkPathDirty("itemstack");
            entityItem.Die();
        }

        entityItem.World.PlaySoundAt(AssetLocation.Create(ModdedSounds.PlaceDirt), (float)blockSelection.Position.X + 0.5f, blockSelection.Position.Y, (float)blockSelection.Position.Z + 0.5f);
    }

    public override string PropertyName() => "danatweaks:autoPlantDroppedTreeSeeds";
}
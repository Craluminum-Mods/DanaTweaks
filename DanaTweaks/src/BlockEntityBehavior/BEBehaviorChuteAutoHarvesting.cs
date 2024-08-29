using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BEBehaviorChuteAutoHarvesting : BlockEntityBehavior
{
    public BEBehaviorChuteAutoHarvesting(BlockEntity blockentity) : base(blockentity) { }

    public override void Initialize(ICoreAPI api, JsonObject properties)
    {
        // try catch is for strange behavior on servers, I have absolutely no idea what causes that
        try
        {
            base.Initialize(api, properties);
            Blockentity.RegisterGameTickListener(TryAutoHarvest, Core.ConfigServer.ChuteAutoHarvestingUpdateMilliseconds);
        }
        catch (System.Exception) { }
    }

    public void TryAutoHarvest(float dt)
    {
        if (!Api.Side.IsServer() || Blockentity is not BlockEntityItemFlow beitemflow)
        {
            return;
        }

        foreach (BlockFacing face in BlockFacing.ALLFACES.Where(face => beitemflow.AcceptFromFaces.Contains(face)))
        {
            BlockPos _nextPos = Pos.AddCopy(face);
            Block _blockAtPos = Api.World.BlockAccessor.GetBlock(_nextPos);
            BlockBehaviorHarvestable behavior = _blockAtPos?.GetBehavior<BlockBehaviorHarvestable>();
            if (behavior == null)
            {
                continue;
            }
            ItemSlot sourceSlot = beitemflow.Inventory.FirstOrDefault((ItemSlot slot) => slot.Empty);
            if (sourceSlot == null)
            {
                continue;
            }
            DummySlot dummySlot = new();
            dummySlot.Itemstack = _blockAtPos.GetDrops(Api.World, _nextPos, null)?.First(x => x.Collectible.Code.Equals(behavior.harvestedStack.Code));
            if (dummySlot.Itemstack == null)
            {
                continue;
            }
            dummySlot.TryPutInto(Api.World, sourceSlot);
            Block toBlock = behavior.GetField<Block>("harvestedBlock");
            if (toBlock == null)
            {
                continue;
            }
            Api.World.BlockAccessor.SetBlock(toBlock.BlockId, _nextPos);
        }
    }
}
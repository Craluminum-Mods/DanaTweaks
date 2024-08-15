using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class AutoPanning : ModSystem
{
    private long autoPanningTickTime;
    public int SearchRange { get; set; } = 5;
    public bool Enabled { get; set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI capi)
    {
        capi.Input.RegisterHotKey("danatweaks:autopanning", Lang.Get("danatweaks:AutoPanning.Toggle"), GlKeys.X, HotkeyType.CharacterControls, ctrlPressed: true);
        capi.Input.SetHotKeyHandler("danatweaks:autopanning", x => Toggle(x, capi));
    }

    private void OnGameTick(float dt, ICoreClientAPI capi)
    {
        if (!Enabled)
        {
            return;
        }

        ItemSlot activeSlot = capi.World.Player.InventoryManager.ActiveHotbarSlot;

        if (activeSlot.Itemstack?.Collectible is not BlockPan blockPan || TryPan(capi, activeSlot))
        {
            return;
        }

        PanNearestPannableBlock(capi, blockPan);
    }

    private void PanNearestPannableBlock(ICoreClientAPI capi, BlockPan blockPan)
    {
        BlockPos playerPos = capi.World.Player.Entity.Pos.AsBlockPos;
        double nearestDist = double.MaxValue;
        BlockPos nearestBlockPos = null;

        capi.World.BlockAccessor.WalkBlocks(playerPos.AddCopy(-SearchRange, -SearchRange, -SearchRange), playerPos.AddCopy(SearchRange, SearchRange, SearchRange), (block, x, y, z) =>
        {
            BlockPos blockPos = new BlockPos(x, y, z, playerPos.dimension);
            if (blockPan.IsPannableMaterial(block) && IsInRangeOfBlock(capi, blockPos, ref nearestDist))
            {
                nearestBlockPos = blockPos;
            }
        });

        if (nearestBlockPos != null)
        {
            Execute(capi, nearestBlockPos);
        }
    }

    public bool IsInRangeOfBlock(ICoreClientAPI capi, BlockPos pos, ref double nearestDist)
    {
        Block block = capi.World.BlockAccessor.GetBlock(pos);
        Cuboidf[] boxes = block.GetSelectionBoxes(capi.World.BlockAccessor, pos);

        if (boxes == null) return false;

        Vec3d playerEye = capi.World.Player.Entity.Pos.XYZ.Add(capi.World.Player.Entity.LocalEyePos);
        double pickingRange = capi.World.Player.WorldData.PickingRange + 0.5;

        foreach (Cuboidf box in boxes)
        {
            double dist = box.ToDouble().Translate(pos.X, pos.Y, pos.Z).ShortestDistanceFrom(playerEye);

            if (dist <= pickingRange && dist < nearestDist)
            {
                nearestDist = dist;
                return true;
            }
        }

        return false;
    }

    private void Execute(ICoreClientAPI capi, BlockPos blockPos)
    {
        Block block = capi.World.BlockAccessor.GetBlock(blockPos);
        BlockSelection blockSel = new BlockSelection(blockPos, BlockFacing.DOWN, block);
        (capi.World as ClientMain)?.SendHandInteraction(
            2, blockSel, null, EnumHandInteract.HeldItemInteract,
            Vintagestory.Common.EnumHandInteractNw.StartHeldItemUse, true
        );
    }

    private static bool TryPan(ICoreClientAPI capi, ItemSlot slot)
    {
        if (slot.Itemstack.Attributes.GetAsString("materialBlockCode") != null)
        {
            capi.Input.InWorldMouseButton.Right = true;
            return true;
        }
        capi.Input.InWorldMouseButton.Right = false;
        return false;
    }

    private bool Toggle(KeyCombination t1, ICoreClientAPI capi)
    {
        Enabled = !Enabled;
        capi.TriggerChatMessage(Lang.Get(Enabled ? "danatweaks:AutoPanning.Enabled" : "danatweaks:AutoPanning.Disabled"));

        if (Enabled)
        {
            autoPanningTickTime = capi.Event.RegisterGameTickListener(dt => OnGameTick(dt, capi), 1000);
        }
        else
        {
            capi.Event.UnregisterGameTickListener(autoPanningTickTime);
        }

        return true;
    }
}

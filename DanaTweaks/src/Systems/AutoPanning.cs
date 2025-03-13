using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
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
        if (!Enabled) return;

        ClientMain clientMain = capi.World as ClientMain;
        EntityPlayer entityPlayer = capi.World.Player.Entity;
        ItemSlot activeSlot = capi.World.Player.InventoryManager.ActiveHotbarSlot;
        BlockPos playerPos = entityPlayer.Pos.AsBlockPos;

        if (activeSlot.Itemstack?.Collectible is not BlockPan blockPan) return;

        if (TryPan(capi, activeSlot)) return;

        capi.World.BlockAccessor.WalkBlocks(playerPos.AddCopy(-SearchRange, -SearchRange, -SearchRange), playerPos.AddCopy(SearchRange, SearchRange, SearchRange), (block, x, y, z) =>
        {
            BlockPos blockPos = new BlockPos(x, y, z, playerPos.dimension);
            if (blockPan.IsPannableMaterial(block))
            {
                BlockSelection blockSel = new BlockSelection(blockPos, BlockFacing.DOWN, block);
                clientMain.SendHandInteraction(2, blockSel, null, EnumHandInteract.HeldItemInteract, EnumHandInteractNw.StartHeldItemUse, true);
            }
        }, centerOrder: true);
    }

    private bool TryPan(ICoreClientAPI capi, ItemSlot slot)
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
            capi.Input.InWorldMouseButton.Right = false;
        }

        return true;
    }
}

using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.CreativeMiddleClickEntity))]
public static class SystemMouseInWorldInteractions_HandleMouseInteractionsNoBlockSelected_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(SystemMouseInWorldInteractions), "HandleMouseInteractionsNoBlockSelected")]
    public static void Postfix(SystemMouseInWorldInteractions __instance, ClientMain ___game)
    {
        if (___game.InWorldMouseState.Middle && ___game.Player.WorldData.CurrentGameMode == EnumGameMode.Creative && ___game.EntitySelection != null)
        {
            OnEntityPick(___game);
            return;
        }
    }

    private static void OnEntityPick(ClientMain game)
    {
        if (game.Player.WorldData.CurrentGameMode != EnumGameMode.Creative)
        {
            return;
        }

        string entityCode = game.EntitySelection.Entity.Code.Domain + ":creature-" + game.EntitySelection.Entity.Code.Path;
        ItemStack entityStack = new ItemStack(game.EntitySelection.Entity.World.GetItem(new AssetLocation(entityCode)));

        if (entityStack == null)
        {
            return;
        }

        IInventory hotbarInv = game.Player.InventoryManager.GetHotbarInventory();
        ItemSlot selectedHotbarSlot = game.Player.InventoryManager.ActiveHotbarSlot;

        selectedHotbarSlot.Itemstack = entityStack;
        selectedHotbarSlot.MarkDirty();
        game.SendPacketClient(new Packet_Client
        {
            Id = 10,
            CreateItemstack = new Packet_CreateItemstack
            {
                Itemstack = StackConverter.ToPacket(entityStack),
                TargetInventoryId = selectedHotbarSlot.Inventory.InventoryID,
                TargetSlot = game.Player.InventoryManager.ActiveHotbarSlotNumber,
                TargetLastChanged = ((InventoryBase)hotbarInv).lastChangedSinceServerStart
            }
        });
    }
}
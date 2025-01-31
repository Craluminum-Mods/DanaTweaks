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
        string entityCode = game.EntitySelection.Entity.Code.Domain + ":creature-" + game.EntitySelection.Entity.Code.Path;

        Item item = game.EntitySelection.Entity.World.GetItem(entityCode);
        if (item == null)
        {
            return;
        }

        ItemStack entityStack = new ItemStack(item);

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
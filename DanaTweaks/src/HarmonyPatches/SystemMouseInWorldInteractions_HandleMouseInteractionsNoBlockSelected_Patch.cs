using Vintagestory.API.Common;
using Vintagestory.API.Client;
using Vintagestory.Client.NoObf;
using Vintagestory.Common;
using System.Reflection;
using HarmonyLib;

namespace DanaTweaks;

public static class SystemMouseInWorldInteractions_HandleMouseInteractionsNoBlockSelected_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(SystemMouseInWorldInteractions).GetMethod("HandleMouseInteractionsNoBlockSelected", AccessTools.all);
    }

    public static MethodInfo GetPostfix() => typeof(SystemMouseInWorldInteractions_HandleMouseInteractionsNoBlockSelected_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(SystemMouseInWorldInteractions __instance)
    {
        ClientMain game = __instance.GetField<ClientMain>("game");
        MouseButtonState mouseState = game.GetField<MouseButtonState>("InWorldMouseState");
        ClientPlayer player = game.GetField<ClientPlayer>("player");
        EntitySelection entitySelection = game.GetProperty<EntitySelection>("EntitySelection");

        if (mouseState.Middle && player.WorldData.CurrentGameMode == EnumGameMode.Creative && entitySelection != null)
        {
            OnEntityPick(entitySelection, game, player);
            return;
        }
    }

    private static void OnEntityPick(EntitySelection entitySel, ClientMain game, ClientPlayer player)
    {
        if (player.WorldData.CurrentGameMode != EnumGameMode.Creative)
        {
            return;
        }

        string entityCode = entitySel.Entity.Code.Domain + ":creature-" + entitySel.Entity.Code.Path;
        ItemStack entityStack = new ItemStack(entitySel.Entity.World.GetItem(new AssetLocation(entityCode)));

        if (entityStack == null)
        {
            return;
        }

        IInventory hotbarInv = player.InventoryManager.GetHotbarInventory();
        ItemSlot selectedHotbarSlot = player.InventoryManager.ActiveHotbarSlot;

        selectedHotbarSlot.Itemstack = entityStack;
        selectedHotbarSlot.MarkDirty();
        game.SendPacketClient(new Packet_Client
        {
            Id = 10,
            CreateItemstack = new Packet_CreateItemstack
            {
                Itemstack = StackConverter.ToPacket(entityStack),
                TargetInventoryId = selectedHotbarSlot.Inventory.InventoryID,
                TargetSlot = player.InventoryManager.ActiveHotbarSlotNumber,
                TargetLastChanged = ((InventoryBase)hotbarInv).lastChangedSinceServerStart
            }
        });
    }
}
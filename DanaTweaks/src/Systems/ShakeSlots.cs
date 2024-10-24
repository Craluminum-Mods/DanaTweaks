using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Common;

namespace DanaTweaks;

public class ShakeSlots : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide.IsClient();

    public override void StartClientSide(ICoreClientAPI api)
    {
        api.Event.RegisterGameTickListener((deltaTime) => TryShakeSlots(deltaTime, api), 1000);
    }

    public static void TryShakeSlots(float deltaTime, ICoreClientAPI api)
    {
        IPlayer player = api?.World?.Player;
        if (player == null) return;

        IPlayerInventoryManager inventoryManager = player.InventoryManager;
        ItemSlot curSlot = inventoryManager.CurrentHoveredSlot;
        if (curSlot == null) return;

        switch (curSlot)
        {
            case ItemSlotBagContent slotInBackpack:
                {
                    if (Core.ConfigClient?.ShakeSlotsWithBags == false) return;

                    InventoryPlayerBackPacks backpackInventory = inventoryManager.GetOwnInventory(GlobalConstants.backpackInvClassName) as InventoryPlayerBackPacks;
                    if (backpackInventory?.HasOpened(player) == true)
                    {
                        backpackInventory.PerformNotifySlot(slotInBackpack.BagIndex);
                    }
                    break;
                }
            case ItemSlotBackpack slotBackpack when !curSlot.Empty:
                {
                    if (Core.ConfigClient?.ShakeSlotsInsideBags == false) return;

                    InventoryPlayerBackPacks backpackInventory = inventoryManager.GetOwnInventory(GlobalConstants.backpackInvClassName) as InventoryPlayerBackPacks;
                    if (backpackInventory?.HasOpened(player) == true)
                    {
                        foreach (ItemSlotBagContent slot in backpackInventory.Where(x => x is ItemSlotBagContent).Select(x => x as ItemSlotBagContent))
                        {
                            if (slot.BagIndex == backpackInventory.GetSlotId(slotBackpack))
                            {
                                backpackInventory.PerformNotifySlot(backpackInventory.GetSlotId(slot));
                            }
                        }
                    }
                    break;
                }
            case ItemSlotCharacter slotCharacter when player.InventoryManager.MouseItemSlot.Empty:
                {
                    if (Core.ConfigClient?.ShakeSlotsWithSuitableClothes == false) return;

                    InventoryPlayerBackPacks backpackInventory = inventoryManager.GetOwnInventory(GlobalConstants.backpackInvClassName) as InventoryPlayerBackPacks;
                    if (backpackInventory?.HasOpened(player) == true)
                    {
                        foreach (ItemSlotBagContent slot in backpackInventory.Where(x => x is ItemSlotBagContent).Select(x => x as ItemSlotBagContent))
                        {
                            if (slotCharacter.CanHold(slot))
                            {
                                backpackInventory.PerformNotifySlot(backpackInventory.GetSlotId(slot));
                            }
                        }
                    }

                    InventoryBasePlayer hotbarInventory = inventoryManager.GetOwnInventory(GlobalConstants.hotBarInvClassName) as InventoryBasePlayer;
                    if (hotbarInventory?.HasOpened(player) == true)
                    {
                        foreach (ItemSlot slot in hotbarInventory.Where(x => x is ItemSlot).Select(x => x as ItemSlot))
                        {
                            if (slotCharacter.CanHold(slot))
                            {
                                hotbarInventory.PerformNotifySlot(hotbarInventory.GetSlotId(slot));
                            }
                        }
                    }
                    break;
                }
        }
    }
}
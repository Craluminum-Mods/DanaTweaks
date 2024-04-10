using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.Client.NoObf;

namespace DanaTweaks;

public static class CollectibleObject_OnHeldUseStart_Patch
{
    public static bool Prefix(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EnumHandInteract useType)
    {
        CollectibleObject heldCollectible = slot?.Itemstack?.Collectible;

        if (useType != EnumHandInteract.HeldItemAttack)
        {
            return true;
        }

        if (byEntity is not EntityPlayer entityPlayer || !byEntity.Api.Side.IsClient() || blockSel == null)
        {
            return true;
        }

        if (Core.Config.AlwaysSwitchToBestToolIgnoredTools?.Any(x => x == heldCollectible?.Tool) == true)
        {
            return true;
        }

        EnumBlockMaterial targetMaterial = blockSel.Block.GetBlockMaterial(byEntity.World.BlockAccessor, blockSel.Position, null);

        ItemSlot bestSlot =
            entityPlayer.Player.InventoryManager.OpenedInventories
            .Where(x => x.IsPlayerInventory())
            .SelectMany(x => x)
            .OrderByDescending(x => GetSuperiority(x?.Itemstack?.Collectible?.Tool, targetMaterial))
            .FirstOrDefault(x => IsFaster(thisObject: x?.Itemstack?.Collectible, otherObject: heldCollectible, targetMaterial), defaultValue: slot);

        if (bestSlot != null && heldCollectible?.Equals(slot?.Itemstack, bestSlot.Itemstack, GlobalConstants.IgnoredStackAttributes) == false)
        {
            object packetClient = bestSlot.Inventory.TryFlipItems(bestSlot.Inventory.GetSlotId(bestSlot), slot);
            (byEntity.Api as ClientCoreAPI)?.Network.SendPacketClient(packetClient);
            return false;
        }

        return true;
    }

    private static float GetMiningSpeed(CollectibleObject collectible, EnumBlockMaterial targetMaterial)
    {
        return collectible?.MiningSpeed != null && collectible.MiningSpeed.TryGetValue(targetMaterial, out float speed) ? speed : 0;
    }

    private static int GetSuperiority(EnumTool? tool, EnumBlockMaterial targetMaterial)
    {
        return targetMaterial switch
        {
            EnumBlockMaterial.Leaves when tool == EnumTool.Axe => 1,
            EnumBlockMaterial.Leaves when tool == EnumTool.Shears => 10,
            EnumBlockMaterial.Plant when tool == EnumTool.Knife => 1,
            EnumBlockMaterial.Plant when tool == EnumTool.Scythe => 10,
            _ => 0,
        };
    }

    private static bool IsFaster(CollectibleObject thisObject, CollectibleObject otherObject, EnumBlockMaterial targetMaterial)
    {
        int left = GetSuperiority(thisObject?.Tool, targetMaterial);
        int right = GetSuperiority(otherObject?.Tool, targetMaterial);
        if (left == right)
        {
            return GetMiningSpeed(thisObject, targetMaterial) > GetMiningSpeed(otherObject, targetMaterial);
        }
        return left > right;
    }
}
using Vintagestory.API.Common;

namespace DanaTweaks;

public static class BookExtensions
{
    public static bool IsSigned(this ItemSlot slot)
    {
        return slot.Itemstack.Attributes.GetString(Constants.SignedBy) != null;
    }

    public static bool IsSignedBySamePlayer(this ItemSlot slot, IPlayer byPlayer)
    {
        return IsSigned(slot) && IsSignedByUid(slot, byPlayer);
    }

    public static bool IsSignedByUid(this ItemSlot slot, IPlayer byPlayer)
    {
        return slot.Itemstack.Attributes.GetString(Constants.SignedByUid) == byPlayer.PlayerUID;
    }

    public static void Unsign(this ItemSlot slot)
    {
        slot.Itemstack.Attributes.RemoveAttribute(Constants.SignedBy);
        slot.Itemstack.Attributes.RemoveAttribute(Constants.SignedByUid);
    }
}
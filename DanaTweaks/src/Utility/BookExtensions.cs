using Vintagestory.API.Common;

namespace DanaTweaks;

public static class BookExtensions
{
    public const string SignedBy = "signedby";
    public const string SignedByUid = "signedbyuid";

    public static bool IsSigned(this ItemSlot slot)
    {
        return slot.Itemstack.Attributes.GetString(SignedBy) != null;
    }

    public static bool IsSignedBySamePlayer(this ItemSlot slot, IPlayer byPlayer)
    {
        return IsSigned(slot) && IsSignedByUid(slot, byPlayer);
    }

    public static bool IsSignedByUid(this ItemSlot slot, IPlayer byPlayer)
    {
        return slot.Itemstack.Attributes.GetString(SignedByUid) == byPlayer.PlayerUID;
    }

    public static void Unsign(this ItemSlot slot)
    {
        slot.Itemstack.Attributes.RemoveAttribute(SignedBy);
        slot.Itemstack.Attributes.RemoveAttribute(SignedByUid);
    }
}
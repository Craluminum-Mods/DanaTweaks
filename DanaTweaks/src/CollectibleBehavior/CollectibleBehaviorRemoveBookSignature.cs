using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace DanaTweaks;

public class CollectibleBehaviorRemoveBookSignature : CollectibleBehavior
{
    public SkillItem[] modes;

    public CollectibleBehaviorRemoveBookSignature(CollectibleObject collObj) : base(collObj) { }

    public override void OnLoaded(ICoreAPI api)
    {
        modes = new[]
        {
            new SkillItem()
            {
                Name = Lang.Get("danatweaks:RemoveBookSignature"),
            }
        };

        if (api is ICoreClientAPI capi)
        {
            modes[0].WithIcon(capi, capi.Gui.Icons.GenTexture(48, 48, (ctx, _) => capi.Gui.Icons.Draweraser_svg(ctx, 5, 5, 38, 38, ColorUtil.WhiteArgbDouble)));
        }
    }

    public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
    {
        if (toolMode == 0 && IsSignedBySamePlayer(slot, byPlayer))
        {
            Unsign(slot);
        }

        slot.MarkDirty();
    }

    public override void OnUnloaded(ICoreAPI api)
    {
        for (int i = 0; modes != null && i < modes.Length; i++)
        {
            modes[i]?.Dispose();
        }
    }

    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
    {
        return IsSigned(slot) ? modes : base.GetToolModes(slot, forPlayer, blockSel);
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        return modes == null
            ? base.GetHeldInteractionHelp(inSlot, ref handling)
            : base.GetHeldInteractionHelp(inSlot, ref handling).Append(new WorldInteraction
            {
                ActionLangCode = "heldhelp-settoolmode",
                HotKeyCode = "toolmodeselect",
                MouseButton = EnumMouseButton.None
            });
    }

    public const string SignedBy = "signedby";
    public const string SignedByUid = "signedbyuid";

    public static bool IsSigned(ItemSlot slot)
    {
        return slot.Itemstack.Attributes.GetString(SignedBy) != null;
    }

    public static bool IsSignedBySamePlayer(ItemSlot slot, IPlayer byPlayer)
    {
        return IsSigned(slot) && IsSignedByUid(slot, byPlayer);
    }

    public static bool IsSignedByUid(ItemSlot slot, IPlayer byPlayer)
    {
        return slot.Itemstack.Attributes.GetString(SignedByUid) == byPlayer.PlayerUID;
    }

    public static void Unsign(ItemSlot slot)
    {
        slot.Itemstack.Attributes.RemoveAttribute(SignedBy);
        slot.Itemstack.Attributes.RemoveAttribute(SignedByUid);
    }
}
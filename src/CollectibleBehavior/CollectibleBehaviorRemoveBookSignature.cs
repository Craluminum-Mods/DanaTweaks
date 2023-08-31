using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace DanaTweaks;

public class CollectibleBehaviorRemoveBookSignature : CollectibleBehavior
{
    public SkillItem[] SkillItems { get; private set; }

    public CollectibleBehaviorRemoveBookSignature(CollectibleObject collObj) : base(collObj) { }

    public override void OnLoaded(ICoreAPI api)
    {
        base.OnLoaded(api);

        SkillItems = new SkillItem[]
        {
            new SkillItem()
            {
                Name = Constants.RemoveBookSignature,
            }
        };

        if (api is not ICoreClientAPI)
        {
            return;
        }

        ICoreClientAPI capi = api as ICoreClientAPI;

        SkillItems[0].WithIcon(capi, capi.Gui.Icons.GenTexture(
            48,
            48,
            (ctx, _) => capi.Gui.Icons.Draweraser_svg(ctx, 5, 5, 38, 38, ColorUtil.WhiteArgbDouble)));
    }

    public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
    {
        if (toolMode == 0 && IsSignedBySamePlayer(slot, byPlayer))
        {
            Unsign(slot);
        }

        slot.MarkDirty();
    }

    public static bool IsSigned(ItemSlot slot)
    {
        return slot.Itemstack.Attributes.GetString(Constants.SignedBy) != null;
    }

    public static bool IsSignedByUid(ItemSlot slot, IPlayer byPlayer)
    {
        return slot.Itemstack.Attributes.GetString(Constants.SignedByUid) == byPlayer.PlayerUID;
    }

    public static bool IsSignedBySamePlayer(ItemSlot slot, IPlayer byPlayer)
    {
        return IsSigned(slot) && IsSignedByUid(slot, byPlayer);
    }

    public static void Unsign(ItemSlot slot)
    {
        slot.Itemstack.Attributes.RemoveAttribute(Constants.SignedBy);
        slot.Itemstack.Attributes.RemoveAttribute(Constants.SignedByUid);
    }

    public override void OnUnloaded(ICoreAPI api)
    {
        for (int i = 0; SkillItems != null && i < SkillItems.Length; i++)
        {
            SkillItems[i]?.Dispose();
        }
    }

    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
    {
        return IsSigned(slot) ? SkillItems : base.GetToolModes(slot, forPlayer, blockSel);
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        return SkillItems == null
            ? base.GetHeldInteractionHelp(inSlot, ref handling)
            : base.GetHeldInteractionHelp(inSlot, ref handling).Append(new WorldInteraction
            {
                ActionLangCode = "heldhelp-settoolmode",
                HotKeyCode = "toolmodeselect",
                MouseButton = EnumMouseButton.None
            });
    }
}
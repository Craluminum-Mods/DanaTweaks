using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Util;

namespace DanaTweaks;

public class CollectibleBehaviorBranchCutter : CollectibleBehavior
{
    SkillItem[] modes;

    public CollectibleBehaviorBranchCutter(CollectibleObject collObj) : base(collObj) { }

    public override void OnLoaded(ICoreAPI api)
    {
        modes = new[]
        {
            Constants.BranchCutterNormalMode,
            Constants.BranchCutterLeavesMode,
        };

        if (api is ICoreClientAPI capi)
        {
            modes[0].WithLetterIcon(capi, "N");
            modes[1].WithIcon(capi, capi.Gui.LoadSvgWithPadding(new AssetLocation("danatweaks:textures/icons/leaves.svg"), 48, 48, 5, ColorUtil.WhiteArgb));
            modes[1].TexturePremultipliedAlpha = false;
        }
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        return base.GetHeldInteractionHelp(inSlot, ref handling).Append(new WorldInteraction()
        {
            ActionLangCode = "heldhelp-settoolmode",
            HotKeyCode = "toolmodeselect"
        });
    }

    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel) => modes;

    public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
    {
        slot.Itemstack.Attributes.SetInt("toolMode", toolMode);
    }

    public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
    {
        return slot.Itemstack.Attributes.GetInt("toolMode", 0);
    }

    public override void OnUnloaded(ICoreAPI api)
    {
        for (int i = 0; modes != null && i < modes.Length; i++)
        {
            modes[i]?.Dispose();
        }
    }
}
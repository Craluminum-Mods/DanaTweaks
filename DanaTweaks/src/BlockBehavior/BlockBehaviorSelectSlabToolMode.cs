using System;
using System.Text.RegularExpressions;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace DanaTweaks;

public class BlockBehaviorSelectSlabToolMode : BlockBehavior
{
    private SkillItem[] modes;

    public BlockBehaviorSelectSlabToolMode(Block block) : base(block) { }

    public override void OnLoaded(ICoreAPI api)
    {
        modes = new SkillItem[3]
        {
            new SkillItem
            {
                Code = new AssetLocation("slab-placemode-auto"),
                Name = Regex.Replace( Lang.Get("slab-placemode-auto"), @"<font[^>]*>|</font>", "")
            },
            new SkillItem
            {
                Code = new AssetLocation("slab-placemode-horizontal"),
                Name = Regex.Replace( Lang.Get("slab-placemode-horizontal"), @"<font[^>]*>|</font>", "")
            },
            new SkillItem
            {
                Code = new AssetLocation("slab-placemode-vertical"),
                Name = Regex.Replace( Lang.Get("slab-placemode-vertical"), @"<font[^>]*>|</font>", "")
            },
        };

        if (api is ICoreClientAPI capi)
        {
            CairoFont fontConfig = new()
            {
                Color = (double[])GuiStyle.DialogDefaultTextColor.Clone(),
                Fontname = GuiStyle.StandardFontName,
                UnscaledFontsize = GuiStyle.LargeFontSize
            };

            TextBackground textBackground = new() { HorPadding = 5 };

            try
            {
                modes[0].WithIcon(capi, capi.Gui.TextTexture.GenTextTexture("A", fontConfig, 48, textBackground));
                modes[1].WithIcon(capi, capi.Gui.TextTexture.GenTextTexture("H", fontConfig, 48, textBackground));
                modes[2].WithIcon(capi, capi.Gui.TextTexture.GenTextTexture("V", fontConfig, 48, textBackground));
            }
            catch (Exception e)
            {
                capi.Logger.Error($"[Dana Tweaks] Can't draw icons for {this}:");
                capi.Logger.Error(e);
            }
        }
    }

    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
    {
        return modes;
    }

    public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection)
    {
        return slot.Itemstack.Attributes.GetInt("slabPlaceMode");
    }

    public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
    {
        if (toolMode == 0)
        {
            slot.Itemstack.Attributes.RemoveAttribute("slabPlaceMode");
            return;
        }
        slot.Itemstack.Attributes.SetInt("slabPlaceMode", toolMode);
    }

    public override void OnUnloaded(ICoreAPI api)
    {
        int i = 0;
        while (modes != null && i < modes.Length)
        {
            modes[i]?.Dispose();
            i++;
        }
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        return new WorldInteraction[1]
        {
            new WorldInteraction
            {
                ActionLangCode = "heldhelp-settoolmode",
                HotKeyCode = "toolmodeselect"
            }
        };
    }
}
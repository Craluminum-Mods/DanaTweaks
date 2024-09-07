using Vintagestory.API.Common;
using Vintagestory.API.Client;

namespace DanaTweaks;

public class Hotkeys : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI api)
    {
        api.Input.RegisterHotKey(OpenCloseLidHotkey, OpenCloseLidName, GlKeys.X, HotkeyType.GUIOrOtherControls, ctrlPressed: true);
        api.Input.SetHotKeyHandler(OpenCloseLidHotkey, _ => OpenCloseLid(api));

        api.Input.RegisterHotKey(RemoveOrAddLabelHotkey, RemoveOrAddLabelName, GlKeys.X, HotkeyType.GUIOrOtherControls, shiftPressed: true);
        api.Input.SetHotKeyHandler(RemoveOrAddLabelHotkey, _ => RemoveOrAddLabel(api));
    }

    public static bool OpenCloseLid(ICoreClientAPI api)
    {
        if (!api.IsCrate())
        {
            return false;
        }

        api.SendChatMessage("/danatweaks openclose");
        return true;
    }

    public static bool RemoveOrAddLabel(ICoreClientAPI api)
    {
        if (!api.IsCrate())
        {
            return false;
        }

        api.SendChatMessage("/danatweaks removeoraddlabel");
        return true;
    }
}
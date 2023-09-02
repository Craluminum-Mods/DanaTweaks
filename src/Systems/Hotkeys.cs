using Vintagestory.API.Common;
using Vintagestory.API.Client;

namespace DanaTweaks;

public class Hotkeys : ModSystem
{
    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI api)
    {
        base.StartClientSide(api);

        api.Input.RegisterHotKey(Constants.OpenCloseLidHotkey, Constants.OpenCloseLidName, GlKeys.X, HotkeyType.GUIOrOtherControls, ctrlPressed: true);
        api.Input.SetHotKeyHandler(Constants.OpenCloseLidHotkey, _ => OpenCloseLid(api));

        api.Input.RegisterHotKey(Constants.RemoveOrAddLabelHotkey, Constants.RemoveOrAddLabelName, GlKeys.X, HotkeyType.GUIOrOtherControls, shiftPressed: true);
        api.Input.SetHotKeyHandler(Constants.RemoveOrAddLabelHotkey, _ => RemoveOrAddLabel(api));
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
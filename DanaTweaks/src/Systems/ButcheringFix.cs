using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace DanaTweaks;

public class ButcheringFix : ModSystem
{
    private static ICoreClientAPI _capi;
    public static bool Enabled { get; set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Client;

    public override void StartClientSide(ICoreClientAPI capi)
    {
        _capi = capi;
        capi.Input.RegisterHotKey("danatweaks:butcheringfix", Lang.Get("danatweaks:ButcheringFix.Toggle"), GlKeys.B, HotkeyType.CharacterControls, shiftPressed: true);
        capi.Input.SetHotKeyHandler("danatweaks:butcheringfix", x => Toggle(x, capi));
    }

    private bool Toggle(KeyCombination t1, ICoreClientAPI capi)
    {
        Enabled = !Enabled;
        capi.TriggerChatMessage(Lang.Get(Enabled ? "danatweaks:ButcheringFix.Enabled" : "danatweaks:ButcheringFix.Disabled"));
        return true;
    }
}
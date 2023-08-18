using Vintagestory.API.Config;

namespace DanaTweaks;

public static class Constants
{
    #region crate tweaks
    public const string DefaultLabel = "paper-empty";

    public const string OpenCloseLidHotkey = "opencloselid";
    public const string RemoveOrAddLabelHotkey = "removeoraddlabel";

    public static readonly string OpenCloseLidName = Lang.Get("danatweaks:Command.OpenCloseLid.Name");
    public static readonly string RemoveOrAddLabelName = Lang.Get("danatweaks:Command.RemoveOrAddLabel.Name");

    public static readonly string NoCrate = Lang.Get("danatweaks:Command.Error.NoCrate");
    public static readonly string NoLabel = Lang.Get("danatweaks:Command.Error.NoLabel");
    public static readonly string HasDiffLabel = Lang.Get("danatweaks:Command.Error.HasDiffLabel");
    #endregion crate tweaks
}
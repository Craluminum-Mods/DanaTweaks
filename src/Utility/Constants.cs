using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;

namespace DanaTweaks;

public static class Constants
{
    public const string ConfigName = "DanaTweaks.json";

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

    public const string Opened = "opened";
    public const string Closed = "closed";

    public static readonly string RemoveBookSignature = Lang.Get("danatweaks:RemoveBookSignature");
    public const string SignedBy = "signedby";
    public const string SignedByUid = "signedbyuid";

    public static readonly SkillItem BranchCutterNormalMode = new()
    {
        Code = new AssetLocation("normal"),
        Name = Lang.Get("worldconfig-globalForestation-Normal")
    };

    public static readonly SkillItem BranchCutterLeavesMode = new()
    {
        Code = new AssetLocation("leaves"),
        Name = Lang.Get("blockmaterial-Leaves")
    };
}
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

    #region Codes
    public const string ParchmentCode = "paper-parchment";
    public const string ResinCode = "resin";
    public const string PitkilnCode = "pitkiln";
    public const string ChiselCodes = "chisel-*";
    public const string MetalblockCodes = "metalblock-new-riveted-*";
    public const string MetalbitPlaceholderCode = "metalbit-{metal}";
    public const string BrassTorchholderCodes = "torchholder-brass-*";
    public const string BrassMetalbitCode = "metalbit-brass";

    #endregion Codes

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

    public static readonly BlockEntityBehaviorType RainCollectorBehavior = new()
    {
        Name = "DanaTweaks:RainCollector",
        properties = null
    };

    public static readonly ModelTransform PotShelfTransform = new()
    {
        Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
        Scale = 0.8f
    };

    public static readonly ModelTransform PieShelfTransform = new()
    {
        Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
        Scale = 0.65f
    };

    public static readonly ModelTransform FirestarterToolrackTransform = new()
    {
        Translation = new() { X = 0.25f, Y = 0.55f, Z = 0.0275f },
        Rotation = new() { X = 180, Y = -135, Z = 0 },
        Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
        Scale = 0.7f
    };
}
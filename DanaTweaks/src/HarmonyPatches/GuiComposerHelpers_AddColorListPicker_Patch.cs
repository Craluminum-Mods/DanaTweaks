using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;

namespace DanaTweaks;

public static class GuiComposerHelpers_AddColorListPicker_Patch
{
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(GuiComposerHelpers), nameof(GuiComposerHelpers.AddColorListPicker));
    }

    public static MethodInfo GetPrefix() => typeof(GuiComposerHelpers_AddColorListPicker_Patch).GetMethod(nameof(Prefix));

    public static void Prefix(GuiComposer composer, ref int maxLineWidth)
    {
        if (composer.DialogName == "worldmap-modwp" || composer.DialogName == "worldmap-addwp" || composer.DialogName.Contains("edittpdlg"))
        {
            maxLineWidth = (int)(maxLineWidth * Core.ConfigClient.ColorsPerRowForWaypointWindowRatio);
        }
    }
}
using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;

namespace DanaTweaks;

public static class GuiComposerHelpers_AddIconListPicker_Patch
{
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(GuiComposerHelpers), nameof(GuiComposerHelpers.AddIconListPicker));
    }

    public static MethodInfo GetPrefix() => typeof(GuiComposerHelpers_AddIconListPicker_Patch).GetMethod(nameof(Prefix));

    public static void Prefix(GuiComposer composer, ref int maxLineWidth)
    {
        if (composer.DialogName == "worldmap-modwp" || composer.DialogName == "worldmap-addwp" || composer.DialogName.Contains("edittpdlg"))
        {
            maxLineWidth = (int)(maxLineWidth * Core.ConfigClient.IconsPerRowForWaypointWindowRatio);
        }
    }
}
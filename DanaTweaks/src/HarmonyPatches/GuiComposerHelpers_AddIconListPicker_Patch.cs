using HarmonyLib;
using Vintagestory.API.Client;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(Core.ConfigClient.IconsPerRowForWaypointWindowEnabled))]
public static class GuiComposerHelpers_AddIconListPicker_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GuiComposerHelpers), nameof(GuiComposerHelpers.AddIconListPicker))]
    public static void Prefix(GuiComposer composer, ref int maxLineWidth)
    {
        if (composer.DialogName == "worldmap-modwp" || composer.DialogName == "worldmap-addwp" || composer.DialogName.Contains("edittpdlg"))
        {
            maxLineWidth = (int)(maxLineWidth * Core.ConfigClient.IconsPerRowForWaypointWindowRatio);
        }
    }
}
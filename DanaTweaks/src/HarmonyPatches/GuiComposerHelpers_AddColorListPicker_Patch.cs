using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Client;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigClient.ColorsPerRowForWaypointWindowEnabled))]
public static class GuiComposerHelpers_AddColorListPicker_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(GuiComposerHelpers), nameof(GuiComposerHelpers.AddColorListPicker))]
    public static void Prefix(GuiComposer composer, ref int maxLineWidth)
    {
        if (composer.DialogName == "worldmap-modwp" || composer.DialogName == "worldmap-addwp" || composer.DialogName.Contains("edittpdlg"))
        {
            maxLineWidth = (int)(maxLineWidth * Core.ConfigClient.ColorsPerRowForWaypointWindowRatio);
        }
    }
}
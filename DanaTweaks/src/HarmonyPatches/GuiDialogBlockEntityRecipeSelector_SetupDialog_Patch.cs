using DanaTweaks.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigClient.ModesPerRowForVoxelRecipesEnabled))]
public static class GuiDialogBlockEntityRecipeSelector_SetupDialog_Patch
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(GuiDialogBlockEntityRecipeSelector), "SetupDialog")]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);

        for (int i = 0; i < codes.Count; i++)
        {
            if (codes[i].opcode == OpCodes.Ldc_I4_7)
            {
                codes[i].opcode = OpCodes.Ldc_I4;
                codes[i].operand = Core.ConfigClient.ModesPerRowForVoxelRecipes;
                break;
            }
        }
        return codes.AsEnumerable();
    }
}
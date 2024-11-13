using DanaTweaks.Configuration;
using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.RegrowResin))]
public static class BlockEntitySapling_CheckGrow_Patch
{
    [HarmonyTranspiler]
    [HarmonyPatch(typeof(BlockEntitySapling), "CheckGrow")]
    public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        int prevIndex = -1;

        List<CodeInstruction> codes = new List<CodeInstruction>(instructions);
        for (int i = 0; i < codes.Count; i++)
        {
            if (prevIndex != -1)
            {
                if (codes[i].opcode == OpCodes.Stfld)
                {

                    FieldInfo fieldInfo = codes[i].operand as FieldInfo;
                    if (fieldInfo.Name == "otherBlockChance" && codes[prevIndex].opcode == OpCodes.Ldc_R4)
                    {
                        codes[prevIndex] = new CodeInstruction(OpCodes.Ldc_R4, 1.0f);
                    }
                }
            }
            prevIndex = i;
        }

        return codes.AsEnumerable();
    }
}
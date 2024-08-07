using HarmonyLib;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockEntitySapling_CheckGrow_Patch
{
    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(BlockEntitySapling), "CheckGrow", new[] { typeof(float) });
    }

    public static MethodInfo GetTranspiler() => typeof(BlockEntitySapling_CheckGrow_Patch).GetMethod(nameof(Transpiler));

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
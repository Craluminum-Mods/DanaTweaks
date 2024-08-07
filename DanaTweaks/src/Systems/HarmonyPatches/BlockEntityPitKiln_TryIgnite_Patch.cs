using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockEntityPitKiln_TryIgnite_Patch
{
    static readonly string[] DIAGONALS = new string[] { "nw", "ne", "se", "sw" };

    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(BlockEntityPitKiln), nameof(BlockEntityPitKiln.TryIgnite), new[] { typeof(IPlayer) });
    }

    public static MethodInfo GetPostfix() => typeof(BlockEntityPitKiln_TryIgnite_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(BlockEntityPitKiln __instance, IPlayer byPlayer)
    {
        if (byPlayer == null) return;
        for (int i = 0; i < DIAGONALS.Length; i++)
        {
            if (__instance.Api.World.BlockAccessor.GetBlockEntity(__instance.Pos.AddCopy(Cardinal.FromInitial(DIAGONALS[i]).Normali)) is BlockEntityPitKiln kiln && kiln.IsComplete && !kiln.Lit)
            {
                kiln.TryIgnite(byPlayer);
            }
        }
    }
}
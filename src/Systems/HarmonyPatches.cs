using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class HarmonyPatches : ModSystem
{
    public const string HarmonyID = "danatweaks";

    public override void StartServerSide(ICoreServerAPI api)
    {
        base.StartServerSide(api);
        if (Core.Config.DropClutterAnyway)
        {
            new Harmony(HarmonyID).Patch(original: typeof(BlockClutter).GetMethod("GetDrops"), prefix: typeof(BlockClutterDropPatch).GetMethod("Prefix"));
        }
    }

    public override void Dispose()
    {
        if (Core.Config.DropClutterAnyway)
        {
            new Harmony(HarmonyID).Unpatch(original: typeof(BlockClutter).GetMethod("GetDrops"), HarmonyPatchType.All, HarmonyID);
        }
        base.Dispose();
    }

    [HarmonyPatch(typeof(BlockClutter), nameof(BlockClutter.GetDrops))]
    public static class BlockClutterDropPatch
    {
        public static bool Prefix(BlockClutter __instance, BlockPos pos)
        {
            BEBehaviorShapeFromAttributes bec = __instance.GetBEBehavior<BEBehaviorShapeFromAttributes>(pos);
            bec.Collected = true;
            return true;
        }
    }
}
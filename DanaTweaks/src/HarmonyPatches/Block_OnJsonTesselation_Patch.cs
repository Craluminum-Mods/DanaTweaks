using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

[HarmonyPatchCategory("UnsortedClient")]
public static class Block_OnJsonTesselation_Patch
{
    [HarmonyPrefix]
    [HarmonyPatch(typeof(Block), nameof(Block.OnJsonTesselation))]
    public static void Prefix(Block __instance, ref MeshData sourceMesh, ref int[] lightRgbsByCorner, BlockPos pos, Block[] chunkExtBlocks, int extIndex3d, ICoreAPI ___api)
    {
        if (___api is not ICoreClientAPI capi || Core.ConfigClient?.ResinOnAllSides == false || !__instance.Code.ToString().Contains("log-resin"))
        {
            return;
        }

        Shape shape = __instance.Variant["type"] switch
        {
            "resin" => capi.Assets.TryGet(AssetLocation.Create("danatweaks:shapes/block/log/withresin1.json"))?.ToObject<Shape>(),
            "resinharvested" => capi.Assets.TryGet(AssetLocation.Create("danatweaks:shapes/block/log/noresin1.json"))?.ToObject<Shape>(),
            _ => null
        };

        if (shape == null)
        {
            return;
        }

        capi.Tesselator.TesselateShape(__instance, shape, out MeshData mesh);
        if (mesh != null)
        {
            sourceMesh = mesh;
        }
    }
}
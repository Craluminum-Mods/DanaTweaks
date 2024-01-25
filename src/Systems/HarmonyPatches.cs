using System.Reflection;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.API.Server;
using Vintagestory.GameContent;

namespace DanaTweaks;

public partial class HarmonyPatches : ModSystem
{
    private Harmony harmony;

    public override void StartServerSide(ICoreServerAPI api)
    {
        harmony = new Harmony(Mod.Info.ModID);

        if (Core.Config.DropClutterAnyway)
        {
            MethodInfo prefix = typeof(BlockClutterDropPatch).GetMethod("Prefix");

            harmony.Patch(original: typeof(BlockClutter).GetMethod("GetDrops"), prefix: prefix);
            harmony.Patch(original: typeof(BlockShapeFromAttributes).GetMethod("GetDrops"), prefix: prefix);
        }
    }

    public override void Dispose()
    {
        if (Core.Config.DropClutterAnyway)
        {
            harmony.Unpatch(original: typeof(BlockClutter).GetMethod(nameof(BlockClutter.GetDrops)), HarmonyPatchType.All, harmony.Id);
            harmony.Unpatch(original: typeof(BlockShapeFromAttributes).GetMethod(nameof(BlockShapeFromAttributes.GetDrops)), HarmonyPatchType.All, harmony.Id);
        }
    }

    [HarmonyPatch(typeof(BlockClutter), nameof(BlockClutter.GetDrops))]
    public static class BlockClutterDropPatch
    {
        public static bool Prefix(BlockClutter __instance, ref ItemStack[] __result, IWorldAccessor world, BlockPos pos)
        {
            BEBehaviorShapeFromAttributes bec = __instance.GetBEBehavior<BEBehaviorShapeFromAttributes>(pos);
            bec.Collected = true;
            ItemStack itemStack = __instance.OnPickBlock(world, pos);
            itemStack.Attributes.SetBool("collected", true);
            __result = new[] { itemStack };
            return true;
        }
    }
}
using DanaTweaks.Configuration;
using HarmonyLib;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.SlabToolModes))]
public static class BlockBehaviorOmniRotatable_TryPlaceBlock_Patch
{
    [HarmonyReversePatch(HarmonyReversePatchType.Original)]
    [HarmonyPatch(typeof(BlockBehaviorOmniRotatable), nameof(BlockBehaviorOmniRotatable.TryPlaceBlock))]
    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool Base(BlockBehaviorOmniRotatable instance, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
    {
        return default;
    }

    [HarmonyPrefix]
    [HarmonyPatch(typeof(BlockBehaviorOmniRotatable), nameof(BlockBehaviorOmniRotatable.TryPlaceBlock))]
    public static bool Prefix(BlockBehaviorOmniRotatable __instance, ref bool __result, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
    {
        try
        {
            bool result = Base(__instance, world, byPlayer, itemstack, blockSel, ref handling, ref failureCode);
            if (result)
            {
                __result = result;
                return false;
            }
        }
        catch
        {
            failureCode = failureOmniRotatable;
            __result = false;
            return false;
        }

        return true;
    }
}
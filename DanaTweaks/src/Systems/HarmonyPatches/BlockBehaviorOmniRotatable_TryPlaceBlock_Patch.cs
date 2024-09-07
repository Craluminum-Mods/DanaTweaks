using System.Reflection;
using System.Runtime.CompilerServices;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace DanaTweaks;

public static class BlockBehaviorOmniRotatable_TryPlaceBlock_Patch
{
    public static MethodBase TargetMethod() => typeof(BlockBehaviorOmniRotatable).GetMethod(nameof(BlockBehaviorOmniRotatable.TryPlaceBlock));
    public static MethodInfo GetOriginal() => typeof(BlockBehaviorOmniRotatable_TryPlaceBlock_Patch).GetMethod(nameof(Base));
    public static MethodInfo GetPrefix() => typeof(BlockBehaviorOmniRotatable_TryPlaceBlock_Patch).GetMethod(nameof(Prefix));

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static bool Base(BlockBehaviorOmniRotatable instance, IWorldAccessor world, IPlayer byPlayer, ItemStack itemstack, BlockSelection blockSel, ref EnumHandling handling, ref string failureCode)
    {
        return default;
    }

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
using DanaTweaks.Configuration;
using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

[HarmonyPatchCategory(nameof(ConfigServer.FirepitHeatsOven))]
public static class BlockEntityFirepit_OnBurnTick_Patch
{
    [HarmonyPostfix]
    [HarmonyPatch(typeof(BlockEntityFirepit), "OnBurnTick")]
    public static void Postfix(BlockEntityFirepit __instance, float dt)
    {
        if (!__instance.Api.Side.IsServer()
            || __instance.Block.Code.Path.Contains("construct")
            || !__instance.IsBurning
            || __instance.Api.World.BlockAccessor.GetBlockEntity(__instance.Pos.UpCopy(1)) is not BlockEntityOven oven)
        {
            return;
        }

        oven.ovenTemperature = oven.ChangeTemperature(oven.ovenTemperature, oven.maxTemperature, dt * oven.fuelitemCapacity / oven.fuelitemCapacity);

        int syncCount = oven.GetField<int>("syncCount");
        if (++syncCount % 5 == 0 && oven.prevOvenTemperature != oven.ovenTemperature)
        {
            oven.MarkDirty();
            oven.prevOvenTemperature = oven.ovenTemperature;
        }
    }
}
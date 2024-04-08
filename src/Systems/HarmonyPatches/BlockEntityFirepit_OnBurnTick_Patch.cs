using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockEntityFirepit_OnBurnTick_Patch
{
    public static void Postfix(BlockEntityFirepit __instance, float dt)
    {
        if (!__instance.Api.Side.IsServer() || __instance.Block.Code.Path.Contains("construct") || !__instance.IsBurning)
        {
            return;
        }

        BlockEntity blockEntity = __instance.Api.World.BlockAccessor.GetBlockEntity(__instance.Pos.UpCopy(1));
        if (blockEntity is not BlockEntityOven oven)
        {
            return;
        }

        oven.ovenTemperature = oven.ChangeTemperature(oven.ovenTemperature, oven.maxTemperature, dt * (float)oven.fuelitemCapacity / (float)oven.fuelitemCapacity);

        int syncCount = oven.GetField<int>("syncCount");
        if (++syncCount % 5 == 0 && (oven.prevOvenTemperature != oven.ovenTemperature))
        {
            oven.MarkDirty();
            oven.prevOvenTemperature = oven.ovenTemperature;
        }
    }
}
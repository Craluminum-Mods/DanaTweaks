using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BEBehaviorRainCollector : BlockEntityBehavior
{
    public BEBehaviorRainCollector(BlockEntity blockentity) : base(blockentity) { }

    private WeatherSystemBase wsys;

    public override void Initialize(ICoreAPI api, JsonObject properties)
    {
        Blockentity.RegisterGameTickListener(TryCollectRain, Core.ConfigServer.RainCollector.UpdateMilliseconds);
        wsys = api.ModLoader.GetModSystem<WeatherSystemBase>();
        base.Initialize(api, properties);
    }

    public void TryCollectRain(float dt)
    {
        float desiredLitres = Core.ConfigServer.RainCollector.LitresPerUpdate;
        ItemStack itemStack = new(Api.World.GetItem(new AssetLocation(Core.ConfigServer.RainCollector.LiquidCode)));

        if (!IsRaining())
        {
            return;
        }

        if (Blockentity is BlockEntityGroundStorage begs && !begs.Inventory.Empty)
        {
            foreach (ItemSlot slot in begs.Inventory)
            {
                if (!slot.Empty && slot.Itemstack.Collectible is BlockLiquidContainerBase blockCnt && blockCnt.IsTopOpened)
                {
                    blockCnt.TryPutLiquid(slot.Itemstack, itemStack, desiredLitres);
                }
            }
            begs.MarkDirty(true);
        }
        else if (Blockentity.Block is BlockLiquidContainerBase blockCnt1)
        {
            if (Blockentity is BlockEntityBarrel blockBarrel)
            {
                if (!blockBarrel.Sealed)
                {
                    blockCnt1.TryPutLiquid(Blockentity.Pos, itemStack, desiredLitres);
                }
            }
            else
            {
                blockCnt1.TryPutLiquid(Blockentity.Pos, itemStack, desiredLitres);
            }
        }
    }

    private bool IsRaining()
    {
        return Api.Side == EnumAppSide.Server
            && Api.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y
            && wsys.GetPrecipitation(Pos.ToVec3d()) > Core.ConfigServer.RainCollector.MinPrecipitation;
    }
}
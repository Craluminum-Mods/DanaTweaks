using Vintagestory.API.Common;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BEBehaviorRainCollector : BlockEntityBehavior
{
    public BEBehaviorRainCollector(BlockEntity blockentity) : base(blockentity) { }

    private WeatherSystemBase wsys;
    private long listenerId;

    public override void Initialize(ICoreAPI api, JsonObject properties)
    {
        listenerId = api.Event.RegisterGameTickListener(UpdateEvery1000ms, Core.Config.RainCollector.UpdateEveryMs);
        wsys = api.ModLoader.GetModSystem<WeatherSystemBase>();

        base.Initialize(api, properties);
    }

    public override void OnBlockRemoved()
    {
        Blockentity.UnregisterGameTickListener(listenerId);
        base.OnBlockRemoved();
    }

    public void UpdateEvery1000ms(float dt)
    {
        float desiredLitres = Core.Config.RainCollector.LitresPerSecond;
        ItemStack itemStack = new(Api.World.GetItem(new AssetLocation(Core.Config.RainCollector.LiquidCode)));

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
            blockCnt1.TryPutLiquid(Blockentity.Pos, itemStack, desiredLitres);
        }
    }

    private bool IsRaining()
    {
        return Api.Side == EnumAppSide.Server
            && Api.World.BlockAccessor.GetRainMapHeightAt(Pos.X, Pos.Z) <= Pos.Y
            && wsys.GetPrecipitation(Pos.ToVec3d()) > Core.Config.RainCollector.MinPrecipitation;
    }
}
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockWateringCan_OnHeldInteractStep_Patch
{
    public static void Postfix(EntityAgent byEntity, BlockSelection blockSel)
    {
        if (blockSel == null)
        {
            return;
        }
        if (byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityToolMold toolMold)
        {
            toolMold.CoolWithWater();
        }
        if (byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is BlockEntityIngotMold ingotMold)
        {
            ingotMold.CoolWithWater();
        }
    }
}
using System.Reflection;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockWateringCan_OnHeldInteractStep_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockWateringCan).GetMethod(nameof(BlockWateringCan.OnHeldInteractStep));
    }

    public static MethodInfo GetPostfix() => typeof(BlockWateringCan_OnHeldInteractStep_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(EntityAgent byEntity, BlockSelection blockSel)
    {
        if (blockSel == null)
        {
            return;
        }
        switch (byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position))
        {
            case BlockEntityToolMold toolMold:
                toolMold.CoolWithWater();
                break;
            case BlockEntityIngotMold ingotMold:
                ingotMold.CoolWithWater();
                break;
        }
    }
}
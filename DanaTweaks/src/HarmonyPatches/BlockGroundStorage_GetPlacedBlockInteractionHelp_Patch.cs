using System;
using System.Linq;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class BlockGroundStorage_GetPlacedBlockInteractionHelp_Patch
{
    public static MethodBase TargetMethod()
    {
        return typeof(BlockGroundStorage).GetMethod(nameof(BlockGroundStorage.GetPlacedBlockInteractionHelp));
    }

    public static MethodInfo GetPostifx() => typeof(BlockGroundStorage_GetPlacedBlockInteractionHelp_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(ref WorldInteraction[] __result, IWorldAccessor world, BlockSelection selection, IPlayer forPlayer)
    {
        if (selection == null || world.BlockAccessor.GetBlockEntity(selection.Position) is not BlockEntityGroundStorage begs)
        {
            return;
        }

        BlockLiquidContainerBase _containerForInteractions = null;
        bool anyLiquidContainer = false;
        foreach (ItemSlot slot in begs.Inventory)
        {
            if (slot?.Itemstack?.Collectible is not ILiquidInterface)
            {
                continue;
            }
            anyLiquidContainer = true;
            _containerForInteractions = slot.Itemstack.Collectible as BlockLiquidContainerBase;
        }
        if (!anyLiquidContainer || _containerForInteractions == null)
        {
            return;
        }


        WorldInteraction[] interactions = _containerForInteractions.GetField<WorldInteraction[]>("interactions");
        WorldInteraction[] result = __result;
        if (interactions.Any(x => result.Any(y => x.ActionLangCode == y.ActionLangCode)))
        {
            return;
        }

        __result ??= Array.Empty<WorldInteraction>();
        __result = __result.Append(interactions);
    }
}
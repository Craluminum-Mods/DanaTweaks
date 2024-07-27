using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using System.Linq;

namespace DanaTweaks;

public class BlockBehaviorCrateInteractionHelp : BlockBehavior
{
    public BlockBehaviorCrateInteractionHelp(Block block) : base(block)
    {
    }

    public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
    {
        ItemStack LabelStack = new(world.GetItem(new AssetLocation(Constants.ParchmentCode)));

        ICoreClientAPI capi = world.Api as ICoreClientAPI;

        WorldInteraction[] interactions = new WorldInteraction[2];
        interactions[0] = new()
        {
            ActionLangCode = capi.GetHotkeyCodes(Constants.OpenCloseLidHotkey) + " " + Constants.OpenCloseLidName,
            MouseButton = EnumMouseButton.None
        };
        interactions[1] = new()
        {
            ActionLangCode = capi.GetHotkeyCodes(Constants.RemoveOrAddLabelHotkey) + " " + Constants.RemoveOrAddLabelName,
            MouseButton = EnumMouseButton.None,
            Itemstacks = new ItemStack[] { LabelStack }
        };

        return base.GetPlacedBlockInteractionHelp(world, selection, forPlayer, ref handling).Append(interactions);
    }
}
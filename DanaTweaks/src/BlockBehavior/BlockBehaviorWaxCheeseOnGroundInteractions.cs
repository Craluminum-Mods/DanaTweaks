using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorWaxCheeseOnGroundInteractions : BlockBehavior
{
    WorldInteraction[] interactions;

    public override bool ClientSideOptional => true;

    public BlockBehaviorWaxCheeseOnGroundInteractions(Block block) : base(block) { }

    public override void OnLoaded(ICoreAPI api)
    {
        List<ItemStack> waxStacks = new();
        waxStacks.AddRange(api.World.Collectibles.Where(x => x?.Attributes?["waxCheeseOnGround"]?.AsBool() == true).Select(obj => new ItemStack(obj)));

        interactions = new WorldInteraction[] {
              new WorldInteraction() {
                  ActionLangCode = "danatweaks:waxcheese",
                  MouseButton = EnumMouseButton.Right,
                  Itemstacks = waxStacks.ToArray(),
                  HotKeyCode = "shift"
              }
          };
    }

    public override WorldInteraction[] GetPlacedBlockInteractionHelp(IWorldAccessor world, BlockSelection selection, IPlayer forPlayer, ref EnumHandling handling)
    {
        if (world.BlockAccessor.GetBlockEntity(selection.Position) is not BECheese bec || bec.Inventory[0].Itemstack?.Collectible.Variant["type"] != "salted")
        {
            return null;
        }

        handling = EnumHandling.PassThrough;
        return interactions;
    }
}
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class CollectibleBehaviorWaxCheeseOnGround : CollectibleBehavior
{
    WorldInteraction[] interactions;

    public override bool ClientSideOptional => true;

    public CollectibleBehaviorWaxCheeseOnGround(CollectibleObject collObj) : base(collObj) { }

    public override void OnLoaded(ICoreAPI api)
    {
        ItemStack[] cheeseStacks = new ItemStack[] { new ItemStack(api.World.GetItem(new AssetLocation("rawcheese-salted"))) };

        interactions = new WorldInteraction[] {
              new WorldInteraction() {
                  ActionLangCode = "danatweaks:waxcheese",
                  MouseButton = EnumMouseButton.Right,
                  Itemstacks = cheeseStacks,
                  HotKeyCode = "shift"
              }
          };
    }

    public override void OnHeldInteractStart(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
    {
        Interact(slot, byEntity, blockSel, entitySel, firstEvent, ref handHandling, ref handling);
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        handling = EnumHandling.PassThrough;
        return interactions;
    }

    public void Interact(ItemSlot slot, EntityAgent byEntity, BlockSelection blockSel, EntitySelection entitySel, bool firstEvent, ref EnumHandHandling handHandling, ref EnumHandling handling)
    {
        if (blockSel == null || byEntity.World.BlockAccessor.GetBlockEntity(blockSel.Position) is not BECheese bec || bec.Inventory[0].Itemstack?.Collectible.Variant["type"] != "salted")
        {
            return;
        }

        slot.TakeOut(1);
        slot.MarkDirty();
        ItemStack _origStack = bec.Inventory[0].Itemstack.Clone();
        ItemStack newStack = new ItemStack(byEntity.World.GetItem(bec.Inventory[0].Itemstack?.Collectible.CodeWithVariant("type", "waxed")));
        newStack.Attributes = _origStack.Attributes;
        bec.Inventory[0].Itemstack.SetFrom(newStack);
        bec.Inventory[0].MarkDirty();
        bec.MarkDirty(true);
        handHandling = EnumHandHandling.PreventDefault;
    }
}
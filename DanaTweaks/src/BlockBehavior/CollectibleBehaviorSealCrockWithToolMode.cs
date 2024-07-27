using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Config;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class CollectibleBehaviorSealCrockWithToolMode : CollectibleBehavior
{
    private SkillItem[] modes;

    public CollectibleBehaviorSealCrockWithToolMode(CollectibleObject collObj) : base(collObj) { }

    public bool IsEmpty(IWorldAccessor world, ItemStack stack)
    {
        ItemStack[] nonEmptyContents = (collObj as BlockCrock)?.GetNonEmptyContents(world, stack);
        return nonEmptyContents == null || nonEmptyContents.Length == 0;
    }

    public override void OnLoaded(ICoreAPI api)
    {
        modes = new SkillItem[1]
        {
            new SkillItem
            {
                Code = new AssetLocation("crock-seal"),
                Name = Lang.Get("danatweaks:crock-seal")
            }
        };

        if (api is ICoreClientAPI capi)
        {
            modes[0].WithIcon(capi, "plus");
            modes[0].TexturePremultipliedAlpha = false;
        }
    }

    public override void OnUnloaded(ICoreAPI api)
    {
        int i = 0;
        while (modes != null && i < modes.Length)
        {
            modes[i]?.Dispose();
            i++;
        }
    }

    public override SkillItem[] GetToolModes(ItemSlot slot, IClientPlayer forPlayer, BlockSelection blockSel)
    {
        if (!IsEmpty(forPlayer.Entity.World, slot.Itemstack) && !slot.Itemstack.Attributes.GetBool("sealed"))
        {
            return modes;
        }
        return null;
    }

    public override int GetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection) => 0;

    public override void SetToolMode(ItemSlot slot, IPlayer byPlayer, BlockSelection blockSelection, int toolMode)
    {
        ItemSlot mouseslot = byPlayer.InventoryManager.MouseItemSlot;

        if (!mouseslot.Empty && mouseslot.Itemstack != null && mouseslot.StackSize > 0 && mouseslot.Itemstack.Collectible.Attributes.KeyExists("canSealCrock") && mouseslot.Itemstack.Collectible.Attributes["canSealCrock"].AsBool())
        {
            slot.Itemstack.Attributes.SetBool("sealed", true);

            mouseslot.TakeOut(1);
            if (mouseslot.StackSize > 0)
            {
                if (!byPlayer.InventoryManager.TryGiveItemstack(mouseslot.Itemstack))
                {
                    byPlayer.InventoryManager.DropMouseSlotItems(true);
                }
                mouseslot.Itemstack = null;
            }
            mouseslot.MarkDirty();
        }
    }

    public override WorldInteraction[] GetHeldInteractionHelp(ItemSlot inSlot, ref EnumHandling handling)
    {
        if (!IsEmpty(inSlot.Inventory.Api.World, inSlot.Itemstack) && !inSlot.Itemstack.Attributes.GetBool("sealed"))
        {
            return new WorldInteraction[1]
            {
                new WorldInteraction
                {
                    ActionLangCode = "heldhelp-settoolmode",
                    HotKeyCode = "toolmodeselect"
                }
            };
        }

        return null;
    }
}
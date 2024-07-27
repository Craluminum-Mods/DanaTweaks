using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class BlockBehaviorGroundStorageParticles : BlockBehavior
{
    public BlockBehaviorGroundStorageParticles(Block block) : base(block) { }

    public override bool ShouldReceiveClientParticleTicks(IWorldAccessor world, IPlayer byPlayer, BlockPos pos, ref EnumHandling handling)
    {
        handling = EnumHandling.PreventDefault;
        return true;
    }

    public override void OnAsyncClientParticleTick(IAsyncParticleManager manager, BlockPos pos, float windAffectednessAtPos, float secondsTicking)
    {
        BlockEntityGroundStorage begs = manager.BlockAccess.GetBlockEntity(pos) as BlockEntityGroundStorage;
        if (begs is null || begs.StorageProps == null || begs.Inventory.Empty)
        {
            return;
        }

        foreach (ItemSlot slot in begs.Inventory)
        {
            CollectibleObject obj = slot?.Itemstack?.Collectible;
            if (slot.Empty)
            {
                continue;
            }

            int slotId = begs.Inventory.GetSlotId(slot);
            BlockFacing facing = BlockFacing.HorizontalFromAngle(begs.MeshAngle).GetCCW();
            Vec3f insidePos = new Vec3f();
            switch (begs.StorageProps.Layout)
            {
                case EnumGroundStorageLayout.Quadrants:
                    switch (facing.Index)
                    {
                        case BlockFacing.indexNORTH:
                            insidePos = slotId switch
                            {
                                0 => new Vec3f(0.25f, 0, 0.25f),
                                1 => new Vec3f(0.25f, 0, -0.25f),
                                2 => new Vec3f(-0.25f, 0, 0.25f),
                                3 => new Vec3f(-0.25f, 0, -0.25f),
                                _ => new Vec3f(0, 0, 0),
                            };
                            break;
                        case BlockFacing.indexSOUTH:
                            insidePos = slotId switch
                            {
                                0 => new Vec3f(-0.25f, 0, -0.25f),
                                1 => new Vec3f(-0.25f, 0, 0.25f),
                                2 => new Vec3f(0.25f, 0, -0.25f),
                                3 => new Vec3f(0.25f, 0, 0.25f),
                                _ => new Vec3f(0, 0, 0),
                            };
                            break;
                        case BlockFacing.indexEAST:
                            insidePos = slotId switch
                            {
                                0 => new Vec3f(-0.25f, 0, 0.25f),
                                1 => new Vec3f(0.25f, 0, 0.25f),
                                2 => new Vec3f(-0.25f, 0, -0.25f),
                                3 => new Vec3f(0.25f, 0, -0.25f),
                                _ => new Vec3f(0, 0, 0),
                            };
                            break;
                        case BlockFacing.indexWEST:
                            {
                                insidePos = slotId switch
                                {
                                    0 => new Vec3f(0.25f, 0, -0.25f),
                                    1 => new Vec3f(-0.25f, 0, -0.25f),
                                    2 => new Vec3f(0.25f, 0, 0.25f),
                                    3 => new Vec3f(-0.25f, 0, 0.25f),
                                    _ => new Vec3f(0, 0, 0),
                                };
                            }
                            break;
                    }
                    break;
                case EnumGroundStorageLayout.WallHalves:
                case EnumGroundStorageLayout.Halves:
                    {
                        switch (facing.Index)
                        {
                            case BlockFacing.indexNORTH:
                                insidePos = slotId switch
                                {
                                    0 => new Vec3f(0.25f, 0, 0),
                                    1 => new Vec3f(-0.25f, 0, 0),
                                    _ => new Vec3f(0, 0, 0),
                                };
                                break;
                            case BlockFacing.indexSOUTH:
                                insidePos = slotId switch
                                {
                                    0 => new Vec3f(-0.25f, 0, 0),
                                    1 => new Vec3f(0.25f, 0, 0),
                                    _ => new Vec3f(0, 0, 0),
                                };
                                break;
                            case BlockFacing.indexEAST:
                                insidePos = slotId switch
                                {
                                    0 => new Vec3f(0, 0, 0.25f),
                                    1 => new Vec3f(0, 0, -0.25f),
                                    _ => new Vec3f(0, 0, 0),
                                };
                                break;
                            case BlockFacing.indexWEST:
                                {
                                    insidePos = slotId switch
                                    {
                                        0 => new Vec3f(0, 0, -0.25f),
                                        1 => new Vec3f(0, 0, 0.25f),
                                        _ => new Vec3f(0, 0, 0),
                                    };
                                }
                                break;
                        }
                    }
                    break;
            }

            float maxY = begs.GetCollisionBoxes().FirstOrDefault().MaxY;
            Vec3d itemPos = new Vec3d(pos.X + 0.5 - insidePos.X, pos.Y + maxY, pos.Z + 0.5 - insidePos.Z);

            SimpleParticleProperties smokeheld = new SimpleParticleProperties(1f, 1f, ColorUtil.ToRgba(50, 220, 220, 220), minPos: itemPos, maxPos: itemPos, new Vec3f(-0.05f, 0.1f, -0.05f), new Vec3f(0.05f, 0.15f, 0.05f), 1.5f, 0f, 0.25f, 0.35f, EnumParticleModel.Quad);
            smokeheld.SelfPropelled = true;
            smokeheld.AddPos.Set(0, 0, 0);

            if (obj is BlockCookedContainer or BlockMeal && obj.GetTemperature(begs.Api.World, slot.Itemstack) > 50f)
            {
                if (begs.Api.World.Rand.NextDouble() >= 0.01)
                {
                    continue;
                }

                manager.Spawn(smokeheld);
            }
            else if (obj is BlockSmeltedContainer smeltedContainer)
            {
                if (begs.Api.World.Rand.NextDouble() >= 0.01)
                {
                    continue;
                }

                KeyValuePair<ItemStack, int> contents = smeltedContainer.CallMethod<KeyValuePair<ItemStack, int>>("GetContents", begs.Api.World, slot.Itemstack);
                if (contents.Key != null && !smeltedContainer.HasSolidifed(slot.Itemstack, contents.Key, begs.Api.World))
                {
                    manager.Spawn(smokeheld);
                }
            }
        }
    }
}

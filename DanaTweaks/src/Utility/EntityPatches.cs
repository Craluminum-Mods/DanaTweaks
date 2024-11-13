using System;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class EntityPatches
{
    public static void AddEntityBehaviors(this Entity entity)
    {
        if (entity is EntityPlayer)
        {
            if (Core.ConfigServer.PlayerWakesUpWhenHungry)
            {
                entity.AddBehavior(new EntityBehaviorHungryWakeUp(entity));
            }

            if (Core.ConfigServer.PlayerDropsHotSlots)
            {
                entity.AddBehavior(new EntityBehaviorDropHotSlots(entity));
            }

            if (Core.ConfigServer.ExtinctSubmergedTorchInEverySlot)
            {
                entity.AddBehavior(new EntityBehaviorExtinctSubmergedTorchInEverySlot(entity));
            }
        }
        if (entity is EntityItem)
        {
            if (Core.ConfigServer.AutoPlantDroppedTreeSeeds)
            {
                entity.AddBehavior(new EntityBehaviorAutoPlantDroppedTreeSeeds(entity));
            }
        }
        CreatureOpenDoors creatureOpenDoors = Core.ConfigServer.CreaturesOpenDoors.FirstOrDefault(keyVal => entity.WildCardMatchExt(keyVal.Key) && keyVal.Value.Enabled).Value;
        if (creatureOpenDoors != null)
        {
            JsonObject jsonAttributes = creatureOpenDoors.GetAsAttributes();
            EntityBehaviorOpenDoors behavior = new EntityBehaviorOpenDoors(entity);
            behavior.Initialize(entity.Properties, jsonAttributes);
            entity.AddBehavior(behavior);
        }
    }

    public static void SetGlowLevel(this Entity entity)
    {
        if (Core.ConfigClient.GlowingProjectiles && (entity is EntityProjectile || entity.Class.Contains("projectile", StringComparison.OrdinalIgnoreCase)))
        {
            entity.Properties.Client.GlowLevel = 255;
        }
    }
}
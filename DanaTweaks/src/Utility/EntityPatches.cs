using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using Vintagestory.API.Util;

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
        CreatureOpenDoors creatureOpenDoors = Core.ConfigServer.CreaturesOpenDoors.FirstOrDefault(keyVal => WildcardUtil.Match(AssetLocation.Create(keyVal.Key), entity.Code) && keyVal.Value.Enabled).Value;
        if (creatureOpenDoors != null)
        {
            JsonObject jsonAttributes = creatureOpenDoors.GetAsAttributes();
            EntityBehaviorOpenDoors behavior = new EntityBehaviorOpenDoors(entity);
            behavior.Initialize(entity.Properties, jsonAttributes);
            entity.AddBehavior(behavior);
        }
    }
}
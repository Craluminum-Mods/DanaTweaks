using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.GameContent;

namespace DanaTweaks;

public class EntityBehaviorHungryWakeUp : EntityBehavior
{
    public EntityBehaviorHungryWakeUp(Entity entity) : base(entity) { }

    public override void OnGameTick(float deltaTime)
    {
        TryWakeUp();
    }

    private void TryWakeUp()
    {
        if (entity is not EntityPlayer entityPlayer)
        {
            return;
        }

        EntityBehaviorTiredness behaviorTiredness = entity.GetBehavior<EntityBehaviorTiredness>();

        if (entityPlayer.MountedOn == null || behaviorTiredness?.IsSleeping != true)
        {
            return;
        }

        if (entity.GetBehavior<EntityBehaviorHunger>().Saturation == 0)
        {
            entityPlayer.TryUnmount();
        }

        return;
    }

    public override string PropertyName() => "danatweaks:hungrywakeup";
}
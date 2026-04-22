using Vintagestory.API.Common;

namespace DanaTweaks;

public class Recipes : ModSystem
{
    public override double ExecuteOrder() => 1.1;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api.Side != EnumAppSide.Server)
        {
            return;
        }

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            if (Core.ConfigServer.FourPlanksFromLog)
            {
                if (recipe.Output.ResolvedItemStack != null && recipe.HasLogAsIngredient() && recipe.Output.IsPlank())
                {
                    recipe.Output.ResolvedItemStack.StackSize = 4;
                }
            }
        }
    }
}
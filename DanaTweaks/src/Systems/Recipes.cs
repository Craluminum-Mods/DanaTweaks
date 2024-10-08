using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace DanaTweaks;

public class Recipes : ModSystem
{
    public GridRecipeLoader GridRecipeLoader { get; set; }

    public static List<GridRecipe> GroundStorableRecipes { get; protected set; } = new();
 
    public override double ExecuteOrder() => 1.1;

    public override void AssetsLoaded(ICoreAPI api)
    {
        if (api.Side != EnumAppSide.Server)
        {
            return;
        }

        GridRecipeLoader = api.ModLoader.GetModSystem<GridRecipeLoader>();

        if (Core.ConfigServer.ScrapRecipes)
        {
            GridRecipeLoader.LoadRecipe(null, CreateTorchholderScrapRecipe(api));
            GridRecipeLoader.LoadRecipe(null, CreateMetalBlockScrapRecipe(api));
        }

        foreach (GridRecipe recipe in api.World.GridRecipes)
        {
            if (Core.ConfigServer.FourPlanksFromLog)
            {
                if (recipe.Output.ResolvedItemstack != null && recipe.HasLogAsIngredient() && recipe.Output.IsPlank())
                {
                    recipe.Output.ResolvedItemstack.StackSize = 4;
                }
            }
        }
    }

    public override void AssetsFinalize(ICoreAPI api)
    {
        foreach (GridRecipe _recipe in api.World.GridRecipes)
        {
            if (_recipe.IngredientPattern.Replace("_", "").Length != 2
                || _recipe.resolvedIngredients.Length != 2
                || !_recipe.resolvedIngredients.Any(ingred => ingred.Quantity == 1))
            {
                continue;
            }

            GroundStorableRecipes.Add(_recipe);

            // for debugging
            //Vintagestory.API.Util.EnumerableExtensions.Foreach(_recipe.resolvedIngredients,x =>
            //{
            //    CollectibleObject obj = x?.ResolvedItemstack?.Collectible;
            //    if (obj == null
            //        || obj?.CreativeInventoryTabs == null
            //        || obj?.CreativeInventoryTabs?.Contains("danatweaks:immersivecrafting-2") == true
            //        || !obj.HasBehavior<Vintagestory.GameContent.CollectibleBehaviorGroundStorable>())
            //    {
            //        return;
            //    }

            //    obj.CreativeInventoryTabs = Vintagestory.API.Util.ArrayExtensions.Append(obj.CreativeInventoryTabs, "danatweaks:immersivecrafting-2");
            //});
        }
    }

    public override void Dispose()
    {
        GroundStorableRecipes.Clear();
    }

    private static GridRecipe CreateTorchholderScrapRecipe(ICoreAPI api)
    {
        GridRecipe recipe = new()
        {
            Name = new AssetLocation("torcholder recycle"),
            IngredientPattern = "CI",
            RecipeGroup = 9,
            Width = 1,
            Height = 2,
            Ingredients = new Dictionary<string, CraftingRecipeIngredient>
            {
                ["C"] = new CraftingRecipeIngredient()
                {
                    Type = EnumItemClass.Item,
                    Code = new AssetLocation("chisel-*"),
                    IsTool = true,
                    Quantity = 1
                },
                ["I"] = new CraftingRecipeIngredient()
                {
                    Type = EnumItemClass.Block,
                    Code = new AssetLocation("torchholder-brass-*"),
                    Quantity = 1
                }
            },
            Output = new CraftingRecipeIngredient()
            {
                Type = EnumItemClass.Item,
                Code = new AssetLocation("metalbit-brass"),
                Quantity = 40
            }
        };

        recipe.ResolveIngredients(api.World);
        return recipe;
    }

    private static GridRecipe CreateMetalBlockScrapRecipe(ICoreAPI api)
    {
        GridRecipe recipe = new()
        {
            Name = new AssetLocation("metalblock recycle"),
            IngredientPattern = "CI",
            RecipeGroup = 10,
            Width = 1,
            Height = 2,
            Ingredients = new Dictionary<string, CraftingRecipeIngredient>
            {
                ["C"] = new CraftingRecipeIngredient()
                {
                    Type = EnumItemClass.Item,
                    Code = new AssetLocation("chisel-*"),
                    IsTool = true,
                    Quantity = 1
                },
                ["I"] = new CraftingRecipeIngredient()
                {
                    Type = EnumItemClass.Block,
                    Code = new AssetLocation("metalblock-new-riveted-*"),
                    Name = "metal",
                    Quantity = 1
                }
            },
            Output = new CraftingRecipeIngredient()
            {
                Type = EnumItemClass.Item,
                Code = new AssetLocation("metalbit-{metal}"),
                Quantity = 240
            }
        };

        recipe.ResolveIngredients(api.World);
        return recipe;
    }
}
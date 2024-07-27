using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.ServerMods;

namespace DanaTweaks;

public class Recipes : ModSystem
{
    public GridRecipeLoader GridRecipeLoader { get; set; }

    public override bool ShouldLoad(EnumAppSide forSide) => forSide == EnumAppSide.Server;

    public override double ExecuteOrder() => 1.01;

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
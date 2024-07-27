using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class PitkilnPatches
{
    public static void PatchBuildMats(this Block blockPitKiln, ICoreAPI api)
    {
        List<JsonItemStackBuildStage> planks = new();

        Item[] items = api.World.SearchItems(new AssetLocation("plank-*"));
        items = items.Append(api.World.SearchItems(new AssetLocation("wildcrafttree:plank-*")));

        foreach (Item item in items)
        {
            if (item.IsMissing)
            {
                continue;
            }

            item.EnsureAttributesNotNull();
            item.Attributes.Token["placeSound"] = JToken.FromObject("block/planks");

            planks.Add(new JsonItemStackBuildStage()
            {
                Type = item.ItemClass,
                Code = item.Code,
                Quantity = 4,
                EleCode = "Plank"
            });
        }

        JsonItemStackBuildStage[] sticks = blockPitKiln.Attributes["buildMats"]["sticks"].AsObject<JsonItemStackBuildStage[]>();
        sticks[0].EleCode = "Stick";
        sticks = sticks.Concat(planks).ToArray();

        blockPitKiln.EnsureAttributesNotNull();
        blockPitKiln.Attributes.Token["buildMats"]["sticks"] = JToken.FromObject(sticks);
    }

    public static void PatchModelConfigs(this Block blockPitKiln)
    {
        Dictionary<string, PitKilnModelConfig> modelConfigs = blockPitKiln.Attributes["modelConfigs"].AsObject<Dictionary<string, PitKilnModelConfig>>();

        foreach (PitKilnModelConfig val in modelConfigs.Values)
        {
            val.BuildStages[5] = "{eleCode}layer1/*";
            val.BuildStages[6] = "{eleCode}layer2/*";
        }

        blockPitKiln.EnsureAttributesNotNull();
        blockPitKiln.Attributes.Token["modelConfigs"] = JToken.FromObject(modelConfigs);
    }
}
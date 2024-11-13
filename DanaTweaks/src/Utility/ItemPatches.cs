using DanaTweaks.Configuration;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class ItemPatches
{
    public static void PatchScythe(this Item item, List<string> scytheMorePrefixes)
    {
        if (Core.ConfigServer.ScytheMore.Enabled && item is ItemScythe)
        {
            item.EnsureAttributesNotNull();

            List<string> codePrefixes = item?.Attributes?["codePrefixes"]?.AsObject<List<string>>();
            List<string> disallowedSuffixes = item?.Attributes?["disallowedSuffixes"]?.AsObject<List<string>>();

            if (codePrefixes?.Any() == true)
            {
                codePrefixes.AddRange(scytheMorePrefixes.Except(codePrefixes));
                item.Attributes.Token["codePrefixes"] = JToken.FromObject(codePrefixes);
            }
            if (disallowedSuffixes?.Any() == true)
            {
                disallowedSuffixes.AddRange(Core.ConfigServer.ScytheMore.DisallowedSuffixes.Except(disallowedSuffixes));
                item.Attributes.Token["disallowedSuffixes"] = JToken.FromObject(disallowedSuffixes);
            }
        }
    }

    public static void PatchCrockSealant(this Item item)
    {
        if (Core.ConfigServer.SealCrockExtraInteractions && item.WildCardMatchExt("@(beeswax|fat)"))
        {
            item.EnsureAttributesNotNull();
            item.Attributes.Token["canSealCrock"] = JToken.FromObject(true);
        }
    }
    
    public static void PatchFirestarter(this Item item)
    {
        if (Core.ConfigServer.RackableFirestarter && item is ItemFirestarter)
        {
            item.EnsureAttributesNotNull();
            item.Attributes.Token["rackable"] = JToken.FromObject(true);
            item.Attributes.Token["toolrackTransform"] = JToken.FromObject(new ModelTransform()
            {
                Translation = new() { X = 0.25f, Y = 0.55f, Z = 0.0275f },
                Rotation = new() { X = 180, Y = -135, Z = 0 },
                Origin = new() { X = 0.5f, Y = 0f, Z = 0.5f },
                Scale = 0.7f
            });
        }
    }

    public static void PatchBranchCutter(this Item item)
    {
        if (Core.ConfigServer.BranchCutter && item is ItemShears)
        {
            item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorBranchCutter(item));
        }
    }

    public static void PatchBook(this Item item)
    {
        if (Core.ConfigServer.RemoveBookSignature && item is ItemBook)
        {
            item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorRemoveBookSignature(item));
        }
    }

    public static void PatchOvenFuel(this Item item)
    {
        OvenFuel ovenFuel = Core.ConfigServer.OvenFuelItems.FirstOrDefault(keyVal => item.WildCardMatchExt(keyVal.Key) && keyVal.Value.Enabled).Value;
        if (ovenFuel == null)
        {
            return;
        }
        item.EnsureAttributesNotNull();
        item.Attributes.Token["isClayOvenFuel"] = JToken.FromObject(true);
        string model = ovenFuel.Model;
        if (!string.IsNullOrEmpty(model))
        {
            item.Attributes.Token["ovenFuelShape"] = JToken.FromObject(model);
        }
    }

    public static void PatchWaxForCheese(this Item item)
    {
        if (Core.ConfigServer.WaxCheeseOnGround && item?.Attributes?["waxCheeseOnGround"]?.AsBool() == true)
        {
            item.CollectibleBehaviors = item.CollectibleBehaviors.Append(new CollectibleBehaviorWaxCheeseOnGround(item));
        }
    }
}
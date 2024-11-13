using Newtonsoft.Json.Linq;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.MathTools;

namespace DanaTweaks;

public static class EntityTypePatches
{
    public static void FillTradersList(this EntityProperties entityType, ref bool any)
    {
        if (!entityType.Code.ToString().Contains("trader") && !entityType.Class.Contains("trader") || Core.ConfigServer.RichTradersList.ContainsKey(entityType.Code.ToString()))
        {
            return;
        }
        any = true;
        Core.ConfigServer.RichTradersList.Add(entityType.Code.ToString(), new NatFloat(averagevalue: 9999f, variance: 0, EnumDistribution.UNIFORM));
    }
    
    public static void MakeTraderRich(this EntityProperties entityType)
    {
        if (Core.ConfigServer.RichTraders && Core.ConfigServer.RichTradersList.TryGetValue(entityType.Code.ToString(), out NatFloat richTraderValue))
        {
            entityType.EnsureAttributesNotNull();
            entityType.Attributes.Token["tradeProps"]["money"]["avg"] = JToken.FromObject(richTraderValue.avg);
            entityType.Attributes.Token["tradeProps"]["money"]["var"] = JToken.FromObject(richTraderValue.var);
        }
    }
}
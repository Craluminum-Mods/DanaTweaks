using Newtonsoft.Json.Linq;
using Vintagestory.API.Datastructures;

namespace DanaTweaks;

public class CreatureOpenDoors
{
    public bool Enabled { get; set; }
    public float Cooldown { get; set; }
    public float Range { get; set; }

    public JsonObject GetAsAttributes()
    {
        var jsonAttributes = new { range = Range, cooldown = Cooldown };
        return new JsonObject(JToken.FromObject(jsonAttributes));
    }
}
using System.Collections.Generic;

namespace DanaTweaks;

public class OvenFuels
{
    public Dictionary<string, OvenFuel> Items { get; set; } = new()
    {
        ["plank-*"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/plankpile" }
    };
    public Dictionary<string, OvenFuel> Blocks { get; set; } = new()
    {
        ["peatbrick"] = new OvenFuel() { Enabled = true, Model = "danatweaks:block/ovenfuel/peatpile" }
    };
}

public class OvenFuel
{
    public bool Enabled { get; set; }
    public string Model { get; set; }
}
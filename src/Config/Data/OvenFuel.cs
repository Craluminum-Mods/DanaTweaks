using System.Collections.Generic;

namespace DanaTweaks;

public class OvenFuel
{
    public Dictionary<string, bool> Items { get; set; } = new()
    {
        ["plank-*"] = true
    };
    public Dictionary<string, bool> Blocks { get; set; } = new()
    {
        ["peatbrick"] = true
    };
}
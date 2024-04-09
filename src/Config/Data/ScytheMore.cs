using System.Collections.Generic;

namespace DanaTweaks;

public class ScytheMore
{
    public bool Enabled { get; set; } = true;
    public List<string> DisallowedParts { get; set; }
    public List<string> DisallowedSuffixes { get; set; }

    public static List<string> DefaultDisallowedParts()
    {
        return new() { "bush", "cactus", "clipping", "glowworms", "hay", "hotspringbacteria", "mushroom", "roofing", "sapling", "seedling", "vine", "waterlily", };
    }

    public static List<string> DefaultDisallowedSuffixes()
    {
        return new() { "empty", "flowering", "harvested-free", "harvested", };
    }
}
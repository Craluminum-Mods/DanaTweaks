using HarmonyLib;
using Vintagestory.API.Common;
using Vintagestory.GameContent;

namespace DanaTweaks;

public partial class HarmonyPatches : ModSystem
{
    public const string HarmonyID = "danatweaks";

    public override void Start(ICoreAPI api)
    {
        base.Start(api);
        if (Core.Config.DropClutterAnyway)
        {
            new Harmony(HarmonyID).Patch(
                original: typeof(BlockClutter).GetMethod(nameof(BlockClutter.GetDrops)),
                prefix: typeof(BlockClutter_GetDrops_Patch).GetMethod(nameof(BlockClutter_GetDrops_Patch.Prefix)));
        }
    }

    public override void Dispose()
    {
        if (Core.Config.DropClutterAnyway)
        {
            new Harmony(HarmonyID).Unpatch(original: typeof(BlockClutter).GetMethod(nameof(BlockClutter.GetDrops)), HarmonyPatchType.All, HarmonyID);
        }
        base.Dispose();
    }
}
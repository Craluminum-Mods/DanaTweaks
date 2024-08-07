using HarmonyLib;
using System.Reflection;
using Vintagestory.API.Client;
using Vintagestory.GameContent;

namespace DanaTweaks;

public static class GuiElementMap_OnKeyDown_Patch
{
    public const float add = 0.25f;
    public const float substract = -0.25f;

    public static MethodBase TargetMethod()
    {
        return AccessTools.Method(typeof(GuiElementMap), nameof(GuiElementMap.OnKeyDown), new[] { typeof(ICoreClientAPI), typeof(KeyEvent) });
    }

    public static MethodInfo GetPostfix() => typeof(GuiElementMap_OnKeyDown_Patch).GetMethod(nameof(Postfix));

    public static void Postfix(GuiElementMap __instance, ICoreClientAPI api)
    {
        if (IsKey(api, (int)GlKeys.PageUp) || IsKey(api, (int)GlKeys.Plus))
        {
            __instance.ZoomAdd(
                zoomDiff: add,
                px: (float)((api.Input.MouseX - __instance.Bounds.absX) / __instance.Bounds.InnerWidth),
                pz: (float)((api.Input.MouseY - __instance.Bounds.absY) / __instance.Bounds.InnerHeight));
        }
        else if (IsKey(api, (int)GlKeys.PageDown) || IsKey(api, (int)GlKeys.Minus))
        {
            __instance.ZoomAdd(
                zoomDiff: substract,
                px: (float)((api.Input.MouseX - __instance.Bounds.absX) / __instance.Bounds.InnerWidth),
                pz: (float)((api.Input.MouseY - __instance.Bounds.absY) / __instance.Bounds.InnerHeight));
        }
    }

    private static bool IsKey(ICoreClientAPI api, int key) => api.Input.KeyboardKeyStateRaw[key];
}
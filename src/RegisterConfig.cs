using Vintagestory.API.Common;

namespace CraluTweaks.Utility
{
  class RegisterConfig : ModSystem
  {
    public override void StartPre(ICoreAPI api)
    {
      base.StartPre(api);

      ModConfig.ReadConfig(api);
      api.World.Logger.Event("started 'CraluTweaks' mod");
    }
  }
}
namespace DanaTweaks;

public class RainCollector
{
    public bool Enabled = true;
    public string LiquidCode = "game:waterportion";
    public float LitresPerSecond = 0.01f;
    public float MinPrecipitation = 0.04f;
    public int UpdateEveryMs = 1000;
}
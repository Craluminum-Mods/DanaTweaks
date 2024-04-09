namespace DanaTweaks;

public class RainCollector
{
    public bool Enabled { get; set; } = true;
    public float LitresPerUpdate { get; set; } = 0.01f;
    public float MinPrecipitation { get; set; } = 0.04f;
    public int UpdateMilliseconds { get; set; } = 1000;
    public string LiquidCode { get; set; } = "game:waterportion";
}
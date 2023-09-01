namespace DanaTweaks;

public class RainCollector
{
    public bool Enabled { get; set; } = true;
    public float LitresPerSecond { get; set; } = 0.01f;
    public float MinPrecipitation { get; set; } = 0.04f;
    public int UpdateEveryMs { get; set; } = 1000;
    public string LiquidCode { get; set; } = "game:waterportion";
}
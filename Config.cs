namespace RealisticBossHP;

internal class Config
{
	public BossCfg Boss { get; set; } = new BossCfg();
	public RaiderCfg Raider { get; set; } = new RaiderCfg();
}

internal class BossCfg
{
	public bool Enabled { get; set; }
	public double Head { get; set; }
	public double Chest { get; set; }
	public double Stomach { get; set; }
	public double LeftArm { get; set; }
	public double RightArm { get; set; }
	public double LeftLeg { get; set; }
	public double RightLeg { get; set; }
}

internal class RaiderCfg : BossCfg { }
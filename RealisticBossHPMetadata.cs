using SPTarkov.Server.Core.Models.Spt.Mod;

namespace DrakiaXYZ.LiveFleaPrices;

public record ModMetadata : AbstractModMetadata
{
	public override string ModGuid { get; init; } = "net.limbofps.realisticbosshp";
	public override string Name { get; init; } = "RealisticBossHP";
	public override string Author { get; init; } = "LimboFPS";
	public override List<string>? Contributors { get; init; }
	public override SemanticVersioning.Version Version { get; init; } = new("2.0.0");
	public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
	public override List<string>? Incompatibilities { get; init; }
	public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
	public override string? Url { get; init; } = "https://github.com/StuckInLimbo/RealisticBossHP";
	public override bool? IsBundleMod { get; init; } = false;
	public override string? License { get; init; } = "CC BY-NC-SA 4.0";
}
using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using System.Reflection;

namespace RealisticBossHP;

// We want to load after PostDBModLoader is complete, so we set our type priority to that, plus 1.
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class RealisticBossHP(ISptLogger<RealisticBossHP> logger, DatabaseService databaseService, ModHelper modHelper) : IOnLoad
{
	private Config HPConfig = new();

	private readonly string[] BossIDs = { 
		"bossboar",
		"bossboarsniper",
		"bossbully",
		"bossgluhar",
		"bosskilla",
		"bosskillaagro",
		"bossknight",
		"bosskojaniy",
		"bosskolontay",
		"bosspartisan",
		"bosssanitar",
		"bosstagilla",
		"bosstagillaagro", // Shadow of Tagilla
		"bosszryachiy",
		"followerbigpipe", 
		"followerbirdeye",
		"sectantoni",
		"sectantpredvestnik",
		"sectantpriest",
		"sectantprizrak",
	};

	private readonly string[] RaiderIDs =
	{
		"exusec",
		"followerboar",
		"followerboarclose1",
		"followerboarclose2",
		"followerbully",
		"followergluharassault",
		"followergluharscout",
		"followergluharsecurity",
		"followerkojaniy",
		"followerkolontayassault",
		"followerkolontaysecurity",
		"followersanitar",
		"followerzryachiy",
		"sectantwarrior",
		"tagillahelperagro",
	};

	public Task OnLoad()
	{
		// Load our config file
		try
		{
			var pathToMod = modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly());
			HPConfig = modHelper.GetJsonDataFromFile<Config>(pathToMod, "config.json");
		}
		catch (Exception ex)
		{
			logger.Error($"[RealisticBossHP] Failed to load config file: {ex}");
			return Task.CompletedTask;
		}

		// When SPT starts, it stores all the data found in (SPT_Data\Server\database) in memory
		// We can use the 'databaseService' we injected to access this data, this includes files from EFT and SPT
		if (HPConfig.boss_Enabled)
			EditBossSettings();
		
		if (HPConfig.raider_Enabled)
			EditRaiderSettings();

		// lets write a nice log message to the server console so players know our mod has made changes
		logger.Success("[RealisticBossHP] Mod loaded!");

		// Inform server we have finished
		return Task.CompletedTask;
	}

	private void EditBossSettings()
	{
		var bots = databaseService.GetBots();

		bots.Types.TryGetValue("bear", out var bearBot);
		var bossBody = CreateBossBody(bearBot.BotHealth.BodyParts.ToArray());

		foreach (var boss in BossIDs)
		{
			try
			{
				logger.Debug($"[RealisticBossHP] Editing boss {boss} HP settings.");
				bots.Types.TryGetValue(boss, out var bossBot);
				bossBot.BotHealth.BodyParts = bossBody;
			}
			catch (Exception ex)
			{
				logger.Error($"[RealisticBossHP] Failed to edit boss {boss}: {ex}");
			}
		}
	}

	private void EditRaiderSettings()
	{
		var bots = databaseService.GetBots();

		bots.Types.TryGetValue("bear", out var bearBot);
		var raiderBody = CreateRaiderBody(bearBot.BotHealth.BodyParts.ToArray());

		foreach (var raider in RaiderIDs)
		{
			try {
				logger.Debug($"[RealisticBossHP] Editing boss {raider} HP settings.");
				bots.Types.TryGetValue(raider, out var raiderBot);
				raiderBot.BotHealth.BodyParts = raiderBody;
			}
			catch (Exception ex)
			{
				logger.Error($"[RealisticBossHP] Failed to edit raider {raider}: {ex}");
			}
		}
	}

	private IEnumerable<BodyPart> CreateBossBody(BodyPart[] bodyParts)
	{
		return bodyParts.Select(bodyParts =>
		{
			bodyParts.Head.Min		= HPConfig.boss_Head;
			bodyParts.Head.Max		= HPConfig.boss_Head;
			bodyParts.Chest.Min		= HPConfig.boss_Chest;
			bodyParts.Chest.Max		= HPConfig.boss_Chest;
			bodyParts.Stomach.Min	= HPConfig.boss_Stomach;
			bodyParts.Stomach.Max	= HPConfig.boss_Stomach;
			bodyParts.LeftArm.Min	= HPConfig.boss_LeftArm;
			bodyParts.LeftArm.Max	= HPConfig.boss_LeftArm;
			bodyParts.RightArm.Min	= HPConfig.boss_RightArm;
			bodyParts.RightArm.Max	= HPConfig.boss_RightArm;
			bodyParts.LeftLeg.Min	= HPConfig.boss_LeftLeg;
			bodyParts.LeftLeg.Max	= HPConfig.boss_LeftLeg;
			bodyParts.RightLeg.Min	= HPConfig.boss_RightLeg;
			bodyParts.RightLeg.Max	= HPConfig.boss_RightLeg;
			return bodyParts;
		});
	}

	private IEnumerable<BodyPart> CreateRaiderBody(BodyPart[] bodyParts)
	{
		return bodyParts.Select(bodyParts =>
		{
			bodyParts.Head.Min		= HPConfig.raider_Head;
			bodyParts.Head.Max		= HPConfig.raider_Head;
			bodyParts.Chest.Min		= HPConfig.raider_Chest;
			bodyParts.Chest.Max		= HPConfig.raider_Chest;
			bodyParts.Stomach.Min	= HPConfig.raider_Stomach;
			bodyParts.Stomach.Max	= HPConfig.raider_Stomach;
			bodyParts.LeftArm.Min	= HPConfig.raider_LeftArm;
			bodyParts.LeftArm.Max	= HPConfig.raider_LeftArm;
			bodyParts.RightArm.Min	= HPConfig.raider_RightArm;
			bodyParts.RightArm.Max	= HPConfig.raider_RightArm;
			bodyParts.LeftLeg.Min	= HPConfig.raider_LeftLeg;
			bodyParts.LeftLeg.Max	= HPConfig.raider_LeftLeg;
			bodyParts.RightLeg.Min	= HPConfig.raider_RightLeg;
			bodyParts.RightLeg.Max	= HPConfig.raider_RightLeg;
			return bodyParts;
		});
	}
}


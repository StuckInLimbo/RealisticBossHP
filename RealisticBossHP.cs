using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Common;
using SPTarkov.Server.Core.Models.Eft.Common.Tables;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using System.Reflection;

namespace RealisticBossHP;

// We want to load after PostDBModLoader is complete, and after any other mod that could adjust bots.
[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 999)]
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
		if (HPConfig.Boss.Enabled)
			EditBossSettings();
		
		if (HPConfig.Raider.Enabled)
			EditRaiderSettings();

		// lets write a nice log message to the server console so players know our mod has made changes
		logger.Success("[RealisticBossHP] Mod loaded!");

		// Inform server we have finished
		return Task.CompletedTask;
	}

	private void EditBossSettings()
	{
		var bots = databaseService.GetBots();

		foreach (var boss in BossIDs)
		{
			try
			{
				logger.Debug($"[RealisticBossHP] Editing boss {boss} HP settings.");
				bots.Types.TryGetValue(boss, out var bossBot);
				if (bossBot == null)
				{
					logger.Error($"[RealisticBossHP] Boss ID {boss} not found in database.");
					continue;
			}
				foreach (var bodyPart in bossBot.BotHealth.BodyParts)
				{
					SetHealth(bodyPart.Head, HPConfig.Boss.Head);
					SetHealth(bodyPart.Chest, HPConfig.Boss.Chest);
					SetHealth(bodyPart.Stomach, HPConfig.Boss.Stomach);
					SetHealth(bodyPart.LeftArm, HPConfig.Boss.LeftArm);
					SetHealth(bodyPart.RightArm, HPConfig.Boss.RightArm);
					SetHealth(bodyPart.LeftLeg, HPConfig.Boss.LeftLeg);
					SetHealth(bodyPart.RightLeg, HPConfig.Boss.RightLeg);
				}
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

		foreach (var raider in RaiderIDs)
		{
			try {
				logger.Debug($"[RealisticBossHP] Editing boss {raider} HP settings.");
				bots.Types.TryGetValue(raider, out var raiderBot);
				if (raiderBot == null)
				{
					logger.Error($"[RealisticBossHP] Raider ID {raider} not found in database.");
					continue;
			}
				foreach (var bodyPart in raiderBot.BotHealth.BodyParts)
				{
					SetHealth(bodyPart.Head, HPConfig.Raider.Head);
					SetHealth(bodyPart.Chest, HPConfig.Raider.Chest);
					SetHealth(bodyPart.Stomach, HPConfig.Raider.Stomach);
					SetHealth(bodyPart.LeftArm, HPConfig.Raider.LeftArm);
					SetHealth(bodyPart.RightArm, HPConfig.Raider.RightArm);
					SetHealth(bodyPart.LeftLeg, HPConfig.Raider.LeftLeg);
					SetHealth(bodyPart.RightLeg, HPConfig.Raider.RightLeg);
				}
			}
			catch (Exception ex)
			{
				logger.Error($"[RealisticBossHP] Failed to edit raider {raider}: {ex}");
			}
		}
	}

	private void SetHealth(MinMax<double> bodyPart, double health)
	{
		bodyPart.Min = health;
		bodyPart.Max = health;
	}
}


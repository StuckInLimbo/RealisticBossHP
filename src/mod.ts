import { DependencyContainer } from "tsyringe";

import { IPostDBLoadMod } from "@spt/models/external/IPostDBLoadMod";
import { DatabaseServer } from "@spt/servers/DatabaseServer";
import { IDatabaseTables } from "@spt/models/spt/server/IDatabaseTables";
import { ILogger } from "@spt/models/spt/utils/ILogger";

import * as config from "../cfg/config.json";

// EFT BodyPart interfaces
interface BodyPart 
{
    max: number;
    min: number;
}

interface BodyParts 
{
    Chest: BodyPart;
    Head: BodyPart;
    LeftArm: BodyPart;
    LeftLeg: BodyPart;
    RightArm: BodyPart;
    RightLeg: BodyPart;
    Stomach: BodyPart;
}

// Create array of IDs
const bossIDs: string[] = [
    "bossbully",
    "bossboar",
    "bossboarsniper",
    "bossgluhar",
    "bosskilla",
    "bossknight",
    "followerbigpipe",
    "followerbirdeye",
    "bosskojaniy",
    "bosskolontay",
    "bosssanitar",
    "bosstagilla",
    "bosszryachiy"
];

const raiderIDs: string[] = [
    "followerboar",
    "followerboarclose1",
    "followerboarclose2",
    "followerbully",
    "followergluharassault",
    "followergluharscout",
    "followergluharsecurity",
    "followergluharsnipe",
    "followerkojaniy",
    "followerkojaniyassault",
    "followerkojaniysecurity",
    "followersanitar",
    "followertagilla",
    "followerzryachiy",
    "sectantpriest",
    "sectantwarrior",
    "sectactpriestevent",
    "exusec"
];

class Mod implements IPostDBLoadMod
{
    public postDBLoad(container: DependencyContainer): void
    {
        // Create logger object
        const logger = container.resolve<ILogger>("WinstonLogger");
        // Load up the SPT database
        const databaseServer = container.resolve<DatabaseServer>("DatabaseServer");
        // Get all tables
        const tables: IDatabaseTables = databaseServer.getTables();

        const tempBodyPartsArray = tables.bots.types.bear.health.BodyParts;
        const bossBody = createNewBody(tempBodyPartsArray, true);
        const raiderBody = createNewBody(tempBodyPartsArray, false);

        if (config.Boss.Enabled) 
            bossIDs.forEach(e => 
            {
            // Get current element from db tables
                logger.info("Changing " + e);
                const bot = tables.bots.types[e];
                if (bot) 
                    bot.health.BodyParts = bossBody;
            });

        if (config.Raider.Enabled) 
            raiderIDs.forEach((e) => 
            {
            // Get current element from db tables
                logger.info("Changing " + e);
                const bot = tables.bots.types[e];
                if (bot) 
                    bot.health.BodyParts = raiderBody;
            });

        function createNewBody(bodyParts: BodyParts[], isBoss: boolean): BodyParts[] 
        {
            if (isBoss) 
            {
                return bodyParts.map(() => 
                {
                    return {
                        Chest: {
                            max: config.Boss.Chest,
                            min: config.Boss.Chest
                        },
                        Head: {
                            max: config.Boss.Head,
                            min: config.Boss.Head
                        },
                        LeftArm: {
                            max: config.Boss.LeftArm,
                            min: config.Boss.LeftArm
                        },
                        LeftLeg: {
                            max: config.Boss.LeftLeg,
                            min: config.Boss.LeftLeg
                        },
                        RightArm: {
                            max: config.Boss.RightArm,
                            min: config.Boss.RightArm
                        },
                        RightLeg: {
                            max: config.Boss.RightLeg,
                            min: config.Boss.RightLeg
                        },
                        Stomach: {
                            max: config.Boss.Stomach,
                            min: config.Boss.Stomach
                        }
                    };
                });
            }
            else 
            {
                return bodyParts.map(() => 
                {
                    return {
                        Chest: {
                            max: config.Raider.Chest,
                            min: config.Raider.Chest
                        },
                        Head: {
                            max: config.Raider.Head,
                            min: config.Raider.Head
                        },
                        LeftArm: {
                            max: config.Raider.LeftArm,
                            min: config.Raider.LeftArm
                        },
                        LeftLeg: {
                            max: config.Raider.LeftLeg,
                            min: config.Raider.LeftLeg
                        },
                        RightArm: {
                            max: config.Raider.RightArm,
                            min: config.Raider.RightArm
                        },
                        RightLeg: {
                            max: config.Raider.RightLeg,
                            min: config.Raider.RightLeg
                        },
                        Stomach: {
                            max: config.Raider.Stomach,
                            min: config.Raider.Stomach
                        }
                    };
                });
            }
        }
    }
}

export const mod = new Mod();

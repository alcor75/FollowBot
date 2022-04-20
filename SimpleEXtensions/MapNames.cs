using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game.GameData;

namespace FollowBot.SimpleEXtensions
{
    [SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public static class MapNames
    {
        [AreaId("MapWorldsLookout")]
        public static readonly string Lookout;

        [AreaId("MapWorldsBeach")]
        public static readonly string Beach;

        [AreaId("MapWorldsGraveyard")]
        public static readonly string Graveyard;

        [AreaId("MapWorldsDungeon")]
        public static readonly string Dungeon;

        [AreaId("MapWorldsAlleyways")]
        public static readonly string Alleyways;

        [AreaId("MapWorldsPen")]
        public static readonly string Pen;

        [AreaId("MapWorldsDesert")]
        public static readonly string Desert;

        [AreaId("MapWorldsAridLake")]
        public static readonly string AridLake;

        [AreaId("MapWorldsFloodedMine")]
        public static readonly string FloodedMine;

        [AreaId("MapWorldsMarshes")]
        public static readonly string Marshes;

        [AreaId("MapWorldsIceberg")]
        public static readonly string Iceberg;

        [AreaId("MapWorldsCage")]
        public static readonly string Cage;

        [AreaId("MapWorldsSprings")]
        public static readonly string FungalHollow;

        [AreaId("MapWorldsExcavation")]
        public static readonly string Excavation;

        [AreaId("MapWorldsLeyline")]
        public static readonly string Leyline;

        [AreaId("MapWorldsPeninsula")]
        public static readonly string Peninsula;

        [AreaId("MapWorldsPort")]
        public static readonly string Port;

        [AreaId("MapWorldsBurialChambers")]
        public static readonly string BurialChambers;

        [AreaId("MapWorldsCells")]
        public static readonly string Cells;

        [AreaId("MapWorldsArcade")]
        public static readonly string Arcade;

        [AreaId("MapWorldsCitySquare")]
        public static readonly string CitySquare;

        [AreaId("MapWorldsRelicChambers")]
        public static readonly string RelicChambers;

        [AreaId("MapWorldsCourthouse")]
        public static readonly string Courthouse;

        [AreaId("MapWorldsStrand")]
        public static readonly string Strand;

        [AreaId("MapWorldsChateau")]
        public static readonly string Chateau;

        [AreaId("MapWorldsGrotto")]
        public static readonly string Grotto;

        [AreaId("MapWorldsGorge")]
        public static readonly string Glacier;

        [AreaId("MapWorldsVolcano")]
        public static readonly string Volcano;

        [AreaId("MapWorldsLighthouse")]
        public static readonly string Lighthouse;

        [AreaId("MapWorldsCanyon")]
        public static readonly string Canyon;

        [AreaId("MapWorldsConservatory")]
        public static readonly string Conservatory;

        [AreaId("MapWorldsSulphurVents")]
        public static readonly string SulphurVents;

        [AreaId("MapWorldsHauntedMansion")]
        public static readonly string HauntedMansion;

        [AreaId("MapWorldsMaze")]
        public static readonly string Maze;

        [AreaId("MapWorldsChannel")]
        public static readonly string Channel;

        [AreaId("MapWorldsToxicSewer")]
        public static readonly string ToxicSewer;

        [AreaId("MapWorldsAncientCity")]
        public static readonly string AncientCity;

        [AreaId("MapWorldsIvoryTemple")]
        public static readonly string IvoryTemple;

        [AreaId("MapWorldsSpiderLair")]
        public static readonly string SpiderLair;

        [AreaId("MapWorldsBarrows")]
        public static readonly string Barrows;

        [AreaId("MapWorldsMausoleum")]
        public static readonly string Mausoleum;

        [AreaId("MapWorldsFields")]
        public static readonly string Fields;

        [AreaId("MapWorldsJungleValley")]
        public static readonly string JungleValley;

        [AreaId("MapWorldsPhantasmagoria")]
        public static readonly string Phantasmagoria;

        [AreaId("MapWorldsAcademy")]
        public static readonly string Academy;

        [AreaId("MapWorldsThicket")]
        public static readonly string Thicket;

        [AreaId("MapWorldsWharf")]
        public static readonly string Wharf;

        [AreaId("MapWorldsAshenWood")]
        public static readonly string AshenWood;

        [AreaId("MapWorldsAtoll")]
        public static readonly string Atoll;

        [AreaId("MapWorldsCemetery")]
        public static readonly string Cemetery;

        [AreaId("MapWorldsUndergroundSea")]
        public static readonly string UndergroundSea;

        [AreaId("MapWorldsTribunal")]
        public static readonly string Crater;

        [AreaId("MapWorldsCoralRuins")]
        public static readonly string CoralRuins;

        [AreaId("MapWorldsLavaChamber")]
        public static readonly string LavaChamber;

        [AreaId("MapWorldsResidence")]
        public static readonly string Residence;

        [AreaId("MapWorldsRamparts")]
        public static readonly string Ramparts;

        [AreaId("MapWorldsDunes")]
        public static readonly string Dunes;

        [AreaId("MapWorldsBoneCrypt")]
        public static readonly string BoneCrypt;

        [AreaId("MapWorldsUndergroundRiver")]
        public static readonly string UndergroundRiver;

        [AreaId("MapWorldsGardens")]
        public static readonly string Gardens;

        [AreaId("MapWorldsArachnidNest")]
        public static readonly string ArachnidNest;

        [AreaId("MapWorldsBazaar")]
        public static readonly string Bazaar;

        [AreaId("MapWorldsLaboratory")]
        public static readonly string Laboratory;

        [AreaId("MapWorldsInfestedValley")]
        public static readonly string InfestedValley;

        [AreaId("MapWorldsOvergrownRuin")]
        public static readonly string OvergrownRuin;

        [AreaId("MapWorldsVaalPyramid")]
        public static readonly string VaalPyramid;

        [AreaId("MapWorldsGeode")]
        public static readonly string Geode;

        [AreaId("MapWorldsArmoury")]
        public static readonly string Armoury;

        [AreaId("MapWorldsCourtyard")]
        public static readonly string Courtyard;

        [AreaId("MapWorldsMudGeyser")]
        public static readonly string MudGeyser;

        [AreaId("MapWorldsShore")]
        public static readonly string Shore;

        [AreaId("MapWorldsTropicalIsland")]
        public static readonly string TropicalIsland;

        [AreaId("MapWorldsMineralPools")]
        public static readonly string MineralPools;

        [AreaId("MapWorldsMoonTemple")]
        public static readonly string MoonTemple;

        [AreaId("MapWorldsSepulchre")]
        public static readonly string Sepulchre;

        [AreaId("MapWorldsTower")]
        public static readonly string Tower;

        [AreaId("MapWorldsWastePool")]
        public static readonly string WastePool;

        [AreaId("MapWorldsPlateau")]
        public static readonly string Plateau;

        [AreaId("MapWorldsEstuary")]
        public static readonly string Estuary;

        [AreaId("MapWorldsVault")]
        public static readonly string Vault;

        [AreaId("MapWorldsTemple")]
        public static readonly string Temple;

        [AreaId("MapWorldsArena")]
        public static readonly string Arena;

        [AreaId("MapWorldsMuseum")]
        public static readonly string Museum;

        [AreaId("MapWorldsScriptorium")]
        public static readonly string Scriptorium;

        [AreaId("MapWorldsSiege")]
        public static readonly string Siege;

        [AreaId("MapWorldsShipyard")]
        public static readonly string Shipyard;

        [AreaId("MapWorldsBelfry")]
        public static readonly string Belfry;

        [AreaId("MapWorldsArachnidTomb")]
        public static readonly string ArachnidTomb;

        [AreaId("MapWorldsWasteland")]
        public static readonly string Wasteland;

        [AreaId("MapWorldsPrecinct")]
        public static readonly string Precinct;

        [AreaId("MapWorldsBog")]
        public static readonly string Bog;

        [AreaId("MapWorldsPier")]
        public static readonly string Pier;

        [AreaId("MapWorldsCursedCrypt")]
        public static readonly string CursedCrypt;

        [AreaId("MapWorldsOrchard")]
        public static readonly string Orchard;

        [AreaId("MapWorldsPromenade")]
        public static readonly string Promenade;

        [AreaId("MapWorldsLair")]
        public static readonly string Lair;

        [AreaId("MapWorldsColonnade")]
        public static readonly string Colonnade;

        [AreaId("MapWorldsPrimordialPool")]
        public static readonly string PrimordialPool;

        [AreaId("MapWorldsSpiderForest")]
        public static readonly string SpiderForest;

        [AreaId("MapWorldsCoves")]
        public static readonly string Coves;

        [AreaId("MapWorldsWaterways")]
        public static readonly string Waterways;

        [AreaId("MapWorldsFactory")]
        public static readonly string Factory;

        [AreaId("MapWorldsMesa")]
        public static readonly string Mesa;

        [AreaId("MapWorldsPit")]
        public static readonly string Pit;

        [AreaId("MapWorldsDefiledCathedral")]
        public static readonly string DefiledCathedral;

        [AreaId("MapWorldsSummit")]
        public static readonly string Summit;

        [AreaId("MapWorldsOvergrownShrine")]
        public static readonly string OvergrownShrine;

        [AreaId("MapWorldsCastleRuins")]
        public static readonly string CastleRuins;

        [AreaId("MapWorldsCrystalOre")]
        public static readonly string CrystalOre;

        [AreaId("MapWorldsVilla")]
        public static readonly string Villa;

        [AreaId("MapWorldsNecropolis")]
        public static readonly string Necropolis;

        [AreaId("MapWorldsRacecourse_")]
        public static readonly string Racecourse;

        [AreaId("MapWorldsCaldera")]
        public static readonly string Caldera;

        [AreaId("MapWorldsGhetto")]
        public static readonly string Ghetto;

        [AreaId("MapWorldsPark")]
        public static readonly string Park;

        [AreaId("MapWorldsMalformation")]
        public static readonly string Malformation;

        [AreaId("MapWorldsTerrace")]
        public static readonly string Terrace;

        [AreaId("MapWorldsShrine")]
        public static readonly string Shrine;

        [AreaId("MapWorldsArsenal")]
        public static readonly string Arsenal;

        [AreaId("MapWorldsDesertSpring")]
        public static readonly string DesertSpring;

        [AreaId("MapWorldsCore")]
        public static readonly string Core;

        [AreaId("MapWorldsColosseum")]
        public static readonly string Colosseum;

        [AreaId("MapWorldsAcidLakes")]
        public static readonly string AcidCaverns;

        [AreaId("MapWorldsDarkForest")]
        public static readonly string DarkForest;

        [AreaId("MapWorldsCrimsonTemple")]
        public static readonly string CrimsonTemple;

        [AreaId("MapWorldsPlaza")]
        public static readonly string Plaza;

        [AreaId("MapWorldsDig")]
        public static readonly string Dig;

        [AreaId("MapWorldsPalace")]
        public static readonly string Palace;

        [AreaId("MapWorldsLavaLake")]
        public static readonly string LavaLake;

        [AreaId("MapWorldsBasilica")]
        public static readonly string Basilica;

        [AreaId("MapWorldsSunkenCity")]
        public static readonly string SunkenCity;

        [AreaId("MapWorldsReef")]
        public static readonly string Reef;

        [AreaId("MapWorldsCarcass")]
        public static readonly string Carcass;

        [AreaId("MapWorldsChimera")]
        public static readonly string PitOfChimera;

        [AreaId("MapWorldsHydra")]
        public static readonly string LairOfHydra;

        [AreaId("MapWorldsPhoenix")]
        public static readonly string ForgeOfPhoenix;

        [AreaId("MapWorldsMinotaur")]
        public static readonly string MazeOfMinotaur;

        [AreaId("MapWorldsShapersRealm")]
        public static readonly string ShaperRealm;

        [AreaId("MapWorldsVaalTemple")]
        public static readonly string VaalTemple;

        [AreaId("MapWorldsHarbinger")]
        public static readonly string HarbingerIsle;

        //[AreaId("MapWorldsHarbingerLow")]
        //public static readonly string Beachhead;

        //[AreaId("MapWorldsHarbingerMid")]
        //public static readonly string Beachhead;

        [AreaId("MapWorldsHarbingerHigh")]
        public static readonly string Beachhead;

        [AreaId("MapWorldsElder_Desert")]
        public static readonly string DesertOfDementia;

        [AreaId("MapWorldsElder_Volcano")]
        public static readonly string RiverOfHysteria;

        [AreaId("MapWorldsElder_Sulphur")]
        public static readonly string WastesOfLunacy;

        [AreaId("MapWorldsElder_Reliquary")]
        public static readonly string PitsOfSorrow;

        [AreaId("MapWorldsElder_Temple")]
        public static readonly string VaultsOfInsanity;

        [AreaId("MapWorldsElder_Elegant")]
        public static readonly string HallsOfDelirium;

        [AreaId("MapWorldsElder_Sceptre")]
        public static readonly string ManorOfMadness;

        [AreaId("MapWorldsElder_Tower")]
        public static readonly string SpiresOfDelusion;

        [AreaId("MapWorldsElder_Refinery")]
        public static readonly string RepositoryOfDerision;

        [AreaId("MapWorldsElder_Reef")]
        public static readonly string SeaOfIsolation;

        [AreaId("MapWorldsElder_River")]
        public static readonly string IslandsOfDevastation;

        [AreaId("MapWorldsElder_Garden")]
        public static readonly string RuinsOfDespair;

        [AreaId("MapWorldsElderArena")]
        public static readonly string AbsenceOfValueandMeaning;

        [AreaId("MapWorldsStrandUnique")]
        public static readonly string WhakawairuaTuahu;

        [AreaId("MapWorldsChateauUnique")]
        public static readonly string PerandusManor;

        [AreaId("MapWorldsMazeUnique")]
        public static readonly string DoryaniMachinarium;

        [AreaId("MapWorldsAtollUnique")]
        public static readonly string MaelstromOfChaos;

        [AreaId("MapWorldsCemeteryUnique")]
        public static readonly string HallowedGround;

        [AreaId("MapWorldsDunesUnique")]
        public static readonly string PillarsOfArun;

        [AreaId("MapWorldsBoneCryptUnique")]
        public static readonly string OlmecSanctum;

        [AreaId("MapWorldsUndergroundRiverUnique")]
        public static readonly string CaerBlaiddWolfpackDen;

        [AreaId("MapWorldsVaalPyramidUnique")]
        public static readonly string VaultsOfAtziri;

        [AreaId("MapWorldsCourtyardUnique")]
        public static readonly string VinktarSquare;

        [AreaId("MapWorldsShoreUnique")]
        public static readonly string MaoKun;

        [AreaId("MapWorldsTropicalIslandUnique")]
        public static readonly string UntaintedParadise;

        [AreaId("MapWorldsMoonTempleUnique")]
        public static readonly string TwilightTemple;

        [AreaId("MapWorldsTempleUnique")]
        public static readonly string PoorjoyAsylum;

        [AreaId("MapWorldsMuseumUnique")]
        public static readonly string PutridCloister;

        [AreaId("MapWorldsCursedCryptUnique")]
        public static readonly string CowardTrial;

        [AreaId("MapWorldsPromenadeUnique")]
        public static readonly string HallOfGrandmasters;

        [AreaId("MapWorldsPitUnique")]
        public static readonly string DarbelPromise;

        [AreaId("MapWorldsOvergrownShrineUnique")]
        public static readonly string ActonNightmare;

        [AreaId("MapWorldsTortureChamber")]
        public static readonly string PrimordialBlocks;

        [AreaId("MapWorldsFrozenCabins")]
        public static readonly string FrozenCabins;

        [AreaId("MapWorldsForkingRiver")]
        public static readonly string ForkingRiver;

        [AreaId("MapWorldsSilo")]
        public static readonly string Silo;

        [AreaId("MapWorldsDrySea")]
        public static readonly string DrySea;

        [AreaId("MapWorldsStagnation")]
        public static readonly string Stagnation;

        [AreaId("MapWorldsForbiddenWoods")]
        public static readonly string ForbiddenWoods;

        [AreaId("MapWorldsGraveTrough")]
        public static readonly string GraveTrough;

        //[AreaId("MapWorldsColdRiver")]
        //public static readonly string ColdRiver;

        [AreaId("MapWorldsCrimsonTownship")]
        public static readonly string CrimsonTownship;

        [AreaId("MapWorldsBrambleValley")]
        public static readonly string BrambleValley;

        [AreaId("MapWorldsFoundry")]
        public static readonly string Foundry;



        static MapNames()
        {
            var areaDict = Dat.WorldAreas.ToDictionary(a => a.Id, a => a.Name);
            bool error = false;

            foreach (var field in typeof(MapNames).GetFields())
            {
                var attr = field.GetCustomAttribute<AreaId>();

                if (attr == null)
                    continue;

                if (areaDict.TryGetValue(attr.Id, out var name))
                {
                    field.SetValue(null, name);
                }
                else
                {
                    GlobalLog.Error($"[MapNames] Cannot initialize \"{field.Name}\" field. DatWorldAreas does not contain area with \"{attr.Id}\" id.");
                    error = true;
                }
            }
            if (error) BotManager.Stop();
        }
    }
}

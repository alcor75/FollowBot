using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;

namespace FollowBot.SimpleEXtensions
{
    [SuppressMessage("ReSharper", "UnassignedReadonlyField")]
    public static class World
    {
        public static class Act1
        {
            public static readonly AreaInfo TwilightStrand = new AreaInfo("1_1_1", "The Twilight Strand");
            public static readonly AreaInfo LioneyeWatch = new AreaInfo("1_1_town", "Lioneye's Watch");
            public static readonly AreaInfo Coast = new AreaInfo("1_1_2", "The Coast");
            public static readonly AreaInfo TidalIsland = new AreaInfo("1_1_2a", "The Tidal Island");
            public static readonly AreaInfo MudFlats = new AreaInfo("1_1_3", "The Mud Flats");
            public static readonly AreaInfo FetidPool = new AreaInfo("1_1_3a", "The Fetid Pool");
            public static readonly AreaInfo SubmergedPassage = new AreaInfo("1_1_4_1", "The Submerged Passage");
            public static readonly AreaInfo FloodedDepths = new AreaInfo("1_1_4_0", "The Flooded Depths");
            public static readonly AreaInfo Ledge = new AreaInfo("1_1_5", "The Ledge");
            public static readonly AreaInfo Climb = new AreaInfo("1_1_6", "The Climb");
            public static readonly AreaInfo LowerPrison = new AreaInfo("1_1_7_1", "The Lower Prison");
            public static readonly AreaInfo UpperPrison = new AreaInfo("1_1_7_2", "The Upper Prison");
            public static readonly AreaInfo PrisonerGate = new AreaInfo("1_1_8", "Prisoner's Gate");
            public static readonly AreaInfo ShipGraveyard = new AreaInfo("1_1_9", "The Ship Graveyard");
            public static readonly AreaInfo ShipGraveyardCave = new AreaInfo("1_1_9a", "The Ship Graveyard Cave");
            public static readonly AreaInfo CavernOfWrath = new AreaInfo("1_1_11_1", "The Cavern of Wrath");
            public static readonly AreaInfo CavernOfAnger = new AreaInfo("1_1_11_2", "The Cavern of Anger");
        }

        public static class Act2
        {
            public static readonly AreaInfo SouthernForest = new AreaInfo("1_2_1", "The Southern Forest");
            public static readonly AreaInfo ForestEncampment = new AreaInfo("1_2_town", "The Forest Encampment");
            public static readonly AreaInfo OldFields = new AreaInfo("1_2_2", "The Old Fields");
            public static readonly AreaInfo Den = new AreaInfo("1_2_2a", "The Den");
            public static readonly AreaInfo Riverways = new AreaInfo("1_2_7", "The Riverways");
            public static readonly AreaInfo WesternForest = new AreaInfo("1_2_9", "The Western Forest");
            public static readonly AreaInfo WeaverChambers = new AreaInfo("1_2_10", "The Weaver's Chambers");
            public static readonly AreaInfo Crossroads = new AreaInfo("1_2_3", "The Crossroads");
            public static readonly AreaInfo ChamberOfSins1 = new AreaInfo("1_2_6_1", "The Chamber of Sins Level 1");
            public static readonly AreaInfo ChamberOfSins2 = new AreaInfo("1_2_6_2", "The Chamber of Sins Level 2");
            public static readonly AreaInfo FellshrineRuins = new AreaInfo("1_2_15", "The Fellshrine Ruins");
            public static readonly AreaInfo Crypt1 = new AreaInfo("1_2_5_1", "The Crypt Level 1");
            public static readonly AreaInfo Crypt2 = new AreaInfo("1_2_5_2", "The Crypt Level 2");
            public static readonly AreaInfo BrokenBridge = new AreaInfo("1_2_4", "The Broken Bridge");
            public static readonly AreaInfo Wetlands = new AreaInfo("1_2_12", "The Wetlands");
            public static readonly AreaInfo VaalRuins = new AreaInfo("1_2_11", "The Vaal Ruins");
            public static readonly AreaInfo NorthernForest = new AreaInfo("1_2_8", "The Northern Forest");
            public static readonly AreaInfo DreadThicket = new AreaInfo("1_2_13", "The Dread Thicket");
            public static readonly AreaInfo Caverns = new AreaInfo("1_2_14_2", "The Caverns");
            public static readonly AreaInfo AncientPyramid = new AreaInfo("1_2_14_3", "The Ancient Pyramid");
        }

        public static class Act3
        {
            public static readonly AreaInfo CityOfSarn = new AreaInfo("1_3_1", "The City of Sarn");
            public static readonly AreaInfo SarnEncampment = new AreaInfo("1_3_town", "The Sarn Encampment");
            public static readonly AreaInfo Slums = new AreaInfo("1_3_2", "The Slums");
            public static readonly AreaInfo Crematorium = new AreaInfo("1_3_3_1", "The Crematorium");
            public static readonly AreaInfo Sewers = new AreaInfo("1_3_10_1", "The Sewers");
            public static readonly AreaInfo Marketplace = new AreaInfo("1_3_5", "The Marketplace");
            public static readonly AreaInfo Catacombs = new AreaInfo("1_3_6", "The Catacombs");
            public static readonly AreaInfo Battlefront = new AreaInfo("1_3_7", "The Battlefront");
            public static readonly AreaInfo Docks = new AreaInfo("1_3_9", "The Docks");
            public static readonly AreaInfo SolarisTemple1 = new AreaInfo("1_3_8_1", "The Solaris Temple Level 1");
            public static readonly AreaInfo SolarisTemple2 = new AreaInfo("1_3_8_2", "The Solaris Temple Level 2");
            public static readonly AreaInfo EbonyBarracks = new AreaInfo("1_3_13", "The Ebony Barracks");
            public static readonly AreaInfo LunarisTemple1 = new AreaInfo("1_3_14_1", "The Lunaris Temple Level 1");
            public static readonly AreaInfo LunarisTemple2 = new AreaInfo("1_3_14_2", "The Lunaris Temple Level 2");
            public static readonly AreaInfo ImperialGardens = new AreaInfo("1_3_15", "The Imperial Gardens");
            public static readonly AreaInfo Library = new AreaInfo("1_3_17_1", "The Library");
            public static readonly AreaInfo Archives = new AreaInfo("1_3_17_2", "The Archives");
            public static readonly AreaInfo SceptreOfGod = new AreaInfo("1_3_18_1", "The Sceptre of God");
            public static readonly AreaInfo UpperSceptreOfGod = new AreaInfo("1_3_18_2", "The Upper Sceptre of God");
        }

        public static class Act4
        {
            public static readonly AreaInfo Aqueduct = new AreaInfo("1_4_1", "The Aqueduct");
            public static readonly AreaInfo Highgate = new AreaInfo("1_4_town", "Highgate");
            public static readonly AreaInfo DriedLake = new AreaInfo("1_4_2", "The Dried Lake");
            public static readonly AreaInfo Mines1 = new AreaInfo("1_4_3_1", "The Mines Level 1");
            public static readonly AreaInfo Mines2 = new AreaInfo("1_4_3_2", "The Mines Level 2");
            public static readonly AreaInfo CrystalVeins = new AreaInfo("1_4_3_3", "The Crystal Veins");
            public static readonly AreaInfo KaomDream = new AreaInfo("1_4_4_1", "Kaom's Dream");
            public static readonly AreaInfo KaomStronghold = new AreaInfo("1_4_4_3", "Kaom's Stronghold");
            public static readonly AreaInfo DaressoDream = new AreaInfo("1_4_5_1", "Daresso's Dream");
            public static readonly AreaInfo GrandArena = new AreaInfo("1_4_5_2", "The Grand Arena");
            public static readonly AreaInfo BellyOfBeast1 = new AreaInfo("1_4_6_1", "The Belly of the Beast Level 1");
            public static readonly AreaInfo BellyOfBeast2 = new AreaInfo("1_4_6_2", "The Belly of the Beast Level 2");
            public static readonly AreaInfo Harvest = new AreaInfo("1_4_6_3", "The Harvest");
            public static readonly AreaInfo Ascent = new AreaInfo("1_4_7", "The Ascent");
        }

        public static class Act5
        {
            public static readonly AreaInfo SlavePens = new AreaInfo("1_5_1", "The Slave Pens");
            public static readonly AreaInfo OverseerTower = new AreaInfo("1_5_town", "Overseer's Tower");
            public static readonly AreaInfo ControlBlocks = new AreaInfo("1_5_2", "The Control Blocks");
            public static readonly AreaInfo OriathSquare = new AreaInfo("1_5_3", "Oriath Square");
            public static readonly AreaInfo TemplarCourts = new AreaInfo("1_5_4", "The Templar Courts");
            public static readonly AreaInfo ChamberOfInnocence = new AreaInfo("1_5_5", "The Chamber of Innocence");
            public static readonly AreaInfo TorchedCourts = new AreaInfo("1_5_4b", "The Torched Courts");
            public static readonly AreaInfo RuinedSquare = new AreaInfo("1_5_3b", "The Ruined Square");
            public static readonly AreaInfo Ossuary = new AreaInfo("1_5_6", "The Ossuary");
            public static readonly AreaInfo Reliquary = new AreaInfo("1_5_7", "The Reliquary");
            public static readonly AreaInfo CathedralRooftop = new AreaInfo("1_5_8", "The Cathedral Rooftop");
        }

        public static class Act6
        {
            public static readonly AreaInfo LioneyeWatch = new AreaInfo("2_6_town", "Lioneye's Watch");
            public static readonly AreaInfo TwilightStrand = new AreaInfo("2_6_1", "The Twilight Strand");
            public static readonly AreaInfo Coast = new AreaInfo("2_6_2", "The Coast");
            public static readonly AreaInfo TidalIsland = new AreaInfo("2_6_3", "The Tidal Island");
            public static readonly AreaInfo MudFlats = new AreaInfo("2_6_4", "The Mud Flats");
            public static readonly AreaInfo KaruiFortress = new AreaInfo("2_6_5", "The Karui Fortress");
            public static readonly AreaInfo Ridge = new AreaInfo("2_6_6", "The Ridge");
            public static readonly AreaInfo LowerPrison = new AreaInfo("2_6_7_1", "The Lower Prison");
            public static readonly AreaInfo ShavronneTower = new AreaInfo("2_6_7_2", "Shavronne's Tower");
            public static readonly AreaInfo PrisonerGate = new AreaInfo("2_6_8", "Prisoner's Gate");
            public static readonly AreaInfo WesternForest = new AreaInfo("2_6_9", "The Western Forest");
            public static readonly AreaInfo Riverways = new AreaInfo("2_6_10", "The Riverways");
            public static readonly AreaInfo Wetlands = new AreaInfo("2_6_11", "The Wetlands");
            public static readonly AreaInfo SouthernForest = new AreaInfo("2_6_12", "The Southern Forest");
            public static readonly AreaInfo CavernOfAnger = new AreaInfo("2_6_13", "The Cavern of Anger");
            public static readonly AreaInfo Beacon = new AreaInfo("2_6_14", "The Beacon");
            public static readonly AreaInfo BrineKingReef = new AreaInfo("2_6_15", "The Brine King's Reef");
        }

        public static class Act7
        {
            public static readonly AreaInfo BridgeEncampment = new AreaInfo("2_7_town", "The Bridge Encampment");
            public static readonly AreaInfo BrokenBridge = new AreaInfo("2_7_1", "The Broken Bridge");
            public static readonly AreaInfo Crossroads = new AreaInfo("2_7_2", "The Crossroads");
            public static readonly AreaInfo FellshrineRuins = new AreaInfo("2_7_3", "The Fellshrine Ruins");
            public static readonly AreaInfo Crypt = new AreaInfo("2_7_4", "The Crypt");
            public static readonly AreaInfo ChamberOfSins1 = new AreaInfo("2_7_5_1", "The Chamber of Sins Level 1");
            public static readonly AreaInfo ChamberOfSins2 = new AreaInfo("2_7_5_2", "The Chamber of Sins Level 2");
            public static readonly AreaInfo MaligaroSanctum = new AreaInfo("2_7_5_map", "Maligaro's Sanctum");
            public static readonly AreaInfo Den = new AreaInfo("2_7_6", "The Den");
            public static readonly AreaInfo AshenFields = new AreaInfo("2_7_7", "The Ashen Fields");
            public static readonly AreaInfo NorthernForest = new AreaInfo("2_7_8", "The Northern Forest");
            public static readonly AreaInfo DreadThicket = new AreaInfo("2_7_9", "The Dread Thicket");
            public static readonly AreaInfo Causeway = new AreaInfo("2_7_10", "The Causeway");
            public static readonly AreaInfo VaalCity = new AreaInfo("2_7_11", "The Vaal City");
            public static readonly AreaInfo TempleOfDecay1 = new AreaInfo("2_7_12_1", "The Temple of Decay Level 1");
            public static readonly AreaInfo TempleOfDecay2 = new AreaInfo("2_7_12_2", "The Temple of Decay Level 2");
        }

        public static class Act8
        {
            public static readonly AreaInfo SarnRamparts = new AreaInfo("2_8_1", "The Sarn Ramparts");
            public static readonly AreaInfo SarnEncampment = new AreaInfo("2_8_town", "The Sarn Encampment");
            public static readonly AreaInfo ToxicConduits = new AreaInfo("2_8_2_1", "The Toxic Conduits");
            public static readonly AreaInfo DoedreCesspool = new AreaInfo("2_8_2_2", "Doedre's Cesspool");
            public static readonly AreaInfo Quay = new AreaInfo("2_8_8", "The Quay");
            public static readonly AreaInfo GrainGate = new AreaInfo("2_8_9", "The Grain Gate");
            public static readonly AreaInfo ImperialFields = new AreaInfo("2_8_10", "The Imperial Fields");
            public static readonly AreaInfo GrandPromenade = new AreaInfo("2_8_3", "The Grand Promenade");
            public static readonly AreaInfo BathHouse = new AreaInfo("2_8_5", "The Bath House");
            public static readonly AreaInfo HighGardens = new AreaInfo("2_8_4", "The High Gardens");
            public static readonly AreaInfo SolarisConcourse = new AreaInfo("2_8_11", "The Solaris Concourse");
            public static readonly AreaInfo SolarisTemple1 = new AreaInfo("2_8_12_1", "The Solaris Temple Level 1");
            public static readonly AreaInfo SolarisTemple2 = new AreaInfo("2_8_12_2", "The Solaris Temple Level 2");
            public static readonly AreaInfo LunarisConcourse = new AreaInfo("2_8_6", "The Lunaris Concourse");
            public static readonly AreaInfo LunarisTemple1 = new AreaInfo("2_8_7_1_", "The Lunaris Temple Level 1");
            public static readonly AreaInfo LunarisTemple2 = new AreaInfo("2_8_7_2", "The Lunaris Temple Level 2");
            public static readonly AreaInfo HarbourBridge = new AreaInfo("2_8_13", "The Harbour Bridge");
        }

        public static class Act9
        {
            public static readonly AreaInfo BloodAqueduct = new AreaInfo("2_9_1", "The Blood Aqueduct");
            public static readonly AreaInfo Highgate = new AreaInfo("2_9_town", "Highgate");
            public static readonly AreaInfo Descent = new AreaInfo("2_9_2", "The Descent");
            public static readonly AreaInfo VastiriDesert = new AreaInfo("2_9_3", "The Vastiri Desert");
            public static readonly AreaInfo Oasis = new AreaInfo("2_9_4", "The Oasis");
            public static readonly AreaInfo Foothills = new AreaInfo("2_9_5", "The Foothills");
            public static readonly AreaInfo BoilingLake = new AreaInfo("2_9_6", "The Boiling Lake");
            public static readonly AreaInfo Tunnel = new AreaInfo("2_9_7", "The Tunnel");
            public static readonly AreaInfo Quarry = new AreaInfo("2_9_8", "The Quarry");
            public static readonly AreaInfo Refinery = new AreaInfo("2_9_9", "The Refinery");
            public static readonly AreaInfo BellyOfBeast = new AreaInfo("2_9_10_1", "The Belly of the Beast");
            public static readonly AreaInfo RottingCore = new AreaInfo("2_9_10_2", "The Rotting Core");
        }

        public static class Act10
        {
            public static readonly AreaInfo OriathDocks = new AreaInfo("2_10_town", "Oriath Docks");
            public static readonly AreaInfo CathedralRooftop = new AreaInfo("2_10_1", "The Cathedral Rooftop");
            public static readonly AreaInfo RavagedSquare = new AreaInfo("2_10_2", "The Ravaged Square");
            public static readonly AreaInfo Ossuary = new AreaInfo("2_10_9", "The Ossuary");
            public static readonly AreaInfo ControlBlocks = new AreaInfo("2_10_7", "The Control Blocks");
            public static readonly AreaInfo Reliquary = new AreaInfo("2_10_8", "The Reliquary");
            public static readonly AreaInfo TorchedCourts = new AreaInfo("2_10_3", "The Torched Courts");
            public static readonly AreaInfo DesecratedChambers = new AreaInfo("2_10_4", "The Desecrated Chambers");
            public static readonly AreaInfo Canals = new AreaInfo("2_10_5", "The Canals");
            public static readonly AreaInfo FeedingTrough = new AreaInfo("2_10_6", "The Feeding Trough");
        }

        public static class Act11
        {
            public static readonly AreaInfo Oriath = new AreaInfo("2_11_town", "Oriath");
            public static readonly AreaInfo TemplarLaboratory = new AreaInfo("2_11_lab", "The Templar Laboratory");
            public static readonly AreaInfo FallenCourts = new AreaInfo("2_11_1", "The Fallen Courts");
            public static readonly AreaInfo HauntedReliquary = new AreaInfo("2_11_2", "The Haunted Reliquary");
        }

        static World()
        {
            var areaDict = Dat.WorldAreas.ToDictionary(a => a.Id, a => a.Name);
            bool error = false;

            foreach (var act in typeof(World).GetNestedTypes())
            {
                foreach (var field in act.GetFields())
                {
                    var area = (AreaInfo)field.GetValue(field);
                    if (areaDict.TryGetValue(area.Id, out var name))
                    {
                        if (name != area.Name)
                        {
                            area.Name = name;
                            GlobalLog.Error($"[World] Invalid area info in \"{field.Name}\" field. Area name: \"{area.Name}\". Correct name: \"{name}\".");
                            //error = true;
                        }
                    }
                    else
                    {
                        GlobalLog.Error($"[World] Invalid area info in \"{field.Name}\" field. DatWorldAreas does not contain an area with \"{area.Id}\" id.");
                        error = true;
                    }
                }
            }
            if (error) BotManager.Stop();
        }

        public static DatWorldAreaWrapper CurrentArea => LokiPoe.LocalData.WorldArea;

        public static DatWorldAreaWrapper LastOpenedAct
        {
            get
            {
                var act = LokiPoe.InstanceInfo.AvailableWaypoints.Values
                    .Where(a => a.IsTown)
                    .OrderByDescending(a => a.Act)
                    .FirstOrDefault();

                if (act == null)
                {
                    GlobalLog.Error("[GetLastOpenedAct] Unknown error. Fail to get any opened act.");
                    ErrorManager.ReportCriticalError();
                    return null;
                }
                return act;
            }
        }

        public static bool IsWaypointOpened(string areaId)
        {
            return LokiPoe.InstanceInfo.AvailableWaypoints.ContainsKey(areaId);
        }
    }
    [AttributeUsage(AttributeTargets.Field)]
    public class AreaId : Attribute
    {
        public readonly string Id;

        public AreaId(string id)
        {
            Id = id;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Components;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.SimpleEXtensions.Positions;
using AreaTransition = DreamPoeBot.Loki.Game.Objects.AreaTransition;
using Chest = DreamPoeBot.Loki.Game.Objects.Chest;

namespace FollowBot.SimpleEXtensions.Global
{
    public static class Travel
    {
        private static readonly HashSet<AreaInfo> NewInstanceRequests = new HashSet<AreaInfo>();
        public static async Task To(AreaInfo area)
        {
            if (Handlers.TryGetValue(area, out Func<Task> handler))
            {
                await handler();
            }
            else
            {
                GlobalLog.Error($"[Travel] Unsupported area: {area}.");
                ErrorManager.ReportCriticalError();
            }
        }
        public static void RequestNewInstance(AreaInfo area)
        {
            if (NewInstanceRequests.Add(area))
            {
                GlobalLog.Debug($"[Travel] New instance requested for {area}");
            }
        }

        private static readonly Dictionary<AreaInfo, Func<Task>> Handlers = new Dictionary<AreaInfo, Func<Task>>
        {
            [World.Act1.LioneyeWatch] = LioneyeWatch,
            [World.Act1.Coast] = Coast,
            [World.Act1.TidalIsland] = TidalIsland,
            [World.Act1.MudFlats] = MudFlats,
            [World.Act1.FetidPool] = FetidPool,
            [World.Act1.SubmergedPassage] = SubmergedPassage,
            [World.Act1.FloodedDepths] = FloodedDepths,
            [World.Act1.Ledge] = Ledge,
            [World.Act1.Climb] = Climb,
            [World.Act1.LowerPrison] = LowerPrison,
            [World.Act1.UpperPrison] = UpperPrison,
            [World.Act1.PrisonerGate] = PrisonersGate,
            [World.Act1.ShipGraveyard] = ShipGraveyard,
            [World.Act1.ShipGraveyardCave] = ShipGraveyardCave,
            [World.Act1.CavernOfWrath] = CavernOfWrath,
            [World.Act1.CavernOfAnger] = CavernOfAnger,
            [World.Act2.SouthernForest] = SouthernForest,
            [World.Act2.ForestEncampment] = ForestEncampment,
            [World.Act2.Riverways] = Riverways,
            [World.Act2.WesternForest] = WesternForest,
            [World.Act2.WeaverChambers] = WeaverChambers,
            [World.Act2.OldFields] = OldFields,
            [World.Act2.Den] = Den,
            [World.Act2.Crossroads] = Crossroads,
            [World.Act2.ChamberOfSins1] = ChamberOfSins1,
            [World.Act2.ChamberOfSins2] = ChamberOfSins2,
            [World.Act2.BrokenBridge] = BrokenBridge,
            [World.Act2.FellshrineRuins] = FellshrineRuins,
            [World.Act2.Crypt1] = Crypt1,
            [World.Act2.Crypt2] = Crypt2,
            [World.Act2.Wetlands] = Wetlands,
            [World.Act2.VaalRuins] = VaalRuins,
            [World.Act2.NorthernForest] = NorthernForest,
            [World.Act2.DreadThicket] = DreadThicket,
            [World.Act2.Caverns] = Caverns,
            [World.Act2.AncientPyramid] = AncientPyramid,
            [World.Act3.CityOfSarn] = CityOfSarn,
            [World.Act3.SarnEncampment] = SarnEncampment,
            [World.Act3.Slums] = Slums,
            [World.Act3.Crematorium] = Crematorium,
            [World.Act3.Sewers] = Sewers,
            [World.Act3.Marketplace] = Marketplace,
            [World.Act3.Catacombs] = Catacombs,
            [World.Act3.Battlefront] = Battlefront,
            [World.Act3.Docks] = Docks,
            [World.Act3.SolarisTemple1] = Solaris1,
            [World.Act3.SolarisTemple2] = Solaris2,
            [World.Act3.EbonyBarracks] = EbonyBarracks,
            [World.Act3.LunarisTemple1] = Lunaris1,
            [World.Act3.LunarisTemple2] = Lunaris2,
            [World.Act3.ImperialGardens] = ImperialGardens,
            [World.Act3.Library] = Library,
            [World.Act3.Archives] = Archives,
            [World.Act3.SceptreOfGod] = SceptreOfGod,
            [World.Act3.UpperSceptreOfGod] = UpperSceptreOfGod,
            [World.Act4.Aqueduct] = Aqueduct,
            [World.Act4.Highgate] = Highgate,
            [World.Act4.DriedLake] = DriedLake,
            [World.Act4.Mines1] = Mines1,
            [World.Act4.Mines2] = Mines2,
            [World.Act4.CrystalVeins] = CrystalVeins,
            [World.Act4.KaomDream] = KaomDream,
            [World.Act4.KaomStronghold] = KaomStronghold,
            [World.Act4.DaressoDream] = DaressoDream,
            [World.Act4.GrandArena] = GrandArena,
            [World.Act4.BellyOfBeast1] = Belly1,
            [World.Act4.BellyOfBeast2] = Belly2,
            [World.Act4.Harvest] = Harvest,
            [World.Act4.Ascent] = Ascent,
            [World.Act5.SlavePens] = SlavePens,
            [World.Act5.OverseerTower] = OverseerTower,
            [World.Act5.ControlBlocks] = ControlBlocks,
            [World.Act5.OriathSquare] = OriathSquare,
            [World.Act5.TemplarCourts] = TemplarCourts,
            [World.Act5.ChamberOfInnocence] = ChamberOfInnocence,
            [World.Act5.TorchedCourts] = TorchedCourts,
            [World.Act5.RuinedSquare] = RuinedSquare,
            [World.Act5.Reliquary] = Reliquary,
            [World.Act5.Ossuary] = Ossuary,
            [World.Act5.CathedralRooftop] = CathedralRooftop,
            [World.Act6.LioneyeWatch] = LioneyeWatch_A6,
            [World.Act6.TwilightStrand] = TwilightStrand_A6,
            [World.Act6.Coast] = Coast_A6,
            [World.Act6.TidalIsland] = TidalIsland_A6,
            [World.Act6.MudFlats] = MudFlats_A6,
            [World.Act6.KaruiFortress] = KaruiFortress,
            [World.Act6.Ridge] = Ridge,
            [World.Act6.LowerPrison] = LowerPrison_A6,
            [World.Act6.ShavronneTower] = ShavronneTower,
            [World.Act6.PrisonerGate] = PrisonersGate_A6,
            [World.Act6.WesternForest] = WesternForest_A6,
            [World.Act6.Riverways] = Riverways_A6,
            [World.Act6.Wetlands] = Wetlands_A6,
            [World.Act6.SouthernForest] = SouthernForest_A6,
            [World.Act6.CavernOfAnger] = CavernOfAnger_A6,
            [World.Act6.Beacon] = Beacon,
            [World.Act6.BrineKingReef] = BrineKingReef,
            [World.Act7.BridgeEncampment] = BridgeEncampment,
            [World.Act7.BrokenBridge] = BrokenBridge_A7,
            [World.Act7.Crossroads] = Crossroads_A7,
            [World.Act7.FellshrineRuins] = FellshrineRuins_A7,
            [World.Act7.Crypt] = Crypt_A7,
            [World.Act7.ChamberOfSins1] = ChamberOfSins1_A7,
            [World.Act7.ChamberOfSins2] = ChamberOfSins2_A7,
            [World.Act7.Den] = Den_A7,
            [World.Act7.AshenFields] = AshenFields,
            [World.Act7.NorthernForest] = NorthernForest_A7,
            [World.Act7.DreadThicket] = DreadThicket_A7,
            [World.Act7.Causeway] = Causeway,
            [World.Act7.VaalCity] = VaalCity,
            [World.Act7.TempleOfDecay1] = TempleOfDecay1,
            [World.Act7.TempleOfDecay2] = TempleOfDecay2,
            [World.Act8.SarnRamparts] = SarnRamparts,
            [World.Act8.SarnEncampment] = SarnEncampment_A8,
            [World.Act8.ToxicConduits] = ToxicConduits,
            [World.Act8.DoedreCesspool] = DoedreCesspool,
            [World.Act8.GrandPromenade] = GrandPromenade,
            [World.Act8.Quay] = Quay,
            [World.Act8.GrainGate] = GrainGate,
            [World.Act8.ImperialFields] = ImperialFields,
            [World.Act8.SolarisTemple1] = Solaris1_A8,
            [World.Act8.SolarisTemple2] = Solaris2_A8,
            [World.Act8.SolarisConcourse] = SolarisConcourse,
            [World.Act8.BathHouse] = BathHouse,
            [World.Act8.HighGardens] = HighGardens,
            [World.Act8.LunarisConcourse] = LunarisConcourse,
            [World.Act8.LunarisTemple1] = Lunaris1_A8,
            [World.Act8.LunarisTemple2] = Lunaris2_A8,
            [World.Act8.HarbourBridge] = HarbourBridge,
            [World.Act9.BloodAqueduct] = BloodAqueduct,
            [World.Act9.Highgate] = Highgate_A9,
            [World.Act9.Descent] = Descent,
            [World.Act9.VastiriDesert] = VastiriDesert,
            [World.Act9.Oasis] = Oasis,
            [World.Act9.Foothills] = Foothills,
            [World.Act9.BoilingLake] = BoilingLake,
            [World.Act9.Tunnel] = Tunnel,
            [World.Act9.Quarry] = Quarry,
            [World.Act9.Refinery] = Refinery,
            [World.Act9.BellyOfBeast] = Belly_A9,
            [World.Act9.RottingCore] = RottingCore,
            [World.Act10.OriathDocks] = OriathDocks,
            [World.Act10.CathedralRooftop] = CathedralRooftop_A10,
            [World.Act10.RavagedSquare] = RavagedSquare,
            [World.Act10.Ossuary] = Ossuary_A10,
            [World.Act10.TorchedCourts] = TorchedCourts_A10,
            [World.Act10.Reliquary] = Reliquary_A10,
            [World.Act10.ControlBlocks] = ControlBlocks_A10,
            [World.Act10.DesecratedChambers] = DesecratedChambers,
            [World.Act10.Canals] = Canals,
            [World.Act10.FeedingTrough] = FeedingTrough,
            [World.Act11.Oriath] = Oriath,
            [World.Act11.TemplarLaboratory] = TemplarLaboratory,
            [World.Act11.FallenCourts] = FallenCourts,
            [World.Act11.HauntedReliquary] = HauntedReliquary
        };
        #region Handlers

        #region Act 1

        private static async Task UnknownState()
        {
            GlobalLog.Error($"[Travel] Lioneye's Watch waypoint is not opened and we are not inside Twilight Strand. Current area: {World.CurrentArea}");
            ErrorManager.ReportCriticalError();
        }

        private static async Task LioneyeWatch()
        {
            await WpAreaHandler(World.Act1.LioneyeWatch, LioneyeWatchTgt, World.Act1.TwilightStrand, UnknownState);
        }

        private static async Task Coast()
        {
            await TownConnectedAreaHandler(World.Act1.Coast, CoastFromTown, World.Act1.LioneyeWatch, LioneyeWatch);
        }

        private static async Task TidalIsland()
        {
            await NoWpAreaHandler(World.Act1.TidalIsland, TidalIslandTgt, World.Act1.Coast, Coast);
        }

        private static async Task MudFlats()
        {
            await NoWpAreaHandler(World.Act1.MudFlats, MudFlatsTgt, World.Act1.Coast, Coast);
        }

        private static async Task FetidPool()
        {
            await NoWpAreaHandler(World.Act1.FetidPool, FetidPoolTgt, World.Act1.MudFlats, MudFlats);
        }

        private static async Task SubmergedPassage()
        {
            await WpAreaHandler(World.Act1.SubmergedPassage, SubmergedPassageTgt, World.Act1.MudFlats, MudFlats);
        }

        private static async Task FloodedDepths()
        {
            await NoWpAreaHandler(World.Act1.FloodedDepths, FloodedDepthsTgt, World.Act1.SubmergedPassage, SubmergedPassage);
        }

        private static async Task Ledge()
        {
            await WpAreaHandler(World.Act1.Ledge, LedgeTgt, World.Act1.SubmergedPassage, SubmergedPassage);
        }

        private static async Task Climb()
        {
            await WpAreaHandler(World.Act1.Climb, ClimbTgt, World.Act1.Ledge, Ledge);
        }

        private static async Task LowerPrison()
        {
            await WpAreaHandler(World.Act1.LowerPrison, LowerPrisonTgt, World.Act1.Climb, Climb);
        }

        private static async Task UpperPrison()
        {
            await NoWpAreaHandler(World.Act1.UpperPrison, UpperPrisonTgt, World.Act1.LowerPrison, LowerPrison,
                () => PrisonerGateTgt.ResetCurrentPosition());
        }

        private static async Task PrisonersGate()
        {
            await ThroughMultilevelAreaHander(World.Act1.PrisonerGate, PrisonerGateTgt, World.Act1.UpperPrison, UpperPrison);
        }

        private static async Task ShipGraveyard()
        {
            await WpAreaHandler(World.Act1.ShipGraveyard, ShipGraveyardTgt, World.Act1.PrisonerGate, PrisonersGate);
        }

        private static async Task ShipGraveyardCave()
        {
            await NoWpAreaHandler(World.Act1.ShipGraveyardCave, ShipGraveyardCaveTgt, World.Act1.ShipGraveyard, ShipGraveyard);
        }

        private static async Task CavernOfWrath()
        {
            await WpAreaHandler(World.Act1.CavernOfWrath, CavernOfWrathTgt, World.Act1.ShipGraveyard, ShipGraveyard);
        }

        private static async Task CavernOfAnger()
        {
            await NoWpAreaHandler(World.Act1.CavernOfAnger, CavernOfAngerTgt, World.Act1.CavernOfWrath, CavernOfWrath,
                () => SouthernForestTgt.ResetCurrentPosition());
        }

        #endregion

        #region Act 2

        private static async Task SouthernForest()
        {
            await ThroughMultilevelAreaHander(World.Act2.SouthernForest, SouthernForestTgt, World.Act1.CavernOfAnger, CavernOfAnger);
        }

        private static async Task ForestEncampment()
        {
            await WpAreaHandler(World.Act2.ForestEncampment, ForestEncampmentTgt, World.Act2.SouthernForest, SouthernForest);
        }

        private static async Task Riverways()
        {
            await TownConnectedAreaHandler(World.Act2.Riverways, RiverwaysFromTown, World.Act2.ForestEncampment, ForestEncampment);
        }

        private static async Task WesternForest()
        {
            await WpAreaHandler(World.Act2.WesternForest, WesternForestTgt, World.Act2.Riverways, Riverways);
        }

        private static async Task WeaverChambers()
        {
            await NoWpAreaHandler(World.Act2.WeaverChambers, WeaverChambersTgt, World.Act2.WesternForest, WesternForest);
        }

        private static async Task OldFields()
        {
            await TownConnectedAreaHandler(World.Act2.OldFields, OldFieldsFromTown, World.Act2.ForestEncampment, ForestEncampment);
        }

        private static async Task Den()
        {
            await NoWpAreaHandler(World.Act2.Den, DenTgt, World.Act2.OldFields, OldFields);
        }

        private static async Task Crossroads()
        {
            await WpAreaHandler(World.Act2.Crossroads, CrossroadsTgt, World.Act2.OldFields, OldFields);
        }

        private static async Task ChamberOfSins1()
        {
            await WpAreaHandler(World.Act2.ChamberOfSins1, ChamberOfSins1Tgt, World.Act2.Crossroads, Crossroads);
        }

        private static async Task ChamberOfSins2()
        {
            await NoWpAreaHandler(World.Act2.ChamberOfSins2, ChamberOfSins2Tgt, World.Act2.ChamberOfSins1, ChamberOfSins1);
        }

        private static async Task BrokenBridge()
        {
            await WpAreaHandler(World.Act2.BrokenBridge, BrokenBridgeTgt, World.Act2.Crossroads, Crossroads);
        }

        private static async Task FellshrineRuins()
        {
            if (World.Act2.FellshrineRuins.IsCurrentArea)
            {
                OuterLogicError(World.Act2.FellshrineRuins);
                return;
            }
            if (World.Act2.Crossroads.IsCurrentArea)
            {
                var cachedPos = GetCachedTransitionPos(World.Act2.FellshrineRuins);
                if (cachedPos != null)
                {
                    if (cachedPos.IsFar)
                    {
                        cachedPos.Come();
                        return;
                    }
                    await TakeTransition(World.Act2.FellshrineRuins, null);
                    return;
                }
                if (FellshrineTransitionPos.IsFar)
                {
                    FellshrineTransitionPos.Come();
                }
                return;
            }
            await Crossroads();
        }

        private static async Task Crypt1()
        {
            await WpAreaHandler(World.Act2.Crypt1, Crypt1Tgt, World.Act2.FellshrineRuins, FellshrineRuins);
        }

        private static async Task Crypt2()
        {
            await NoWpAreaHandler(World.Act2.Crypt2, Crypt2Tgt, World.Act2.Crypt1, Crypt1);
        }

        private static async Task Wetlands()
        {
            await WpAreaHandler(World.Act2.Wetlands, WetlandsTgt, World.Act2.Riverways, Riverways);
        }

        private static async Task VaalRuins()
        {
            await NoWpAreaHandler(World.Act2.VaalRuins, VaalRuinsTgt, World.Act2.Wetlands, Wetlands);
        }

        private static async Task NorthernForest()
        {
            await WpAreaHandler(World.Act2.NorthernForest, NorthernForestTgt, World.Act2.VaalRuins, VaalRuins);
        }

        private static async Task DreadThicket()
        {
            await NoWpAreaHandler(World.Act2.DreadThicket, DreadThicketTgt, World.Act2.NorthernForest, NorthernForest);
        }

        private static async Task Caverns()
        {
            await WpAreaHandler(World.Act2.Caverns, CavernsTgt, World.Act2.NorthernForest, NorthernForest);
        }

        private static async Task AncientPyramid()
        {
            await NoWpAreaHandler(World.Act2.AncientPyramid, AncientPyramidTgt, World.Act2.Caverns, Caverns,
                () => CityOfSarnTgt.ResetCurrentPosition());
        }

        #endregion

        #region Act 3

        private static async Task CityOfSarn()
        {
            await ThroughMultilevelAreaHander(World.Act3.CityOfSarn, CityOfSarnTgt, World.Act2.AncientPyramid, AncientPyramid);
        }

        private static async Task SarnEncampment()
        {
            await WpAreaHandler(World.Act3.SarnEncampment, SarnEncampmentTgt, World.Act3.CityOfSarn, CityOfSarn);
        }

        private static async Task Slums()
        {
            await TownConnectedAreaHandler(World.Act3.Slums, SlumsFromTown, World.Act3.SarnEncampment, SarnEncampment);
        }

        private static async Task Crematorium()
        {
            await WpAreaHandler(World.Act3.Crematorium, CrematoriumTgt, World.Act3.Slums, Slums);
        }

        private static async Task Sewers()
        {
            await WpAreaHandler(World.Act3.Sewers, SewersTgt, World.Act3.Slums, Slums);
        }

        private static async Task Marketplace()
        {
            await WpAreaHandler(World.Act3.Marketplace, MarketplaceTgt, World.Act3.Sewers, Sewers);
        }

        private static async Task Catacombs()
        {
            await NoWpAreaHandler(World.Act3.Catacombs, CatacombsTgt, World.Act3.Marketplace, Marketplace);
        }

        private static async Task Battlefront()
        {
            await WpAreaHandler(World.Act3.Battlefront, BattlefrontTgt, World.Act3.Marketplace, Marketplace);
        }

        private static async Task Docks()
        {
            await WpAreaHandler(World.Act3.Docks, DocksTgt, World.Act3.Battlefront, Battlefront);
        }

        private static async Task Solaris1()
        {
            await WpAreaHandler(World.Act3.SolarisTemple1, Solaris1Tgt, World.Act3.Battlefront, Battlefront);
        }

        private static async Task Solaris2()
        {
            await WpAreaHandler(World.Act3.SolarisTemple2, Solaris2Tgt, World.Act3.SolarisTemple1, Solaris1);
        }

        private static async Task EbonyBarracks()
        {
            await WpAreaHandler(World.Act3.EbonyBarracks, EbonyBarracksTgt, World.Act3.Sewers, Sewers);
        }

        private static async Task Lunaris1()
        {
            await WpAreaHandler(World.Act3.LunarisTemple1, Lunaris1Tgt, World.Act3.EbonyBarracks, EbonyBarracks);
        }

        private static async Task Lunaris2()
        {
            await NoWpAreaHandler(World.Act3.LunarisTemple2, Lunaris2Tgt, World.Act3.LunarisTemple1, Lunaris1);
        }

        private static async Task ImperialGardens()
        {
            await WpAreaHandler(World.Act3.ImperialGardens, ImperialGardensTgt, World.Act3.EbonyBarracks, EbonyBarracks);
        }

        private static async Task Library()
        {
            await WpAreaHandler(World.Act3.Library, LibraryTgt, World.Act3.ImperialGardens, ImperialGardens);
        }

        private static async Task Archives()
        {
            if (World.Act3.Archives.IsCurrentArea)
            {
                OuterLogicError(World.Act3.Archives);
                return;
            }
            if (World.Act3.Library.IsCurrentArea)
            {
                var cachedPos = GetCachedTransitionPos(World.Act3.Archives);
                if (cachedPos != null)
                {
                    if (cachedPos.IsFar)
                    {
                        cachedPos.Come();
                        return;
                    }
                    await TakeTransition(World.Act3.Archives, ArchivesTgt, null);
                    return;
                }

                var candle = LooseCandle;
                if (candle != null)
                {
                    var candleWalkablePos = new WalkablePosition("Loose Candle", candle.Position);
                    if (candleWalkablePos.IsFar)
                    {
                        candleWalkablePos.Come();
                        return;
                    }

                    if (candle.IsTargetable)
                    {
                        if (!await PlayerAction.Interact(candle))
                            ErrorManager.ReportError();

                        await Wait.SleepSafe(200);
                    }
                    else
                    {
                        GlobalLog.Debug("Waiting for Archives transition.");
                    }
                }

                if (ArchivesTgt.IsFar)
                {
                    ArchivesTgt.Come();
                    return;
                }
                //var candle = LooseCandle;
                //if (candle != null && candle.IsTargetable)
                //{
                //    if (!await PlayerAction.Interact(candle))
                //        ErrorManager.ReportError();

                //    await Wait.SleepSafe(200);
                //}
                //else
                //{
                //    GlobalLog.Debug("Waiting for Archives transition.");
                //    await Wait.StuckDetectionSleep(200);
                //}
                return;
            }
            await Library();
        }

        private static async Task SceptreOfGod()
        {
            await WpAreaHandler(World.Act3.SceptreOfGod, SceptreOfGodTgt, World.Act3.ImperialGardens, ImperialGardens,
                () => UpperSceptreTgt.ResetCurrentPosition());
        }

        private static async Task UpperSceptreOfGod()
        {
            await ThroughMultilevelAreaHander(World.Act3.UpperSceptreOfGod, UpperSceptreTgt, World.Act3.SceptreOfGod, SceptreOfGod,
                () => AqueductTgt.ResetCurrentPosition());
        }

        #endregion

        #region Act 4

        private static async Task Aqueduct()
        {
            await ThroughMultilevelAreaHander(World.Act4.Aqueduct, AqueductTgt, World.Act3.UpperSceptreOfGod, UpperSceptreOfGod);
        }

        private static async Task Highgate()
        {
            await WpAreaHandler(World.Act4.Highgate, HighgateTgt, World.Act4.Aqueduct, Aqueduct);
        }

        private static async Task DriedLake()
        {
            await TownConnectedAreaHandler(World.Act4.DriedLake, DriedLakeFromTown, World.Act4.Highgate, Highgate);
        }

        private static async Task Mines1()
        {
            if (World.Act4.Mines1.IsCurrentArea)
            {
                OuterLogicError(World.Act4.Mines1);
                return;
            }
            if (World.Act4.Highgate.IsCurrentArea)
            {
                await MinesFromTown.ComeAtOnce();

                var seal = DeshretSeal;
                if (seal != null && seal.IsTargetable)
                {
                    GlobalLog.Error($"[Travel] Cannot travel to {World.Act4.Mines1}. Deshret Seal is active.");
                    ErrorManager.ReportCriticalError();
                    return;
                }
                await TakeTransition(World.Act4.Mines1, null);
                return;
            }
            await Highgate();
        }

        private static async Task Mines2()
        {
            await NoWpAreaHandler(World.Act4.Mines2, Mines2Tgt, World.Act4.Mines1, Mines1);
        }

        private static async Task CrystalVeins()
        {
            await WpAreaHandler(World.Act4.CrystalVeins, CrystalVeinsTgt, World.Act4.Mines2, Mines2);
        }

        private static async Task KaomDream()
        {
            await NoWpAreaHandler(World.Act4.KaomDream, RaptureDeviceTgt, World.Act4.CrystalVeins, CrystalVeins);
        }

        private static async Task KaomStronghold()
        {
            await WpAreaHandler(World.Act4.KaomStronghold, KaomStrongholdTgt, World.Act4.KaomDream, KaomDream);
        }

        private static async Task DaressoDream()
        {
            await NoWpAreaHandler(World.Act4.DaressoDream, RaptureDeviceTgt, World.Act4.CrystalVeins, CrystalVeins);
        }

        private static async Task GrandArena()
        {
            await WpAreaHandler(World.Act4.GrandArena, GrandArenaTgt, World.Act4.DaressoDream, DaressoDream);
        }

        private static async Task Belly1()
        {
            await NoWpAreaHandler(World.Act4.BellyOfBeast1, RaptureDeviceTgt, World.Act4.CrystalVeins, CrystalVeins);
        }

        private static async Task Belly2()
        {
            await NoWpAreaHandler(World.Act4.BellyOfBeast2, Belly2Tgt, World.Act4.BellyOfBeast1, Belly1);
        }

        private static async Task Harvest()
        {
            await WpAreaHandler(World.Act4.Harvest, HarvestTgt, World.Act4.BellyOfBeast2, Belly2);
        }

        private static async Task Ascent()
        {
            await TownConnectedAreaHandler(World.Act4.Ascent, AscentFromTown, World.Act4.Highgate, Highgate);
        }

        #endregion

        #region Act 5

        private static async Task SlavePens()
        {
            await StrictlyWpAreaHandler(World.Act5.SlavePens, "Use QuestBot to enter Act 5 first");
        }

        private static async Task OverseerTower()
        {
            await WpAreaHandler(World.Act5.OverseerTower, OverseerTowerTgt, World.Act5.SlavePens, SlavePens);
        }

        private static async Task ControlBlocks()
        {
            await TownConnectedAreaHandler(World.Act5.ControlBlocks, ControlBlocksFromTown, World.Act5.OverseerTower, OverseerTower);
        }

        private static async Task OriathSquare()
        {
            await WpAreaHandler(World.Act5.OriathSquare, OriathSquareTgt, World.Act5.ControlBlocks, ControlBlocks);
        }

        private static async Task TemplarCourts()
        {
            await WpAreaHandler(World.Act5.TemplarCourts, TemplarCourtsTgt, World.Act5.OriathSquare, OriathSquare);
        }

        private static async Task ChamberOfInnocence()
        {
            await WpAreaHandler(World.Act5.ChamberOfInnocence, ChamberOfInnocenceTgt, World.Act5.TemplarCourts, TemplarCourts);
        }

        private static async Task TorchedCourts()
        {
            await NoWpAreaHandler(World.Act5.TorchedCourts, TorchedCourtsTgt, World.Act5.ChamberOfInnocence, ChamberOfInnocence);
        }

        private static async Task RuinedSquare()
        {
            await WpAreaHandler(World.Act5.RuinedSquare, RuinedSquareTgt, World.Act5.TorchedCourts, TorchedCourts);
        }

        private static async Task Reliquary()
        {
            await WpAreaHandler(World.Act5.Reliquary, ReliquaryTgt, World.Act5.RuinedSquare, RuinedSquare);
        }

        private static async Task Ossuary()
        {
            await NoWpAreaHandler(World.Act5.Ossuary, OssuaryTgt, World.Act5.RuinedSquare, RuinedSquare);
        }

        private static async Task CathedralRooftop()
        {
            await WpAreaHandler(World.Act5.CathedralRooftop, CathedralRooftopTgt, World.Act5.RuinedSquare, RuinedSquare);
        }

        #endregion

        #region Act 6

        private static async Task LioneyeWatch_A6()
        {
            await StrictlyWpAreaHandler(World.Act6.LioneyeWatch, "Use QuestBot to enter Act 6 first");
        }

        private static async Task TwilightStrand_A6()
        {
            await TownConnectedAreaHandler(World.Act6.TwilightStrand, TwilightStrandFromTown_A6, World.Act6.LioneyeWatch, LioneyeWatch_A6);
        }

        private static async Task Coast_A6()
        {
            await TownConnectedAreaHandler(World.Act6.Coast, CoastFromTown_A6, World.Act6.LioneyeWatch, LioneyeWatch_A6);
        }

        private static async Task TidalIsland_A6()
        {
            await NoWpAreaHandler(World.Act6.TidalIsland, TidalIslandTgt_A6, World.Act6.Coast, Coast_A6);
        }

        private static async Task MudFlats_A6()
        {
            await NoWpAreaHandler(World.Act6.MudFlats, MudFlatsTgt_A6, World.Act6.Coast, Coast_A6);
        }

        private static async Task KaruiFortress()
        {
            await NoWpAreaHandler(World.Act6.KaruiFortress, KaruiFortressTgt, World.Act6.MudFlats, MudFlats_A6);
        }

        private static async Task Ridge()
        {
            await WpAreaHandler(World.Act6.Ridge, RidgeTgt, World.Act6.KaruiFortress, KaruiFortress);
        }

        private static async Task LowerPrison_A6()
        {
            await WpAreaHandler(World.Act6.LowerPrison, LowerPrisonTgt_A6, World.Act6.Ridge, Ridge);
        }

        private static async Task ShavronneTower()
        {
            await NoWpAreaHandler(World.Act6.ShavronneTower, ShavronneTowerTgt, World.Act6.LowerPrison, LowerPrison_A6,
                () => PrisonerGateTgt_A6.ResetCurrentPosition());
        }

        private static async Task PrisonersGate_A6()
        {
            await ThroughMultilevelAreaHander(World.Act6.PrisonerGate, PrisonerGateTgt_A6, World.Act6.ShavronneTower, ShavronneTower);
        }

        private static async Task WesternForest_A6()
        {
            await WpAreaHandler(World.Act6.WesternForest, WesternForestTgt_A6, World.Act6.PrisonerGate, PrisonersGate_A6);
        }

        private static async Task Riverways_A6()
        {
            await WpAreaHandler(World.Act6.Riverways, RiverwaysTgt_A6, World.Act6.WesternForest, WesternForest_A6);
        }

        private static async Task Wetlands_A6()
        {
            await NoWpAreaHandler(World.Act6.Wetlands, WetlandsTgt_A6, World.Act6.Riverways, Riverways_A6);
        }

        private static async Task SouthernForest_A6()
        {
            await WpAreaHandler(World.Act6.SouthernForest, SouthernForestTgt_A6, World.Act6.Riverways, Riverways_A6);
        }

        private static async Task CavernOfAnger_A6()
        {
            await NoWpAreaHandler(World.Act6.CavernOfAnger, CavernOfAngerTgt_A6, World.Act6.SouthernForest, SouthernForest_A6);
        }

        private static async Task Beacon()
        {
            if (World.Act6.Beacon.IsCurrentArea)
            {
                OuterLogicError(World.Act6.Beacon);
                return;
            }
            if (World.Act6.CavernOfAnger.IsCurrentArea)
            {
                var flagChest = LokiPoe.ObjectManager.GetObjects(LokiPoe.ObjectManager.PoeObjectEnum.Flag_Chest)
                    .FirstOrDefault<Chest>();

                if (flagChest != null && flagChest.PathExists())
                {
                    var roomExit = LokiPoe.ObjectManager.Objects
                        .FirstOrDefault<AreaTransition>(a => a.Metadata == "Metadata/QuestObjects/BossArenaEntranceTransition");

                    if (!await PlayerAction.TakeTransition(roomExit))
                        ErrorManager.ReportError();

                    return;
                }
                await MoveAndEnter(World.Act6.Beacon, BeaconTgt, null);
                return;
            }
            if (World.Act6.Beacon.IsWaypointOpened)
            {
                if (AnyWaypointNearby)
                {
                    await TakeWaypoint(World.Act6.Beacon, null);
                }
                else
                {
                    await TpToTown();
                }
                return;
            }
            await CavernOfAnger_A6();
        }

        private static async Task BrineKingReef()
        {
            await StrictlyWpAreaHandler(World.Act6.BrineKingReef, "Use QuestBot to traverse The Beacon area.");
        }

        #endregion

        #region Act 7

        private static async Task BridgeEncampment()
        {
            await StrictlyWpAreaHandler(World.Act7.BridgeEncampment, "Use QuestBot to enter Act 7 first");
        }

        private static async Task BrokenBridge_A7()
        {
            await TownConnectedAreaHandler(World.Act7.BrokenBridge, BrokenBridgeFromTown, World.Act7.BridgeEncampment, BridgeEncampment);
        }

        private static async Task Crossroads_A7()
        {
            await WpAreaHandler(World.Act7.Crossroads, CrossroadsTgt_A7, World.Act7.BrokenBridge, BrokenBridge_A7);
        }

        private static async Task FellshrineRuins_A7()
        {
            await NoWpAreaHandler(World.Act7.FellshrineRuins, FellshrineTgt_A7, World.Act7.Crossroads, Crossroads_A7);
        }

        private static async Task Crypt_A7()
        {
            await WpAreaHandler(World.Act7.Crypt, CryptTgt_A7, World.Act7.FellshrineRuins, FellshrineRuins_A7);
        }

        private static async Task ChamberOfSins1_A7()
        {
            await WpAreaHandler(World.Act7.ChamberOfSins1, ChamberOfSins1Tgt_A7, World.Act7.Crossroads, Crossroads_A7);
        }

        private static async Task ChamberOfSins2_A7()
        {
            await NoWpAreaHandler(World.Act7.ChamberOfSins2, ChamberOfSins2Tgt_A7, World.Act7.ChamberOfSins1, ChamberOfSins1_A7);
        }

        private static async Task Den_A7()
        {
            await WpAreaHandler(World.Act7.Den, DenTgt_A7, World.Act7.ChamberOfSins2, ChamberOfSins2_A7);
        }

        private static async Task AshenFields()
        {
            await WpAreaHandler(World.Act7.AshenFields, AshenFieldsTgt, World.Act7.Den, Den_A7,
                () => NorthernForestTgt_A7.ResetCurrentPosition());
        }

        private static async Task NorthernForest_A7()
        {
            await ThroughMultilevelAreaHander(World.Act7.NorthernForest, NorthernForestTgt_A7, World.Act7.AshenFields, AshenFields);
        }

        private static async Task DreadThicket_A7()
        {
            await NoWpAreaHandler(World.Act7.DreadThicket, DreadThicketTgt_A7, World.Act7.NorthernForest, NorthernForest_A7);
        }

        private static async Task Causeway()
        {
            await WpAreaHandler(World.Act7.Causeway, CausewayTgt, World.Act7.NorthernForest, NorthernForest_A7);
        }

        private static async Task VaalCity()
        {
            await WpAreaHandler(World.Act7.VaalCity, VaalCityTgt, World.Act7.Causeway, Causeway);
        }

        private static async Task TempleOfDecay1()
        {
            if (World.Act7.TempleOfDecay1.IsCurrentArea)
            {
                OuterLogicError(World.Act7.TempleOfDecay1);
                return;
            }
            if (World.Act7.VaalCity.IsCurrentArea)
            {
                bool hasFireflies = Inventories.InventoryItems
                                        .Count(i => i.Class == ItemClasses.QuestItem && i.Metadata.ContainsIgnorecase("Act7/Firefly")) == 7;

                if (hasFireflies)
                {
                    var yeena = LokiPoe.ObjectManager.Objects.Find(o => o.Metadata == "Metadata/NPC/Act7/YeenaVaalCity");
                    if (yeena != null && yeena.IsTargetable)
                    {
                        var pos = yeena.WalkablePosition();
                        if (pos.IsFar)
                        {
                            pos.Come();
                            return;
                        }
                        if (!await PlayerAction.Interact(yeena))
                        {
                            ErrorManager.ReportError();
                            return;
                        }
                        await Coroutines.CloseBlockingWindows();
                        await Wait.SleepSafe(1000);
                        return;
                    }
                }
                await MoveAndEnter(World.Act7.TempleOfDecay1, TempleOfDecay1Tgt, () => TempleOfDecay2Tgt.ResetCurrentPosition());
                return;
            }
            await VaalCity();
        }

        private static async Task TempleOfDecay2()
        {
            await ThroughMultilevelAreaHander(World.Act7.TempleOfDecay2, TempleOfDecay2Tgt, World.Act7.TempleOfDecay1, TempleOfDecay1,
                () => SarnRampartsTgt.ResetCurrentPosition());
        }

        #endregion

        #region Act 8

        private static async Task SarnRamparts()
        {
            await ThroughMultilevelAreaHander(World.Act8.SarnRamparts, SarnRampartsTgt, World.Act7.TempleOfDecay2, TempleOfDecay2,
                () => SarnEncampmentTgt_A8.ResetCurrentPosition());
        }

        private static async Task SarnEncampment_A8()
        {
            await ThroughMultilevelAreaHander(World.Act8.SarnEncampment, SarnEncampmentTgt_A8, World.Act8.SarnRamparts, SarnRamparts);
        }

        private static async Task ToxicConduits()
        {
            await TownConnectedAreaHandler(World.Act8.ToxicConduits, ToxicConduitsFromTown, World.Act8.SarnEncampment, SarnEncampment_A8);
        }

        private static async Task DoedreCesspool()
        {
            await WpAreaHandler(World.Act8.DoedreCesspool, DoedreCesspoolTgt, World.Act8.ToxicConduits, ToxicConduits,
                () =>
                {
                    GrandPromenadeTgt.ResetCurrentPosition();
                    QuayTgt.ResetCurrentPosition();
                });
        }

        private static async Task GrandPromenade()
        {
            await ThroughMultilevelAreaHander(World.Act8.GrandPromenade, GrandPromenadeTgt, World.Act8.DoedreCesspool, DoedreCesspool);
        }

        private static async Task Quay()
        {
            await ThroughMultilevelAreaHander(World.Act8.Quay, QuayTgt, World.Act8.DoedreCesspool, DoedreCesspool);
        }

        private static async Task GrainGate()
        {
            await WpAreaHandler(World.Act8.GrainGate, GrainGateTgt, World.Act8.Quay, Quay);
        }

        private static async Task ImperialFields()
        {
            await WpAreaHandler(World.Act8.ImperialFields, ImperialFieldsTgt, World.Act8.GrainGate, GrainGate);
        }

        private static async Task Solaris1_A8()
        {
            await WpAreaHandler(World.Act8.SolarisTemple1, Solaris1Tgt_A8, World.Act8.ImperialFields, ImperialFields);
        }

        private static async Task Solaris2_A8()
        {
            await NoWpAreaHandler(World.Act8.SolarisTemple2, Solaris2Tgt_A8, World.Act8.SolarisTemple1, Solaris1_A8);
        }

        private static async Task SolarisConcourse()
        {
            await WpAreaHandler(World.Act8.SolarisConcourse, SolarisConcourseTgt, World.Act8.SolarisTemple1, Solaris1_A8);
        }

        private static async Task BathHouse()
        {
            await WpAreaHandler(World.Act8.BathHouse, BathHouseTgt, World.Act8.GrandPromenade, GrandPromenade);
        }

        private static async Task HighGardens()
        {
            await NoWpAreaHandler(World.Act8.HighGardens, HighGardensTgt, World.Act8.BathHouse, BathHouse);
        }

        private static async Task LunarisConcourse()
        {
            await WpAreaHandler(World.Act8.LunarisConcourse, LunarisConcourseTgt, World.Act8.BathHouse, BathHouse);
        }

        private static async Task Lunaris1_A8()
        {
            await WpAreaHandler(World.Act8.LunarisTemple1, Lunaris1Tgt_A8, World.Act8.LunarisConcourse, LunarisConcourse);
        }

        private static async Task Lunaris2_A8()
        {
            await NoWpAreaHandler(World.Act8.LunarisTemple2, Lunaris2Tgt_A8, World.Act8.LunarisTemple1, Lunaris1_A8);
        }

        private static async Task HarbourBridge()
        {
            await NoWpAreaHandler(World.Act8.HarbourBridge, HarbourBridgeTgt, World.Act8.LunarisConcourse, LunarisConcourse,
                () => BloodAqueductTgt.ResetCurrentPosition());
        }

        #endregion

        #region Act 9

        private static async Task BloodAqueduct()
        {
            await ThroughMultilevelAreaHander(World.Act9.BloodAqueduct, BloodAqueductTgt, World.Act8.HarbourBridge, HarbourBridge);
        }

        private static async Task Highgate_A9()
        {
            await WpAreaHandler(World.Act9.Highgate, HighgateTgt_A9, World.Act9.BloodAqueduct, BloodAqueduct);
        }

        private static async Task Descent()
        {
            await TownConnectedAreaHandler(World.Act9.Descent, DescentFromTown, World.Act9.Highgate, Highgate_A9,
                () => VastiriDesertTgt.ResetCurrentPosition());
        }

        private static async Task VastiriDesert()
        {
            await ThroughMultilevelAreaHander(World.Act9.VastiriDesert, VastiriDesertTgt, World.Act9.Descent, Descent);
        }

        private static async Task Oasis()
        {
            await NoWpAreaHandler(World.Act9.Oasis, OasisTgt, World.Act9.VastiriDesert, VastiriDesert);
        }

        private static async Task Foothills()
        {
            await WpAreaHandler(World.Act9.Foothills, FoothillsTgt, World.Act9.VastiriDesert, VastiriDesert);
        }

        private static async Task BoilingLake()
        {
            await NoWpAreaHandler(World.Act9.BoilingLake, BoilingLakeTgt, World.Act9.Foothills, Foothills);
        }

        private static async Task Tunnel()
        {
            await WpAreaHandler(World.Act9.Tunnel, TunnelTgt, World.Act9.Foothills, Foothills);
        }

        private static async Task Quarry()
        {
            await WpAreaHandler(World.Act9.Quarry, QuarryTgt, World.Act9.Tunnel, Tunnel);
        }

        private static async Task Refinery()
        {
            await NoWpAreaHandler(World.Act9.Refinery, RefineryTgt, World.Act9.Quarry, Quarry);
        }

        private static async Task Belly_A9()
        {
            if (World.Act9.BellyOfBeast.IsCurrentArea)
            {
                OuterLogicError(World.Act9.BellyOfBeast);
                return;
            }
            if (World.Act9.Quarry.IsCurrentArea)
            {
                var sin = LokiPoe.ObjectManager.Objects.Find(o => o.Metadata == "Metadata/NPC/Act9/SinQuarry");
                if (sin != null && sin.HasNpcFloatingIcon)
                {
                    var pos = sin.WalkablePosition();
                    if (pos.IsFar)
                    {
                        pos.Come();
                        return;
                    }
                    if (!await PlayerAction.Interact(sin))
                    {
                        ErrorManager.ReportError();
                        return;
                    }
                    await Coroutines.CloseBlockingWindows();
                    await Wait.SleepSafe(1000);
                    return;
                }
                await MoveAndEnter(World.Act9.BellyOfBeast, BellyTgt_A9, null);
                return;
            }
            await Quarry();
        }

        private static async Task RottingCore()
        {
            await NoWpAreaHandler(World.Act9.RottingCore, RottingCoreTgt, World.Act9.BellyOfBeast, Belly_A9);
        }

        #endregion

        #region Act 10

        private static async Task OriathDocks()
        {
            await StrictlyWpAreaHandler(World.Act10.OriathDocks, "Use QuestBot to enter Act 10 first");
        }

        private static async Task CathedralRooftop_A10()
        {
            await TownConnectedAreaHandler(World.Act10.CathedralRooftop, RooftopFromTown, World.Act10.OriathDocks, OriathDocks);
        }

        private static async Task RavagedSquare()
        {
            await WpAreaHandler(World.Act10.RavagedSquare, RavagedSquareTgt, World.Act10.CathedralRooftop, CathedralRooftop_A10);
        }

        private static async Task Ossuary_A10()
        {
            await NoWpAreaHandler(World.Act10.Ossuary, OssuaryTgt_A10, World.Act10.RavagedSquare, RavagedSquare);
        }

        private static async Task TorchedCourts_A10()
        {
            await NoWpAreaHandler(World.Act10.TorchedCourts, TorchedCourtsTgt_A10, World.Act10.RavagedSquare, RavagedSquare);
        }

        private static async Task Reliquary_A10()
        {
            await WpAreaHandler(World.Act10.Reliquary, ReliquaryTgt_A10, World.Act10.RavagedSquare, RavagedSquare);
        }

        private static async Task ControlBlocks_A10()
        {
            await WpAreaHandler(World.Act10.ControlBlocks, ControlBlocksTgt_A10, World.Act10.RavagedSquare, RavagedSquare);
        }

        private static async Task DesecratedChambers()
        {
            await WpAreaHandler(World.Act10.DesecratedChambers, DesecratedChambersTgt, World.Act10.TorchedCourts, TorchedCourts_A10);
        }

        private static async Task Canals()
        {
            if (World.Act10.Canals.IsCurrentArea)
            {
                OuterLogicError(World.Act10.Canals);
                return;
            }
            if (World.Act10.RavagedSquare.IsCurrentArea)
            {
                var innocence = LokiPoe.ObjectManager.Objects.Find(o => o.Metadata == "Metadata/NPC/Act10/InnocenceSquare");
                if (innocence != null && innocence.HasNpcFloatingIcon)
                {
                    var pos = innocence.WalkablePosition();
                    if (pos.IsFar)
                    {
                        pos.Come();
                        return;
                    }
                    if (!await PlayerAction.Interact(innocence))
                    {
                        ErrorManager.ReportError();
                        return;
                    }
                    await Coroutines.CloseBlockingWindows();
                    await Wait.SleepSafe(1000);
                    return;
                }
                await MoveAndEnter(World.Act10.Canals, CanalsTgt, null);
                return;
            }
            await RavagedSquare();
        }

        private static async Task FeedingTrough()
        {
            await NoWpAreaHandler(World.Act10.FeedingTrough, FeedingTroughTgt, World.Act10.Canals, Canals);
        }

        #endregion

        #region Act 11

        private static async Task Oriath()
        {
            await StrictlyWpAreaHandler(World.Act11.Oriath, "Use QuestBot to enter Act 11 first");
        }

        private static async Task TemplarLaboratory()
        {
            await TownConnectedAreaHandler(World.Act11.TemplarLaboratory, LaboratoryFromTown, World.Act11.Oriath, Oriath);
        }

        private static async Task FallenCourts()
        {
            await TownConnectedAreaHandler(World.Act11.FallenCourts, FallenCourtsFromTown, World.Act11.Oriath, Oriath);
        }

        private static async Task HauntedReliquary()
        {
            await TownConnectedAreaHandler(World.Act11.HauntedReliquary, HauntedReliquaryFromTown, World.Act11.Oriath, Oriath);
        }

        #endregion

        #endregion

        #region Helpers

        private static bool AnyWaypointNearby
        {
            get
            {
                var area = World.CurrentArea;
                //return area.IsTown || area.IsHideoutArea || area.IsMapRoom || LokiPoe.ObjectManager.Objects.Exists(o => o is Waypoint && o.Distance <= 70 && o.PathDistance() <= 73);
                return area.IsTown || area.IsHideoutArea ||
                       LokiPoe.ObjectManager.Objects.Exists(o => o is Waypoint && o.Distance <= 70 && o.PathDistance() <= 73);
            }
        }

        private static async Task WpAreaHandler(AreaInfo area, TgtPosition tgtPos, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
        {
            if (area.IsCurrentArea)
            {
                OuterLogicError(area);
                return;
            }
            if (prevArea.IsCurrentArea)
            {
                await MoveAndEnter(area, tgtPos, postEnter);
                return;
            }
            if (area.IsWaypointOpened)
            {
                if (AnyWaypointNearby)
                {
                    await TakeWaypoint(area, postEnter);
                }
                else
                {
                    await TpToTown();
                }
                return;
            }
            await prevAreaHandler();
        }

        private static async Task NoWpAreaHandler(AreaInfo area, TgtPosition tgtPos, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
        {
            if (area.IsCurrentArea)
            {
                OuterLogicError(area);
                return;
            }
            if (prevArea.IsCurrentArea)
            {
                await MoveAndEnter(area, tgtPos, postEnter);
                return;
            }
            await prevAreaHandler();
        }

        private static async Task ThroughMultilevelAreaHander(AreaInfo area, TgtPosition nextLevelTgt, AreaInfo prevArea, Func<Task> prevAreaHandler, Action postEnter = null)
        {
            if (area.IsCurrentArea)
            {
                OuterLogicError(area);
                return;
            }
            if (area.IsWaypointOpened)
            {
                if (AnyWaypointNearby)
                {
                    await TakeWaypoint(area, postEnter);
                }
                else
                {
                    await TpToTown();
                }
                return;
            }
            if (prevArea.IsCurrentArea)
            {
                await MoveAndEnterMultilevel(area, nextLevelTgt, postEnter);
                return;
            }

            await prevAreaHandler();
        }

        private static async Task TownConnectedAreaHandler(AreaInfo area, WalkablePosition transitionPos, AreaInfo town, Func<Task> townHandler, Action postEnter = null)
        {
            if (area.IsCurrentArea)
            {
                OuterLogicError(area);
                return;
            }
            if (area.IsWaypointOpened)
            {
                if (AnyWaypointNearby)
                {
                    await TakeWaypoint(area, postEnter);
                }
                else
                {
                    await TpToTown();
                }
                return;
            }
            if (town.IsCurrentArea)
            {
                await transitionPos.ComeAtOnce();
                await TakeTransition(area, postEnter);
                return;
            }
            await townHandler();
        }

        private static async Task StrictlyWpAreaHandler(AreaInfo area, string hint, Action postEnter = null)
        {
            if (!area.IsWaypointOpened)
            {
                GlobalLog.Error($"[Travel] {area.Name} waypoint is not available. {hint}.");
                ErrorManager.ReportCriticalError();
                return;
            }
            if (AnyWaypointNearby)
            {
                await TakeWaypoint(area, postEnter);
            }
            else
            {
                await TpToTown();
            }
        }

        private static async Task MoveAndEnter(AreaInfo area, TgtPosition tgtPos, Action postEnter)
        {
            var pos = GetCachedTransitionPos(area);
            if (pos != null)
            {
                if (pos.IsFar)
                {
                    pos.Come();
                }
                else
                {
                    await TakeTransition(area, tgtPos, postEnter);
                }
            }
            else
            {
                if (tgtPos.IsFar)
                {
                    tgtPos.Come();
                }
                else
                {
                    await TakeTransition(area, tgtPos, postEnter);
                }
            }
        }

        private static async Task MoveAndEnterMultilevel(AreaInfo area, TgtPosition tgtPos, Action postEnter)
        {
            if (tgtPos.IsFar)
            {
                tgtPos.Come();
                return;
            }

            var transition = await GetTransitionObject(tgtPos, null);
            if (transition == null)
                return;

            bool isDestination = transition.LeadsTo(area);
            bool newInstance = isDestination && NewInstanceRequests.Contains(area);

            if (!await PlayerAction.TakeTransition(transition, newInstance))
            {
                ErrorManager.ReportError();
                return;
            }
            if (isDestination)
            {
                if (newInstance) NewInstanceRequests.Remove(area);
                postEnter?.Invoke();
            }
            else
            {
                tgtPos.ResetCurrentPosition();
            }
        }

        private static async Task TakeWaypoint(AreaInfo area, Action postEnter)
        {
            bool newInstance = NewInstanceRequests.Contains(area);
            if (!await PlayerAction.TakeWaypoint(area, newInstance))
            {
                ErrorManager.ReportError();
                return;
            }
            if (newInstance) NewInstanceRequests.Remove(area);
            postEnter?.Invoke();
        }

        private static async Task TakeTransition(AreaInfo area, Action postEnter)
        {
            var transition = LokiPoe.ObjectManager.Objects.FirstOrDefault<AreaTransition>(a => a.LeadsTo(area));
            if (transition == null)
            {
                GlobalLog.Error($"[Travel] There is no transition that leads to {area}");
                ErrorManager.ReportError();
                return;
            }

            bool newInstance = NewInstanceRequests.Contains(area);
            if (!await PlayerAction.TakeTransition(transition, newInstance))
            {
                ErrorManager.ReportError();
                return;
            }
            if (newInstance) NewInstanceRequests.Remove(area);
            postEnter?.Invoke();
        }

        private static async Task TakeTransition(AreaInfo area, TgtPosition tgtPos, Action postEnter)
        {
            var transition = await GetTransitionObject(tgtPos, area);
            if (transition == null)
                return;

            bool newInstance = NewInstanceRequests.Contains(area);
            if (!await PlayerAction.TakeTransition(transition, newInstance))
            {
                ErrorManager.ReportError();
                return;
            }
            if (newInstance) NewInstanceRequests.Remove(area);
            postEnter?.Invoke();
        }

        private static async Task<AreaTransition> GetTransitionObject(TgtPosition tgtPos, AreaInfo area)
        {
            var transition = LokiPoe.ObjectManager.Objects.Closest<AreaTransition>();
            if (transition == null)
            {
                if (area == World.Act5.OverseerTower)
                {
                    var ladder = LokiPoe.ObjectManager.Objects
                        .Find(o => o.Metadata == "Metadata/Terrain/Act5/Area1/Objects/ProximitySpearLadderOnce");

                    if (ladder != null)
                    {
                        GlobalLog.Debug($"[Travel] Ladder detected");
                        if (ladder.Distance > 10 && ladder.Distance < 90)
                        {
                            await new WalkablePosition("Ladder activation", ladder.Position).TryComeAtOnce(10);
                        }
                        else if (ladder.Distance > 20)
                        {
                            PlayerMoverManager.MoveTowards(ladder.Position);
                        }
                    }
                    GlobalLog.Debug($"[Travel] Waiting for \"{area.Name}\" transition activation.");
                    return null;
                }
                GlobalLog.Warn("[Travel] There is no area transition near tgt position.");
                tgtPos.ProceedToNext();
                return null;
            }
            if (transition.TransitionType == DreamPoeBot.Loki.Game.GameData.TransitionTypes.NormalToCorrupted)
            {
                GlobalLog.Warn("[Travel] Corrupted area entrance has the same tgt as our destination.");
                tgtPos.ProceedToNext();
                return null;
            }
            if (!transition.IsTargetable)
            {
                if (area == null)
                {
                    GlobalLog.Debug("[Travel] Waiting for transition activation.");
                }
                else
                {
                    GlobalLog.Debug($"[Travel] Waiting for \"{area.Name}\" transition activation.");
                }
                return null;
            }
            if (area != null)
            {
                var dest = transition.Destination;
                if (area != dest)
                {
                    GlobalLog.Warn($"[Travel] Transition leads to \"{dest.Name}\". Expected: \"{area.Name}\".");
                    tgtPos.ProceedToNext();
                    return null;
                }
            }
            return transition;
        }

        private static async Task TpToTown()
        {
            if (!await PlayerAction.TpToTown())
                ErrorManager.ReportError();
        }

        private static WalkablePosition GetCachedTransitionPos(AreaInfo area)
        {
            return CombatAreaCache.Current.AreaTransitions.Find(t => t.Destination == area)?.Position;
        }

        private static void OuterLogicError(AreaInfo area)
        {
            GlobalLog.Error($"[Travel] Outer logic error. Travel to {area} has been called, but we are already here.");
            ErrorManager.ReportError();
        }

        #endregion

        #region Tgt positions

        private static readonly TgtPosition LioneyeWatchTgt
            = new TgtPosition(World.Act1.LioneyeWatch.Name, "beachtown_south_entrance.tgt");

        private static readonly TgtPosition TidalIslandTgt
            = new TgtPosition(World.Act1.TidalIsland.Name, "act1_karui_coast_to_island_transition_v01_01.tgt");

        private static readonly TgtPosition MudFlatsTgt
            = new TgtPosition(World.Act1.MudFlats.Name, "act1_area2_transition_v01_01.tgt");

        private static readonly TgtPosition FetidPoolTgt
            = new TgtPosition(World.Act1.FetidPool.Name, "act1_beach_toswamp_fetid_v01_01.tgt");

        private static readonly TgtPosition SubmergedPassageTgt
            = new TgtPosition(World.Act1.SubmergedPassage.Name, "Beach_to_watercave_v2.tgt");

        private static readonly TgtPosition FloodedDepthsTgt
            = new TgtPosition(World.Act1.FloodedDepths.Name, "watery_depth_entrance_v01_01.tgt");

        private static readonly TgtPosition LedgeTgt
            = new TgtPosition(World.Act1.Ledge.Name, "caveup_exit_v01_01.tgt");

        private static readonly TgtPosition ClimbTgt
            = new TgtPosition(World.Act1.Climb.Name, "beach_passageway_v01_01.tgt");

        private static readonly TgtPosition LowerPrisonTgt
            = new TgtPosition(World.Act1.LowerPrison.Name, "beach_prisonback.tgt");

        private static readonly TgtPosition UpperPrisonTgt
            = new TgtPosition(World.Act1.UpperPrison.Name, "dungeon_prison_exit_up_v01_01.tgt");

        private static readonly TgtPosition PrisonerGateTgt
            = new TgtPosition("Next prison level", "dungeon_prison_exit_up_v01_01.tgt | dungeon_prison_boss_exit_v01_02.tgt | dungeon_prison_door_up_v01_01.tgt", true);

        private static readonly TgtPosition ShipGraveyardTgt
            = new TgtPosition(World.Act1.ShipGraveyard.Name, "shipgraveyard_passageway_v01_01.tgt");

        private static readonly TgtPosition ShipGraveyardCaveTgt
            = new TgtPosition(World.Act1.ShipGraveyardCave.Name, "ship_entrance_v01_01.tgt");

        private static readonly TgtPosition CavernOfWrathTgt
            = new TgtPosition(World.Act1.CavernOfWrath.Name, "beach_caveentranceskeleton_v01_01.tgt | beach_caveentranceskeleton_v01_02.tgt");

        private static readonly TgtPosition CavernOfAngerTgt
            = new TgtPosition(World.Act1.CavernOfAnger.Name, "caveup_exit_v01_01.tgt");

        private static readonly TgtPosition SouthernForestTgt
            = new TgtPosition(World.Act2.SouthernForest.Name, "caveup_exit_v01_01.tgt | merveil_exit_clean_v01_01.tgt", true);

        private static readonly TgtPosition ForestEncampmentTgt
            = new TgtPosition(World.Act2.ForestEncampment.Name, "forestcamp_dock_v01_01.tgt");

        private static readonly TgtPosition WesternForestTgt
            = new TgtPosition(World.Act2.WesternForest.Name, "roadtothickforest_entrance_v01_01.tgt");

        private static readonly TgtPosition WeaverChambersTgt
            = new TgtPosition(World.Act2.WeaverChambers.Name, "spidergrove_entrance_v01_01.tgt");

        private static readonly TgtPosition DenTgt
            = new TgtPosition(World.Act2.Den.Name, "forestcave_entrance_hole_v01_01.tgt");

        private static readonly TgtPosition CrossroadsTgt
            = new TgtPosition(World.Act2.Crossroads.Name, "wall_gate_v01_01.tgt");

        private static readonly TgtPosition ChamberOfSins1Tgt
            = new TgtPosition(World.Act2.ChamberOfSins1.Name, "temple_entrance_v01_01.tgt");

        private static readonly TgtPosition ChamberOfSins2Tgt
            = new TgtPosition(World.Act2.ChamberOfSins2.Name, "templeruinforest_exit_down_v01_01.tgt");

        private static readonly TgtPosition BrokenBridgeTgt
            = new TgtPosition(World.Act2.BrokenBridge.Name, "bridgeconnection_v01_01.tgt");

        private static readonly TgtPosition Crypt1Tgt
            = new TgtPosition(World.Act2.Crypt1.Name, "church_dungeon_entrance_v01_01.tgt");

        private static readonly TgtPosition Crypt2Tgt
            = new TgtPosition(World.Act2.Crypt2.Name, "dungeon_church_exit_down_v01_01.tgt");

        private static readonly TgtPosition WetlandsTgt
            = new TgtPosition(World.Act2.Wetlands.Name, "bridgeconnection_v01_01.tgt");

        private static readonly TgtPosition VaalRuinsTgt
            = new TgtPosition(World.Act2.VaalRuins.Name, "forest_caveentrance_inca_v01_01.tgt");

        private static readonly TgtPosition NorthernForestTgt
            = new TgtPosition(World.Act2.NorthernForest.Name, "dungeon_inca_exit_v01_01.tgt");

        private static readonly TgtPosition DreadThicketTgt
            = new TgtPosition(World.Act2.DreadThicket.Name, "grovewall_entrance_v01_01.tgt");

        private static readonly TgtPosition CavernsTgt
            = new TgtPosition(World.Act2.Caverns.Name, "waterfall_cave_entrance_v01_01.tgt");

        private static readonly TgtPosition AncientPyramidTgt
            = new TgtPosition(World.Act2.AncientPyramid.Name, "dungeon_stairs_up_v01_01.tgt");

        private static readonly TgtPosition CityOfSarnTgt
            = new TgtPosition("Next pyramid level", "dungeon_stairs_up_v01_01.tgt | dungeon_huangdoor_v01_01.tgt", true);

        private static readonly TgtPosition SarnEncampmentTgt
            = new TgtPosition(World.Act3.SarnEncampment.Name, "act3_docks_to_town_lower_01_01.tgt");

        private static readonly TgtPosition CrematoriumTgt
            = new TgtPosition(World.Act3.Crematorium.Name, "act3_prison_entrance_01_01.tgt");

        private static readonly TgtPosition SewersTgt
            = new TgtPosition(World.Act3.Sewers.Name, "slum_sewer_entrance_v02_01.tgt");

        private static readonly TgtPosition MarketplaceTgt
            = new TgtPosition(World.Act3.Marketplace.Name, "sewerwall_exit_v01_01.tgt");

        private static readonly TgtPosition CatacombsTgt
            = new TgtPosition(World.Act3.Catacombs.Name, "markettochurchdungeon_v01_01.tgt");

        private static readonly TgtPosition BattlefrontTgt
            = new TgtPosition(World.Act3.Battlefront.Name, "market_to_battlefront_v01_01.tgt");

        private static readonly TgtPosition DocksTgt
            = new TgtPosition(World.Act3.Docks.Name, "battlefield_arch_v01_03.tgt");

        private static readonly TgtPosition Solaris1Tgt
            = new TgtPosition(World.Act3.SolarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");

        private static readonly TgtPosition Solaris2Tgt
            = new TgtPosition(World.Act3.SolarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");

        private static readonly TgtPosition EbonyBarracksTgt
            = new TgtPosition(World.Act3.EbonyBarracks.Name, "sewerexit_v01_01.tgt");

        private static readonly TgtPosition Lunaris1Tgt
            = new TgtPosition(World.Act3.LunarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");

        private static readonly TgtPosition Lunaris2Tgt
            = new TgtPosition(World.Act3.LunarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");

        private static readonly TgtPosition ImperialGardensTgt
            = new TgtPosition(World.Act3.ImperialGardens.Name, "garden_arch_v01_01.tgt");

        private static readonly TgtPosition LibraryTgt
            = new TgtPosition(World.Act3.Library.Name, "Library_LargeBuilding_entrance_v01_01.tgt");

        private static readonly TgtPosition ArchivesTgt
            = new TgtPosition(World.Act3.Archives.Name, "library_entrance_v02_01.tgt");

        private static readonly TgtPosition SceptreOfGodTgt
            = new TgtPosition(World.Act3.SceptreOfGod.Name, "Act3_EpicDoor_v02_01.tgt");

        private static readonly TgtPosition UpperSceptreTgt
            = new TgtPosition("Next tower level", "tower_transition_up_01_01.tgt", true);

        private static readonly TgtPosition AqueductTgt
            = new TgtPosition("Next tower level", "tower_transition_up_01_01.tgt | tower_totowertop_v01_01.tgt | Act3_tower_01_01.tgt", true, 10, 40);

        private static readonly TgtPosition HighgateTgt
            = new TgtPosition(World.Act4.Highgate.Name, "mountiantown_connection.tgt");

        private static readonly TgtPosition Mines2Tgt
            = new TgtPosition(World.Act4.Mines2.Name, "mine_areatransition_v0?_0?.tgt");

        private static readonly TgtPosition CrystalVeinsTgt
            = new TgtPosition(World.Act4.CrystalVeins.Name, "mine_areatransition_v03_01.tgt");

        private static readonly TgtPosition RaptureDeviceTgt
            = new TgtPosition("Rapture Device", "crystals_openAnimation_v01_01.tgt");

        private static readonly TgtPosition KaomStrongholdTgt
            = new TgtPosition(World.Act4.KaomStronghold.Name, "lava_abyss_transition_entrance_v01_01.tgt", false, 10, 50);

        private static readonly TgtPosition GrandArenaTgt
            = new TgtPosition(World.Act4.GrandArena.Name, "arena_areatransition_v01_01.tgt");

        private static readonly TgtPosition Belly2Tgt
            = new TgtPosition(World.Act4.BellyOfBeast2.Name, "belly_tunnel_v01_01.tgt");

        private static readonly TgtPosition HarvestTgt
            = new TgtPosition(World.Act4.Harvest.Name, "belly_tunnel_level2_v01_02.tgt");

        private static readonly TgtPosition OverseerTowerTgt
            = new TgtPosition(World.Act5.OverseerTower.Name, "tower_v01_01.tgt", false, 10, 25);

        private static readonly TgtPosition OriathSquareTgt
            = new TgtPosition(World.Act5.OriathSquare.Name, "security_exit_v01_01.tgt");

        private static readonly TgtPosition TemplarCourtsTgt
            = new TgtPosition(World.Act5.TemplarCourts.Name, "Oriath_AreaTransition_v01_03.tgt");

        private static readonly TgtPosition ChamberOfInnocenceTgt
            = new TgtPosition(World.Act5.TemplarCourts.Name, "templar_to_innocents_v01_01.tgt");

        private static readonly TgtPosition TorchedCourtsTgt
            = new TgtPosition(World.Act5.TorchedCourts.Name, "transition_chamber_to_courts_v01_01.tgt");

        private static readonly TgtPosition RuinedSquareTgt
            = new TgtPosition(World.Act5.RuinedSquare.Name, "templar_oriath_transition_v01_01.tgt");

        private static readonly TgtPosition ReliquaryTgt
            = new TgtPosition(World.Act5.Reliquary.Name, "Oriath_AreaTransition_v01_02.tgt");

        private static readonly TgtPosition OssuaryTgt
            = new TgtPosition(World.Act5.Ossuary.Name, "Oriath_AreaTransition_v01_04.tgt");

        private static readonly TgtPosition CathedralRooftopTgt
            = new TgtPosition(World.Act5.CathedralRooftop.Name, "chitus_statuewall_transition_v01_01.tgt");

        private static readonly TgtPosition TidalIslandTgt_A6
            = new TgtPosition(World.Act6.TidalIsland.Name, "karui_coast_to_island_transition_v01_01.tgt");

        private static readonly TgtPosition MudFlatsTgt_A6
            = new TgtPosition(World.Act6.MudFlats.Name, "act6_area2_transition_v01_01.tgt");

        private static readonly TgtPosition KaruiFortressTgt
            = new TgtPosition(World.Act6.KaruiFortress.Name, "beach_karuipools_v01_01.tgt");

        private static readonly TgtPosition RidgeTgt
            = new TgtPosition(World.Act6.Ridge.Name, "swamp_to_ridge_v01_01.tgt");

        private static readonly TgtPosition LowerPrisonTgt_A6
            = new TgtPosition(World.Act6.LowerPrison.Name, "ledge_prisonback.tgt");

        private static readonly TgtPosition ShavronneTowerTgt
            = new TgtPosition(World.Act6.ShavronneTower.Name, "shavronne_prison_door_up_v01_01.tgt");

        private static readonly TgtPosition PrisonerGateTgt_A6
            = new TgtPosition("Next tower level", "dungeon_prison_exit_up_v01_01.tgt | prison_ladder_v01_01.tgt | tower_spiral_stair_v01_01.tgt | dungeon_prison_door_up_v01_01.tgt", true);

        private static readonly TgtPosition WesternForestTgt_A6
            = new TgtPosition(World.Act6.WesternForest.Name, "beach_passageblock_v01_01.tgt");

        private static readonly TgtPosition RiverwaysTgt_A6
            = new TgtPosition(World.Act6.Riverways.Name, "roadtothickforest_entrance_v01_01.tgt");

        private static readonly TgtPosition WetlandsTgt_A6
            = new TgtPosition(World.Act6.Wetlands.Name, "bridgeconnection_v01_01.tgt");

        private static readonly TgtPosition SouthernForestTgt_A6
            = new TgtPosition(World.Act6.SouthernForest.Name, "forest_to_river_v01_01.tgt");

        private static readonly TgtPosition CavernOfAngerTgt_A6
            = new TgtPosition(World.Act6.CavernOfAnger.Name, "forest_caveentrance_v01_01.tgt");

        private static readonly TgtPosition BeaconTgt
            = new TgtPosition(World.Act6.Beacon.Name, "caveup_exit_v01_01.tgt");

        private static readonly TgtPosition CrossroadsTgt_A7
            = new TgtPosition(World.Act7.Crossroads.Name, "bridgeconnection_v01_01.tgt", false, 10, 50);

        private static readonly TgtPosition FellshrineTgt_A7
            = new TgtPosition(World.Act7.FellshrineRuins.Name, "wall_gate_v01_01.tgt", false, 10, 50);

        private static readonly TgtPosition CryptTgt_A7
            = new TgtPosition(World.Act7.Crypt.Name, "church_dungeon_entrance_v01_01.tgt");

        private static readonly TgtPosition ChamberOfSins1Tgt_A7
            = new TgtPosition(World.Act7.ChamberOfSins1.Name, "temple_entrance_v01_01.tgt");

        private static readonly TgtPosition ChamberOfSins2Tgt_A7
            = new TgtPosition(World.Act7.ChamberOfSins2.Name, "templeruinforest_exit_down_v01_01.tgt");

        private static readonly TgtPosition DenTgt_A7
            = new TgtPosition(World.Act7.Den.Name, "templeruinforest_maligaro_passage.tgt");

        private static readonly TgtPosition AshenFieldsTgt
            = new TgtPosition(World.Act7.AshenFields.Name, "forestcaveup_exit_v01_01.tgt");

        private static readonly TgtPosition NorthernForestTgt_A7
            = new TgtPosition(World.Act7.NorthernForest.Name, "oldfields_campboss_v01_01.tgt | oldfields_campboss_v01_01.tgt", true, 10, 20);

        private static readonly TgtPosition DreadThicketTgt_A7
            = new TgtPosition(World.Act7.DreadThicket.Name, "grovewall_entrance_v01_01.tgt");

        private static readonly TgtPosition CausewayTgt
            = new TgtPosition(World.Act7.Causeway.Name, "forestriver_plinthtransition_v01_01.tgt");

        private static readonly TgtPosition VaalCityTgt
            = new TgtPosition(World.Act7.VaalCity.Name, "vaal_stairs_bottom_v01_01.tgt", false, 15);

        private static readonly TgtPosition TempleOfDecay1Tgt
            = new TgtPosition(World.Act7.TempleOfDecay1.Name, "BanteaySrei_Web.tgt");

        private static readonly TgtPosition TempleOfDecay2Tgt
            = new TgtPosition("Next temple level", "dungeon_web_stairs_down_v01_01.tgt", true);

        private static readonly TgtPosition SarnRampartsTgt
            = new TgtPosition("Next temple level", "dungeon_web_stairs_down_v01_01.tgt | dungeon_web_inca_exit_v03_01.tgt | dungeon_web_inca_exit_v01_01.tgt", true);

        private static readonly TgtPosition SarnEncampmentTgt_A8
            = new TgtPosition("Next ramparts level", "ramparts_wall_accesss_v01_01.tgt | act8_docks_v01_01.tgt");

        private static readonly TgtPosition DoedreCesspoolTgt
            = new TgtPosition(World.Act8.DoedreCesspool.Name, "sewerwall_end_tunnel_v01_0?.tgt");

        private static readonly TgtPosition GrandPromenadeTgt
            = new TgtPosition(World.Act8.GrandPromenade.Name, "slum_sewer_entrance_v03_01.tgt | doedre_sewer_grate_v01_01.tgt | sewer_ladder_up_v01_01.tgt", true);

        private static readonly TgtPosition QuayTgt
            = new TgtPosition(World.Act8.Quay.Name, "slum_sewer_entrance_v03_01.tgt | doedre_sewer_grate_v01_01.tgt | sewerwall_exit_v01_01.tgt", true, 10, 17);

        private static readonly TgtPosition GrainGateTgt
            = new TgtPosition(World.Act8.GrainGate.Name, "market_transition_warehouse_v01_01.tgt");

        private static readonly TgtPosition ImperialFieldsTgt
            = new TgtPosition(World.Act8.ImperialFields.Name, "act8_grain_gate_transition_v01_01.tgt");

        private static readonly TgtPosition Solaris1Tgt_A8
            = new TgtPosition(World.Act8.SolarisTemple1.Name, "act8_temple_entrance_v01_01.tgt");

        private static readonly TgtPosition Solaris2Tgt_A8
            = new TgtPosition(World.Act8.SolarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");

        private static readonly TgtPosition SolarisConcourseTgt
            = new TgtPosition(World.Act8.SolarisConcourse.Name, "temple_to_battlefront_v01_01.tgt");

        private static readonly TgtPosition BathHouseTgt
            = new TgtPosition(World.Act8.BathHouse.Name, "arch_promenade_to_arsenal_v01_01.tgt");

        private static readonly TgtPosition HighGardensTgt
            = new TgtPosition(World.Act8.HighGardens.Name, "bathhouse_transition_v01_01.tgt", true);

        private static readonly TgtPosition LunarisConcourseTgt
            = new TgtPosition(World.Act8.LunarisConcourse.Name, "bathhouse_transition_v01_01.tgt");

        private static readonly TgtPosition Lunaris1Tgt_A8
            = new TgtPosition(World.Act8.LunarisTemple1.Name, "act3_temple_entrance_v01_01.tgt");

        private static readonly TgtPosition Lunaris2Tgt_A8
            = new TgtPosition(World.Act8.LunarisTemple2.Name, "templeclean_exit_down_v01_01.tgt");

        private static readonly TgtPosition HarbourBridgeTgt
            = new TgtPosition(World.Act8.HarbourBridge.Name, "act3_riverbridge_transition_v01_01.tgt");

        private static readonly TgtPosition BloodAqueductTgt
            = new TgtPosition(World.Act9.BloodAqueduct.Name, "bridge_arena_v01_01.tgt | bridge_arena_v01_01.tgt", true);

        private static readonly TgtPosition HighgateTgt_A9
            = new TgtPosition(World.Act9.Highgate.Name, "mountiantown_connection_blood.tgt");

        private static readonly TgtPosition VastiriDesertTgt
            = new TgtPosition("Next Descent level", "descent_top_winch_v01_01.tgt | descent_ravine_convex_winch_v01_01.tgt | decent_ravinestraight_tocliff_winch_v01_01.tgt", true);

        private static readonly TgtPosition OasisTgt
            = new TgtPosition(World.Act9.Oasis.Name, "wagons_transition_v01_01.tgt");

        private static readonly TgtPosition FoothillsTgt
            = new TgtPosition(World.Act9.Foothills.Name, "desert_to_foothills_v01_01.tgt");

        private static readonly TgtPosition BoilingLakeTgt
            = new TgtPosition(World.Act9.BoilingLake.Name, "foothills_to_boilinglakev01_01.tgt");

        private static readonly TgtPosition TunnelTgt
            = new TgtPosition(World.Act9.Tunnel.Name, "foothills_tunnel_exit_v01_01.tgt");

        private static readonly TgtPosition QuarryTgt
            = new TgtPosition(World.Act9.Quarry.Name, "tunnels_to_quarry_transition_v01_01.tgt");

        private static readonly TgtPosition RefineryTgt
            = new TgtPosition(World.Act9.Refinery.Name, "warehouse_entrance_v01_01.tgt");

        private static readonly TgtPosition BellyTgt_A9
            = new TgtPosition(World.Act9.BellyOfBeast.Name, "BeastMembrane.tgt");

        private static readonly TgtPosition RottingCoreTgt
            = new TgtPosition(World.Act9.RottingCore.Name, "belly_to_rottingcore_v01_01.tgt");
        //= new TgtPosition(World.Act9.RottingCore.Name, "belly_to_quarry_v01_01.tgt");

        private static readonly TgtPosition RavagedSquareTgt
            = new TgtPosition(World.Act10.RavagedSquare.Name, "cathedral_roof_transition_v01_01.tgt");

        private static readonly TgtPosition OssuaryTgt_A10
            = new TgtPosition(World.Act10.Ossuary.Name, "Oriath_AreaTransition_v01_04.tgt");

        private static readonly TgtPosition TorchedCourtsTgt_A10
            = new TgtPosition(World.Act10.TorchedCourts.Name, "Oriath_AreaTransition_v01_03.tgt");

        private static readonly TgtPosition ReliquaryTgt_A10
            = new TgtPosition(World.Act10.Reliquary.Name, "Oriath_AreaTransition_v01_02.tgt");

        private static readonly TgtPosition ControlBlocksTgt_A10
            = new TgtPosition(World.Act10.ControlBlocks.Name, "slaveden_entrance_steps_v01_01.tgt");

        private static readonly TgtPosition DesecratedChambersTgt
            = new TgtPosition(World.Act10.DesecratedChambers.Name, "templar_to_innocents_v01_01.tgt");

        private static readonly TgtPosition CanalsTgt
            = new TgtPosition(World.Act10.Canals.Name, "OriathBlockage_v02_01.tgt");

        private static readonly TgtPosition FeedingTroughTgt
            = new TgtPosition(World.Act10.FeedingTrough.Name, "CanalBridgeTransition_v01_01.tgt");

        #endregion

        #region Static positions

        private static readonly WalkablePosition CoastFromTown
            = new WalkablePosition(World.Act1.Coast.Name, 384, 217);

        private static readonly WalkablePosition RiverwaysFromTown
            = new WalkablePosition(World.Act2.Riverways.Name, 81, 262);

        private static readonly WalkablePosition OldFieldsFromTown
            = new WalkablePosition(World.Act2.OldFields.Name, 303, 264);

        private static readonly WalkablePosition FellshrineTransitionPos
            = new WalkablePosition(World.Act2.FellshrineRuins.Name, 1000, 280);

        private static readonly WalkablePosition SlumsFromTown
            = new WalkablePosition(World.Act3.Slums.Name, 597, 524);

        private static readonly WalkablePosition DriedLakeFromTown
            = new WalkablePosition(World.Act4.DriedLake.Name, 88, 442);

        private static readonly WalkablePosition MinesFromTown
            = new WalkablePosition(World.Act4.Mines1.Name, 330, 620);

        private static readonly WalkablePosition AscentFromTown
            = new WalkablePosition(World.Act4.Ascent.Name, 600, 403);

        private static readonly WalkablePosition ControlBlocksFromTown
            = new WalkablePosition(World.Act5.ControlBlocks.Name, 356, 410);

        private static readonly WalkablePosition TwilightStrandFromTown_A6
            = new WalkablePosition(World.Act6.TwilightStrand.Name, 121, 447);

        private static readonly WalkablePosition CoastFromTown_A6
            = new WalkablePosition(World.Act6.Coast.Name, 378, 356);

        private static readonly WalkablePosition BrokenBridgeFromTown
            = new WalkablePosition(World.Act7.BrokenBridge.Name, 550, 710);

        private static readonly WalkablePosition ToxicConduitsFromTown
            = new WalkablePosition(World.Act8.ToxicConduits.Name, 176, 659);

        private static readonly WalkablePosition DescentFromTown
            = new WalkablePosition(World.Act9.Descent.Name, 600, 403);

        private static readonly WalkablePosition RooftopFromTown
            = new WalkablePosition(World.Act10.CathedralRooftop.Name, 620, 347);

        private static readonly WalkablePosition LaboratoryFromTown
            = new WalkablePosition(World.Act11.TemplarLaboratory.Name, 993, 792);

        private static readonly WalkablePosition FallenCourtsFromTown =
            new WalkablePosition(World.Act11.FallenCourts.Name, 920, 483);

        private static readonly WalkablePosition HauntedReliquaryFromTown =
            new WalkablePosition(World.Act11.HauntedReliquary.Name, 471, 686);

        #endregion

        #region Blocking objects

        private static NetworkObject LooseCandle => LokiPoe.ObjectManager.Objects
            .Find(o => o.Metadata == "Metadata/QuestObjects/Library/HiddenDoorTrigger");

        private static NetworkObject DeshretSeal => LokiPoe.ObjectManager.Objects
            .Find(o => o.Metadata == "Metadata/QuestObjects/Act4/MineEntranceSeal");

        #endregion
    }
}

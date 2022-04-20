using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Common;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Bot.Pathfinding;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using FollowBot.Helpers;
using FollowBot.SimpleEXtensions;
using log4net;


namespace FollowBot
{
    class TravelToPartyZoneTask : ITask
    {
        private readonly ILog Log = Logger.GetLoggerInstanceForType();
        private bool _enabled = true;
        private Stopwatch _portalRequestStopwatch = Stopwatch.StartNew();

        public string Name { get { return "TravelToPartyZone"; } }
        public string Description { get { return "This task will travel to party grind zone."; } }
        public string Author { get { return "NotYourFriend original from Unknown"; } }
        public string Version { get { return "0.0.0.1"; } }
        
        public void Start()
        {
        }
        public void Stop()
        {
        }
        public void Tick()
        {
        }

        public async Task<bool> Run()
        {
            if (!LokiPoe.IsInGame || LokiPoe.Me.IsDead)
            {
                return false;
            }

            await Coroutines.CloseBlockingWindows();

            var leader = LokiPoe.InstanceInfo.PartyMembers.FirstOrDefault(x => x.MemberStatus == PartyStatus.PartyLeader);
            if (leader == null) return false;
            if (LokiPoe.InGameState.PartyHud.IsInSameZone(leader.PlayerEntry.Name)) return false;


            //First check for Delve portals:
            var delveportal = LokiPoe.ObjectManager.GetObjectsByType<AreaTransition>().FirstOrDefault(x => x.Name == "Azurite Mine" && (x.Metadata == "Metadata/MiscellaneousObject/PortalTransition" || x.Metadata == "Metadata/MiscellaneousObjects/PortalTransition"));
            if (delveportal != null)
            {
                Log.DebugFormat("[{0}] Found walkable delve portal.", Name);
                if (LokiPoe.Me.Position.Distance(delveportal.Position) > 20)
                {
                    var walkablePosition = ExilePather.FastWalkablePositionFor(delveportal, 20);

                    // Cast Phase run if we have it.
                    FollowBot.PhaseRun();

                    Move.Towards(walkablePosition, "moving to delve portal");
                    return true;
                }

                var tele = await Coroutines.InteractWith(delveportal);

                if (!tele)
                {
                    Log.DebugFormat("[{0}] delve portal error.", Name);
                }

                FollowBot.Leader = null;
                return true;
            }
            if (leader.PlayerEntry.Area.IsMap || leader.PlayerEntry.Area.IsTempleOfAtzoatl)
            {
                await TakePortal();
            }
            else if (leader.PlayerEntry.Area.IsLabyrinthArea)
            {
                if (leader.PlayerEntry.Area.Name == "Aspirants' Plaza")
                {
                    await PartyHelper.FastGotoPartyZone(leader.PlayerEntry.Name);
                    return true;
                }

                if (World.CurrentArea.Name == "Aspirants' Plaza")
                {
                    var trans = LokiPoe.ObjectManager.GetObjectByType<AreaTransition>();
                    if (trans == null)
                    {
                        var loc = ExilePather.FastWalkablePositionFor(new Vector2i(363, 423));
                        if (loc != Vector2i.Zero)
                        {
                            Move.Towards(loc, "Bronze Plaque");
                            return true;
                        }
                        else
                        {
                            GlobalLog.Warn($"[TravelToPartyZoneTask] Cant find Bronze Plaque location.");
                            return false;
                        }
                    }

                    if (LokiPoe.Me.Position.Distance(trans.Position) > 20)
                    {
                        var loc = ExilePather.FastWalkablePositionFor(trans.Position, 20);
                        Move.Towards(loc, $"{trans.Name}");
                        return true;
                    }

                    await PlayerAction.Interact(trans);
                    return true;
                }
                else if (World.CurrentArea.IsLabyrinthArea)
                {
                    AreaTransition areatransition = null;
                    areatransition = LokiPoe.ObjectManager.GetObjectsByType<AreaTransition>().OrderBy(x => x.Distance).FirstOrDefault(x => ExilePather.PathExistsBetween(LokiPoe.Me.Position, ExilePather.FastWalkablePositionFor(x.Position, 20)));
                    if (areatransition != null)
                    {
                        Log.DebugFormat("[{0}] Found walkable Area Transition [{1}].", Name, areatransition.Name);
                        if (LokiPoe.Me.Position.Distance(areatransition.Position) > 20)
                        {
                            var walkablePosition = ExilePather.FastWalkablePositionFor(areatransition, 20);

                            // Cast Phase run if we have it.
                            FollowBot.PhaseRun();

                            Move.Towards(walkablePosition, "moving to area transition");
                            return true;
                        }

                        var trans = await PlayerAction.TakeTransition(areatransition);

                        if (!trans)
                        {
                            Log.DebugFormat("[{0}] Areatransition error.", Name);
                        }

                        FollowBot.Leader = null;
                        return true;
                    }
                }
                GlobalLog.Warn($"[TravelToPartyZoneTask] Cant follow the leader in the Labirynt when the lab is already started.");
                return false;
            }

            await PartyHelper.FastGotoPartyZone(leader.PlayerEntry.Name);
            return true;
            //if (World.CurrentArea.WorldAreaId == leader.PlayerEntry.Area.WorldAreaId) return false;
            //Log.DebugFormat("[{0}] party leader is: {1}, zone: {2}", Name, leader.PlayerEntry.Name, leader.PlayerEntry.Area.Name);
            //if (leader.PlayerEntry.Area.IsTown)
            //{
            //    if (!LokiPoe.Me.IsInTown)
            //    {
            //        if (LokiPoe.LocalData.WorldArea.IsMap || LokiPoe.LocalData.WorldArea.IsOverworldArea)
            //        {
            //            if (await TakePortal())
            //                return true;
            //        }
            //        await PlayerAction.TpToTown();
            //        return true;
            //    }
            //    Log.DebugFormat("[{0}] Party leader:{1} is still in Town.", Name, leader.PlayerEntry.Name);
            //    return true;
            //}
            //if (leader.PlayerEntry.Area.IsHideoutArea)
            //{
            //    if (!LokiPoe.CurrentWorldArea.IsHideoutArea)
            //    {
            //        if (LokiPoe.LocalData.WorldArea.IsMap || LokiPoe.LocalData.WorldArea.IsOverworldArea)
            //        {
            //            if (await TakePortal())
            //                return true;
            //            else
            //            {
            //                await PlayerAction.TpToTown();
            //                return true;
            //            }
            //        }
            //        else
            //        {
            //            Log.DebugFormat("[{0}] Moving to {1} Hideout.", Name, leader.PlayerEntry.Name);
            //            if (!await PartyHelper.GoToPartyHideOut(leader.PlayerEntry.Name))
            //            {
            //                Log.DebugFormat("[{0}] Failed to visit Party leader:{1} Hideout.", Name, leader.PlayerEntry.Name);
            //                return true;
            //            }
            //        }
                    
            //        return true;
            //    }
            //    Log.DebugFormat("[{0}] Party leader:{1} is still in Hideout.", Name, leader.PlayerEntry.Name);
            //    return true;
            //}
            //if (leader.PlayerEntry.Area.IsMap)
            //{
            //    if (!LokiPoe.Me.IsInHideout)
            //    {
            //        if (!await PartyHelper.GoToPartyHideOut(leader.PlayerEntry.Name))
            //        {
            //            Log.DebugFormat("[{0}] Failed to visit Party leader:{1} Hideout.", Name, leader.PlayerEntry.Name);
            //            return true;
            //        }
            //        return true;
            //    }
            //    if (!await TakePortal())
            //        return true;                
            //}
            //if (leader.PlayerEntry.Area.IsOverworldArea)
            //{
            //    if (LokiPoe.Me.IsInOverworld)
            //    {
            //        await PlayerAction.TpToTown();
            //        return true;
            //    }
            //    if (LokiPoe.CurrentWorldArea != leader.PlayerEntry.Area.GoverningTown)
            //    {
            //        var result = await TravelHelper.TravelToZone(leader.PlayerEntry.Area.GoverningTown);
            //        if (!result)
            //        {
            //            FollowBot.Log.DebugFormat("[PartyPlugin] Can't navigate overworld zone the leader is in");
            //        }
            //        return true;
            //    }
                
            //    if (_portalRequestStopwatch.ElapsedMilliseconds > 10000 )
            //    {
            //        _portalRequestStopwatch.Restart();
            //        Log.DebugFormat("[{0}] Failed to find portals, requesting one.", Name);
            //        Random rnd = new Random(DateTime.Now.Millisecond);
            //        string str = string.Format("%{0}", PartyHelper.PartyPortal[rnd.Next(0, PartyHelper.PartyPortal.Count - 1)]);
            //        LokiPoe.InGameState.ChatPanel.Chat(str);
            //        await Coroutines.ReactionWait();
            //        return true;
            //    }
            //    else
            //    {
            //        var portal = LokiPoe.LocalData.TownPortals.FirstOrDefault(x => x.OwnerName == leader.PlayerEntry.Name && x.Area.WorldAreaId == leader.PlayerEntry.Area.WorldAreaId);

            //        if (portal != null)
            //        {
            //            if (portal.NetworkObject.Position.Distance(LokiPoe.Me.Position) > 18)
            //                await Move.AtOnce(portal.NetworkObject.Position, "Move to portal");

            //            await Coroutines.InteractWith<Portal>(portal.NetworkObject);

            //            await Coroutines.ReactionWait();
            //            _portalRequestStopwatch.Stop();
            //            return true;
            //        }
            //        if (_portalRequestStopwatch.ElapsedMilliseconds > 30000)
            //            _portalRequestStopwatch.Stop();
            //        return true;
            //    }
            //}
            //if (leader.PlayerEntry.Area.IsLabyrinthArea)
            //{
            //    if (!World.CurrentArea.IsLabyrinthArea)
            //    {
            //        if (LokiPoe.Me.IsInOverworld)
            //        {
            //            await PlayerAction.TpToTown();
            //            return true;
            //        }
            //        if (LokiPoe.CurrentWorldArea != leader.PlayerEntry.Area.GoverningTown)
            //        {
            //            var result = await TravelHelper.TravelToZone(leader.PlayerEntry.Area.GoverningTown);
            //            if (!result)
            //            {
            //                FollowBot.Log.DebugFormat("[PartyPlugin] Can't navigate GoverningTown zone the leader is in");
            //            }
            //            return true;
            //        }
            //        var plaza = LokiPoe.ObjectManager.AreaTransition("Aspirants' Plaza");
            //        if (plaza != null)
            //        {
            //            await Move.AtOnce(plaza.Position, "Move to Aspirant's Plaza");
            //            await Coroutines.LatencyWait();
            //            await PlayerAction.Interact(plaza);
            //            await Coroutines.LatencyWait();
            //            await Coroutines.ReactionWait();
            //            await Coroutines.LatencyWait();
            //            await PlayerAction.Interact(plaza);
            //            await Coroutines.LatencyWait();
            //            return true;
            //        }
            //    }
            //}
            //Log.DebugFormat("[{0}] Failed to Travel to party zone", Name);
            //return true;
        }

        private async Task<bool> TakePortal()
        {
            var portal = LokiPoe.ObjectManager.GetObjectsByType<Portal>().FirstOrDefault(x => x.IsTargetable);
            if (portal != null)
            {
                if (portal.Position.Distance(LokiPoe.Me.Position) > 18)
                    await Move.AtOnce(portal.Position, "Move to portal");
                if (await Coroutines.InteractWith<Portal>(portal))
                    return true;
                else
                    return false;
            }
            else
            {
                Log.DebugFormat("[{0}] Failed to find portals.", Name);
                return false;
            }
        }

        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public MessageResult Message(Message message)
        {
            if (message.Id == "Enable")
            {
                _enabled = true;
                return MessageResult.Processed;
            }
            if (message.Id == "Disable")
            {
                _enabled = false;
                return MessageResult.Processed;
            }
            return MessageResult.Unprocessed;
        }
    }
}
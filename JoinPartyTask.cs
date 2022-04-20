using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Common;
using DreamPoeBot.Loki.Game;
using FollowBot.Helpers;
using log4net;


namespace FollowBot
{
    class JoinPartyTask : ITask
    {
        private readonly ILog Log = Logger.GetLoggerInstanceForType();

        public string Name { get { return "JoinPartyTask"; } }
        public string Description { get { return "This task will ask for party."; } }
        public string Author { get { return "NotYourFriend, origial code from Unknown"; } }
        public string Version { get { return "0.0.0.1"; } }


        public void Start()
        {
            Log.InfoFormat("[{0}] Task Loaded.", Name);
        }
        public void Stop()
        {

        }
        public void Tick()
        {

        }

        public async Task<bool> Run()
        {
            if (LokiPoe.InstanceInfo.PartyStatus == DreamPoeBot.Loki.Game.GameData.PartyStatus.PartyMember)
            {
                return false;
            }
            if (LokiPoe.InstanceInfo.PartyStatus == DreamPoeBot.Loki.Game.GameData.PartyStatus.PartyLeader)
            {
                await PartyHelper.LeaveParty();
                return true;
            }

            var invite = LokiPoe.InstanceInfo.PendingPartyInvites;
            if(invite.Any())
            {
                await PartyHelper.HandlePartyInvite();
            }
            else if (LokiPoe.InstanceInfo.PartyStatus == DreamPoeBot.Loki.Game.GameData.PartyStatus.None)
            {
                return true;
            }

            return true;
        }
        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public MessageResult Message(Message message)
        {
            return MessageResult.Unprocessed;
        }
    }
}
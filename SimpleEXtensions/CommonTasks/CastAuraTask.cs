using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DreamPoeBot.Loki.Bot;
using DreamPoeBot.Loki.Game;
using DreamPoeBot.Loki.Game.Objects;
using DreamPoeBot.Loki.RemoteMemoryObjects;
using FollowBot.SimpleEXtensions;
using SkillBar = DreamPoeBot.Loki.Game.LokiPoe.InGameState.SkillBarHud;

namespace FollowBot
{
    public class CastAuraTask : ITask
    {
        private const int MinGolemHpPercent = 80;
        private static List<int> _temporaryBlacklistedAuras = new List<int>();
        public async Task<bool> Run()
        {
            var area = World.CurrentArea;
            //if (!area.IsHideoutArea && !area.IsMap && !area.IsMapRoom && !area.IsOverworldArea)
            if (area.IsTown)
                return false;

            await Coroutines.CloseBlockingWindows();

            if (FollowBotSettings.Instance.UseStalkerSentinel)
            {
                if (!LokiPoe.InGameState.SentinelSkillUi.StalkerSentinel.IsActive && LokiPoe.InGameState.SentinelSkillUi.StalkerSentinel.CanUse)
                    LokiPoe.InGameState.SentinelSkillUi.StalkerSentinel.Activate();
            }

            var golemSkill = SkillBar.Skills.FirstOrDefault(s => s.IsOnSkillBar && s.SkillTags.Contains("golem"));
            if (golemSkill != null)
            {
                var golemObj = golemSkill.DeployedObjects.FirstOrDefault() as Monster;
                if (golemObj == null || golemObj.HealthPercent < MinGolemHpPercent)
                {
                    GlobalLog.Debug($"[CastAuraTask] Now summoning \"{golemSkill.Name}\".");
                    SkillBar.Use(golemSkill.Slot, false);
                    await Wait.SleepSafe(100);
                    await Coroutines.FinishCurrentAction();
                    await Wait.SleepSafe(100);
                }
            }
            var auras = GetAurasForCast();
            if (auras.Count > 0 && AllAuras.Any(a => a.IsOnSkillBar))
            {
                GlobalLog.Info($"[CastAuraTask] Found {auras.Count} aura(s) for casting.");
                await CastAuras(auras);
            }
            return false;
        }

        private static async Task CastAuras(IEnumerable<Skill> auras)
        {
            int slotForHidden = AllAuras.First(a => a.IsOnSkillBar).Slot;
            foreach (var aura in auras.OrderByDescending(a => a.Slot))
            {
                if (aura.Slot == -1)
                {
                    await SetAuraToSlot(aura, slotForHidden);
                }
                await ApplyAura(aura);
            }
        }

        private static async Task ApplyAura(Skill aura)
        {
            string name = aura.Name;
            int id = aura.Id;
            GlobalLog.Debug($"[CastAuraTask] Now casting \"{name}\".");
            var used = SkillBar.Use(aura.Slot, false);
            if (used != LokiPoe.InGameState.UseResult.None)
            {
                GlobalLog.Error($"[CastAuraTask] Fail to cast \"{name}\". Error: \"{used}\".");
                return;
            }

            if(!await Wait.For(() => !LokiPoe.Me.HasCurrentAction && (PlayerHasAura(name) || PlayerHasAura(id)), "aura applying"))
            {
                GlobalLog.Warn($"[CastAuraTask] Failed to apply aura \"{name}\".");
                GlobalLog.Warn($"[CastAuraTask] Pls make sure you can cast this aura (you have ennoght mana).");
                GlobalLog.Warn($"[CastAuraTask] Also make sure you have blacklisted the auras you dont want to use (Settings-Content-SkillBlacklist).");
                GlobalLog.Warn($"[CastAuraTask] This error Usually indicate that you have more Auras, slotted, than you can substain with your mana.");
                GlobalLog.Warn($"[CastAuraTask] The aura \"{name}\" will not be blacklisted to allow the bot to continue, the aura will be recasted next time you stop/start the bot.");
                _temporaryBlacklistedAuras.Add(id);
            }
            await Wait.SleepSafe(100);
        }

        private static async Task SetAuraToSlot(Skill aura, int slot)
        {
            string name = aura.Name;
            GlobalLog.Debug($"[CastAuraTask] Now setting \"{name}\" to slot {slot}.");
            var isSet = SkillBar.SetSlot(slot, aura);
            if (isSet != LokiPoe.InGameState.SetSlotResult.None)
            {
                GlobalLog.Error($"[CastAuraTask] Fail to set \"{name}\" to slot {slot}. Error: \"{isSet}\".");
                return;
            }
            await Wait.For(() => IsInSlot(slot, name), "aura slot changing");
            await Wait.SleepSafe(100);
        }

        private static bool IsInSlot(int slot, string name)
        {
            var skill = SkillBar.Slot(slot);
            return skill != null && skill.Name == name;
        }

        private static List<Skill> GetAurasForCast()
        {
            var auras = new List<Skill>();
            foreach (var aura in AllWhitelistedAuras)
            {
                if (FollowBotSettings.Instance.IgnoreHiddenAuras && !aura.IsOnSkillBar)
                    continue;

                if (PlayerHasAura(aura.Name))
                    continue;
                if (PlayerHasAura(aura.Id))
                    continue;

                auras.Add(aura);
            }
            return auras;
        }

        private static IEnumerable<Skill> AllAuras
        {
            get
            {
                return SkillBar.Skills.Where(skill => AuraNames.Contains(skill.Name) || AuraInternalId.Contains(skill.InternalId) || skill.IsAurifiedCurse);
            }
        }
        private static IEnumerable<Skill> AllWhitelistedAuras
        {
            get
            {
                return SkillBar.Skills.Where(skill => _temporaryBlacklistedAuras.All(x => x != skill.Id) &&
                !SkillBlacklist.IsBlacklisted(skill) &&
                (AuraNames.Contains(skill.Name) || AuraInternalId.Contains(skill.InternalId) || skill.IsAurifiedCurse));
            }
        }

        private static bool PlayerHasAura(string auraName)
        {
            return LokiPoe.Me.Auras.Any(a => (a.Name.EqualsIgnorecase(auraName) || a.Name.EqualsIgnorecase(auraName + " aura")) && a.CasterId == LokiPoe.Me.Id);            
        }
        private static bool PlayerHasAura(int skillId)
        {
            //return LokiPoe.Me.Auras.Any(a => (a.Name.EqualsIgnorecase(auraName) || a.Name.EqualsIgnorecase(auraName + " aura")) && a.CasterId == LokiPoe.Me.Id);

            return LokiPoe.Me.Auras.Any(a => a.SkillOwnerId == skillId);
        }

        private static readonly HashSet<string> AuraNames = new HashSet<string>
        {
            // auras
            "Anger",
            "Clarity",
            "Determination",
            "Discipline",
            "Grace",
            "Haste",
            "Hatred",
            "Malevolence",
            "Precision",
            "Pride",
            "Purity of Elements",
            "Purity of Fire",
            "Purity of Ice",
            "Purity of Lightning",
            "Vitality",
            "Wrath",
		    "Defiance Banner",
            "Zealotry",

            // heralds
            "Herald of Agony",
            "Herald of Ice",
            "Herald of Thunder",
            "Herald of Ash",
            "Herald of Purity",

            // the rest
            "Arctic Armour"
        };
        private static readonly HashSet<string> AuraInternalId = new HashSet<string>
        {
            // auras
            "anger",
            "clarity",
            "determination",
            "discipline",
            "grace",
            "haste",
            "hatred",
            "damage_over_time_aura",//Malevolence
            "aura_accuracy_and_crits",//Precision
            "physical_damage_aura",//Pride
            "purity",//Purity of Elements
            "fire_resist_aura",//Purity of Fire
            "cold_resist_aura",//Purity of Ice
            "lightning_resist_aura",//Purity of Lightning
            "vitality",
            "wrath",
            "banner_armour_evasion",//Defiance Banner
            "banner_dread",
            "banner_war",
            "spell_damage_aura",//Zealotry

            // heralds
            "herald_of_agony",
            "herald_of_ice",
            "herald_of_thunder",
            "herald_of_ash",
            "herald_of_light",//Herald of Purity

            // the rest
            "new_arctic_armour",
            "tempest_shield",
            "skitterbots",
            "petrified_blood",


        };

        #region Unused interface methods

        public MessageResult Message(Message message)
        {
            return MessageResult.Unprocessed;
        }

        public async Task<LogicResult> Logic(Logic logic)
        {
            return LogicResult.Unprovided;
        }

        public void Tick()
        {
        }

        public void Start()
        {
            _temporaryBlacklistedAuras.Clear();
        }

        public void Stop()
        {
        }

        public string Name => "CastAuraTask";
        public string Description => "Task for casting auras before entering a map.";
        public string Author => "ExVault";
        public string Version => "1.0";

        #endregion
    }
}
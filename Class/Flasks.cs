using DreamPoeBot.Loki.Game.GameData;
using DreamPoeBot.Loki.Game.Objects;
using System;
using System.Collections.Generic;
using FollowBot.SimpleEXtensions;
using FlaskHud = DreamPoeBot.Loki.Game.LokiPoe.InGameState.QuickFlaskHud;

namespace FollowBot.Class
{
	public static class Flasks
	{
		public static Item LifeFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.LifeFlask && !f.IsInstantRecovery);

		public static Item InstantLifeFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.LifeFlask && (f.IsInstantRecovery || IsPanicked(f)));

		public static Item ManaFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.ManaFlask && !f.IsInstantRecovery);

		public static Item InstantManaFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.ManaFlask && f.IsInstantRecovery);

		public static Item HybridFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.HybridFlask && f.FullName != FlaskNames.DivinationDistillate && !f.IsInstantRecovery);

		public static Item InstantHybridFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Class == ItemClasses.HybridFlask && f.IsInstantRecovery);

		public static Item QuicksilverFlask => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.Name == FlaskNames.Quicksilver);

		public static Item KiaraDetermination => FlaskHud.InventoryControl.Inventory.Items
			.HighestCharge(f => f.FullName == FlaskNames.KiaraDetermination);

		public static bool IsPanicked(Item item)
		{
			return item.FullName.StartsWith("Panicked");
		}

		public static Item ByProperName(string name)
		{
			return FlaskHud.InventoryControl.Inventory.Items.HighestCharge(f => f.ProperName() == name);
		}

		public static Item ByStat(StatTypeGGG stat)
		{
			return FlaskHud.InventoryControl.Inventory.Items.HighestCharge(f => f.LocalStats.ContainsKey(stat));
		}

		public static string GetEffect(string properName)
		{
			return FlaskEffects.TryGetValue(properName, out string effect) ? effect : null;
		}

		public static string ProperName(this Item item)
		{
			return item.RarityLite() == Rarity.Unique ? item.FullName : item.Name;
		}

		private static Item HighestCharge(this IEnumerable<Item> flasks, Func<Item, bool> match)
		{
			Item highest = null;
			foreach (Item flask in flasks)
			{
				if (!match(flask))
				{
					continue;
				}

				if (!flask.CanUse)
				{
					continue;
				}

				if (highest == null || highest.CurrentCharges < flask.CurrentCharges)
				{
					highest = flask;
				}
			}
			return highest;
		}

		private static readonly Dictionary<string, string> FlaskEffects = new Dictionary<string, string>
		{
			[FlaskNames.Diamond] = "flask_utility_critical_strike_chance",
			[FlaskNames.Ruby] = "flask_utility_resist_fire",
			[FlaskNames.Sapphire] = "flask_utility_resist_cold",
			[FlaskNames.Topaz] = "flask_utility_resist_lightning",
			[FlaskNames.Granite] = "flask_utility_ironskin",
			[FlaskNames.Quicksilver] = "flask_utility_sprint",
			[FlaskNames.Amethyst] = "flask_utility_resist_chaos",
			[FlaskNames.Quartz] = "flask_utility_phase",
			[FlaskNames.Jade] = "flask_utility_evasion",
			[FlaskNames.Basalt] = "flask_utility_stone",
			[FlaskNames.Aquamarine] = "flask_utility_aquamarine",
			[FlaskNames.Stibnite] = "flask_utility_smoke",
			[FlaskNames.Sulphur] = "flask_utility_consecrate",
			[FlaskNames.Silver] = "flask_utility_haste",
			[FlaskNames.Bismuth] = "flask_utility_prismatic",


			[FlaskNames.BloodOfKarui] = "unique_flask_blood_of_the_karui",
			[FlaskNames.ZerphiLastBreath] = "unique_flask_zerphis_last_breath",
			[FlaskNames.DivinationDistillate] = "unique_flask_divination_distillate",
			[FlaskNames.CoruscatingElixir] = "unique_flask_chaos_damage_damages_es",
			[FlaskNames.TasteOfHate] = "unique_flask_taste_of_hate",
			[FlaskNames.KiaraDetermination] = "kiaras_determination",
			[FlaskNames.ForbiddenTaste] = "unique_flask_forbidden_taste",
			[FlaskNames.LionRoar] = "unique_flask_lions_roar",
			[FlaskNames.WitchfireBrew] = "unique_flask_witchfire_brew",
			[FlaskNames.AtziriPromise] = "unique_flask_atziris_promise",
			[FlaskNames.DyingSun] = "unique_flask_dying_sun",
			[FlaskNames.RumiConcoction] = "unique_flask_rumis_concoction",
			[FlaskNames.LaviangaSpirit] = "unique_flask_laviangas_cup",
			[FlaskNames.SorrowOfDivine] = "unique_flask_zealots_oath",
			[FlaskNames.OverflowingChalice] = "overflowing_chalice",
			[FlaskNames.SinRebirth] = "unholy_might_from_flask",
			[FlaskNames.VesselOfVinktar] = "lightning_flask",
			[FlaskNames.WiseOak] = "unique_flask_the_basics",
			[FlaskNames.CoralitoSignature] = "unique_flask_gorgon_poison",

			// No unique effects
			[FlaskNames.Rotgut] = " flask_utility_sprint",
			[FlaskNames.SoulCatcher] = "unique_flask_soul_catcher",
			[FlaskNames.DoedreElixir] = string.Empty,
			[FlaskNames.WrithingJar] = string.Empty,
		};
	}

	public static class FlaskNames
	{
		public const string Diamond = "Diamond Flask";
		public const string Ruby = "Ruby Flask";
		public const string Sapphire = "Sapphire Flask";
		public const string Topaz = "Topaz Flask";
		public const string Granite = "Granite Flask";
		public const string Quicksilver = "Quicksilver Flask";
		public const string Amethyst = "Amethyst Flask";
		public const string Quartz = "Quartz Flask";
		public const string Jade = "Jade Flask";
		public const string Basalt = "Basalt Flask";
		public const string Aquamarine = "Aquamarine Flask";
		public const string Stibnite = "Stibnite Flask";
		public const string Sulphur = "Sulphur Flask";
		public const string Silver = "Silver Flask";
		public const string Bismuth = "Bismuth Flask";


		public const string BloodOfKarui = "Blood of the Karui";
		public const string DoedreElixir = "Doedre's Elixir";
		public const string ZerphiLastBreath = "Zerphi's Last Breath";
		public const string LaviangaSpirit = "Lavianga's Spirit";
		public const string DivinationDistillate = "Divination Distillate";
		public const string WrithingJar = "The Writhing Jar";
		public const string WiseOak = "The Wise Oak";
		public const string SinRebirth = "Sin's Rebirth";
		public const string CoruscatingElixir = "Coruscating Elixir";
		public const string TasteOfHate = "Taste of Hate";
		public const string KiaraDetermination = "Kiara's Determination";
		public const string ForbiddenTaste = "Forbidden Taste";
		public const string LionRoar = "Lion's Roar";
		public const string OverflowingChalice = "The Overflowing Chalice";
		public const string SorrowOfDivine = "The Sorrow of the Divine";
		public const string Rotgut = "Rotgut";
		public const string WitchfireBrew = "Witchfire Brew";
		public const string AtziriPromise = "Atziri's Promise";
		public const string DyingSun = "Dying Sun";
		public const string RumiConcoction = "Rumi's Concoction";
		public const string VesselOfVinktar = "Vessel of Vinktar";
		public const string CoralitoSignature = "Coralito's Signature";
		public const string SoulCatcher = "Soul Catcher";
	}
}

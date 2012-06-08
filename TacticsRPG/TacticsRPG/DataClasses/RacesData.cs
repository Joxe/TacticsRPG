using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class RacesData {
		public static void load() {
			string[] t_racesDirectory = Directory.GetDirectories("Content/Images/Sprites/Races/");
			ChampionRace[] t_availableRaces = new ChampionRace[t_racesDirectory.Length];

			for (int i = 0; i < t_racesDirectory.Length; i++) {
				string[] t_race = t_racesDirectory[i].Split('/');
				t_availableRaces[i] = new ChampionRace(t_race[t_race.Length - 1]);
				t_availableRaces[i].p_male = t_racesDirectory[i].Split('_')[1].Equals("Male");
				t_availableRaces[i].load();
			}
		}

		private static Dictionary<string, int> Human() {
			Dictionary<string, int> t_baseStats = new Dictionary<string, int>();
			t_baseStats["strength"]		= 20;
			t_baseStats["intellect"]	= 10;
			t_baseStats["agility"]		= 15;
			t_baseStats["luck"]			= 1;
			t_baseStats["manaRegen"]	= 5;
			t_baseStats["move"]			= 0;
			t_baseStats["jump"]			= 2;
			t_baseStats["speed"]		= 50;
			t_baseStats["maxHealth"]	= 80;
			t_baseStats["maxMana"]		= 40;

			return t_baseStats;
		}

		public static Dictionary<string, int> getStats(ChampionRace a_race) {
			if (a_race.getName().Split('_')[0].Equals("Human")) {
				return Human();
			}
			return new Dictionary<string, int>();
		}

		#region Attack
		public static float getStrAtkConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.3f; }
			return 0.0f;
		}

		public static float getAgiAtkConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.2f; }
			return 0.0f;
		}

		public static float getIntAtkConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.1f; }
			return 0.0f;
		}
		#endregion

		#region Defense
		public static float getStrDefConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.3f; }
			return 0.0f;
		}

		public static float getAgiDefConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.2f; }
			return 0.0f;
		}

		public static float getIntDefConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 1.1f; }
			return 0.0f;
		}
		#endregion

		#region Resist
		public static float getStrResConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.5f; }
			return 0.0f;
		}

		public static float getAgiResConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.3f; }
			return 0.0f;
		}

		public static float getIntResConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.1f; }
			return 0.0f;
		}
		#endregion

		#region Magic
		public static float getStrMagConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.5f; }
			return 0.0f;
		}

		public static float getAgiMagConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.3f; }
			return 0.0f;
		}

		public static float getIntMagConversion(ChampionRace a_race) {
			if (a_race.getName().Equals("Human"))	{ return 0.1f; }
			return 0.0f;
		}
		#endregion
	}
}

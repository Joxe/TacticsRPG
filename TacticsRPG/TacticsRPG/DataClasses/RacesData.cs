using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class RacesData {
		private static Dictionary<string, ChampionRace> m_races;

		public static void load() {
			m_races	= new Dictionary<string, ChampionRace>();
			List<ChampionRace> t_raceList = XMLParser.loadAvailableRaces();
			foreach (ChampionRace t_race in t_raceList) {
				m_races.Add(t_race.getName(), t_race);
			}
		}

		public static Dictionary<string, int> getStats(ChampionRace a_race) {
			return m_races[a_race.getName()].getBaseStats();
		}

		public static List<ChampionRace> availableRaces() {
			return m_races.Values.ToList();
		}

		public static ChampionRace getRace(string a_race) {
			try {
				return m_races[a_race];
			} catch (InvalidOperationException) {
				return null;
			}
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

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
	}
}

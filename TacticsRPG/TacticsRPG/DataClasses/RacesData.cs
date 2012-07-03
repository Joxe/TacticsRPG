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
			List<ChampionRace> l_raceList = XMLParser.loadAvailableRaces();
			foreach (ChampionRace l_race in l_raceList) {
				m_races.Add(l_race.getName(), l_race);
			}
		}

		public static Dictionary<string, int> getStats(ChampionRace a_race) {
			return m_races[a_race.getName()].getBaseStats();
		}

		public static List<ChampionRace> availableRaces() {
			return m_races.Values.ToList();
		}

		public static ChampionRace getRace(string a_race) {
			#if DEBUG
			return m_races[a_race];
			#else
			try {
				return m_races[a_race];
			} catch (InvalidOperationException) {
				return null;
			}
			#endif
		}
	}
}

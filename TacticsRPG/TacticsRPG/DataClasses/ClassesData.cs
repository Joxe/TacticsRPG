using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class ClassesData {
		private static Dictionary<string, ChampionClass> m_classes;

		public static void load() {
			m_classes = new Dictionary<string, ChampionClass>();
			List<ChampionClass> t_classList = XMLParser.loadAvailableClasses();
			foreach (ChampionClass t_class in t_classList) {
				m_classes.Add(t_class.getName(), t_class);
			}
		}

		public static Dictionary<string, int> getStats(ChampionClass a_class) {
			return m_classes[a_class.ToString()].getBaseStats();
		}
		
		public static List<ChampionClass> availableClasses() {
			return m_classes.Values.ToList();
		}

		public static ChampionClass getClass(string a_class) {
			#if DEBUG
			return m_classes[a_class];
			#else
			try {
				return m_classes[a_class];
			} catch (InvalidOperationException) {
				return null;
			}
			#endif
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace TacticsRPG {
	public class ChampionRace {
		private string m_name;
		private Dictionary<string, int> m_baseStats;

		public ChampionRace(string a_name) {
			m_name = a_name;
			m_baseStats = new Dictionary<string, int>();		
		}

		public Dictionary<string, int> getBaseStats() {
			return m_baseStats;
		}

		public int getStat(string a_stat) {
			try {
				return m_baseStats[a_stat];
			} catch (InvalidOperationException) {
				return 0;
			}
		}

		public string getName() {
			return m_name;
		}

		public void setBaseStats(XmlNode a_xmlNode) {
			m_baseStats = new Dictionary<string, int>();
			for (int i = 0; i < a_xmlNode.ChildNodes.Count; i++) {
				m_baseStats.Add(a_xmlNode.ChildNodes.Item(i++).Name, int.Parse(a_xmlNode.ChildNodes.Item(i).Value));
			}
		}
	}
}

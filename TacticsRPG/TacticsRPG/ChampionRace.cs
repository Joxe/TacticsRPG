using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Globalization;

namespace TacticsRPG {
	public class ChampionRace {
		private string m_name;
		private Dictionary<string, int> m_baseStats;
		private Dictionary<string, float> m_baseRatios;
		private List<Ability> m_abilities;

		public ChampionRace(string a_name) {
			m_name = a_name;
			m_baseStats = new Dictionary<string, int>();		
			m_abilities = new List<Ability>();
		}

		public Dictionary<string, int> getBaseStats() {
			return m_baseStats;
		}

		public int getStat(string a_stat) {
			#if DEBUG
			return m_baseStats[a_stat];
			#else
			try {
				return m_baseStats[a_stat];
			} catch (InvalidOperationException) {
				return 0;
			}
			#endif
		}

		public float getRatio(string a_ratio) {
			#if DEBUG
			return m_baseRatios[a_ratio];
			#else
			try {
				return m_baseRatios[a_ratio];
			} catch (InvalidOperationException) {
				return 0;
			}
			#endif
		}

		public string getName() {
			return m_name;
		}

		public void setBaseStats(XmlNode a_xmlNode) {
			m_baseStats = new Dictionary<string, int>();
			XmlNode t_thisRace = a_xmlNode.SelectSingleNode(m_name);

			for (int i = 0; i < a_xmlNode.ChildNodes.Count; i++) {
				m_baseStats.Add(a_xmlNode.ChildNodes.Item(i++).Name, int.Parse(a_xmlNode.ChildNodes.Item(i).Value));
			}
		}

		public void setBaseRatios(XmlNode a_xmlNode) {
			m_baseRatios = new Dictionary<string, float>();

			for (int i = 0; i < a_xmlNode.ChildNodes.Count; i++) {
				for (int j = 0; j < a_xmlNode.ChildNodes.Item(i).ChildNodes.Count; j++) {
					XmlNode t_currentRatio = a_xmlNode.ChildNodes.Item(i);
					string t_name = t_currentRatio.Name + t_currentRatio.ChildNodes.Item(j++).Name;
					m_baseRatios.Add(t_name, float.Parse(t_currentRatio.ChildNodes.Item(j).Value, CultureInfo.InvariantCulture));
				}
			}
		}

		public void addAbility(Ability a_ability) {
			if (!m_abilities.Contains(a_ability)) {
				m_abilities.Add(a_ability);
			}
		}
		
		public List<Ability> getAbilities() {
			return m_abilities;
		}
	}
}

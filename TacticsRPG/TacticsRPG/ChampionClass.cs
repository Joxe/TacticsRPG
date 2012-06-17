using System;
using System.Xml;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class ChampionClass {
		private string m_name;
		private Dictionary<string, int> m_baseStats;

		public ChampionClass(string a_class) {
			m_name = a_class.Split('_')[0];
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

		public override string ToString() {
			return m_name;
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

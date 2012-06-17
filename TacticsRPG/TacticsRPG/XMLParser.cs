using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TacticsRPG {
	public static class XMLParser {
		public static List<ChampionRace> loadAvailableRaces() {
			XmlDocument t_xmlDocument = new XmlDocument();
			List<ChampionRace> t_returnList = new List<ChampionRace>();

			t_xmlDocument.Load("XML Data/RacesData.xml");
			XmlNode t_raceNode = t_xmlDocument.SelectSingleNode("RacesData");
			for (int i = 0; i < t_raceNode.ChildNodes.Count; i++) {
				t_returnList.Add(new ChampionRace(t_raceNode.ChildNodes[i].Name));
				t_returnList.Last().setBaseStats(t_raceNode.ChildNodes[i]);
			}

			return t_returnList;
		}

		public static List<ChampionClass> loadAvailableClasses() {
			XmlDocument t_xmlDocument = new XmlDocument();
			List<ChampionClass> t_returnList = new List<ChampionClass>();

			t_xmlDocument.Load("XML Data/ClassesData.xml");
			XmlNode t_raceNode = t_xmlDocument.SelectSingleNode("ClassesData");
			for (int i = 0; i < t_raceNode.ChildNodes.Count; i++) {
				t_returnList.Add(new ChampionClass(t_raceNode.ChildNodes[i].Name));
				t_returnList.Last().setBaseStats(t_raceNode.ChildNodes[i]);
			}

			return t_returnList;
		}
	}
}

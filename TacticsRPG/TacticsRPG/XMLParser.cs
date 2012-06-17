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
			XmlNode t_raceNode = t_xmlDocument.SelectSingleNode("/RacesData/BaseStats");
			for (int i = 0; i < t_raceNode.ChildNodes.Count; i++) {
				t_returnList.Add(new ChampionRace(t_raceNode.ChildNodes[i].Name));
				t_returnList.Last().setBaseStats(t_raceNode.ChildNodes[i]);
			}

			t_raceNode = t_xmlDocument.SelectSingleNode("/RacesData/Ratios");
			for (int i = 0; i < t_raceNode.ChildNodes.Count; i++) {
				t_returnList.ElementAt(i).setBaseRatios(t_raceNode.ChildNodes[i]);
			}

			return t_returnList;
		}

		public static List<ChampionClass> loadAvailableClasses() {
			XmlDocument t_xmlDocument = new XmlDocument();
			List<ChampionClass> t_returnList = new List<ChampionClass>();

			t_xmlDocument.Load("XML Data/ClassesData.xml");
			XmlNode t_classNode = t_xmlDocument.SelectSingleNode("/ClassesData/BaseStats");
			for (int i = 0; i < t_classNode.ChildNodes.Count; i++) {
				t_returnList.Add(new ChampionClass(t_classNode.ChildNodes[i].Name));
				t_returnList.Last().setBaseStats(t_classNode.ChildNodes[i]);
			}

			t_classNode = t_xmlDocument.SelectSingleNode("/ClassesData/Ratios");
			for (int i = 0; i < t_classNode.ChildNodes.Count; i++) {
				t_returnList.ElementAt(i).setBaseRatios(t_classNode.ChildNodes[i]);
			}

			return t_returnList;
		}
	}
}

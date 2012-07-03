using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace TacticsRPG {
	public static class XMLParser {
		public static List<ChampionRace> loadAvailableRaces() {
			XmlDocument l_xmlDocument = new XmlDocument();
			List<ChampionRace> l_returnList = new List<ChampionRace>();

			l_xmlDocument.Load("XML Data/RacesData.xml");
			XmlNode l_raceNode = l_xmlDocument.SelectSingleNode("/RacesData/BaseStats");
			for (int i = 0; i < l_raceNode.ChildNodes.Count; i++) {
				l_returnList.Add(new ChampionRace(l_raceNode.ChildNodes[i].Name));
				l_returnList.Last().setBaseStats(l_raceNode.ChildNodes[i]);
			}

			l_raceNode = l_xmlDocument.SelectSingleNode("/RacesData/Ratios");
			for (int i = 0; i < l_raceNode.ChildNodes.Count; i++) {
				l_returnList.ElementAt(i).setBaseRatios(l_raceNode.ChildNodes[i]);
			}

			return l_returnList;
		}

		public static List<ChampionClass>  loadAvailableClasses() {
			XmlDocument l_xmlDocument = new XmlDocument();
			List<ChampionClass> l_returnList = new List<ChampionClass>();

			l_xmlDocument.Load("XML Data/ClassesData.xml");
			XmlNode l_classNode = l_xmlDocument.SelectSingleNode("/ClassesData/BaseStats");
			for (int i = 0; i < l_classNode.ChildNodes.Count; i++) {
				l_returnList.Add(new ChampionClass(l_classNode.ChildNodes[i].Name));
				l_returnList.Last().setBaseStats(l_classNode.ChildNodes[i]);
			}

			l_classNode = l_xmlDocument.SelectSingleNode("/ClassesData/Ratios");
			for (int i = 0; i < l_classNode.ChildNodes.Count; i++) {
				l_returnList.ElementAt(i).setBaseRatios(l_classNode.ChildNodes[i]);
			}

			return l_returnList;
		}

		public static List<Element> loadAvailableElements() {
			XmlDocument l_xmlDocument = new XmlDocument();
			List<Element> l_returnList = new List<Element>();

			l_xmlDocument.Load("XML Data/ElementsData.xml");
			XmlNode l_elementNode = l_xmlDocument.SelectSingleNode("/ElementsData");
			for (int i = 0; i < l_elementNode.ChildNodes.Count; i++) {
				l_returnList.Add(new Element(l_elementNode.ChildNodes[i].Name));
			}

			return l_returnList;
		}

		public static void loadElementProperties(List<Element> a_elementList) {
			XmlDocument l_xmlDocument = new XmlDocument();
			l_xmlDocument.Load("XML Data/ElementsData.xml");

			foreach (Element l_element in a_elementList) {
				XmlNodeList l_propertyList = l_xmlDocument.SelectNodes("ElementsData/" + l_element.getName() + "/Strength");
				foreach (XmlNode l_node in l_propertyList) {
					l_element.addStrength(ElementsData.getElement(l_node.NextSibling.InnerText.Trim()));
				}

				l_propertyList = l_xmlDocument.SelectNodes("ElementsData/" + l_element.getName() + "/Weakness");
				foreach (XmlNode l_node in l_propertyList) {
					l_element.addWeakness(ElementsData.getElement(l_node.NextSibling.InnerText.Trim()));
				}
			}
		}
		
		public static List<Ability> loadAvailableAbilities() {
			XmlDocument l_xmlDocument = new XmlDocument();
			List<Ability> l_returnList = new List<Ability>();

			l_xmlDocument.Load("XML Data/AbilitiesData.xml");
			XmlNodeList l_abilityNodes = l_xmlDocument.SelectSingleNode("AbilitiesData").ChildNodes;
			for (int i = 0; i < l_abilityNodes.Count; i++) {
				l_returnList.Add(new Ability(l_abilityNodes[i].Name));
			}

			foreach (Ability l_ability in l_returnList) {
				XmlNode l_abilityNode = l_xmlDocument.SelectSingleNode("AbilitiesData/" + l_ability.getName());
				l_ability.setProperties(
					l_abilityNode.SelectSingleNode("Desc").NextSibling.InnerText.Trim(),
					int.Parse(l_abilityNode.SelectSingleNode("Range").NextSibling.InnerText.Trim()),
					int.Parse(l_abilityNode.SelectSingleNode("AoE").NextSibling.InnerText.Trim()),
					int.Parse(l_abilityNode.SelectSingleNode("Cost").NextSibling.InnerText.Trim())
				);
			}

			return l_returnList;
		}

		public static void setAbilities() {
			XmlDocument l_xmlDocument = new XmlDocument();

			l_xmlDocument.Load("XML Data/ClassesData.xml");
			foreach (ChampionClass l_class in ClassesData.availableClasses()) {
				XmlNodeList l_abilityList = l_xmlDocument.SelectNodes("/ClassesData/Abilities/" + l_class.getName());
				string l_trimmedAbility;
				for (int i = 0; i < l_abilityList.Count; i++) {
					l_trimmedAbility = l_abilityList[i].InnerText.Trim();
					if (l_trimmedAbility.Equals("")) {
						continue;
					}
					l_class.addAbility(AbilitiesData.getAbility(l_trimmedAbility));
				}
			}

			l_xmlDocument.Load("XML Data/RacesData.xml");
			foreach (ChampionRace l_race in RacesData.availableRaces()) {
				XmlNodeList l_abilityList = l_xmlDocument.SelectNodes("/RacesData/Abilities/" + l_race.getName());
				string l_trimmedAbility;
				for (int i = 0; i < l_abilityList.Count; i++) {
					l_trimmedAbility = l_abilityList[i].InnerText.Trim();
					if (l_trimmedAbility.Equals("")) {
						continue;
					}
					l_race.addAbility(AbilitiesData.getAbility(l_trimmedAbility));
				}
			}
		}
	}
}

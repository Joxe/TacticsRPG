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

		public static List<ChampionClass>  loadAvailableClasses() {
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

		public static List<Element> loadAvailableElements() {
			XmlDocument t_xmlDocument = new XmlDocument();
			List<Element> t_returnList = new List<Element>();

			t_xmlDocument.Load("XML Data/ElementsData.xml");
			XmlNode t_elementNode = t_xmlDocument.SelectSingleNode("/ElementsData");
			for (int i = 0; i < t_elementNode.ChildNodes.Count; i++) {
				t_returnList.Add(new Element(t_elementNode.ChildNodes[i].Name));
			}

			return t_returnList;
		}

		public static void loadElementProperties(List<Element> a_elementList) {
			XmlDocument t_xmlDocument = new XmlDocument();
			t_xmlDocument.Load("XML Data/ElementsData.xml");

			foreach (Element t_element in a_elementList) {
				XmlNodeList t_propertyList = t_xmlDocument.SelectNodes("ElementsData/" + t_element.getName() + "/Strength");
				foreach (XmlNode t_node in t_propertyList) {
					t_element.addStrength(ElementsData.getElement(t_node.NextSibling.InnerText.Trim()));
				}

				t_propertyList = t_xmlDocument.SelectNodes("ElementsData/" + t_element.getName() + "/Weakness");
				foreach (XmlNode t_node in t_propertyList) {
					t_element.addWeakness(ElementsData.getElement(t_node.NextSibling.InnerText.Trim()));
				}
			}
		}
		
		public static List<Ability> loadAvailableAbilities() {
			XmlDocument t_xmlDocument = new XmlDocument();
			List<Ability> t_returnList = new List<Ability>();

			t_xmlDocument.Load("XML Data/AbilitiesData.xml");
			XmlNodeList t_abilityNodes = t_xmlDocument.SelectSingleNode("AbilitiesData").ChildNodes;
			for (int i = 0; i < t_abilityNodes.Count; i++) {
				t_returnList.Add(new Ability(t_abilityNodes[i].Name));
			}

			foreach (Ability t_ability in t_returnList) {
				XmlNode t_abilityNode = t_xmlDocument.SelectSingleNode("AbilitiesData/" + t_ability.getName());
				t_ability.setProperties(
					t_abilityNode.SelectSingleNode("Desc").NextSibling.InnerText.Trim(),
					int.Parse(t_abilityNode.SelectSingleNode("Range").NextSibling.InnerText.Trim()),
					int.Parse(t_abilityNode.SelectSingleNode("AoE").NextSibling.InnerText.Trim()),
					int.Parse(t_abilityNode.SelectSingleNode("Cost").NextSibling.InnerText.Trim()),
					ElementsData.getElement(t_abilityNode.SelectSingleNode("Element").NextSibling.InnerText.Trim())
				);
			}

			return t_returnList;
		}

		public static void setAbilities() {
			XmlDocument t_xmlDocument = new XmlDocument();

			t_xmlDocument.Load("XML Data/ClassesData.xml");
			foreach (ChampionClass t_class in ClassesData.availableClasses()) {
				XmlNodeList t_abilityList = t_xmlDocument.SelectNodes("/ClassesData/Abilities/" + t_class.getName());
				string t_trimmedAbility;
				for (int i = 0; i < t_abilityList.Count; i++) {
					t_trimmedAbility = t_abilityList[i].InnerText.Trim();
					if (t_trimmedAbility.Equals("")) {
						continue;
					}
					t_class.addAbility(AbilitiesData.getAbility(t_trimmedAbility));
				}
			}

			t_xmlDocument.Load("XML Data/RacesData.xml");
			foreach (ChampionRace t_race in RacesData.availableRaces()) {
				XmlNodeList t_abilityList = t_xmlDocument.SelectNodes("/RacesData/Abilities/" + t_race.getName());
				string t_trimmedAbility;
				for (int i = 0; i < t_abilityList.Count; i++) {
					t_trimmedAbility = t_abilityList[i].InnerText.Trim();
					if (t_trimmedAbility.Equals("")) {
						continue;
					}
					t_race.addAbility(AbilitiesData.getAbility(t_trimmedAbility));
				}
			}
		}
	}
}

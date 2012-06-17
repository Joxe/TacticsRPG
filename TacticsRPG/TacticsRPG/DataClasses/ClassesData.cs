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
			try {
				return m_classes[a_class];
			} catch (InvalidOperationException) {
				return null;
			}
		}

		#region Attack
		public static float getStrAtkConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 2.0f; }
			if (a_class.getName().Equals("Mage"))		{ return 0.1f; }
			return 0.0f;
		}

		public static float getAgiAtkConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 1.5f; }
			if (a_class.getName().Equals("Mage"))		{ return 0.2f; }
			return 0.0f;
		}

		public static float getIntAtkConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.2f; }
			if (a_class.getName().Equals("Mage"))		{ return 0.3f; }
			return 0.0f;
		}
		#endregion

		#region Defense
		public static float getStrDefConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 1.5f; }
			if (a_class.getName().Equals("Mage"))		{ return 0.2f; }
			return 0.0f;
		}

		public static float getAgiDefConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 1.1f; }
			if (a_class.getName().Equals("Mage"))		{ return 0.3f; }
			return 0.0f;
		}

		public static float getIntDefConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.8f; }
			if (a_class.getName().Equals("Mage"))		{ return 1.0f; }
			return 0.0f;
		}
		#endregion

		#region Resist
		public static float getStrResConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.5f; }
			if (a_class.getName().Equals("Mage"))		{ return 1.0f; }
			return 0.0f;
		}

		public static float getAgiResConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.3f; }
			if (a_class.getName().Equals("Mage"))		{ return 1.2f; }
			return 0.0f;
		}

		public static float getIntResConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.1f; }
			if (a_class.getName().Equals("Mage"))		{ return 2.5f; }
			return 0.0f;
		}
		#endregion

		#region Magic
		public static float getStrMagConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.3f; }
			if (a_class.getName().Equals("Mage"))		{ return 1.0f; }
			return 0.0f;
		}

		public static float getAgiMagConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.2f; }
			if (a_class.getName().Equals("Mage"))		{ return 1.4f; }
			return 0.0f;
		}

		public static float getIntMagConversion(ChampionClass a_class) {
			if (a_class.getName().Equals("Warrior"))	{ return 0.1f; }
			if (a_class.getName().Equals("Mage"))		{ return 3.0f; }
			return 0.0f;
		}
		#endregion
	}
}
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class ClassesData {
		public static void load() {
			string[] t_classesDirectory = Directory.GetDirectories("Content/Images/Sprites/Classes/");
			ChampionClass[] t_availableClasses = new ChampionClass[t_classesDirectory.Length];

			for (int i = 0; i < t_classesDirectory.Length; i++) {
				string[] t_class = t_classesDirectory[i].Split('/');
				t_availableClasses[i] = new ChampionClass(t_class[t_class.Length - 1]);
				t_availableClasses[i].p_male = t_classesDirectory[i].Split('_')[1].Equals("Male");
				t_availableClasses[i].load();
			}
		}

		private static Dictionary<string, int> Warrior() {	
			Dictionary<string, int> t_baseStats = new Dictionary<string, int>();
			t_baseStats["strength"]		= 40;
			t_baseStats["intellect"]	= 10;
			t_baseStats["agility"]		= 20;
			t_baseStats["luck"]			= 2;
			t_baseStats["manaRegen"]	= 5;
			t_baseStats["move"]			= 1000;
			t_baseStats["jump"]			= 3;
			t_baseStats["speed"]		= 100;
			t_baseStats["maxHealth"]	= 140;
			t_baseStats["maxMana"]		= 40;

			return t_baseStats;
		}

		private static Dictionary<string, int> Mage() {
			Dictionary<string, int> t_baseStats = new Dictionary<string, int>();
			t_baseStats["strength"]		= 10;
			t_baseStats["intellect"]	= 50;
			t_baseStats["agility"]		= 15;
			t_baseStats["luck"]			= 2;
			t_baseStats["manaRegen"]	= 15;
			t_baseStats["move"]			= 2;
			t_baseStats["jump"]			= 2;
			t_baseStats["speed"]		= 90;
			t_baseStats["maxHealth"]	= 90;
			t_baseStats["maxMana"]		= 70;

			return t_baseStats;
		}

		public static Dictionary<string, int> getStats(ChampionClass a_class) {
			string t_class = a_class.getName().Split('_')[0];
			if (t_class.Equals("Warrior")) {
				return Warrior();
			}
			if (t_class.Equals("Mage")) {
				return Mage();
			}
			return new Dictionary<string, int>();
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
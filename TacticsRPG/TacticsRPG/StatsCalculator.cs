using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class StatsCalculator {
		#region Attack
		public static float strengthToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["strength"] * RacesData.getStrAtkConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["strength"] * ClassesData.getStrAtkConversion(a_champion.getClass());
			return t_float;
		}

		public static float agilityToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["agility"] * RacesData.getAgiAtkConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["agility"] * ClassesData.getAgiAtkConversion(a_champion.getClass());
			return t_float;
		}

		public static float intellectToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["intellect"] * RacesData.getIntAtkConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["intellect"] * ClassesData.getIntAtkConversion(a_champion.getClass());
			return t_float;
		}
		#endregion

		#region Defense
		public static float strengthToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["strength"] * RacesData.getStrDefConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["strength"] * ClassesData.getStrDefConversion(a_champion.getClass());
			return t_float;
		}

		public static float agilityToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["agility"] * RacesData.getAgiDefConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["agility"] * ClassesData.getAgiDefConversion(a_champion.getClass());
			return t_float;
		}

		public static float intellectToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["intellect"] * RacesData.getIntDefConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["intellect"] * ClassesData.getIntDefConversion(a_champion.getClass());
			return t_float;
		}
		#endregion

		#region Resist
		public static float strengthToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["strength"] * RacesData.getStrResConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["strength"] * ClassesData.getStrResConversion(a_champion.getClass());
			return t_float;
		}

		public static float agilityToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["agility"] * RacesData.getAgiResConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["agility"] * ClassesData.getAgiResConversion(a_champion.getClass());
			return t_float;
		}

		public static float intellectToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["intellect"] * RacesData.getIntResConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["intellect"] * ClassesData.getIntResConversion(a_champion.getClass());
			return t_float;
		}
		#endregion

		#region Magic Attack
		public static float strengthToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["strength"] * RacesData.getStrMagConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["strength"] * ClassesData.getStrMagConversion(a_champion.getClass());
			return t_float;
		}

		public static float agilityToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["agility"] * RacesData.getAgiMagConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["agility"] * ClassesData.getAgiMagConversion(a_champion.getClass());
			return t_float;
		}

		public static float intellectToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["intellect"] * RacesData.getIntMagConversion(a_champion.getRace());
			t_float += a_champion.getClass().getBaseStats()["intellect"] * ClassesData.getIntMagConversion(a_champion.getClass());
			return t_float;
		}
		#endregion

		public static float summedAttack(Champion a_champion) {
			return strengthToAttack(a_champion) + agilityToAttack(a_champion) + intellectToAttack(a_champion);
		}

		public static float summedDefense(Champion a_champion) {
			return strengthToDefense(a_champion) + agilityToDefense(a_champion) + intellectToDefense(a_champion);
		}

		public static float summedResist(Champion a_champion) {
			return strengthToResist(a_champion) + agilityToResist(a_champion) + intellectToResist(a_champion);
		}

		public static float summedMagic(Champion a_champion) {
			return strengthToMagic(a_champion) + agilityToMagic(a_champion) + intellectToMagic(a_champion);			
		}
	}
}

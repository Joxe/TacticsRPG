using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class StatsCalculator {
		#region Attack
		public static float StrengthToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("Strength");
			t_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("Strength");
			return t_float;
		}

		public static float AgilityToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("Agility");
			t_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("Agility");
			return t_float;
		}

		public static float IntellectToAttack(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("Intellect");
			t_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("Intellect");
			return t_float;
		}
		#endregion

		#region Defense
		public static float StrengthToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("Strength");
			t_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("Strength");
			return t_float;
		}

		public static float AgilityToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("Agility");
			t_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("Agility");
			return t_float;
		}

		public static float IntellectToDefense(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("Intellect");
			t_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("Intellect");
			return t_float;
		}
		#endregion

		#region Resist
		public static float StrengthToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("Strength");
			t_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("Strength");
			return t_float;
		}

		public static float AgilityToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("Agility");
			t_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("Agility");
			return t_float;
		}

		public static float IntellectToResist(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("Intellect");
			t_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("Intellect");
			return t_float;
		}
		#endregion

		#region Magic Attack
		public static float StrengthToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("Strength");
			t_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("Strength");
			return t_float;
		}

		public static float AgilityToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("Agility");
			t_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("Agility");
			return t_float;
		}

		public static float IntellectToMagic(Champion a_champion) {
			float t_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("Intellect");
			t_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("Intellect");
			return t_float;
		}
		#endregion

		public static float summedAttack(Champion a_champion) {
			return StrengthToAttack(a_champion) + AgilityToAttack(a_champion) + IntellectToAttack(a_champion);
		}

		public static float summedDefense(Champion a_champion) {
			return StrengthToDefense(a_champion) + AgilityToDefense(a_champion) + IntellectToDefense(a_champion);
		}

		public static float summedResist(Champion a_champion) {
			return StrengthToResist(a_champion) + AgilityToResist(a_champion) + IntellectToResist(a_champion);
		}

		public static float summedMagic(Champion a_champion) {
			return StrengthToMagic(a_champion) + AgilityToMagic(a_champion) + IntellectToMagic(a_champion);			
		}
	}
}

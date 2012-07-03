using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class StatsCalculator {
		#region Attack
		public static float StrengthToAttack(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("AttackStrength");
			l_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("AttackStrength");
			return l_float;
		}

		public static float AgilityToAttack(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("AttackAgility");
			l_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("AttackAgility");
			return l_float;
		}

		public static float IntellectToAttack(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("AttackIntellect");
			l_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("AttackIntellect");
			return l_float;
		}
		#endregion

		#region Defense
		public static float StrengthToDefense(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("DefenseStrength");
			l_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("DefenseStrength");
			return l_float;
		}

		public static float AgilityToDefense(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("DefenseAgility");
			l_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("DefenseAgility");
			return l_float;
		}

		public static float IntellectToDefense(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("DefenseIntellect");
			l_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("DefenseIntellect");
			return l_float;
		}
		#endregion

		#region Resist
		public static float StrengthToResist(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("MagicResistStrength");
			l_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("MagicResistStrength");
			return l_float;
		}

		public static float AgilityToResist(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("MagicResistAgility");
			l_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("MagicResistAgility");
			return l_float;
		}

		public static float IntellectToResist(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("MagicResistIntellect");
			l_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("MagicResistIntellect");
			return l_float;
		}
		#endregion

		#region Magic Attack
		public static float StrengthToMagic(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Strength"] * a_champion.getRace().getRatio("MagicAttackStrength");
			l_float += a_champion.getClass().getBaseStats()["Strength"] * a_champion.getClass().getRatio("MagicAttackStrength");
			return l_float;
		}

		public static float AgilityToMagic(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Agility"] * a_champion.getRace().getRatio("MagicAttackAgility");
			l_float += a_champion.getClass().getBaseStats()["Agility"] * a_champion.getClass().getRatio("MagicAttackAgility");
			return l_float;
		}

		public static float IntellectToMagic(Champion a_champion) {
			float l_float = a_champion.getRace().getBaseStats()["Intellect"] * a_champion.getRace().getRatio("MagicAttackIntellect");
			l_float += a_champion.getClass().getBaseStats()["Intellect"] * a_champion.getClass().getRatio("MagicAttackIntellect");
			return l_float;
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

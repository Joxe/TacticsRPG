using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class AbilitiesData {
		private static Dictionary<string, Ability> m_abilities;

		public static void load() {
			m_abilities = new Dictionary<string, Ability>();
			List<Ability> l_abilityList = XMLParser.loadAvailableAbilities();

			foreach (Ability l_ability in l_abilityList) {
				m_abilities.Add(l_ability.getName(), l_ability);
			}
		}

		public static Ability getAbility(string a_ability) {
			#if DEBUG
			return m_abilities[a_ability];
			#else
			try {
				return m_abilities[a_ability];
			} catch (InvalidOperationException) {
				return null;
			}
			#endif
		}

		public static List<Ability> getAbilities() {
			return m_abilities.Values.ToList();
		}
	}
}

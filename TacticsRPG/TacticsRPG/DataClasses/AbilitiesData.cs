using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class AbilitiesData {
		private static Dictionary<string, Ability> m_abilities;

		public static void load() {
			m_abilities = new Dictionary<string, Ability>();
			List<Ability> t_abilityList = XMLParser.loadAvailableAbilities();

			foreach (Ability t_ability in t_abilityList) {
				m_abilities.Add(t_ability.getName(), t_ability);
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

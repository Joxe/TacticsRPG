using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class DoTEffect : Effect {
		private int m_turnsLeft;
		private int m_minDamage;
		private int m_maxDamage;

		public DoTEffect(string a_name, Element a_element, int a_maxTurns, int a_minDamage, int a_maxDamage) : base(a_name, a_element) {
			m_turnsLeft = MathManager.randomInt(1, a_maxTurns);
			m_minDamage = a_minDamage;
			m_maxDamage = a_maxDamage;
		}

		public override void invokeEffect(Champion a_champion) {
			a_champion.damage(MathManager.randomInt(m_minDamage, m_maxDamage));
			if (m_turnsLeft-- <= 0) {

			}
		}

		public string getInfo() {
			return m_name + ": " + m_turnsLeft + " turns left";
		}
	}
}

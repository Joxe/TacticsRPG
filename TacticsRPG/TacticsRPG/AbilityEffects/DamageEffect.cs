using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class DamageEffect : Effect {
		private int m_minDamage;
		private int m_maxDamage;

		public DamageEffect(string a_name, Element a_element, int a_min, int a_max) : base(a_name, a_element) {
			m_minDamage = a_min;
			m_maxDamage = a_max;
		}

		public override void invokeEffect(Champion a_champion) {
			a_champion.damage(MathManager.randomInt(m_minDamage, m_maxDamage));
		}
	}
}
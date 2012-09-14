using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class Ability {
		private string m_name;
		private string m_desc;
		private int m_range;
		private int m_aoe;
		private int m_cost;
		private List<Effect> m_effects;

		public Ability(string a_name) {
			m_name		= a_name;
		}

		public void setProperties(string a_desc, int a_range, int a_aoe, int a_cost) {
			m_desc		= a_desc;
			m_range		= a_range;
			m_aoe		= a_aoe;
			m_cost		= a_cost;
			m_effects	= new List<Effect>();
		}

		public string getName() {
			return m_name;
		}

		public string getDescription() {
			return m_desc;
		}

		public int getRange() {
			return m_range;
		}

		public int getAoE() {
			return m_aoe;
		}

		public int getCost() {
			return m_cost;
		}

		public void addEffect(Effect a_effect) {
			if (!m_effects.Contains(a_effect)) {
				m_effects.Add(a_effect);
			}
		}

		public List<Effect> getEffects() {
			return m_effects;
		}

		public void invokeAbility(Tile a_tile) {
			LinkedList<Tile> l_affectedTiles = ((GameState)Game.getInstance().getCurrentState()).getTileMap().getRangeOfTiles(a_tile, m_aoe);
			foreach (Tile l_tile in l_affectedTiles) {
				foreach (Effect l_effect in m_effects) {
					castEffect(l_effect, l_tile.p_object);
				}
			}
		}

		public void castEffect(Effect a_effect, BattlefieldObject a_target) {
			if (a_target != null) {
				a_target.addEffect(a_effect);
			}
		}
	}
}
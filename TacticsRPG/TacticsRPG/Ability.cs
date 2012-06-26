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
		private Element m_element;

		public Ability(string a_name) {
			m_name		= a_name;
		}

		public void setProperties(string a_desc, int a_range, int a_aoe, int a_cost, Element a_element) {
			m_desc		= a_desc;
			m_range		= a_range;
			m_aoe		= a_aoe;
			m_cost		= a_cost;
			m_element	= a_element;
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

		public Element getElement() {
			return m_element;
		}
	}
}
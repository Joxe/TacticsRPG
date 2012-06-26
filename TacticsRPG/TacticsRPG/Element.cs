using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class Element {
		private string m_name;
		private List<Element> m_strength;
		private List<Element> m_weakness;

		public Element(string a_name) {
			m_name = a_name;
			m_strength = new List<Element>();
			m_weakness = new List<Element>();
		}

		public void addStrength(Element a_element) {
			if (!m_strength.Contains(a_element)) {
				m_strength.Add(a_element);
			}
		}

		public bool isStrongAgainst(Element a_element) {
			return m_strength.Contains(a_element);
		}

		public void addWeakness(Element a_element) {
			if (!m_weakness.Contains(a_element)) {
				m_weakness.Add(a_element);
			}
		}

		public bool isWeakTo(Element a_element) {
			return m_weakness.Contains(a_element);
		}

		public string getName() {
			return m_name;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public abstract class Effect {
		protected string m_name;
		protected Element m_element;
		
		protected Effect(string a_name, Element a_element) {
			m_name = a_name;
			m_element = a_element;
		}

		public string getName() {
			return m_name;
		}

		public Element getElement() {
			return m_element;
		}

		public abstract void invokeEffect(Champion a_champion);
	}
}

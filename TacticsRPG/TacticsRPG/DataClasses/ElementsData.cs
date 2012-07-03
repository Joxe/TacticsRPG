using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class ElementsData {
		private static Dictionary<string, Element> m_elements;

		public static void load() {
			m_elements = new Dictionary<string,Element>();
			List<Element> l_elementList = XMLParser.loadAvailableElements();
			foreach (Element l_element in l_elementList) {
				m_elements.Add(l_element.getName(), l_element);
			}
			XMLParser.loadElementProperties(m_elements.Values.ToList());
		}

		public static Element getElement(string a_element) {
			#if DEBUG
			return m_elements[a_element];
			#else
			try {
				return m_elements[a_element];
			} catch (InvalidOperationException) {
				return null;
			}
			#endif
		}

		public static List<Element> getElements() {
			return m_elements.Values.ToList();
		}
	}
}

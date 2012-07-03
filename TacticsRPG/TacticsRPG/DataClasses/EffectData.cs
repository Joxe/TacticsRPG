using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public static class EffectData {
		private static Dictionary<string, Effect> m_effects;

		public static void load() {
			m_effects = new Dictionary<string, Effect>();
		}

		public static Effect getEffect(string a_effect) {
			#if DEBUG
			return m_effects[a_effect];
			#else
			try {
				return m_effects[a_effect];
			} catch (InvalidOperationException) {
				return null;
			}
			#endif
		}
	}
}

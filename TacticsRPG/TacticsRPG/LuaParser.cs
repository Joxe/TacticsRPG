using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LuaInterface;

namespace TacticsRPG {
	class LuaParser {
		private static Lua m_lua = new Lua();

		public static void registerMethod(string a_identifier, Object a_target) {
			m_lua.RegisterFunction(a_identifier, a_target, a_target.GetType().GetMethod(a_identifier));
		}

		public static void doFile(string a_filepath) {
			m_lua.DoFile(a_filepath);
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LuaInterface;

namespace TacticsRPG {
	class GUIAddon {
		private LuaFunction m_onLoad;
		private LuaFunction m_onUpdate;
		private LuaFunction m_onDraw;
		private Lua m_lua;

		public GUIAddon(string a_filePath) {
			m_lua = new Lua();
			((GameState)Game.getInstance().getCurrentState()).getGUI().registerFunctions(m_lua);
			m_lua.DoFile(a_filePath);

			m_onLoad = m_lua.GetFunction("OnLoad");
			m_onUpdate = m_lua.GetFunction("OnUpdate");
			m_onDraw = m_lua.GetFunction("OnDraw");
		}

		public void load() {
			m_onLoad.Call();
		}

		public void update() {
			m_onUpdate.Call();
		}

		public void draw() {
			m_onDraw.Call();
		}
	}
}

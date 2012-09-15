using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using LuaInterface;

namespace TacticsRPG {
	class GuiElement {
		private LuaFunction m_onLoad;
		private LuaFunction m_onUpdate;
		private LuaFunction m_onDraw;
		private Lua m_luaFile = new Lua();

		public GuiElement(string a_filePath) {
			m_luaFile.LoadFile(a_filePath);
			m_onLoad = m_luaFile.GetFunction("kuk");
			m_onUpdate = m_luaFile.GetFunction("OnUpdate");
			m_onDraw = m_luaFile.GetFunction("OnDraw");
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

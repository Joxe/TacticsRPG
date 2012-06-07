using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public class KeyboardHandler {
		private static KeyboardState m_currentKeyboard;
		private static KeyboardState m_previousKeyboard;

		public static void setCurrentKeyboard() {
			m_currentKeyboard = Keyboard.GetState();
		}

		public static void setPreviousKeyboard() {
			m_previousKeyboard = m_currentKeyboard;
		}

		public static bool keyPressed(Keys k) {
			return m_currentKeyboard.IsKeyDown(k) && m_previousKeyboard.IsKeyUp(k);
		}

		public static bool keyReleased(Keys k) {
			return m_currentKeyboard.IsKeyUp(k) && m_previousKeyboard.IsKeyDown(k);
		}

		public static bool keyDown(Keys k) {
			return m_currentKeyboard.IsKeyDown(k);
		}
		
		public static bool keyWasDown(Keys k) {
			return m_previousKeyboard.IsKeyDown(k) && m_currentKeyboard.IsKeyUp(k);
		}

		public static Keys[] getPressedKeys() {
			return m_currentKeyboard.GetPressedKeys();
		}
	}
}

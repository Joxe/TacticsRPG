﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public class MouseHandler {
		private static MouseState m_currentMouse;
		private static MouseState m_previousMouse;

		public static void setCurrentMouse() {
			m_currentMouse = Mouse.GetState();
		}

		public static void setPreviousMouse() {
			m_previousMouse = m_currentMouse;
		}

		public static Vector2 getCurPos() {
			return new Vector2(m_currentMouse.X, m_currentMouse.Y);
		}

		public static Vector2 getPrePos() {
			return new Vector2(m_previousMouse.X, m_previousMouse.Y);
		}

		public static bool lmbPressed() {
			return m_currentMouse.LeftButton == ButtonState.Pressed;
		}

		public static bool lmbDown() {
			return m_currentMouse.LeftButton == ButtonState.Pressed && m_previousMouse.LeftButton == ButtonState.Released;
		}

		public static bool lmbUp() {
			return m_currentMouse.LeftButton == ButtonState.Released && m_previousMouse.LeftButton == ButtonState.Pressed;
		}

		public static bool rmbPressed() {
			return m_currentMouse.RightButton == ButtonState.Pressed;
		}

		public static bool rmbDown() {
			return m_currentMouse.RightButton == ButtonState.Pressed && m_previousMouse.RightButton == ButtonState.Released;
		}

		public static bool rmbUp() {
			return m_currentMouse.RightButton == ButtonState.Released && m_previousMouse.RightButton == ButtonState.Pressed;
		}

		public static bool mmbPressed() {
			return m_currentMouse.MiddleButton == ButtonState.Pressed;
		}

		public static bool mmbDown() {
			return m_currentMouse.MiddleButton == ButtonState.Pressed && m_previousMouse.MiddleButton == ButtonState.Released;
		}

		public static bool mmbUp() {
			return m_currentMouse.MiddleButton == ButtonState.Released && m_previousMouse.MiddleButton == ButtonState.Pressed;
		}

		public static bool scrollUp() {
			return m_currentMouse.ScrollWheelValue > m_previousMouse.ScrollWheelValue;
		}

		public static bool scrollDown() {
			return m_currentMouse.ScrollWheelValue < m_previousMouse.ScrollWheelValue;
		}

		public static int scrollDiff() {
			return m_currentMouse.ScrollWheelValue - m_previousMouse.ScrollWheelValue;
		}

		public static Vector2 worldMouse() {
			Camera l_camera = Game.getInstance().m_camera;
			return MouseHandler.getCurPos() / l_camera.p_zoom + l_camera.p_position - ((Game.getInstance().getResolution() / 2) / l_camera.p_zoom);
		}

		public static Vector2 previousWorldMouse() {
			Camera l_camera = Game.getInstance().m_camera;
			return MouseHandler.getPrePos() / l_camera.p_zoom + l_camera.p_position - ((Game.getInstance().getResolution() / 2) / l_camera.p_zoom);
		}
	}
}

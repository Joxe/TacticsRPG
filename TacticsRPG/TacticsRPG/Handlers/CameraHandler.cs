using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	//Static class in charge of handling camera movement and other camera related methods
	public static class CameraHandler {
		public static void cameraDrag() {
			Vector2 t_difference = (MouseHandler.getCurPos() - Game.getInstance().getResolution() / 2) / 20 / Game.getInstance().m_camera.p_zoom;
			Game.getInstance().m_camera.setPosition(Game.getInstance().m_camera.p_position + t_difference);
		}

		public static void zoomIn(float a_zoom) {
			Game.getInstance().m_camera.p_zoom = Math.Min(Game.getInstance().m_camera.p_zoom + a_zoom, 2.0f);
		}

		public static void zoomOut(float a_zoom) {
			Game.getInstance().m_camera.p_zoom = Math.Max(Game.getInstance().m_camera.p_zoom - a_zoom, 0.1f);
		}

		public static void move(Vector2 a_posV2) {
			Game.getInstance().m_camera.setPosition(Game.getInstance().m_camera.p_position + a_posV2);
		}

		public static bool isInCamera(GameObject a_gameObject) {
			return Game.getInstance().m_camera.getRectangle().contains(a_gameObject.p_position);
		}
	}
}
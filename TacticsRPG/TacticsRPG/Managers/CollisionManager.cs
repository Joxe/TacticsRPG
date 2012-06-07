using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class CollisionManager {
		public static bool isLeftOfLine(Vector2 a_lineStart, Vector2 a_lineEnd, Vector2 a_point) {
			return ((a_lineEnd.X - a_lineStart.X) * (a_point.Y - a_lineStart.Y) - (a_lineEnd.Y - a_lineStart.Y) * (a_point.X - a_lineStart.X)) > 0;
		}

		public static bool hexagonContains(GameObject a_hexagon, Vector2 a_point, int a_width, int a_height) {
			float t_middleX = a_hexagon.p_position.X + a_width / 2;

			Vector2 t_ptLeft		= new Vector2(a_hexagon.p_position.X					, a_hexagon.p_position.Y + a_height / 2	);
			Vector2 t_ptTopLeft		= new Vector2(a_hexagon.p_position.X + (a_width * 0.25f), a_hexagon.p_position.Y					);
			Vector2 t_ptTopRight	= new Vector2(a_hexagon.p_position.X + (a_width * 0.75f), a_hexagon.p_position.Y					);
			Vector2 t_ptRight		= new Vector2(a_hexagon.p_position.X + a_width			, a_hexagon.p_position.Y + a_height / 2	);
			Vector2 t_ptBtmRight	= new Vector2(a_hexagon.p_position.X + (a_width * 0.75f), a_hexagon.p_position.Y + a_height		);
			Vector2 t_ptBtmLeft		= new Vector2(a_hexagon.p_position.X + (a_width * 0.25f), a_hexagon.p_position.Y + a_height		);

			if ((a_point.X > a_hexagon.p_position.X && a_point.X < a_hexagon.p_position.X + a_width) && (a_point.Y > a_hexagon.p_position.Y && a_point.Y < a_hexagon.p_position.Y + a_height)) {
				if (a_point.Y < t_ptLeft.Y) {
					if (a_point.X < t_middleX) {
						return CollisionManager.isLeftOfLine(t_ptLeft, t_ptTopLeft, a_point);
					} else {
						return CollisionManager.isLeftOfLine(t_ptTopRight, t_ptRight, a_point);
					}
				} else {
					if (a_point.X < t_middleX) {
						return !CollisionManager.isLeftOfLine(t_ptLeft, t_ptBtmLeft, a_point);
					} else {
						return !CollisionManager.isLeftOfLine(t_ptBtmRight, t_ptRight, a_point);
					}
				}
			} else {
				return false;
			}
		}
	}
}

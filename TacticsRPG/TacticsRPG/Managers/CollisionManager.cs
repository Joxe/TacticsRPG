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
			float l_middleX = a_hexagon.p_position.X + a_width / 2;

			Vector2 l_ptLeft		= new Vector2(a_hexagon.p_position.X					, a_hexagon.p_position.Y + a_height / 2	);
			Vector2 l_ptTopLeft		= new Vector2(a_hexagon.p_position.X + (a_width * 0.25f), a_hexagon.p_position.Y					);
			Vector2 l_ptTopRight	= new Vector2(a_hexagon.p_position.X + (a_width * 0.75f), a_hexagon.p_position.Y					);
			Vector2 l_ptRight		= new Vector2(a_hexagon.p_position.X + a_width			, a_hexagon.p_position.Y + a_height / 2	);
			Vector2 l_ptBtmRight	= new Vector2(a_hexagon.p_position.X + (a_width * 0.75f), a_hexagon.p_position.Y + a_height		);
			Vector2 l_ptBtmLeft		= new Vector2(a_hexagon.p_position.X + (a_width * 0.25f), a_hexagon.p_position.Y + a_height		);

			if ((a_point.X > a_hexagon.p_position.X && a_point.X < a_hexagon.p_position.X + a_width) && (a_point.Y > a_hexagon.p_position.Y && a_point.Y < a_hexagon.p_position.Y + a_height)) {
				if (a_point.Y < l_ptLeft.Y) {
					if (a_point.X < l_middleX) {
						return CollisionManager.isLeftOfLine(l_ptLeft, l_ptTopLeft, a_point);
					} else {
						return CollisionManager.isLeftOfLine(l_ptTopRight, l_ptRight, a_point);
					}
				} else {
					if (a_point.X < l_middleX) {
						return !CollisionManager.isLeftOfLine(l_ptLeft, l_ptBtmLeft, a_point);
					} else {
						return !CollisionManager.isLeftOfLine(l_ptBtmRight, l_ptRight, a_point);
					}
				}
			} else {
				return false;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class MathManager {
		private static Random m_random = new Random();
		private const double Rad2Deg = 180.0 / Math.PI;

		public static int randomInt(int a_minimum, int a_maximum) {
			return m_random.Next(a_minimum, a_maximum);	
		}

		public static void updateRandomSeed() {
			m_random = new Random(Game.getInstance().getGameTime().TotalGameTime.Milliseconds);
		}

		public static bool isEven(int a_number) {
			if ((a_number & 1) == 0) {
				return true;
			} else {
				return false;
			}
		}

		public static double angle(Vector2 a_startPoint, Vector2 a_endPoint){
			return Math.Atan2(a_startPoint.Y - a_endPoint.Y, a_endPoint.X - a_startPoint.X) * Rad2Deg;
		}

		public static int attack(Champion a_attacker, Champion a_defender) {
			return Math.Max((a_attacker.getStat("attack") - a_defender.getStat("defense")), 0);
		}
	}
}

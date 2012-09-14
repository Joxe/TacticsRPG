using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public abstract class PathFinder {
		public abstract Stack<Tile> findPath(Vector2 a_startPos, Vector2 a_endPos);
	}
}

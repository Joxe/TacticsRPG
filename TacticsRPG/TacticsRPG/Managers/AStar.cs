using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class AStar {
		public enum PathfindState {
			Hexagon, Square
		}

		private static double getPathValue(Tile a_curTile, Tile a_endTile) {
			return Math.Sqrt(Math.Pow(a_curTile.getMapPosition().X - a_endTile.getMapPosition().X, 2) + Math.Pow(a_curTile.getMapPosition().Y - a_endTile.getMapPosition().Y, 2));
		}

		public static Stack<Tile> findPath(Vector2 a_startPos, Vector2 a_endPos, PathfindState a_currentMapType) {
			TileMap l_tileMap = ((GameState)Game.getInstance().getCurrentState()).getTileMap();
			return findPath(l_tileMap.getTile(a_startPos), l_tileMap.getTile(a_endPos), a_currentMapType);
		}

		public static Stack<Tile> findPath(Tile a_startTile, Tile a_endTile, PathfindState a_currentMapType) {
			TileMap l_tileMap = ((GameState)Game.getInstance().getCurrentState()).getTileMap();
			Dictionary<Tile, double> l_closedSet	= new Dictionary<Tile, double>();
			Dictionary<Tile, double> l_openSet		= new Dictionary<Tile, double>();
			Dictionary<Tile, Tile> l_cameFrom		= new Dictionary<Tile, Tile>();

			Tile l_neighbor;

			int[] Xcheck = new int[] { 0 };
			int[] Ycheck = new int[] { 0 };

			l_openSet.Add(a_startTile, getPathValue(a_startTile, a_endTile));
			l_cameFrom.Add(a_startTile, a_startTile);

			while (l_openSet.Count > 0) {
				KeyValuePair<Tile, double> l_current = l_openSet.First();

				foreach (KeyValuePair<Tile, double> l_kvPair in l_openSet) {
					if (l_kvPair.Value < l_current.Value) {
						l_current = l_kvPair;
					}
				}

				if (l_current.Key.getMapPosition() == a_endTile.getMapPosition()) {
					Stack<Tile> l_reconstructedPath = new Stack<Tile>();
					l_reconstructedPath.Push(l_current.Key);
					return reconstructPath(l_cameFrom, l_current.Key, l_reconstructedPath);
				}

				l_openSet.Remove(l_current.Key);
				l_closedSet.Add(l_current.Key, l_current.Value);

				switch (a_currentMapType) {
					case PathfindState.Hexagon:
						Xcheck = MathManager.isEven(l_current.Key.X) ? MathManager.evenX : MathManager.oddX;
						Ycheck = MathManager.isEven(l_current.Key.X) ? MathManager.evenY : MathManager.oddY;
						break;
					case PathfindState.Square:
						Xcheck = new int[] { 0, 1, 0, -1 };
						Ycheck = new int[] { -1, 0, 1, 0 };
						break;
				}

				for (int i = 0; i < Xcheck.Length && i < Ycheck.Length; i++) {
					int l_newX = (int)l_current.Key.getMapPosition().X + Xcheck[i];
					int l_newY = (int)l_current.Key.getMapPosition().Y + Ycheck[i];

					l_neighbor = l_tileMap.getTile(l_newX, l_newY);

					if (l_neighbor != null) {
						//int l_heightDifference = l_current.Key.p_height + l_neighbor.p_height;
						if (l_closedSet.ContainsKey(l_neighbor) || l_neighbor.isObstructed()) {
							continue;
						}

						double l_tentativeGScore = l_current.Value + getPathValue(l_neighbor, a_endTile)/* + l_heightDifference*/;

						if (!l_openSet.ContainsKey(l_neighbor) || l_tentativeGScore < l_openSet[l_neighbor]) {
							l_openSet[l_neighbor] = l_tentativeGScore;
							l_cameFrom[l_neighbor] = l_current.Key;
						}
					}
				}
			}
			return new Stack<Tile>();
		}

		public static Stack<Tile> reconstructPath(Dictionary<Tile, Tile> a_cameFrom, Tile a_currentNode, Stack<Tile> a_workingList) {
			if (a_currentNode.getMapPosition() != a_cameFrom[a_currentNode].getMapPosition()) {
				a_workingList.Push(a_cameFrom[a_currentNode]);
				return reconstructPath(a_cameFrom, a_cameFrom[a_currentNode], a_workingList);
			} else {
				return a_workingList;
			}
		}
	}
}
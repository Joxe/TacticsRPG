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
			TileMap t_tileMap = ((GameState)Game.getInstance().getCurrentState()).getTileMap();
			return findPath(t_tileMap.getTile(a_startPos), t_tileMap.getTile(a_endPos), a_currentMapType);
		}

		public static Stack<Tile> findPath(Tile a_startTile, Tile a_endTile, PathfindState a_currentMapType) {
			TileMap t_tileMap = ((GameState)Game.getInstance().getCurrentState()).getTileMap();
			Dictionary<Tile, double> t_closedSet	= new Dictionary<Tile, double>();
			Dictionary<Tile, double> t_openSet		= new Dictionary<Tile, double>();
			Dictionary<Tile, Tile> t_cameFrom = new Dictionary<Tile, Tile>();

			Tile t_neighbor;

			int[] Xcheck = new int[] { 0 };
			int[] Ycheck = new int[] { 0 };

			t_openSet.Add(a_startTile, getPathValue(a_startTile, a_endTile));
			t_cameFrom.Add(a_startTile, a_startTile);

			while (t_openSet.Count > 0) {
				KeyValuePair<Tile, double> t_current = t_openSet.First();

				foreach (KeyValuePair<Tile, double> t_kvPair in t_openSet) {
					if (t_kvPair.Value < t_current.Value) {
						t_current = t_kvPair;
					}
				}

				if (t_current.Key.getMapPosition() == a_endTile.getMapPosition()) {
					Stack<Tile> t_reconstructedPath = new Stack<Tile>();
					t_reconstructedPath.Push(t_current.Key);
					return reconstructPath(t_cameFrom, t_current.Key, t_reconstructedPath);
				}

				t_openSet.Remove(t_current.Key);
				t_closedSet.Add(t_current.Key, t_current.Value);

				switch (a_currentMapType) {
					case PathfindState.Hexagon:
						if (MathManager.isEven((int)t_current.Key.getMapPosition().X)) {
							Xcheck = new int[] { -1,  0,  1,  1,  0, -1 };
							Ycheck = new int[] {  1, -1,  0,  1,  1,  0 };
						} else {
							Xcheck = new int[] {  1,  0,  1, -1,  0, -1 };
							Ycheck = new int[] { -1, -1,  0, -1,  1,  0 };
						}
						break;
					case PathfindState.Square:
						Xcheck = new int[] { 0, 1, 0, -1 };
						Ycheck = new int[] { -1, 0, 1, 0 };
						break;
				}


				for (int i = 0; i < Xcheck.Length && i < Ycheck.Length; i++) {
					int t_newX = (int)t_current.Key.getMapPosition().X + Xcheck[i];
					int t_newY = (int)t_current.Key.getMapPosition().Y + Ycheck[i];

					t_neighbor = t_tileMap.getTile(t_newX, t_newY);

					if (t_neighbor != null) {
						//int t_heightDifference = t_current.Key.p_height + t_neighbor.p_height;
						if (t_closedSet.ContainsKey(t_neighbor)) {
							continue;
						}
						if (t_neighbor.p_champion != null) {
							continue;
						}
						double t_tentativeGScore = t_current.Value + getPathValue(t_neighbor, a_endTile)/* + t_heightDifference*/;

						if (!t_openSet.ContainsKey(t_neighbor) || t_tentativeGScore < t_openSet[t_neighbor]) {
							t_openSet[t_neighbor] = t_tentativeGScore;
							t_cameFrom[t_neighbor] = t_current.Key;
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
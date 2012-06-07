using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class TileMap {
		private Tile[,] m_tileMap;
		private int m_width;
		private int m_height;
		private string m_tileSet;
		private Dictionary<Tile.TileState, Sprite> m_spriteDict;
		private Tile m_hoveredTile;

		public TileMap(int width, int height, string a_tileSet) {
			m_width = width;
			m_height = height;
			m_tileSet = a_tileSet + "/";
			m_spriteDict = new Dictionary<Tile.TileState, Sprite>();

			m_spriteDict.Add(Tile.TileState.Normal	, new Sprite("Tiles/" + m_tileSet + "normal"	, 1));
			m_spriteDict.Add(Tile.TileState.Hover	, new Sprite("Tiles/" + m_tileSet + "hover"		, 1));
			m_spriteDict.Add(Tile.TileState.Pressed	, new Sprite("Tiles/" + m_tileSet + "pressed"	, 1));
			m_spriteDict.Add(Tile.TileState.Toggle	, new Sprite("Tiles/" + m_tileSet + "toggle"	, 1));

			m_tileMap = new Tile[width, height];
		}

		public void load() {
			foreach (Sprite t_sprite in m_spriteDict.Values) {
				t_sprite.load();
			}
			
			int t_heightIndex = MathManager.randomInt(3, 7);

			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					m_tileMap[i, j] = new Tile(new Vector2(i, j), MathManager.randomInt(t_heightIndex - 3, t_heightIndex + 3));
					//m_tileMap[i, j] = new Tile(new Vector2(i, j), 1/*MathManager.randomInt(t_heightIndex - 3, t_heightIndex + 3)*/);
					m_tileMap[i, j].p_tileMap = this;
					m_tileMap[i, j].load();
					if (MathManager.isEven(i)) {
						m_tileMap[i, j].move(new Vector2(0, 111));
					}
				}
				MathManager.randomInt(3, 7);
			}
		}

		public Tile getTile(int x, int y) {
			try {
				return m_tileMap[x, y];
			} catch (IndexOutOfRangeException) {
				return null;
			}
		}

		public Tile getTile(Tile a_tile, Vector2 a_mapOffset) {
			try {
				return m_tileMap[a_tile.X + (int)a_mapOffset.X, a_tile.Y + (int)a_mapOffset.Y];
			} catch (IndexOutOfRangeException) {
				return null;
			}
		}

		public Tile getTile(Vector2 a_position) {
			try {
				return m_tileMap[(int)a_position.X, (int)a_position.Y];
			} catch (IndexOutOfRangeException) {
				return null;
			}
		}

		public void update() {
			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					m_tileMap[i, j].update();
				}
			}
		}

		public void draw() {
			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					m_tileMap[i, j].draw();
				}
			}
		}

		public string getTileSet() {
			return m_tileSet;
		}

		public Dictionary<Tile.TileState, Sprite> getSpriteDict() {
			return m_spriteDict;
		}

		public LinkedList<Tile> getSurroundingTiles(Tile a_tile, int a_stepsTaken, int a_range, LinkedList<Tile> a_list) {
			if (a_stepsTaken > a_range || a_list.Contains(a_tile)) {
				return a_list;
			} else {
				a_list.AddLast(a_tile);
				if (a_stepsTaken == a_range) {
					return a_list;
				}
			}

			int[] Xcheck = new int[] { 0 };
			int[] Ycheck = new int[] { 0 };

			if (MathManager.isEven(a_tile.X)) {
				Xcheck = new int[] { -1,  0,  1,  1,  0, -1 };
				Ycheck = new int[] {  1, -1,  0,  1,  1,  0 };
			} else {
				Xcheck = new int[] {  1,  0,  1, -1,  0, -1 };
				Ycheck = new int[] { -1, -1,  0, -1,  1,  0 };
			}

			Tile t_tile;
			for (int i = 0; i < Xcheck.Length; i++) {
				for (int j = 0; j < Ycheck.Length; j++) {
					if ((t_tile = getTile(a_tile, new Vector2(Xcheck[i], Ycheck[i]))) != null) {
						getSurroundingTiles(t_tile, a_stepsTaken + 1, a_range, a_list);
					}
				}
			}
			return a_list;
		}

		public void toggleSurroundingTiles(Tile a_tile, int a_stepsTaken, int a_range) {
			if (a_stepsTaken > a_range) {
				return;
			} else {
				a_tile.p_tileState = Tile.TileState.Toggle;
				if (a_stepsTaken == a_range) {
					return;
				}
			}
			int[] Xcheck = new int[0];
			int[] Ycheck = new int[0];

			if (MathManager.isEven(a_tile.X)) {
				Xcheck = new int[] { -1,  0,  1,  1,  0, -1 };
				Ycheck = new int[] {  1, -1,  0,  1,  1,  0 };
			} else {
				Xcheck = new int[] {  1,  0,  1, -1,  0, -1 };
				Ycheck = new int[] { -1, -1,  0, -1,  1,  0 };
			}

			Tile t_tile;
			for (int i = 0; i < Xcheck.Length; i++) {
				for (int j = 0; j < Xcheck.Length; j++) {
					if ((t_tile = getTile(a_tile, new Vector2(Xcheck[i], Ycheck[i]))) != null) {
						toggleSurroundingTiles(t_tile, a_stepsTaken + 1, a_range);
					}
				}
			}
		}

		public Tile p_hover {
			get {
				return m_hoveredTile;
			}
			set {
				m_hoveredTile = value;
			}
		}

		public void restoreStates() {
			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					m_tileMap[i, j].restoreState();
				}
			}
		}
	}
}

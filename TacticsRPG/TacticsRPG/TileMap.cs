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
		private bool m_ignoreMouse;

		public TileMap(int width, int height, string a_tileSet) {
			m_width = width;
			m_height = height;
			m_tileSet = a_tileSet + "/";
			m_spriteDict = new Dictionary<Tile.TileState, Sprite>();

			m_spriteDict.Add(Tile.TileState.Normal	, new Sprite("Tiles/" + m_tileSet + "normal.png"	, 1));
			m_spriteDict.Add(Tile.TileState.Hover	, new Sprite("Tiles/" + m_tileSet + "hover.png"		, 1));
			m_spriteDict.Add(Tile.TileState.Pressed	, new Sprite("Tiles/" + m_tileSet + "pressed.png"	, 1));
			m_spriteDict.Add(Tile.TileState.Toggle	, new Sprite("Tiles/" + m_tileSet + "toggle.png"	, 1));

			m_tileMap = new Tile[width, height];
		}

		public void load() {
			foreach (Sprite l_sprite in m_spriteDict.Values) {
				l_sprite.load();
			}
			
			int l_heightIndex = MathManager.randomInt(3, 7);

			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					//m_tileMap[i, j] = new Tile(new Vector2(i, j), MathManager.randomInt(l_heightIndex - 3, l_heightIndex + 3));
					m_tileMap[i, j] = new Tile(new Vector2(i, j), 1, this);
					m_tileMap[i, j].load();
					if (MathManager.isEven(i)) {
						m_tileMap[i, j].move(new Vector2(0, 111));
					}
				}
			}
		}

		public Tile getTile(int x, int y) {
			if (x < 0 || x >= m_width || y < 0 || y >= m_height) {
				return null;
			}
			return m_tileMap[x, y];
		}

		public Tile getTile(Tile a_tile, Vector2 a_mapOffset) {
			return getTile((int)(a_tile.getMapPosition().X + a_mapOffset.X), (int)(a_tile.getMapPosition().Y + a_mapOffset.Y));
		}

		public Tile getTile(Vector2 a_position) {
			return getTile((int)a_position.X, (int)a_position.Y);
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

		public LinkedList<Tile> getRangeOfTiles(Tile a_tile, int a_range) {
			LinkedList<Tile> l_list1 = new LinkedList<Tile>();
			LinkedList<Tile> l_list2 = new LinkedList<Tile>();
			LinkedList<Tile> l_list3 = new LinkedList<Tile>();

			l_list3.AddLast(a_tile);

			for (int i = 0; i < a_range; i++) {
				l_list2 = l_list3;
				l_list3 = new LinkedList<Tile>();
				if (l_list2 != null && l_list2.Count > 0) {
					foreach (Tile l_tile in l_list2) {
						foreach (Tile l_tile2 in getSurroundingTiles(l_tile)) {
							if (!l_list1.Contains(l_tile2)) {
								l_list3.AddLast(l_tile2);
								l_list1.AddLast(l_tile2);
							}
						}
					}
				}
			}
			return l_list1;
		}

		public LinkedList<Tile> getSurroundingTiles(Tile a_tile) {
			int[] Xcheck = MathManager.isEven(a_tile.X) ? new[] { -1,  0,  1,  1,  0, -1 } : new[] {  1,  0,  1, -1,  0, -1 };
			int[] Ycheck = MathManager.isEven(a_tile.X) ? new[] {  1, -1,  0,  1,  1,  0 } : new[] { -1, -1,  0, -1,  1,  0 };

			LinkedList<Tile> l_list = new LinkedList<Tile>();

			Tile l_tile;
			for (int i = 0; i < Xcheck.Length; i++) {
				for (int j = 0; j < Xcheck.Length; j++) {
					if ((l_tile = getTile(a_tile, new Vector2(Xcheck[i], Ycheck[i]))) != null) {
						if (!l_list.Contains(l_tile)) {
							l_list.AddLast(l_tile);
						}
					}
				}
			}

			return l_list;
		}

		public Tile p_hover {
			get {
				return m_hoveredTile;
			}
			set {
				m_hoveredTile = value;
			}
		}

		public bool p_ignoreMouse {
			get {
				return m_ignoreMouse;
			}
			set {
				m_ignoreMouse = value;
			}
		}

		public void restoreStates() {
			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					m_tileMap[i, j].restoreState();
				}
			}
		}

		public LinkedList<Tile> toLinkedList(Tile.TileState a_state) {
			LinkedList<Tile> l_list = new LinkedList<Tile>();

			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					if (m_tileMap[i, j].p_tileState == a_state) {
						l_list.AddLast(m_tileMap[i, j]);
					}
				}
			}

			return l_list;
		}
	}
}

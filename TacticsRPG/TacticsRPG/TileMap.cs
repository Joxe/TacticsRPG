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
					//m_tileMap[i, j] = new Tile(new Vector2(i, j), MathManager.randomInt(t_heightIndex - 3, t_heightIndex + 3));
					m_tileMap[i, j] = new Tile(new Vector2(20, 20), 1);
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
			//try {
				return m_tileMap[x, y];
			//} catch (IndexOutOfRangeException) {
				//return null;
			//}
		}

		public Tile getTile(Tile a_tile, Vector2 a_mapOffset) {
			return getTile(a_tile.getMapPosition() + a_mapOffset);
		}

		public Tile getTile(Vector2 a_position) {
			if (a_position.X < 0 || a_position.X > m_width || a_position.Y < 0 || a_position.Y > m_height) {
				return null;	
			}
			return m_tileMap[(int)a_position.X, (int)a_position.Y];
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
			LinkedList<Tile> lista1 = new LinkedList<Tile>();
			LinkedList<Tile> lista2 = new LinkedList<Tile>();
			LinkedList<Tile> lista3 = new LinkedList<Tile>();

			lista3.AddLast(a_tile);

			for (int i = 0; i < a_range; i++) {
				lista2 = lista3;
				lista3 = new LinkedList<Tile>();
				if (lista2 != null && lista2.Count > 0) {
					foreach (Tile t_tile in lista2) {
						foreach (Tile t_tile2 in getSurroundingTiles(t_tile)) {
							if (!lista1.Contains(t_tile2)) {
								lista3.AddLast(t_tile2);
								lista1.AddLast(t_tile2);
							}
						}
					}
				}
			}
			return lista1;
		}

		public LinkedList<Tile> getSurroundingTiles(Tile a_tile) {
			int[] Xcheck = MathManager.isEven(a_tile.X) ? MathManager.evenX : MathManager.oddX;
			int[] Ycheck = MathManager.isEven(a_tile.X) ? MathManager.evenY : MathManager.oddY;

			LinkedList<Tile> t_list = new LinkedList<Tile>();

			Tile t_tile;
			for (int i = 0; i < Xcheck.Length; i++) {
				for (int j = 0; j < Xcheck.Length; j++) {
					if ((t_tile = getTile(a_tile, new Vector2(Xcheck[i], Ycheck[i]))) != null) {
						if (!t_list.Contains(t_tile)) {
							t_list.AddLast(t_tile);
						}
					}
				}
			}

			return t_list;
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

		public LinkedList<Tile> toLinkedList(Tile.TileState a_state) {
			LinkedList<Tile> t_list = new LinkedList<Tile>();

			for (int i = 0; i < m_width; i++) {
				for (int j = 0; j < m_height; j++) {
					if (m_tileMap[i, j].p_tileState == a_state) {
						t_list.AddLast(m_tileMap[i, j]);
					}
				}
			}

			return t_list;
		}
	}
}

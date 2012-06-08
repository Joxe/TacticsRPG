using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class Tile : TexturedObject {
		private const int TILE_WIDTH	= 225;
		private const int TILE_HEIGHT	= 222;
		public readonly int X;
		public readonly int Y;
		private int m_height;
		private Sprite[] m_heightSprites;
		private TileMap m_tileMap;
		private Tile m_tileAbove;
		private bool m_hoverOverToggle;
		private bool m_ignoreMouse;
		private Champion m_currentChampion;

		private TileState m_tileState;
		public enum TileState {
			Normal, Hover, Pressed, Toggle
		}

		public Tile(Vector2 a_position, int a_height) : base(a_position * new Vector2(TILE_WIDTH - 32, TILE_HEIGHT) - new Vector2(0, a_height * 40)) {
			X = (int)a_position.X;
			Y = (int)a_position.Y;
			m_layer = 0.500f - a_position.Y / 1000f;

			if (MathManager.isEven((int)a_position.X)) {
				m_layer -= 0.0001f;
			}

			m_tileState = TileState.Normal;
			m_color = Color.White;
			m_heightSprites = new Sprite[a_height];
			m_height = a_height;
			m_tileMap = ((GameState)Game.getInstance().getCurrentState()).getTileMap();

			for (int i = 0; i < a_height; i++) {
				m_heightSprites[i] = new Sprite("Tiles//" + m_tileMap.getTileSet() + "mellangrej", 1);
			}
		}

		public override void load() {
			for (int i = 0; i < m_heightSprites.Length; i++) {
				m_heightSprites[i].load();
				m_heightSprites[i].p_offset = new Vector2(0, (m_heightSprites[i].getTexture().Height * i / 3.5f) + TILE_HEIGHT / 2);
			}
			if (Y > 0) {
				m_tileAbove = m_tileMap.getTile(X, Y - 1);
			}
			m_hitbox = new Rectangle(m_position.X, m_position.Y, m_tileMap.getSpriteDict()[m_tileState].getTexture().Width, m_tileMap.getSpriteDict()[m_tileState].getTexture().Height);
			m_hitbox.setParent(this);
		}

		public override void update() {
			if (m_isInCamera = CameraHandler.isInCamera(this)) {
				m_hitbox.update();
				bool t_collided = CollisionManager.hexagonContains(this, MouseHandler.worldMouse(), TILE_WIDTH, TILE_HEIGHT);

				if (t_collided && !m_ignoreMouse) {
					m_tileMap.p_hover = this;
					if (m_tileAbove != null) {
						m_tileAbove.ignoreMouse(true);
					}
					switch (m_tileState) {
						case TileState.Normal:
							m_tileState = TileState.Hover;
							break;
						case TileState.Hover:
							if (MouseHandler.lmbDown()) {
								m_tileState = TileState.Pressed;
							}
							break;
						case TileState.Pressed:
							if (MouseHandler.lmbUp()) {
								m_tileState = TileState.Hover;
							}
							break;
						case TileState.Toggle:
							if (MouseHandler.lmbUp()) {
								m_tileState = TileState.Normal;
							}
							m_tileState = TileState.Hover;
							break;
					}
				} else {
					if (m_hoverOverToggle) {
						m_tileState = TileState.Toggle;
					} else {
						m_tileState = TileState.Normal;
					}
					if (m_tileAbove != null) {
						m_tileAbove.ignoreMouse(false);
					}
				}
			}
			base.update();
		}

		public override void draw() {
			if (m_isInCamera) {
				foreach (Sprite t_sprite in m_heightSprites) {
					t_sprite.draw(this, m_layer + 0.00001f);
				}
				m_tileMap.getSpriteDict()[m_tileState].draw(this);
			}
		}

		public TileMap p_tileMap {
			get {
				return m_tileMap;
			}
			set {
				m_tileMap = value;
			}
		}

		public Vector2 getMapPosition() {
			return new Vector2(X, Y);
		}

		public TileState p_tileState {
			get {
				if (m_hoverOverToggle) {
					return TileState.Toggle;
				} else {
					return m_tileState;
				}
			}
			set {
				if (value == TileState.Toggle) {
					m_hoverOverToggle = true;
				}
				m_tileState = value;
			}
		}

		public int p_height {
			get {
				return m_height;
			}
			set {
				m_height = value;
			}
		}

		public Champion p_champion {
			get {
				return m_currentChampion;
			}
			set {
				m_currentChampion = value;
			}
		}

		public void ignoreMouse(bool a_ignore) {
			m_ignoreMouse = a_ignore;
		}

		public override string ToString() {
			return base.ToString() + X + ", " + Y + "<" + m_tileState + ">";
		}

		public void restoreState() {
			m_hoverOverToggle = false;
			m_tileState = TileState.Normal;
		}

		public bool isObstructed() {
			return m_currentChampion != null;
		}
	}
}
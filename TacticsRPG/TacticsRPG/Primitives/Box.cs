using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class Box : TexturedObject {
		#region Members
		private Texture2D			m_boxTexture;
		private float				m_width;
		private float				m_height;
		private LinkedList<Line>	m_lineList;
		private Color				m_boxColor;
		private bool				m_worldBox;

		private Vector2 m_from;
		private Vector2 m_to;
		private Rectangle m_bounds;
		private float m_timer;
		private float m_timeStart;
		#endregion

		#region Constructor & Load
		public Box(Vector2 a_position, float a_width, float a_height, Color a_color, bool a_worldBox)
			: base(a_position)
		{
			m_boxTexture	= new Texture2D(Game.getInstance().GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			if (!a_worldBox) {
				m_parent = Game.getInstance().m_camera;
			}
			m_lineList = new LinkedList<Line>();
			m_boxColor		= a_color;
			m_width			= a_width;
			m_height		= a_height;
			m_worldBox		= a_worldBox;
			m_layer			= 0.110f;
			m_bounds		= new Rectangle(a_position.X, a_position.Y, a_width, a_height);
			m_boxTexture.SetData(new[] { a_color });
		}

		public Box(Vector2 a_position, float a_width, float a_height, Color a_color, Color a_lineColor, int a_lineWidth, bool a_worldBox)
			: base(a_position)
		{
			m_boxTexture	= new Texture2D(Game.getInstance().GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			m_boxTexture.SetData(new[] { a_color });
			if (!a_worldBox) {
				m_parent = Game.getInstance().m_camera;
			}
			m_boxColor		= a_color;
			m_width			= a_width;
			m_height		= a_height;
			m_worldBox		= a_worldBox;
			m_layer			= 0.110f;
			m_bounds = new Rectangle(a_position.X, a_position.Y, a_width, a_height);
			
			Vector2 topLeft = a_position;
			Vector2 topRight = a_position;
			topRight.X += a_width;
			Vector2 btmRight = topRight;
			btmRight.Y += a_height;
			Vector2 btmLeft = btmRight;
			btmLeft.X = topLeft.X;

			m_lineList = new LinkedList<Line>();
			if (a_worldBox) {
				m_lineList.AddLast(new Line(topLeft,	topRight, Vector2.Zero, Vector2.Zero, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(topRight,	btmRight, Vector2.Zero, Vector2.Zero, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(btmRight,	btmLeft	, Vector2.Zero, Vector2.Zero, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(btmLeft,	topLeft	, Vector2.Zero, Vector2.Zero, a_lineColor, a_lineWidth, a_worldBox));
			} else {
				Vector2 t_camera = Game.getInstance().m_camera.p_position;
				Vector2 t_halfResolution = Game.getInstance().getResolution() / 2;
				m_lineList.AddLast(new Line(t_camera, t_camera, topLeft	 - t_halfResolution, topRight - t_halfResolution, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(t_camera, t_camera, topRight - t_halfResolution, btmRight - t_halfResolution, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(t_camera, t_camera, btmRight - t_halfResolution, btmLeft  - t_halfResolution, a_lineColor, a_lineWidth, a_worldBox));
				m_lineList.AddLast(new Line(t_camera, t_camera, btmLeft	 - t_halfResolution, topLeft  - t_halfResolution, a_lineColor, a_lineWidth, a_worldBox));
			}
		}
		#endregion

		#region Update & Draw
		public override void update() {
			if (m_timer >= (float)Game.getInstance().getGameTime().TotalGameTime.TotalMilliseconds) {
				float t_moveDelta = ((float)Game.getInstance().getGameTime().TotalGameTime.TotalMilliseconds - m_timeStart) / (m_timer - m_timeStart);
				Vector2 tLerp = Vector2.Lerp(m_from, m_to, t_moveDelta);
				p_position = tLerp;
			} else if (m_timer > 0) {
				p_position = m_to;
				m_timer = 0;
			}
		}

		public override void draw() {
			if (m_worldBox) {
				Game.getInstance().m_spriteBatch.Draw(m_boxTexture, m_position, null, m_boxColor, 0.0f, Vector2.Zero, new Vector2(m_width, m_height), SpriteEffects.None, m_layer);
				
				if (m_lineList != null && m_lineList.Count > 0) {
					foreach (Line t_line in m_lineList) {
						t_line.draw();
					}
				}
			} else {
				float t_zoom = Game.getInstance().m_camera.p_zoom;
				Vector2 t_cartCoord;
				t_cartCoord = m_position / t_zoom + Game.getInstance().m_camera.p_position;
				
				Game.getInstance().m_spriteBatch.Draw(m_boxTexture, t_cartCoord, null, m_boxColor, 0.0f, Vector2.Zero, new Vector2(m_width / t_zoom, m_height / t_zoom), SpriteEffects.None, m_layer);
				
				if (m_lineList != null && m_lineList.Count > 0) {
					foreach (Line t_line in m_lineList) {
						t_line.draw();
					}
				}
			}
		}
		#endregion

		#region Box Methods
		public bool contains(Vector2 a_position) {
			return m_bounds.contains(a_position);
		}

		public override Rectangle getHitBox() {
			if (m_bounds.Width == 0 || m_bounds.Height == 0) {
				m_bounds.Width = m_width;
				m_bounds.Height = m_height;
			}
			return m_bounds;
		}

		public void setLineColor(Color a_color) {
			foreach (Line t_line in m_lineList) {
				t_line.setColor(a_color);
			}
		}

		public float getHeight() {
			return m_height;
		}

		public float getWidth() {
			return m_width;
		}

		public void setMove(Vector2 a_from, Vector2 a_to, GameTime a_gameTime,float a_timer) {
			m_to = a_to;
			m_from = a_from;
			m_timer = (float)a_gameTime.TotalGameTime.TotalMilliseconds + a_timer * 1000;
			m_timeStart = (float)a_gameTime.TotalGameTime.TotalMilliseconds;
		}
		#endregion
	}
}
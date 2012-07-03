using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class Line : TexturedObject {
		#region Members
		private Texture2D m_lineTexture;
		private Vector2 m_startPosition;
		private Vector2 m_endPosition;
		private Color m_lineColor;
		private Vector2 m_startOffset;
		private Vector2 m_endOffset;
		private int m_width;
		private bool m_worldLine;
		private bool m_hasTexture;
		#endregion
		
		#region Constructor & Load
		public Line(Vector2 a_startPosition, Vector2 a_endPosition, Vector2 a_startOffset, Vector2 a_endOffset, Color a_color, int a_width, bool a_worldLine)
			: base(a_startPosition)
		{
			m_startOffset	= a_startOffset;
			m_endOffset		= a_endOffset;
			m_lineColor		= a_color;
			m_width			= a_width;
			m_worldLine		= a_worldLine;
			m_layer			= 0.010f;
			m_lineTexture = new Texture2D(Game.getInstance().GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
			m_lineTexture.SetData(new[] { a_color });

			if (!a_worldLine) {
				m_parent = Game.getInstance().m_camera;
			}
		}
		#endregion

		#region Start- & Endpoint Methods

		public void setTexture(string a_path) {
			float l_length = Vector2.Distance(m_startPosition, m_endPosition); 

			Texture2D l_texture = Game.getInstance().Content.Load<Texture2D>(a_path);
			Color[] l_colorArray = new Color[l_texture.Width * l_texture.Height];
			l_texture.GetData(l_colorArray);

			m_lineTexture = new Texture2D(Game.getInstance().GraphicsDevice, l_texture.Width, (int)l_length, false, SurfaceFormat.Color);
			Color[] l_lineTexture = new Color[m_lineTexture.Width * (int)l_length];
			m_lineTexture.GetData(l_lineTexture);

			for (int i = 0, j = 0; i < l_lineTexture.Length; i++, j++) {
				if (j >= l_colorArray.Length) {
					j = 0;
				}
				l_lineTexture[i] = l_colorArray[j];
			}

			m_lineTexture.SetData(l_lineTexture);
			m_hasTexture = true;
		}

		public void updateTexture() {
			float l_length = Vector2.Distance(m_startPosition, m_endPosition);			
			Color[] l_colorArray = new Color[m_lineTexture.Width * m_lineTexture.Height];
			m_lineTexture.GetData(l_colorArray);

			m_lineTexture = new Texture2D(Game.getInstance().GraphicsDevice, m_lineTexture.Width, (int)l_length, false, SurfaceFormat.Color);
			Color[] l_lineTexture = new Color[m_lineTexture.Width * (int)l_length];
			m_lineTexture.GetData(l_lineTexture);

			for (int i = 0, j = 0; i < l_lineTexture.Length; i++, j++) {
				if (j >= l_colorArray.Length) {
					j = 0;
				}
				l_lineTexture[i] = l_colorArray[j];
			}

			m_lineTexture.SetData(l_lineTexture);
			m_hasTexture = true;
		}

		public Vector2 getEndPoint() {
			return m_endPosition;
		}

		public Vector2 getStartPoint() {
			return m_startPosition;
		}

		public void setEndPoint(Vector2 a_endPoint) {
			m_endPosition = a_endPoint + m_endOffset;
		}

		public void setEndPoint(Vector2 a_endPoint, Vector2 a_endOffset) {
			m_endPosition = a_endPoint + a_endOffset;
		}

		public void setStartPoint(Vector2 a_startPoint) {
			m_startPosition = a_startPoint + m_startOffset;
		}

		public void setOffset(Vector2 a_offset) {
			m_startOffset = a_offset;
			m_endPosition = a_offset;
		}

		public void setXOffset(float a_offset) {
			m_startPosition.X = a_offset;
			m_endPosition.Y = a_offset;
		}

		public void setYOffset(float a_offset) {
			m_startPosition.Y = a_offset;
			m_endPosition.Y = a_offset;
		}
		#endregion

		#region Draw
		public override void draw()
		{
			float l_angle = (float)Math.Atan2(m_endPosition.Y - m_startPosition.Y, m_endPosition.X - m_startPosition.X);
			float l_length = Vector2.Distance(m_startPosition, m_endPosition);
			Rectangle drawRect = new Rectangle(m_startPosition.X + m_startOffset.X, m_startPosition.Y + m_startOffset.Y, m_width, l_length);

			if (m_worldLine) {
				if (m_hasTexture) {
					Game.getInstance().m_spriteBatch.Draw(m_lineTexture, m_startPosition, null, m_lineColor, l_angle - (float)(Math.PI / 2), Vector2.Zero, new Vector2(1, 1), SpriteEffects.None, m_layer);
				} else {
					Game.getInstance().m_spriteBatch.Draw(m_lineTexture, m_startPosition, null, m_lineColor, l_angle, Vector2.Zero, new Vector2(l_length, m_width), SpriteEffects.None, m_layer);
				}
			} else {
				float l_zoom = Game.getInstance().m_camera.p_zoom;	
				Vector2 l_cartCoord = m_startPosition / l_zoom + Game.getInstance().m_camera.p_position;

				Game.getInstance().m_spriteBatch.Draw(m_lineTexture, l_cartCoord, null, m_lineColor, l_angle, Vector2.Zero, new Vector2(l_length / l_zoom, m_width / l_zoom), SpriteEffects.None, m_layer);
			}
		}
		#endregion

		public void setColor(Color a_color) {
			m_lineTexture.SetData(new[] { a_color });
		}
	}
}
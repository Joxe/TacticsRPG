using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class Text : GuiObject {
		#region Members
		private SpriteFont m_spriteFont;
		private string m_font;
		private string m_text;
		private bool m_worldFont;
		#endregion

		#region Constructor & Load
		// Summary:
		//	Class for writing on-screen Text with SpriteBath.DrawString()
		//	Call load() after creation
		public Text(Vector2 a_position, string a_text, string a_spriteFont, Color a_color, bool a_worldFont)
			: base(a_position)
		{
			if (a_worldFont) {
				m_position	= a_position - Game.getInstance().getResolution() / 2;
				p_parentOffset = Vector2.Zero;
			}
			m_worldFont		= a_worldFont;
			m_text			= a_text;
			m_font			= a_spriteFont;
			m_color			= a_color;
			m_worldFont		= a_worldFont;
		}

		public override void load() {
			if (!m_worldFont) {
				m_parent = Game.getInstance().m_camera;
			} 
			m_spriteFont = Game.getInstance().Content.Load<SpriteFont>("Fonts\\" + m_font);
		}
		#endregion

		#region Text-related Methods
		public void erase(int a_charsToErase) {
			m_text = m_text.Remove(m_text.Length - a_charsToErase);
		}

		public void addText(char a_char) {
			m_text += a_char;
		}

		public void addText(string a_string) {
			m_text += a_string;
		}

		public Vector2 measureString() {
			return m_spriteFont.MeasureString(m_text);
		}
		#endregion

		#region Position-methods
		public string p_text {
			get {
				return m_text;
			}
			set {
				m_text = value;
			}
		}
		
		public SpriteFont p_font {
			get {
				return m_spriteFont;			
			}
			set {
				m_spriteFont = value;
			}
		}

		public override Rectangle getHitBox() {
			if (m_worldFont) {
				return new Rectangle(m_position.X, m_position.Y, measureString().X, measureString().Y);
			} else {
				return new Rectangle(m_parentOffset.X, m_parentOffset.Y, measureString().X, measureString().Y);
			}
		}
		#endregion

		#region Update & Draw
		public override void draw() {
			if (m_text == null || !m_visible) {
				return;
			}

			if (m_worldFont) {
				Game.getInstance().m_spriteBatch.DrawString(m_spriteFont, m_text, m_position, m_color);
			} else {	
				float t_zoom = Game.getInstance().m_camera.p_zoom;

				try {
					Game.getInstance().m_spriteBatch.DrawString(m_spriteFont, m_text, m_position + p_parentOffset / t_zoom, m_color, m_rotation, Vector2.Zero, 1.0f / t_zoom, SpriteEffects.None, m_layer);
				} catch (System.ArgumentException) {
					erase(1);
				}
			}
		}
		#endregion
	}
}
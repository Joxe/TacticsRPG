using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG
{
	public class Sprite {
		private int			m_frames;
		private string		m_file;
		private Texture2D	m_texture;
		private int m_animationHeight;
		private int m_animationWidth;
		private int m_animationFrame;
		private Vector2 m_offset;

		public Sprite(string a_file, int a_frames) {
			m_file = a_file;
			m_frames = a_frames;
		}

		public void destroy() {
			m_texture = null;
		}

		public void load() {
			m_texture = ContentLoader.loadTexture("Images/Sprites/" + m_file);
			m_animationHeight = m_texture.Height;
			m_animationWidth = m_texture.Width;
			m_animationFrame = 0;
		}

		public Texture2D getTexture() {
			return m_texture;
		}

		public void draw(TexturedObject a_gameObject) {
			Game.getInstance().m_spriteBatch.Draw(
				m_texture,
				new Rectangle(a_gameObject.p_position.X, a_gameObject.p_position.Y, m_animationWidth, m_animationHeight).toXNARectangle(),
				new Rectangle(m_animationWidth * m_animationFrame, 0, m_animationWidth, m_animationHeight).toXNARectangle(),
				a_gameObject.p_color,
				a_gameObject.p_rotation,
				-m_offset,
				a_gameObject.p_spriteEffect,
				a_gameObject.p_layer
			);
		}

		public void draw(TexturedObject a_gameObject, float a_layer) {
			Game.getInstance().m_spriteBatch.Draw(
				m_texture,
				new Rectangle(a_gameObject.p_position.X, a_gameObject.p_position.Y, m_animationWidth, m_animationHeight).toXNARectangle(),
				new Rectangle(m_animationWidth * m_animationFrame, 0, m_animationWidth, m_animationHeight).toXNARectangle(),
				a_gameObject.p_color,
				a_gameObject.p_rotation,
				-m_offset,
				a_gameObject.p_spriteEffect,
				a_layer
			);
		}

		public int getWidth() {
			return m_animationWidth;
		}

		public int getHeight() {
			return m_animationHeight;
		}

		public Vector2 getSize() {
			return new Vector2(m_animationWidth, m_animationHeight);
		}

		public Vector2 p_offset {
			get {
				return m_offset;
			}
			set {
				m_offset = value;
			}
		}

		public int p_frames {
			get {
				return m_frames;
			}
			set {
				m_frames = value;
			}
		}

		public int getCurrentFrame() {
			return m_animationFrame;
		}
	}
}

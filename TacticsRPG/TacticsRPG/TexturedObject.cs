using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class TexturedObject : GameObject {
		protected Color			m_color;
		protected float			m_rotation;
		protected SpriteEffects m_spriteEffect;
		protected float			m_scale;
		protected Sprite		m_sprite;
		protected float			m_layer;
		
		public TexturedObject(Vector2 a_position) : base(a_position) {
			m_color = Color.White;
			m_rotation = 0.0f;
			m_spriteEffect = SpriteEffects.None;
			m_scale = 1.0f;
		}

		public override void load() {
			m_hitbox = new Rectangle(m_position.X, m_position.Y, m_sprite.getWidth(), m_sprite.getHeight());
			base.load();
		}

		public Color p_color {
			get {
				return m_color;
			}
			set {
				m_color = value;
			}
		}

		public float p_rotation {
			get {
				return m_rotation;
			}
			set {
				m_rotation = value;
			}
		}

		public SpriteEffects p_spriteEffect {
			get {
				return m_spriteEffect;
			}
			set {
				m_spriteEffect = value;
			}
		}

		public float p_scale {
			get {
				return m_scale;
			}
			set {
				m_scale = value;
			}
		}

		public Sprite p_sprite {
			get {
				return m_sprite;
			}
			set {
				m_sprite = value;
			}
		}

		public float p_layer {
			get {
				return m_layer;
			}
			set {
				m_layer = value;
			}
		}
	}
}
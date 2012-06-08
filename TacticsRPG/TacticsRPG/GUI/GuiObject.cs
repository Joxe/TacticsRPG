using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class GuiObject : TexturedObject {
		protected bool m_visible;
		protected Rectangle m_bounds;

		public GuiObject(Vector2 a_position) : base(a_position) {
			m_visible = true;
			m_parent = Game.getInstance().m_camera;
			m_parentOffset = a_position;
			m_layer = 0.001f;
			m_bounds = new Rectangle(a_position.X + m_parentOffset.X, a_position.Y + m_parentOffset.Y, 1, 1);
		}

		public override void update() {
			m_position = m_parent.p_position;
		}

		public virtual Vector2 p_screenPosition {
			get {
				return m_parentOffset;
			}
			set {
				m_parentOffset = value - Game.getInstance().getResolution() / 2;
			}
		}

		public virtual bool p_visible {
			get {
				return m_visible;
			}
			set {
				m_visible = value;
			}
		}

		public bool contains(Vector2 a_position) {
			return m_bounds.contains(a_position);
		}
	}
}

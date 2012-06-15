using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class GameObject : IComparable<GameObject> {
		protected Vector2		m_position;
		protected Vector2		m_parentOffset;
		protected GameObject	m_parent;
		protected Rectangle		m_hitbox;
		protected bool			m_isInCamera;

		public GameObject(Vector2 a_position) {
			m_position = a_position;
		}

		public virtual void load() {

		}

		public virtual void unload() {

		}

		public virtual void update() {
			if (m_parent != null) {
				m_position = m_parent.p_position;
			}
		}

		public virtual void draw() {

		}

		public virtual Vector2 p_position {
			get {
				return m_position;
			}
			set {
				m_position = value;
			}
		}

		public virtual Vector2 p_parentOffset {
			get {
				return m_parentOffset;
			}
			set {
				m_parentOffset = value;
			}
		}

		public virtual bool p_isInCamera {
			get {
				return m_isInCamera;
			}
			set {
				m_isInCamera = value;
			}
		}

		public virtual void setParent(GameObject a_parent) {
			m_parent = a_parent;
		}

		public virtual void move(Vector2 a_distance) {
			m_position += a_distance;
		}

		public virtual Rectangle getHitBox() {
			if (m_hitbox != null) {
				return m_hitbox;
			} else {
				return new Rectangle(m_position.X, m_position.Y, 1, 1);
			}
		}

		public virtual int CompareTo(GameObject a_gameObject) {
			return 0;
		}
	}
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class Rectangle {
		public float X;
		public float Y;
		public float Width;
		public float Height;
		private GameObject m_parent;

		public Rectangle(float x, float y, float width, float height) {
			X = x;
			Y = y;
			Width = width;
			Height = height;
		}

		public void setParent(GameObject a_gameObject) {
			m_parent = a_gameObject;
		}

		public void update() {
			if (m_parent != null) {
				X = m_parent.p_position.X;
				Y = m_parent.p_position.Y;
			}
		}

		public bool contains(Vector2 a_point) {
			return a_point.X > X 
				&& a_point.X < X + Width 
				&& a_point.Y > Y
				&& a_point.Y < Y + Height;
		}

		public bool contains(Rectangle a_rectangle) {
			return (a_rectangle.X > X && a_rectangle.X < X + Width)
				|| (a_rectangle.X < X + Width && a_rectangle.X > X)
				|| (a_rectangle.Y > Y && a_rectangle.Y < Y + Height)
				|| (a_rectangle.Y < Y + Height && a_rectangle.Y > Y);
		}

		public override string ToString() {
			return p_coordinates + " | " + p_dimensions;
		}

		public Microsoft.Xna.Framework.Rectangle toXNARectangle() {
			return new Microsoft.Xna.Framework.Rectangle((int)X, (int)Y, (int)Width, (int)Height);
		}

		public static Microsoft.Xna.Framework.Rectangle toXNARectangle(Rectangle a_rectangle) {
			return new Microsoft.Xna.Framework.Rectangle((int)a_rectangle.X, (int)a_rectangle.Y, (int)a_rectangle.Width, (int)a_rectangle.Height);
		}

		public Vector2 p_coordinates {
			get {
				return new Vector2(X, Y);
			}
			set {
				X = value.X;
				Y = value.Y;
			}
		}

		public Vector2 p_dimensions {
			get {
				return new Vector2(Width, Height);
			}
			set {
				Width = value.X;
				Height = value.Y;
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class Rectangle : GameObject {
		public float X;
		public float Y;
		public float Width;
		public float Height;

		public Rectangle(float x, float y, float width, float height) : base(new Vector2(x, y)) {
			Width = width;
			Height = height;
		}

		public override void load() {
			X = m_position.X;
			Y = m_position.Y;
			base.load();
		}

		public bool contains(Vector2 a_point) {
			return a_point.X > m_position.X 
				&& a_point.X < m_position.X + Width 
				&& a_point.Y > m_position.Y
				&& a_point.Y < m_position.Y + Height;
		}

		public bool contains(Rectangle a_rectangle) {
			return (a_rectangle.X > m_position.X && a_rectangle.X < m_position.X + Width)
				|| (a_rectangle.X < m_position.X + Width && a_rectangle.X > m_position.X)
				|| (a_rectangle.Y > m_position.Y && a_rectangle.Y < m_position.Y + Height)
				|| (a_rectangle.Y < m_position.Y + Height && a_rectangle.Y > m_position.Y);
		}

		public override string ToString() {
			return p_coordinates + " | " + p_dimensions;
		}

		public Microsoft.Xna.Framework.Rectangle toXNARectangle() {
			return new Microsoft.Xna.Framework.Rectangle((int)m_position.X, (int)m_position.Y, (int)Width, (int)Height);
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

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class Camera : GameObject {
		private float m_zoom;
		private float m_rotation;
		private Rectangle m_cameraBox; //TODO returnera inte en ny rektangel hela tiden

		public Camera(Vector2 a_position) : base(a_position) {
			m_zoom = 1.0f;
			m_rotation = 0.0f;
		}

		public override void load() {
			m_cameraBox = new Rectangle(m_position.X - Game.getInstance().getResolution().X / 2, m_position.Y - Game.getInstance().getResolution().Y / 2, 5000, 5000);
			//m_cameraBox = new Rectangle(m_position.X - Game.getInstance().getResolution().X / 2, m_position.Y - Game.getInstance().getResolution().Y / 2, Game.getInstance().getResolution().X * 2, Game.getInstance().getResolution().Y * 2);
		}

		public override void update() {
			base.update();
		}

		public float getZoom() {
			return m_zoom;
		}

		public float getRotation() {
			return m_rotation;
		}

		public void setRotation(float a_rotation) {
			m_rotation = a_rotation;
		}

		public void setPosition(Vector2 a_posV2) {
			m_position = a_posV2;
			m_cameraBox.X = a_posV2.X - Game.getInstance().getResolution().X;
			m_cameraBox.Y = a_posV2.Y - Game.getInstance().getResolution().Y;
		}

		public Matrix getTransformation(GraphicsDevice a_gd) {
			return Matrix.CreateTranslation(
				new Vector3(-m_position.X, -m_position.Y, 0)) 
				* Matrix.CreateRotationZ(m_rotation) 
				* Matrix.CreateScale(new Vector3(m_zoom, m_zoom, 1)) 
				* Matrix.CreateTranslation(new Vector3(Game.getInstance().getResolution().X * 0.5f, Game.getInstance().getResolution().Y * 0.5f, 0)
			);
		}

		public void printInfo() {
			System.Console.WriteLine(m_cameraBox.ToString());
		}

		public Rectangle getRectangle() {
			return m_cameraBox;
			//return new Rectangle(m_position.X - Game.getInstance().getResolution().X, m_position.Y - Game.getInstance().getResolution().Y, Game.getInstance().getResolution().X * 2, Game.getInstance().getResolution().Y * 2);
		}

		public float p_zoom {
			get {
				return m_zoom;
			}
			set {
				m_zoom = Math.Max(value, 0.1f);
			}
		}
	}
}
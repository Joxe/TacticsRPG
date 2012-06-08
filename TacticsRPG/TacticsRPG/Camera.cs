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
			//m_cameraBox = new Rectangle(m_position.X - Game.getInstance().getResolution().X / 2, m_position.Y - Game.getInstance().getResolution().Y / 2, 5000, 5000);
			m_cameraBox = new Rectangle(m_position.X - Game.getInstance().getResolution().X / 2, m_position.Y - Game.getInstance().getResolution().Y / 2, Game.getInstance().getResolution().X * 2, Game.getInstance().getResolution().Y * 2);
		}

		public override void update() {
			m_cameraBox.p_dimensions = (Game.getInstance().getResolution() * 2) / m_zoom;
			m_cameraBox.p_coordinates = this.p_position - m_cameraBox.p_dimensions / 2;
		}

		public void setPosition(Vector2 a_posV2) {
			m_position = a_posV2;
			//m_cameraBox.p_coordinates = a_posV2 - Game.getInstance().getResolution();
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
		}

		public float p_zoom {
			get {
				return m_zoom;
			}
			set {
				m_zoom = Math.Max(value, 0.1f);
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
	}
}
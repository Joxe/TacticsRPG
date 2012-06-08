using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace TacticsRPG {
	public class Game : Microsoft.Xna.Framework.Game {
		private static Game m_game;

		public GraphicsDeviceManager m_graphics;
		public SpriteBatch m_spriteBatch;
		public Camera m_camera;
		
		private GameTime m_currentGameTime;
		private GameTime m_previousGameTime;
		private State m_currentState;
		private State m_previousState;

		public static Game getInstance() {
			if (m_game != null) {
				return m_game;
			} else {
				return m_game = new Game();
			}
		}

		private Game() {
			m_graphics = new GraphicsDeviceManager(this);
			Content.RootDirectory = "Content";
			IsMouseVisible = true;
		}

		protected override void Initialize() {
			m_currentState = new GameState();
			Loader.genGraphSettings("settings");
			base.Initialize();
		}

		protected override void LoadContent() {
			m_camera = new Camera(Vector2.Zero);
			m_camera.load();
			m_camera.p_zoom = Game.getInstance().getResolution().Y / 720;
			m_spriteBatch = new SpriteBatch(GraphicsDevice);
			m_currentState.load();
		}

		protected override void UnloadContent() {
			
		}

		protected override void Update(GameTime a_gameTime) {
			if (!IsActive) {
				return;
			}
			KeyboardHandler.setCurrentKeyboard();
			MouseHandler.setCurrentMouse();
			m_currentGameTime = a_gameTime;

			if (KeyboardHandler.keyPressed(Keys.Escape)) {
				this.Exit();
			}

			m_currentState.update();
			m_camera.update();
			base.Update(a_gameTime);

			m_previousGameTime = m_currentGameTime;
			KeyboardHandler.setPreviousKeyboard();
			MouseHandler.setPreviousMouse();
		}

		protected override void Draw(GameTime a_gameTime) {
			GraphicsDevice.Clear(Color.CornflowerBlue);
			m_spriteBatch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, null, null, null, null, m_camera.getTransformation(m_graphics.GraphicsDevice));
			m_currentState.draw();
			m_spriteBatch.End();
			base.Draw(a_gameTime);
		}

		public Vector2 getResolution() {
			return new Vector2(m_graphics.PreferredBackBufferWidth, m_graphics.PreferredBackBufferHeight);
		}

		public State getCurrentState() {
			return m_currentState;
		}

		public GameTime getGameTime() {
			return m_currentGameTime;
		}
	}
}
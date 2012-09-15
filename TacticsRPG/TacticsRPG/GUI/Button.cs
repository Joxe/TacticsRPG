using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace TacticsRPG {
	public class Button : GuiObject {
		#region Members
		public delegate void clickDelegate(Button a_button);
		public event clickDelegate m_clickEvent;

		protected Text m_text;

		private string m_buttonTexture;
		private Color m_textColor = Color.Black;
		private Vector2 m_textOffset = Vector2.Zero;
		private Texture2D m_normalTexture;
		private Texture2D m_hoverTexture;
		private Texture2D m_pressedTexture;
		private Texture2D m_toggleTexture;
		private Sound m_upSound = null;
		private Sound m_downSound = null;
		private Keys[] m_hotkey;

		protected State m_currentState = State.Normal;
		public enum State {
			Normal,	Hover, Pressed, Toggled, Disabled
		}
		#endregion

		#region Constructor & Load
		//Summary
		//	Constructor only for use with its childclass, TextButton
		protected Button(Vector2 a_screenPosition) : base(a_screenPosition) { }

		//Summary
		//	Class for creating an on-screen Button
		public Button(string a_buttonTexture, Vector2 a_screenPosition)
			: base(a_screenPosition)
		{
			m_buttonTexture = a_buttonTexture;
			LuaParser.registerMethod("updateButton", this, "update");
			LuaParser.registerMethod("drawButton", this, "draw");
		}

		//Summary
		//	Class for creating an on-screen Button with a Text
		public Button(string a_buttonTexture, Vector2 a_screenPosition, string a_buttonText, string a_textFont, Color a_textColor, Vector2 a_textOffset)
			: base(a_screenPosition)
		{
			m_text = new Text(a_textOffset, a_buttonText, a_textFont, a_textColor, false);
			m_text.setParent(this);
			m_buttonTexture = a_buttonTexture;
		}

		public override void load() {
			if (m_buttonTexture != null) {
				setNormalTexture(ContentLoader.loadTexture("Images/GUI/" + m_buttonTexture + "_normal.png"));
				setHoverTexture(ContentLoader.loadTexture("Images/GUI/" + m_buttonTexture + "_hover.png"));
				setPressedTexture(ContentLoader.loadTexture("Images/GUI/" + m_buttonTexture + "_pressed.png"));
				setToggleTexture(ContentLoader.loadTexture("Images/GUI/" + m_buttonTexture + "_toggle.png"));
			} else {
				setNormalTexture(ContentLoader.loadTexture("Images/GUI/DevelopmentHotkeys/btn_selecl_hotkey_normal.png"));
				setHoverTexture(ContentLoader.loadTexture("Images/GUI/DevelopmentHotkeys/btn_selecl_hotkey_hover.png"));
				setPressedTexture(ContentLoader.loadTexture("Images/GUI/DevelopmentHotkeys/btn_selecl_hotkey_pressed.png"));
				setToggleTexture(ContentLoader.loadTexture("Images/GUI/DevelopmentHotkeys/btn_selecl_hotkey_toggle.png"));
			}
			updateBounds();
			updateTextureBounds();
		}
		#endregion
		
		#region Update & Draw
		public override void update() {
			if (!m_visible || m_currentState == State.Disabled) {
				return;
			}
			updateBounds();
			updateMouse();
			updateKeyboard();
		}

		protected virtual void updateBounds() {
			m_bounds.p_coordinates = m_position + m_parentOffset;
			if (m_text != null) {
				m_text.p_parentOffset = m_bounds.p_coordinates + m_textOffset;
			}
		}

		protected virtual void updateMouse() {
			if (m_bounds.contains(MouseHandler.worldMouse())) {
				m_currentState = State.Hover;
				if (MouseHandler.lmbDown() && m_currentState != State.Pressed) {
					playDownSound();
					m_currentState = State.Pressed;
				}
				if (m_currentState != State.Pressed && MouseHandler.lmbUp()) {
					playUpSound();
					m_currentState = State.Hover;
					if (m_clickEvent != null) {
						m_clickEvent(this);
					}
				}
			} else {
				m_currentState = State.Normal;
			}
		}

		protected virtual void updateKeyboard() {
			if (hotkeyPressed()) {
				playDownSound();
				m_currentState = State.Pressed;
				if (m_clickEvent != null) {
					m_clickEvent(this);
				}
			}
		}

		public override void draw() {
			if (!m_visible) {
				return;
			}
			float l_zoom = Game.getInstance().m_camera.p_zoom;
			Vector2 l_cartCoord = m_position / l_zoom;
			Vector2 l_zoomedScale = new Vector2(1.0f / l_zoom, 1.0f / l_zoom);

			switch (m_currentState) {
				case State.Pressed:
					Game.getInstance().m_spriteBatch.Draw(m_pressedTexture, l_cartCoord, null, Color.White, 0.0f, Vector2.Zero, l_zoomedScale, SpriteEffects.None, m_layer);
					break;
				case State.Hover:
					Game.getInstance().m_spriteBatch.Draw(m_hoverTexture, l_cartCoord, null, Color.White, 0.0f, Vector2.Zero, l_zoomedScale, SpriteEffects.None, m_layer);
					break;
				case State.Toggled:
					Game.getInstance().m_spriteBatch.Draw(m_toggleTexture, l_cartCoord, null, Color.White, 0.0f, Vector2.Zero, l_zoomedScale, SpriteEffects.None, m_layer);
					break;
				case State.Normal:
					Game.getInstance().m_spriteBatch.Draw(m_normalTexture, l_cartCoord, null, Color.White, 0.0f, Vector2.Zero, l_zoomedScale, SpriteEffects.None, m_layer);
					break;
				case State.Disabled:
					Game.getInstance().m_spriteBatch.Draw(m_normalTexture, l_cartCoord, null, Color.White, 0.0f, Vector2.Zero, l_zoomedScale, SpriteEffects.None, m_layer);
					break;
			}
			if (m_text != null) {
				m_text.draw();
			}
		}
		#endregion

		#region Button-methods
		public virtual void setDownSound(string a_name) {
			m_downSound = new Sound(a_name);
		}

		public virtual void playDownSound() {
			if (m_downSound != null) {
				m_downSound.play();
			}
		}

		public virtual void setUpSound(string a_name) {
			m_upSound = new Sound(a_name);
		}

		public virtual void playUpSound() {
			if (m_upSound != null) {
				m_upSound.play();
			}
		}

		public virtual bool isButtonPressed() {
			return m_currentState == State.Pressed;
		}
		
		private void setNormalTexture(Texture2D a_texture) {
			m_normalTexture = a_texture;
		}

		private void setHoverTexture(Texture2D a_texture) {
			m_hoverTexture = a_texture;
		}

		private void setPressedTexture(Texture2D a_texture) {
			m_pressedTexture = a_texture;
		}

		private void setToggleTexture(Texture2D a_texture) {
			m_toggleTexture = a_texture;
		}

		public void setHotkey(Keys[] a_key, clickDelegate a_method) {
			m_clickEvent = new Button.clickDelegate(a_method);
			m_hotkey = a_key;
		}

		protected bool hotkeyPressed() {
			if (m_hotkey == null) {
				return false;
			}
			Keys[] l_keys = KeyboardHandler.getPressedKeys(); 
			foreach (Keys l_key in m_hotkey) {
				if (l_keys.Contains(l_key)) {
					continue;
				}
				return false;
			}

			bool l_returnValue = true;
			if (!m_hotkey.Contains(Keys.LeftShift) && !m_hotkey.Contains(Keys.RightShift)) {
				l_returnValue = !KeyboardHandler.keyPressed(Keys.LeftShift) && !KeyboardHandler.keyPressed(Keys.LeftShift);
			}
			if (!m_hotkey.Contains(Keys.LeftControl) && !m_hotkey.Contains(Keys.RightControl) && l_returnValue) {
				l_returnValue = !KeyboardHandler.keyPressed(Keys.LeftControl) && !KeyboardHandler.keyPressed(Keys.RightControl);
			}
			if (!m_hotkey.Contains(Keys.LeftAlt) && !m_hotkey.Contains(Keys.RightAlt) && l_returnValue) {
				l_returnValue = !KeyboardHandler.keyPressed(Keys.LeftAlt) && !KeyboardHandler.keyPressed(Keys.RightAlt);
			}
			return l_returnValue;
		}

		public virtual void invokeClickEvent() {
			m_clickEvent(this);
		}

		public bool hasEvent() {
			return m_clickEvent != null;
		}

		//Summary
		//	Updates the button's bounds to match the largest image that it can show
		private void updateTextureBounds() {
			#if DEBUG
			Vector2 l_size = new Vector2(m_normalTexture.Width, m_normalTexture.Height);
			l_size.X = Math.Max(l_size.X, m_hoverTexture.Width);
			l_size.Y = Math.Max(l_size.Y, m_hoverTexture.Height);
			l_size.X = Math.Max(l_size.X, m_pressedTexture.Width);
			l_size.Y = Math.Max(l_size.Y, m_pressedTexture.Height);
			l_size.X = Math.Max(l_size.X, m_toggleTexture.Width);
			l_size.Y = Math.Max(l_size.Y, m_toggleTexture.Height);
			m_bounds.p_dimensions = l_size;
			#else
			try {
				Vector2 l_size = new Vector2(m_normalTexture.Width, m_normalTexture.Height);
				l_size.X = Math.Max(l_size.X, m_hoverTexture.Width);
				l_size.Y = Math.Max(l_size.Y, m_hoverTexture.Height);
				l_size.X = Math.Max(l_size.X, m_pressedTexture.Width);
				l_size.Y = Math.Max(l_size.Y, m_pressedTexture.Height);
				l_size.X = Math.Max(l_size.X, m_toggleTexture.Width);
				l_size.Y = Math.Max(l_size.Y, m_toggleTexture.Height);
				m_bounds.p_dimensions = l_size;
			} catch (NullReferenceException) {
				return;
			}
			#endif
		}

		public virtual Rectangle getBox() {
			return m_bounds;
		}
		#endregion

		#region Button Properties
		public string p_buttonText {
			get {
				return m_text.p_text;
			}
			set {
				m_text.p_text = value;
			}
		}

		public virtual State p_state {
			get {
				return m_currentState;
			}
			set {
				m_currentState = value;
			}
		}

		public Vector2 p_textOffset {
			get {
				return m_textOffset;
			}
			set {
				m_textOffset = value;
				m_text.p_parentOffset = m_position + m_textOffset;
			}
		}
		public override Vector2 p_screenPosition {
			get {
				return m_parentOffset;
			}
			set {
				m_parentOffset = value;
				updateBounds();
				if (m_text != null) {
					m_text.p_parentOffset = value + m_textOffset;
				}
			}
		}
		#endregion
	}
}
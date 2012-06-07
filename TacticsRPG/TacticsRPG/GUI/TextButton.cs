using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class TextButton : Button {
		public new delegate void clickDelegate(Button a_button);
		public new event clickDelegate m_clickEvent;
		private Color m_normalColor;
		private Color m_hoverColor;
		private Color m_pressedColor;
		private Color m_toggleColor;

		//Summary
		//	Button-subclass for creating a button from a SpriteFont
		public TextButton(Vector2 a_position, string a_text, string a_font, Color a_normal, Color a_hover, Color a_pressed, Color a_toggle)
			: base(a_position)
		{
			m_normalColor = a_normal;
			m_hoverColor = a_hover;
			m_pressedColor = a_pressed;
			m_toggleColor = a_toggle;
			m_text = new Text(m_parentOffset, a_text, a_font, a_normal, false);
		}

		//Summary
		//	Button-subclass for creating a button from a SpriteFont
		public TextButton(Vector2 a_position, string a_text, string a_font) 
			: base(a_position)
		{
			m_normalColor = new Color(0, 0, 0);
			m_hoverColor = new Color(62, 67, 68);
			m_pressedColor = new Color(40, 40, 40);
			m_toggleColor = new Color(0, 0, 255);
			m_text = new Text(m_parentOffset, a_text, a_font, m_normalColor, false);
		}

		public override void load() {
			m_text.load();
			m_bounds = m_text.getHitBox();
			m_bounds.p_coordinates = m_parentOffset;
		}

		public override State p_state {
			get {
				return m_currentState;
			}
			set {
				base.p_state = value;
				switch (value) {
					case Button.State.Normal:
						m_text.p_color = m_normalColor;
						break;
					case Button.State.Hover:
						m_text.p_color = m_hoverColor;
						break;
					case Button.State.Pressed:
						m_text.p_color = m_pressedColor;
						break;
					case Button.State.Toggled:
						m_text.p_color = m_toggleColor;
						break;
				}
			}
		}

		public override void update() {
			if (!m_visible) {
				return;
			}
			updateBounds();
			m_text.update();
			updateMouse();
			updateKeyboard();
		}

		protected override void updateBounds() {
			m_text.p_screenPosition = m_bounds.p_coordinates;
		}

		protected override void updateMouse() {
			if (m_bounds.contains(MouseHandler.getCurPos())) {	
				m_currentState = State.Hover;
				m_text.p_color = m_hoverColor;
				if (MouseHandler.lmbPressed()) {
					m_currentState = State.Pressed;
					playDownSound();
					m_text.p_color = m_pressedColor;
				}
				if (MouseHandler.lmbUp()) {
					playUpSound();
					if (m_clickEvent != null) {
						m_clickEvent(this);
					}
					m_text.p_color = m_hoverColor;
				}
			} else if (!m_bounds.contains(MouseHandler.getCurPos()) && m_bounds.contains(MouseHandler.getPrePos())) {
				m_currentState = State.Normal;
				m_text.p_color = m_normalColor;
			}
		}

		protected override void updateKeyboard() {
			if (base.hotkeyPressed()) {
				playDownSound();
				m_currentState = State.Pressed;
				if (m_clickEvent != null) {
					m_clickEvent(this);
				}
			}
		}

		public override void draw() {
			m_text.draw();
		}

		public override void invokeClickEvent() {
			m_clickEvent(this);
		}
	}
}
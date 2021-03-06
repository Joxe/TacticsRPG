﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class ChampionSelectList {
		private List<Button> m_buttonList;
		private ButtonListType m_buttonListType;
		public enum ButtonListType {
			None,		ChmpnMain,		ChmpnAction,	ChmpnAbility
		}
		
		public ChampionSelectList(Champion a_champion, Vector2 a_position, ButtonListType a_listType) {
			m_buttonList = new List<Button>();
			m_buttonListType = a_listType;

			switch (m_buttonListType) {
				case ButtonListType.ChmpnMain:
					m_buttonList.Add(new TextButton(a_position, "Move", "Arial"));
					m_buttonList.Add(new TextButton(a_position + new Vector2(0, 20), "Action", "Arial"));
					m_buttonList.Add(new TextButton(a_position + new Vector2(0, 40), "Wait", "Arial"));
					break;
				case  ButtonListType.ChmpnAction:
					m_buttonList.Add(new TextButton(a_position, "Attack", "Arial"));
					m_buttonList.Add(new TextButton(a_position + new Vector2(0, 20), "Ability", "Arial"));
					break;
				case ButtonListType.ChmpnAbility:
					foreach (Ability l_ability in a_champion.getAbilities()) {
						m_buttonList.Add(new TextButton(a_position, l_ability.getName() + ": " + l_ability.getCost(), "Arial"));		
					}
					break;
			}
		}

		public void load() {
			foreach (TextButton l_textButton in m_buttonList) {
				l_textButton.load();
			}
		}

		public void revalidateButtons(Champion a_champion) {
			switch (m_buttonListType) {
				case ButtonListType.ChmpnMain:
					findButton("Move").p_state		= a_champion.p_hasMoved		? Button.State.Disabled : Button.State.Normal;
					findButton("Action").p_state	= a_champion.p_actionTaken	? Button.State.Disabled : Button.State.Normal;
					break;
				case ButtonListType.ChmpnAction:
					//TODO Kolla typ om en champion har silence eller något som gör så att de inte kan röra sig eller attackera
					break;
				case ButtonListType.ChmpnAbility:
					int l_currentMana = a_champion.getStat("CurrentMana");
					foreach (Ability l_ability in a_champion.getAbilities()) {
						#if DEBUG
						findButton(l_ability.getName()).p_state = l_currentMana <= l_ability.getCost() ? Button.State.Disabled : Button.State.Normal;
						#else
						try {
							findButton(l_ability.getName()).p_state = l_currentMana <= l_ability.getCost() ? Button.State.Disabled : Button.State.Normal;
						} catch (NullReferenceException) {
							System.Console.WriteLine("Ability was found in champion ability list but not in menu when trying to revalidate buttons");
						}
						#endif
					}
					break;
			}
		}

		public Button findButton(string a_buttonText) {
			foreach (Button l_button in m_buttonList) {
				if (l_button.p_buttonText.Equals(a_buttonText)) {
					return l_button;
				}
			}
			return null;
		}

		public List<Button> getButtons() {
			return m_buttonList;
		}

		public void setButtonListeners(string a_button, TextButton.clickDelegate a_delegate) {
			if (a_button != null) {
				((TextButton)findButton(a_button)).m_clickEvent += a_delegate;
			} else {
				foreach (TextButton l_button in m_buttonList) {
					l_button.m_clickEvent += a_delegate;
				}
			}
		}

		public void update() {
			foreach (Button l_button in m_buttonList) {
				l_button.update();
			}
		}

		public void draw() {
			foreach (Button l_button in m_buttonList) {
				l_button.draw();
			}
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public class GameGUI : State {
		//private LinkedList<GuiObject> m_menuList = new LinkedList<GuiObject>();
		private TextButton m_gameStart;
		private GameState m_gameState;
		private bool m_collidedWithGui;

		private ButtonList m_mainBtnList;
		private ButtonList m_abilityBtnList;
		private ButtonList m_actionBtnList;
		private ButtonList m_activeBtnList;

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,		SelectTarget,	Move,	SelectFacing,
			Ability,	Action
		}

		public GameGUI() {
			m_mainBtnList = new ButtonList(null, new Vector2(30, 400), ButtonList.ButtonListType.ChmpnMain);
			m_mainBtnList.setButtonListeners("Move", moveClick);
			m_mainBtnList.setButtonListeners("Action", actionClick);
			m_mainBtnList.setButtonListeners("Wait", waitClick);
		}

		public override void load() {
			m_gameState = (GameState)Game.getInstance().getCurrentState();
			m_gameStart = new TextButton(new Vector2(400, 15), "Start Game", "Arial");
			m_gameStart.m_clickEvent += new TextButton.clickDelegate(gameStartClick);
			m_gameStart.load();
			m_mainBtnList.load();
		}

		public override void update() {
			m_gameState.getTileMap().p_ignoreMouse = (m_collidedWithGui = collidedWithGUI());
			updateMouse();
			m_gameStart.update();
			if (m_activeBtnList != null) {
				m_activeBtnList.update();
			}
		}

		public override void draw() {
			m_gameStart.draw();
			if (m_activeBtnList != null) {
				m_activeBtnList.draw();
			}
		}

		private void updateMouse() {
			if (MouseHandler.lmbPressed()) {
				switch (m_state) {
					case GuiState.SelectTarget:
						foreach (Champion l_champion in m_gameState.getChampions()) {
							if (l_champion.getHitBox().contains(MouseHandler.worldMouse()) && l_champion.getTile().p_tileState == Tile.TileState.Toggle) {
								((GameState)Game.getInstance().getCurrentState()).p_selectedChampion.attack(l_champion);
								restoreStates();
								break;
							}
						}
						break;
					case GuiState.Move:
						foreach (Tile l_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (l_tile != null && l_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.p_selectedChampion.moveTo(l_tile);
								restoreStates();
								break;
							}
						}
						break;
					case GuiState.SelectFacing:
						foreach (Tile l_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (l_tile != null && l_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.p_selectedChampion.faceTile(l_tile);
								m_gameState.deselectChampion();
								restoreStates();
								break;
							}
						}
						break;
				}
			}
			if (MouseHandler.rmbPressed()) {
				if (m_state == GuiState.SelectTarget || m_state == GuiState.Move) {
					restoreStates();		
				}
			}
		}

		public void championChanged() {
			Vector2 l_position = new Vector2(30, 400);
			m_abilityBtnList = new ButtonList(m_gameState.p_selectedChampion, l_position, ButtonList.ButtonListType.ChmpnAbility);
			m_abilityBtnList.load();
			m_abilityBtnList.setButtonListeners(null, useAbility);
			m_actionBtnList = new ButtonList(m_gameState.p_selectedChampion, l_position, ButtonList.ButtonListType.ChmpnAction);
			m_actionBtnList.load();
			m_activeBtnList = m_mainBtnList;
			m_activeBtnList.revalidateButtons(m_gameState.p_selectedChampion);
		}

		private void restoreStates() {
			m_gameState.getTileMap().restoreStates();
			m_state = GuiState.Normal;
		}

		#region Menu Buttons
		public void moveClick(Button a_button) {
			toggleTiles(m_gameState.p_selectedChampion.getStat("MoveLeft"));
			setState(GuiState.Move);
		}

		private void actionClick(Button a_button) {
			toggleTiles(1);
			setState(GuiState.Action);
		}

		private void waitClick(Button a_button) {
			toggleTiles(1);
			setState(GuiState.SelectFacing);
		}

		private void useAbility(Button a_button) {
			toggleTiles(m_gameState.p_selectedChampion.getAbility(a_button.p_buttonText).getRange());
			setState(GuiState.Ability);
		}
		#endregion

		private void toggleTiles(int a_range) {
			foreach (Tile l_tile in m_gameState.getTileMap().getRangeOfTiles(m_gameState.p_selectedChampion.getTile(), a_range)) {
				if (l_tile != m_gameState.p_selectedChampion.getTile()) {
					l_tile.p_tileState = Tile.TileState.Toggle;
				}
			}
		}

		private void gameStartClick(Button a_button) {
			m_gameState.startGame();
		}

		private bool collidedWithGUI() {
			if (m_activeBtnList != null) {
				foreach (Button l_button in m_activeBtnList.getButtons()) {
					if (l_button.contains(MouseHandler.getCurPos())) {
						return true;
					}
				}
			}
			return false;
		}

		private void setState(GuiState a_guiState) {
			switch (a_guiState) {
				case GuiState.Normal:
					m_activeBtnList = m_mainBtnList;
					break;
				case GuiState.Move:
					m_activeBtnList = null;
					break;
				case GuiState.SelectFacing:
					m_activeBtnList = null;
					break;
				case GuiState.SelectTarget:
					m_activeBtnList = null;
					break;
				case GuiState.Ability:
					m_activeBtnList = m_abilityBtnList;
					break;
				case GuiState.Action:
					m_activeBtnList = m_abilityBtnList;
					break;
			}
		}

		public GuiState getState() {
			return m_state;
		}

		public bool mouseOverGUI() {
			return m_collidedWithGui;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public class GameGUI : State {
		private LinkedList<GuiObject> m_menuList = new LinkedList<GuiObject>();
		private TextButton m_gameStart;
		private GameState m_gameState;
		private bool m_collidedWithGui;

		private TextButton btn_move;
		private TextButton btn_action;
		private TextButton btn_wait;

		private List<Button> m_abilityButtons;

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,		SelectTarget,	Move,	SelectFacing
		}

		public GameGUI() {
			m_guiList.AddLast(m_menuList);
			m_abilityButtons = new List<Button>();
		}

		public override void load() {
			m_gameState = (GameState)Game.getInstance().getCurrentState();
			m_menuList.AddLast(btn_move		= new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 220), "Move",	"Arial"));
			m_menuList.AddLast(btn_action	= new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 200), "Action", "Arial"));
			m_menuList.AddLast(btn_wait		= new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 180), "Wait",	"Arial"));
			btn_move.m_clickEvent	+= new TextButton.clickDelegate(moveClick);
			btn_action.m_clickEvent += new TextButton.clickDelegate(attackClick);
			btn_wait.m_clickEvent	+= new TextButton.clickDelegate(waitClick);
			m_gameStart = new TextButton(new Vector2(400, 15), "Start Game", "Arial");
			m_gameStart.m_clickEvent += new TextButton.clickDelegate(gameStartClick);
			m_gameStart.load();
			foreach (GuiObject l_go in m_menuList) {
				l_go.load();
			}
		}

		public override void update() {
			m_gameState.getTileMap().p_ignoreMouse = (m_collidedWithGui = collidedWithGUI());
			updateMouse();
			m_gameStart.update();
			if (m_gameState.p_selectedChampion != null) {
				foreach (Button l_button in m_abilityButtons) {
					l_button.update();
				}
				base.update();
				if (m_gameState.p_selectedChampion.getStat("MoveLeft") <= 0) {
					btn_move.p_state = Button.State.Disabled;
				}
				if (m_gameState.p_selectedChampion.p_actionTaken) {
					btn_action.p_state = Button.State.Disabled;
				}
			}

			if (KeyboardHandler.keyDown(Keys.A)) {
				m_abilityButtons = GuiListManager.createAbilityList(m_gameState.p_selectedChampion.getAbilities());
			}
		}

		public override void draw() {
			m_gameStart.draw();
			if (m_gameState.p_selectedChampion != null) {
				foreach (Button l_button in m_abilityButtons) {
					l_button.draw();
				}
				base.draw();
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

		private void restoreStates() {
			m_gameState.getTileMap().restoreStates();
			m_state = GuiState.Normal;
		}

		#region Menu Buttons
		private void moveClick(Button a_button) {
			if (a_button.p_state == Button.State.Toggled) {
				return;
			}
			toggleTiles(m_gameState.p_selectedChampion.getStat("MoveLeft"));
			m_state = GuiState.Move;
		}

		private void attackClick(Button a_button) {
			if (a_button.p_state == Button.State.Toggled) {
				return;
			}
			toggleTiles(1);
			m_state = GuiState.SelectTarget;
		}

		private void waitClick(Button a_button) {
			toggleTiles(1);
			m_state = GuiState.SelectFacing;
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
			foreach (GuiObject l_go in m_menuList) {
				if (l_go.contains(MouseHandler.getCurPos())) {
					return true;
				}
			}
			return false;
		}

		public GuiState getState() {
			return m_state;
		}

		public bool mouseOverGUI() {
			return m_collidedWithGui;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class GameGUI : State {
		private LinkedList<GuiObject> m_menuList = new LinkedList<GuiObject>();
		private TextButton m_gameStart;
		private GameState m_gameState;
		private bool m_collidedWithGui;

		private TextButton btn_move;
		private TextButton btn_action;
		private TextButton btn_wait;

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,		SelectTarget,	Move,	SelectFacing
		}

		public GameGUI() {
			m_guiList.AddLast(m_menuList);
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
			foreach (GuiObject t_go in m_menuList) {
				t_go.load();
			}
		}

		public override void update() {
			m_gameState.getTileMap().p_ignoreMouse = (m_collidedWithGui = collidedWithGUI());
			updateMouse();
			m_gameStart.update();
			if (m_gameState.getSelectedChampion() != null) {
				base.update();
				if (m_gameState.getSelectedChampion().getStat("moveLeft") <= 0) {
					btn_move.p_state = Button.State.Disabled;
				} else {
					btn_move.p_state = Button.State.Normal;					
				}
				if (m_gameState.getSelectedChampion().p_actionTaken) {
					btn_action.p_state = Button.State.Disabled;
				} else {
					btn_action.p_state = Button.State.Normal;					
				}
			}
		}

		public override void draw() {
			m_gameStart.draw();
			if (m_gameState.getSelectedChampion() != null) {
				base.draw();
			}
		}

		private void updateMouse() {
			if (MouseHandler.lmbPressed()) {
				switch (m_state) {
					case GuiState.SelectTarget:
						foreach (Champion t_champion in m_gameState.getChampions()) {
							if (t_champion.getHitBox().contains(MouseHandler.worldMouse()) && t_champion.getTile().p_tileState == Tile.TileState.Toggle) {
								((GameState)Game.getInstance().getCurrentState()).getSelectedChampion().attack(t_champion);
								m_gameState.getTileMap().restoreStates();
								m_state = GuiState.Normal;
								break;
							}
						}
						break;
					case GuiState.Move:
						foreach (Tile t_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (t_tile != null && t_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.getSelectedChampion().moveTo(t_tile);
								m_gameState.getTileMap().restoreStates();
							}
						}
						break;
					case GuiState.SelectFacing:
						foreach (Tile t_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (t_tile != null && t_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.getSelectedChampion().faceTile(t_tile);
								m_gameState.getTileMap().restoreStates();
							}
						}
						m_gameState.deselectChampion();
						break;
				}
			}
			if (MouseHandler.rmbPressed()) {
				if (m_state == GuiState.SelectTarget || m_state == GuiState.Move) {
					m_gameState.getTileMap().restoreStates();
					m_state = GuiState.Normal;
				}
			}
		}

		private void moveClick(Button a_button) {
			if (a_button.p_state == Button.State.Toggled) {
				return;
			}
			Champion t_selectedChampion = m_gameState.getSelectedChampion();
			foreach (Tile t_tile in m_gameState.getTileMap().getRangeOfTiles(t_selectedChampion.getTile(), t_selectedChampion.getStat("moveLeft"))) {
				if (t_tile != m_gameState.getSelectedChampion().getTile() && !t_tile.isObstructed()) {
					t_tile.p_tileState = Tile.TileState.Toggle;
					m_state = GuiState.Move;
				}
			}
		}

		private void attackClick(Button a_button) {
			if (a_button.p_state == Button.State.Toggled) {
				return;
			}
			foreach (Tile t_tile in m_gameState.getTileMap().getRangeOfTiles(m_gameState.getSelectedChampion().getTile(), 1)) {
				if (t_tile != m_gameState.getSelectedChampion().getTile()) {
					t_tile.p_tileState = Tile.TileState.Toggle;
				}
			}
			m_state = GuiState.SelectTarget;
		}

		private void waitClick(Button a_button) {
			foreach (Tile t_tile in m_gameState.getTileMap().getRangeOfTiles(m_gameState.getSelectedChampion().getTile(), 1)) {
				if (t_tile != m_gameState.getSelectedChampion().getTile()) {
					t_tile.p_tileState = Tile.TileState.Toggle;
				}
			}
			m_state = GuiState.SelectFacing;
		}

		private void gameStartClick(Button a_button) {
			m_gameState.startGame();
		}

		private bool collidedWithGUI() {
			foreach (GuiObject t_go in m_menuList) {
				if (t_go.contains(MouseHandler.getCurPos())) {
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

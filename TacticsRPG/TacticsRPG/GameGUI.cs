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

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,		SelectTarget,	Move
		}

		public GameGUI() {
			m_guiList.AddLast(m_menuList);
			m_menuList.AddLast(new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 220), "Move", "BitstreamVS"));
			((TextButton)m_menuList.Last()).m_clickEvent += new TextButton.clickDelegate(moveClick);
			m_menuList.AddLast(new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 200), "Attack", "BitstreamVS"));
			((TextButton)m_menuList.Last()).m_clickEvent += new TextButton.clickDelegate(attackClick);
			m_gameStart = new TextButton(new Vector2(400, 15), "Start Game", "BitstreamVS");
		}

		public override void load() {
			m_gameState = (GameState)Game.getInstance().getCurrentState();
			m_gameStart.load();
			foreach (GuiObject t_go in m_menuList) {
				t_go.load();
			}
		}

		public override void update() {
			m_collidedWithGui = false;
			updateMouse();
			m_gameStart.update();
			if (m_gameState.getSelectedChampion() != null) {
				base.update();
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
				if (m_state == GuiState.SelectTarget) {
					foreach (Champion t_champion in m_gameState.getChampions()) {
						if (t_champion.getHitBox().contains(MouseHandler.worldMouse()) && t_champion.getTile().p_tileState == Tile.TileState.Toggle) {
							((GameState)Game.getInstance().getCurrentState()).getSelectedChampion().attack(t_champion);
							m_gameState.getTileMap().restoreStates();
							m_state = GuiState.Normal;
							return;
						}
					}
				}
				if (m_state == GuiState.Move) {
					foreach (Tile t_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
						if (t_tile != null && t_tile.getHitBox().contains(MouseHandler.worldMouse())) {
							m_gameState.getSelectedChampion().moveTo(t_tile);
							m_gameState.getTileMap().restoreStates();
						}
					}
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
			Champion t_selectedChampion = m_gameState.getSelectedChampion();
			foreach (Tile t_tile in m_gameState.getTileMap().getRangeOfTiles(t_selectedChampion.getTile(), t_selectedChampion.getStat("move"))) {
				if (t_tile != m_gameState.getSelectedChampion().getTile() && !t_tile.isObstructed()) {
					t_tile.p_tileState = Tile.TileState.Toggle;
					m_state = GuiState.Move;
				}
			}
		}

		private void attackClick(Button a_button) {
			foreach (Tile t_tile in m_gameState.getTileMap().getRangeOfTiles(m_gameState.getSelectedChampion().getTile(), 1)) {
				if (t_tile != m_gameState.getSelectedChampion().getTile()) {
					t_tile.p_tileState = Tile.TileState.Toggle;
					m_state = GuiState.SelectTarget;
				}
			}
		}

		public GuiState getState() {
			return m_state;
		}
	}
}

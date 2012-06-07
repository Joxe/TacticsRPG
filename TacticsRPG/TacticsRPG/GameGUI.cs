using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class GameGUI : State {
		private LinkedList<GuiObject> m_menuList = new LinkedList<GuiObject>();
		private GameState m_gameState;

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,	SelectTarget
		}

		public GameGUI() {
			m_guiList.AddLast(m_menuList);
		}

		public override void load() {
			m_gameState = (GameState)Game.getInstance().getCurrentState();
			m_menuList.AddLast(new TextButton(new Vector2(15, Game.getInstance().getResolution().Y - 200), "Attack", "BitstreamVS"));
			((TextButton)m_menuList.Last()).m_clickEvent += new TextButton.clickDelegate(attackClick);
			foreach (GuiObject t_go in m_menuList) {
				t_go.load();
			}
		}

		public override void update() {
			updateMouse();
			base.update();
		}

		public override void draw() {
			base.draw();
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
			}
			if (MouseHandler.rmbPressed()) {
				if (m_state == GuiState.SelectTarget) {
					m_gameState.getTileMap().restoreStates();
					m_state = GuiState.Normal;
				}
			}
		}

		private void attackClick(Button a_button) {
			foreach (Tile t_tile in m_gameState.getTileMap().getSurroundingTiles(m_gameState.getSelectedChampion().getTile(), 0, 1, new LinkedList<Tile>())) {
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

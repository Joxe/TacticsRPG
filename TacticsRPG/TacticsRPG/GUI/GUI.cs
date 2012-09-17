using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using LuaInterface;

namespace TacticsRPG {
	public class GUI {
		private LinkedList<GuiObject> m_menuList = new LinkedList<GuiObject>();
		private TextButton m_gameStart; /*TODO DEBUG!!! */
		private GameState m_gameState;
		private bool m_collidedWithGui;
		private List<GUIAddon> m_addons;
		private Lua m_lua = new Lua();

		private GuiState m_state = GuiState.Normal;
		public enum GuiState {
			Normal,		AttackTarget,	Move,	SelectFacing,
			UseAbility,	ActionMenu
		}

		public GUI() {
			/*
			m_mainBtnList = new ChampionSelectList(null, new Vector2(30, 400), ChampionSelectList.ButtonListType.ChmpnMain);
			m_mainBtnList.setButtonListeners("Move", moveClick);
			m_mainBtnList.setButtonListeners("Action", actionClick);
			m_mainBtnList.setButtonListeners("Wait", waitClick);
			*/
			m_addons = new List<GUIAddon>();
		}

		public void load() {
			m_gameState = (GameState)Game.getInstance().getCurrentState();
			m_gameStart = new TextButton(new Vector2(400, 15), "Start Game", "Arial");
			m_gameStart.m_clickEvent += new TextButton.clickDelegate(gameStartClick);
			m_gameStart.load();

			m_addons.Add(new GUIAddon("Content/Scripts/GUI/CreateGUI.lua"));
			registerFunctions(m_lua);

			foreach (GUIAddon l_guiAddon in m_addons) {
				l_guiAddon.load();
			}
		}
		
		public void update() {
			m_gameState.getTileMap().p_ignoreMouse = (m_collidedWithGui = collidedWithGUI());
			updateMouse();
			m_gameStart.update();

			foreach (GUIAddon l_guiAddon in m_addons) {
				l_guiAddon.update();
			}
			/*
			if (m_activeBtnList != null) {
				m_activeBtnList.update();
			}
			*/
		}

		public void draw() {
			m_gameStart.draw();

			foreach (GUIAddon l_guiAddon in m_addons) {
				l_guiAddon.draw();
			}
			/*
			if (m_activeBtnList != null) {
				m_activeBtnList.draw();
			}
			*/
		}

		private void updateMouse() {
			if (MouseHandler.lmbPressed()) {
				switch (m_state) {
					case GuiState.AttackTarget:
						foreach (Champion l_champion in m_gameState.getChampions()) {
							if (l_champion.getHitBox().contains(MouseHandler.worldMouse()) && l_champion.getTile().p_tileState == Tile.TileState.Toggle) {
								((GameState)Game.getInstance().getCurrentState()).getSelectedChampion().attack(l_champion);
								restoreStates();
								break;
							}
						}
						break;
					case GuiState.Move:
						foreach (Tile l_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (l_tile != null && l_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.getSelectedChampion().moveTo(l_tile);
								restoreStates();
								break;
							}
						}
						break;
					case GuiState.SelectFacing:
						foreach (Tile l_tile in m_gameState.getTileMap().toLinkedList(Tile.TileState.Toggle)) {
							if (l_tile != null && l_tile.getHitBox().contains(MouseHandler.worldMouse())) {
								m_gameState.getSelectedChampion().faceTile(l_tile);
								m_gameState.deselectChampion();
								restoreStates();
								break;
							}
						}
						break;
				}
			}
			if (MouseHandler.rmbPressed()) {
				if (m_state == GuiState.AttackTarget || m_state == GuiState.Move || m_state == GuiState.ActionMenu) {
					restoreStates();		
				}
			}
		}

		public void championChanged() {
			Vector2 l_position = new Vector2(30, 400);
			/*
			m_abilityBtnList = new ChampionSelectList(m_gameState.getSelectedChampion(), l_position, ChampionSelectList.ButtonListType.ChmpnAbility);
			m_abilityBtnList.load();
			m_abilityBtnList.setButtonListeners(null, useAbility);
			m_actionBtnList = new ChampionSelectList(m_gameState.getSelectedChampion(), l_position, ChampionSelectList.ButtonListType.ChmpnAction);
			m_actionBtnList.load();
			m_activeBtnList = m_mainBtnList;
			m_mainBtnList.revalidateButtons(m_gameState.getSelectedChampion());
			*/
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
			toggleTiles(m_gameState.getSelectedChampion().getStat("MoveLeft"));
			m_state = GuiState.Move;
		}

		private void actionClick(Button a_button) {
			//m_activeBtnList = m_actionBtnList;
			m_state = GuiState.ActionMenu;
		}

		private void waitClick(Button a_button) {
			toggleTiles(1);
			//m_activeBtnList = null;
			m_state = GuiState.SelectFacing;
		}

		public void addButton(int a_x, int a_y, string a_text) {
			m_menuList.AddLast(new TextButton(new Vector2(a_x, a_y), a_text, "Arial"));
			//m_mainBtnList.AddLast(new TextButton(new Vector2(a_x, a_y), a_text, a_font));
			//m_mainBtnList.Last().load();
		}

		private void useAbility(Button a_button) {
			toggleTiles(m_gameState.getSelectedChampion().getAbility(a_button.p_buttonText.Split(':')[0]).getRange());
			m_state = GuiState.UseAbility;
		}
		#endregion

		private void toggleTiles(int a_range) {
			foreach (Tile l_tile in m_gameState.getTileMap().getRangeOfTiles(m_gameState.getSelectedChampion().getTile(), a_range)) {
				if (l_tile != m_gameState.getSelectedChampion().getTile()) {
					l_tile.p_tileState = Tile.TileState.Toggle;
				}
			}
		}

		private void gameStartClick(Button a_button) {
			m_gameState.startGame();
		}

		public bool collidedWithGUI() {
			/*
			if (m_activeBtnList != null) {
				foreach (Button l_button in m_activeBtnList.getButtons()) {
					if (l_button.contains(MouseHandler.getCurPos())) {
						return true;
					}
				}
			}
			*/
			return false;
		}

		public GuiState getState() {
			return m_state;
		}

		public string getStateAsString() {
			return m_state.ToString();
		}

		public void registerFunctions(Lua a_lua) {
			a_lua.RegisterFunction("addButtonToGUI", this, this.GetType().GetMethod("addButton"));
			a_lua.RegisterFunction("getStateAsString", this, this.GetType().GetMethod("getStateAsString"));
		}
	}
}

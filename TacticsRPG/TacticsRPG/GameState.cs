using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public class GameState : State {
		private TileMap m_tileMap;
		private Dictionary<string, Champion> m_champions;
		private Champion m_selectedChampion;
		private LinkedList<GuiObject> m_championInfo;
		private GameGUI m_gameGui;
		private List<Champion> m_battleQueue;

		public GameState() : base() {
			m_champions = new Dictionary<string, Champion>();
			m_guiList.AddLast(m_championInfo = new LinkedList<GuiObject>());
			m_gameGui = new GameGUI();
		}

		public override void load() {
			Loader.loadSettings("settings");
			Game.getInstance().m_camera.load();
			ClassesData.load();
			RacesData.load();
			ElementsData.load();
			AbilitiesData.load();
			XMLParser.setAbilities();
			m_gameGui.load();
			createTileMap(20, 20);
		}

		public override void update() {
			updateMouse();
			updateKeyboard();
			foreach (Champion l_champion in m_champions.Values) {
				l_champion.update();
			}
			m_gameGui.update();
			m_tileMap.update();
			if (m_battleQueue != null) {
				updateBattle();
			}
			base.update();
		}

		private void updateMouse() {
			if (m_gameGui.mouseOverGUI()) {
				return;
			}
			if (MouseHandler.mmbPressed()) {
				CameraHandler.cameraDrag();
			}
			if (MouseHandler.lmbDown()) {
				if (KeyboardHandler.keyPressed(Keys.LeftShift)) {
					if (m_gameGui.getState() != GameGUI.GuiState.AttackTarget) {
						foreach (Champion l_champion in m_champions.Values) {
							if (l_champion.getHitBox().contains(MouseHandler.worldMouse())) {
								p_selectedChampion = l_champion;
							}
						}
					}
				}
			}
			if (MouseHandler.rmbDown()) {
				if (KeyboardHandler.keyPressed(Keys.LeftShift)) {
					if (m_selectedChampion != null) {
						deselectChampion();
					}
				}
			}
			if (MouseHandler.scrollUp()) {
				CameraHandler.zoomIn(0.1f);			
			} else if (MouseHandler.scrollDown()) {
				CameraHandler.zoomOut(0.1f);
			}
		}

		private void updateKeyboard() {
			if (KeyboardHandler.keyPressed(Keys.D)) {
				m_champions.Add("Joxe", new Champion(m_tileMap.p_hover.getMapPosition(), "Joxe", "Warrior", true, "Human"));
				m_tileMap.p_hover.p_object = m_champions["Joxe"];
				m_champions["Joxe"].load();
			}
			if (KeyboardHandler.keyPressed(Keys.F)) {
				m_champions.Add("Din Mamma", new Champion(m_tileMap.p_hover.getMapPosition(), "Din Mamma", "Mage", false, "Human"));
				m_tileMap.p_hover.p_object = m_champions["Din Mamma"];
				m_champions["Din Mamma"].load();
			}
		}

		private void updateBattle() {
			if (m_selectedChampion == null) {
				p_selectedChampion = m_battleQueue.First();
				m_selectedChampion.championsTurn();
			}
			int negativeSpeed = m_selectedChampion.p_speed;

			foreach (Champion l_champion in m_battleQueue) {
				l_champion.p_speed -= negativeSpeed;
			}
			m_battleQueue.Sort();
		}

		public override void draw() {
			m_tileMap.draw();
			foreach (Champion l_champion in m_champions.Values) {
				l_champion.draw();
			}
			m_gameGui.draw();
			base.draw();
		}

		public TileMap getTileMap() {
			return m_tileMap;
		}

		private void createTileMap(int width, int height) {
			m_tileMap = new TileMap(width, height, "test");
			m_tileMap.load();
		}

		public Champion p_selectedChampion {
			get {
				return m_selectedChampion;
			}
			set {
				if (m_selectedChampion != null) {
					deselectChampion();
				}
				m_selectedChampion = value;
				m_selectedChampion.p_targetState = Champion.TargetState.Targeted;
				m_selectedChampion.p_actionTaken = false;
				m_championInfo.Clear();
				m_championInfo.AddLast(new Text(new Vector2(10, 10), m_selectedChampion.ToString(), "Arial", Color.Black, false));
				foreach (Text l_text in m_selectedChampion.statsToTextList()) {
					m_championInfo.AddLast(l_text);
				}
				GuiListManager.setListPosition(m_championInfo, new Vector2(10, 10), new Vector2(0, 20));
				GuiListManager.loadList(m_championInfo);
				m_gameGui.championChanged();
			}
		}

		public void deselectChampion() {
			m_selectedChampion.p_targetState = Champion.TargetState.Normal;
			m_championInfo.Clear();
			m_selectedChampion = null;
			if (m_battleQueue != null) {
				m_battleQueue.Sort();
			}
		}

		public LinkedList<Champion> getChampions() {
			LinkedList<Champion> l_list = new LinkedList<Champion>();
			foreach (Champion l_champion in m_champions.Values) {
				l_list.AddLast(l_champion);
			}
			return l_list;
		}

		public void removeChampion(Champion a_champion) {
			m_champions[a_champion.getName()].kill();
			m_champions.Remove(a_champion.getName());
		}

		public void startGame() {
			m_battleQueue = new List<Champion>();
			foreach (Champion l_champion in m_champions.Values) {
				m_battleQueue.Add(l_champion);
			}
			m_battleQueue.Sort();
		}
	}
}
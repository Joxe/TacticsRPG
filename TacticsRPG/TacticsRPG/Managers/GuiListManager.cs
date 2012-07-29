using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework;
using System.Text.RegularExpressions;

namespace TacticsRPG {
	public class GuiListManager {
		public static LinkedList<Button> createListFromDirectory(string a_path, string[] a_extension, string a_buttonGraphic) {
			string[] l_fileList = Directory.GetFiles(a_path);
			LinkedList<Button> l_guiList = new LinkedList<Button>();

			for (int i = 0, j = 0; i < l_fileList.Length; i++) {
				bool l_accepted = false;

				foreach (string l_ext in a_extension) {
					if (l_fileList[i].EndsWith(l_ext)) {
						l_accepted = true;
						continue;
					}
				}

				if (!l_accepted) {
					continue;
				}

				string[] l_splitPath = Regex.Split(l_fileList[i], "//");
				string[] l_extless = l_splitPath[l_splitPath.Length - 1].Split('.');
				l_guiList.AddLast(new Button(a_buttonGraphic, new Vector2(0, 0), l_extless[0], "Arial", Color.Black, new Vector2(0, 0)));
				j++;
			}
			return l_guiList;
		}

		public static List<Button> createListFromStringArray(string[] a_array, Vector2 a_position) {
			List<Button> l_guiList = new List<Button>();

			for (int i = 0, j = 0; i < a_array.Length; i++) {
				l_guiList.Add(new TextButton(a_position + new Vector2(0, i * 15), a_array[i], "Arial"));
			}
			
			return l_guiList;
		}

		public static LinkedList<Text> createTextListFromArray(string[] a_array, string a_font, Color a_color) {
			LinkedList<Text> l_guiList = new LinkedList<Text>();
			
			for (int i = 0; i < a_array.Length; i++) {
				l_guiList.AddLast(new Text(Vector2.Zero, a_array[i], a_font, a_color, false));
			}

			return l_guiList;
		}

		public static LinkedList<Button> createNumeratedList(int a_numberOfElements, string a_buttonGraphic) {
			LinkedList<Button> l_guiList = new LinkedList<Button>();
			for (int i = 0; i < a_numberOfElements; i++) {
				l_guiList.AddLast(new Button(a_buttonGraphic, new Vector2(0, 0), (i + 1).ToString(), "Arial", Color.Black, new Vector2(0, 0)));
			}
			return l_guiList;
		}

		public static List<Button> createAbilityList(List<Ability> l_list) {
			List<Button> l_returnList = new List<Button>();
			foreach (Ability l_ability in l_list) {
				l_returnList.Add(new TextButton(Vector2.Zero, l_ability.getName(), "Arial"));
				l_returnList.Last().load();
			}
			return l_returnList;
		}

		public static void setListPosition(LinkedList<GuiObject> a_list, Vector2 a_position) {
			foreach (GuiObject l_go in a_list) {
				l_go.p_position = a_position;
			}
		}

		public static void setListPosition(LinkedList<GuiObject> a_list, Vector2 a_position, Vector2 a_offset) {
			int i = 0;
			foreach (GuiObject l_go in a_list) {
				l_go.p_parentOffset = a_position + a_offset * i++ - Game.getInstance().getResolution() / 2;
			}
		}

		public static void setListDistance(LinkedList<GuiObject> a_list, Vector2 a_distance) {
			int i = 0;
			foreach (GuiObject l_go in a_list) {
				l_go.p_parentOffset = a_distance * i;
				i++;
			}
		}

		public static void setSelection(LinkedList<Button> a_list, Button.State a_selection) {
			foreach (Button l_button in a_list) {
				l_button.p_state = a_selection;
			}
		}

		public static void loadList(LinkedList<GuiObject> a_list) {
			foreach (GuiObject l_text in a_list) {
				l_text.load();
			}
		}
	}
}
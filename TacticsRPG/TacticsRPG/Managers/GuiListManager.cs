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
			string[] t_fileList = Directory.GetFiles(a_path);
			return createListFromStringArray(t_fileList, a_extension, a_buttonGraphic);
		}

		public static LinkedList<Button> createListFromStringArray(string[] a_fileList, string[] a_extension, string a_buttonGraphic) {
			LinkedList<Button> t_guiList = new LinkedList<Button>();

			for (int i = 0, j = 0; i < a_fileList.Length; i++) {
				bool t_accepted = false;

				foreach (string t_ext in a_extension) {
					if (a_fileList[i].EndsWith(t_ext)) {
						t_accepted = true;
						continue;
					}
				}

				if (!t_accepted) {
					continue;
				}

				string[] t_splitPath = Regex.Split(a_fileList[i], "//");
				string[] t_extless = t_splitPath[t_splitPath.Length - 1].Split('.');
				t_guiList.AddLast(new Button(a_buttonGraphic, new Vector2(0, 0), t_extless[0], "VerdanaBold", Color.Black, new Vector2(0, 0)));
				j++;
			}
			return t_guiList;
		}

		public static LinkedList<Text> createTextListFromArray(string[] a_array, string a_font, Color a_color) {
			LinkedList<Text> t_guiList = new LinkedList<Text>();
			
			for (int i = 0; i < a_array.Length; i++) {
				t_guiList.AddLast(new Text(Vector2.Zero, a_array[i], a_font, a_color, false));
			}

			return t_guiList;
		}

		public static LinkedList<Button> createNumeratedList(int a_numberOfElements, string a_buttonGraphic) {
			LinkedList<Button> t_guiList = new LinkedList<Button>();
			for (int i = 0; i < a_numberOfElements; i++) {
				t_guiList.AddLast(new Button(a_buttonGraphic, new Vector2(0, 0), (i + 1).ToString(), "VerdanaBold", Color.Black, new Vector2(0, 0)));
			}
			return t_guiList;
		}

		public static void setListPosition(LinkedList<GuiObject> a_list, Vector2 a_position) {
			foreach (GuiObject t_go in a_list) {
				t_go.p_position = a_position;
			}
		}

		public static void setListPosition(LinkedList<GuiObject> a_list, Vector2 a_position, Vector2 a_offset) {
			int i = 0;
			foreach (GuiObject t_go in a_list) {
				t_go.p_parentOffset = a_position + a_offset * i++;
			}
		}

		public static void setListDistance(LinkedList<GuiObject> a_list, Vector2 a_distance) {
			int i = 0;
			foreach (GuiObject t_go in a_list) {
				t_go.p_parentOffset = a_distance * i;
				i++;
			}
		}

		public static void setSelection(LinkedList<Button> a_list, Button.State a_selection) {
			foreach (Button t_button in a_list) {
				t_button.p_state = a_selection;
			}
		}

		public static void loadList(LinkedList<GuiObject> a_list) {
			foreach (GuiObject t_text in a_list) {
				t_text.load();
			}
		}
	}
}
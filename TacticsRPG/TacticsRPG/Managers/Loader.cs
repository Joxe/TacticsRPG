using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace TacticsRPG {
	public class Loader {
		private static ParseState m_currentParse;
		private enum ParseState {
			Graphics, Input
		}

		public static void genGraphSettings(string a_file) {
			if (!Directory.Exists("Settings")) {
				Directory.CreateDirectory("Settings");
			}
			if (!File.Exists("Settings//" + a_file + ".ini")) {
				File.Create("Settings/" + a_file + ".ini");
				TextWriter l_writer = new StreamWriter("Settings//" + a_file + ".ini");
				l_writer.WriteLine("[Graphics]\nScreenWidth=1280\nScreenHeight=720\nFullscreen=false");
				l_writer.Close();
			}
		}

		public static void loadSettings(string a_file) {
			string[] l_loadedFile = File.ReadAllLines("Settings//" + a_file + ".ini");

			foreach (string l_currentLine in l_loadedFile) {
				if (l_currentLine.Length > 2 && l_currentLine.First() == '[' && l_currentLine.Last() == ']') {
					if (l_currentLine.Equals("[Input]")) {
						m_currentParse = ParseState.Input;
					}
					else if (l_currentLine.Equals("[Graphics]"))
					{
						m_currentParse = ParseState.Graphics;
					}
				}
				switch (m_currentParse)
				{
					case ParseState.Input:
						string[] l_input = l_currentLine.Split('=');
						break;
					case ParseState.Graphics:
						string[] l_setting = l_currentLine.Split('=');
						if (l_setting[0].Equals("ScreenWidth")) {
							Game.getInstance().m_graphics.PreferredBackBufferWidth = int.Parse(l_setting[1]);
						} else if (l_setting[0].Equals("ScreenHeight")) {
							Game.getInstance().m_graphics.PreferredBackBufferHeight = int.Parse(l_setting[1]);
						} else if (l_setting[0].Equals("Fullscreen")) {
							Game.getInstance().m_graphics.IsFullScreen = bool.Parse(l_setting[1]);
						} else if (l_setting[0].StartsWith("[")) {
							break;
						} else {
							System.Console.WriteLine("Unknown Setting");
						}
						break;
				}
			}
			Game.getInstance().m_graphics.ApplyChanges();
		}
	}
}

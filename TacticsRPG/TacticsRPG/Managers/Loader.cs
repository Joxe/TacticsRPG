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
				TextWriter t_writer = new StreamWriter("Settings//" + a_file + ".ini");
				t_writer.WriteLine("[Graphics]\nScreenWidth=1280\nScreenHeight=720\nFullscreen=false");
				t_writer.Close();
			}
		}

		public static void loadSettings(string a_file) {
			string[] t_loadedFile = File.ReadAllLines("Settings//" + a_file + ".ini");

			foreach (string t_currentLine in t_loadedFile) {
				if (t_currentLine.Length > 2 && t_currentLine.First() == '[' && t_currentLine.Last() == ']') {
					if (t_currentLine.Equals("[Input]")) {
						m_currentParse = ParseState.Input;
					}
					else if (t_currentLine.Equals("[Graphics]"))
					{
						m_currentParse = ParseState.Graphics;
					}
				}
				switch (m_currentParse)
				{
					case ParseState.Input:
						string[] t_input = t_currentLine.Split('=');
						break;
					case ParseState.Graphics:
						string[] t_setting = t_currentLine.Split('=');
						if (t_setting[0].Equals("ScreenWidth")) {
							Game.getInstance().m_graphics.PreferredBackBufferWidth = int.Parse(t_setting[1]);
						} else if (t_setting[0].Equals("ScreenHeight")) {
							Game.getInstance().m_graphics.PreferredBackBufferHeight = int.Parse(t_setting[1]);
						} else if (t_setting[0].Equals("Fullscreen")) {
							Game.getInstance().m_graphics.IsFullScreen = bool.Parse(t_setting[1]);
						} else if (t_setting[0].StartsWith("[")) {
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

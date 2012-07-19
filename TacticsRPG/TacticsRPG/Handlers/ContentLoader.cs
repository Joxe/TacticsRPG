using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace TacticsRPG {
	public static class ContentLoader {
		public static Texture2D loadTexture(string a_filepath) {
			#if DEBUG
			FileStream l_filestream = new FileStream("Content/" + a_filepath, FileMode.Open);
			Texture2D l_texture = Texture2D.FromStream(Game.getInstance().GraphicsDevice, l_filestream);
			l_filestream.Close();
			return l_texture;
			#else
			try {
				FileStream l_filestream = new FileStream(a_filepath, FileMode.Open);
				Texture2D l_texture = Texture2D.FromStream(Game.getInstance().GraphicsDevice, l_filestream);
				l_filestream.Close();
				return l_texture;
			} catch (FileNotFoundException) {
				System.Console.WriteLine("Texture file not found!");
			} catch (DirectoryNotFoundException) {
				System.Console.WriteLine("Texture directory not found!");
			}
			return null;
			#endif
		}

		public static SoundEffect loadSFX(string a_filepath) {
			#if DEBUG
			FileStream l_filestream = new FileStream(a_filepath, FileMode.Open);
			SoundEffect l_sfx = SoundEffect.FromStream(l_filestream);
			l_filestream.Close();
			return l_sfx;
			#else
			try {
				FileStream l_filestream = new FileStream(a_filepath, FileMode.Open);
				SoundEffect l_sfx = SoundEffect.FromStream(l_filestream);
				l_filestream.Close();
				return l_sfx;
			} catch (FileNotFoundException) {
				System.Console.WriteLine("Texture file not found!");
			} catch (DirectoryNotFoundException) {
				System.Console.WriteLine("Texture directory not found!");
			}
			return null;
			#endif
		}
	}
}

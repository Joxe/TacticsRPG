using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;

namespace TacticsRPG {
	public class Sound : GameObject {
		private string			m_file;
		private SoundEffect		m_sound;
		private static Dictionary<string, SoundEffect> m_soundLibrary = new Dictionary<string, SoundEffect>();

		public Sound(string a_file) : base(Vector2.Zero) {
			m_file = a_file;
		}

		public override void load() {
			if (!m_soundLibrary.ContainsKey(m_file)) {
				m_soundLibrary.Add(m_file, m_sound = Game.getInstance().Content.Load<SoundEffect>("Sounds//SoundEffects//" + m_file));
			}
			base.load();
		}

		public void play() {
			if (m_soundLibrary.ContainsKey(m_file)) {
				m_soundLibrary[m_file].Play();
			}
		}
	}
}

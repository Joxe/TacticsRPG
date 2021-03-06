﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace TacticsRPG {
	public class MusicHandler {
		private static MusicHandler s_instance;

		private Dictionary<string, Song> m_loadedMusic = new Dictionary<string, Song>();
		private Song s_next;

		private MusicHandler() {
			MediaPlayer.MediaStateChanged += new EventHandler<EventArgs>(MediaPlayer_MediaStateChanged);
		}

		private void MediaPlayer_MediaStateChanged(object sender, EventArgs e) {
			if (s_next != null && MediaPlayer.State != MediaState.Playing) {
				MediaPlayer.Play(s_next);
				s_next = null;
				MediaPlayer.IsRepeating = true;
			}
		}

		public static MusicHandler getInstance() {
			if (s_instance == null) {
				s_instance = new MusicHandler();
			}
			return s_instance;
		}

		public Song loadSong(string a_path) {
			if (!m_loadedMusic.ContainsKey(a_path)) {
				Song l_ret = Game.getInstance().Content.Load<Song>("Sounds//Music//" + a_path);
				m_loadedMusic.Add(a_path, l_ret);
				return l_ret;
			} else {
				return m_loadedMusic[a_path];
			}
		}

		public void unloadSong(string a_path) {
			m_loadedMusic.Remove(a_path);
		}
 
		public void play(string a_music) {
			if (!m_loadedMusic.ContainsKey(a_music)) {
				loadSong(a_music);
			}
			MediaPlayer.Play(m_loadedMusic[a_music]);
			s_next = null;
			MediaPlayer.IsRepeating = true;
		}

		public void play(string a_intro, string a_loop) {
			MediaPlayer.Play(loadSong(a_intro));
			s_next = loadSong(a_loop);
			MediaPlayer.IsRepeating = false;
		}

		public void stop() {
			MediaPlayer.Stop();
		}

		public void togglePause() {
			if (MediaPlayer.State == MediaState.Paused) {
				MediaPlayer.Resume();
			} else {
				MediaPlayer.Pause();
			}
		}

		public bool musicIsPlaying() {
			return MediaPlayer.State == MediaState.Playing;
		}

		public void setVolume(int a_volume) {
			MediaPlayer.Volume = (float)(a_volume / 100.0f);
		}
	}
}
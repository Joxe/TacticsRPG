using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class ChampionRace {
		private string m_name;
		private bool m_male;
		private Dictionary<string, int> m_baseStats;
		private static Dictionary<ChampionRace, Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>> m_sprites;

		public ChampionRace(string a_name) {
			m_name = a_name.Split('_')[0];
			m_baseStats = RacesData.getStats(this);
			if (m_sprites == null) {
				m_sprites = new Dictionary<ChampionRace, Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>>();				
			}
		}

		public void load() {
			m_sprites[this] = new Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>();
			foreach (Champion.HeroState t_heroState in Enum.GetValues(typeof(Champion.HeroState))) {
				foreach (Champion.FacingState t_facingState in Enum.GetValues(typeof(Champion.FacingState))) {
					int i = Enum.GetValues(typeof(Champion.FacingState)).Length;
					m_sprites[this].Add(
						new KeyValuePair<Champion.HeroState,Champion.FacingState>(t_heroState, t_facingState), 
						new Sprite("Races/" + m_name + "_" + (m_male ? "Male" : "Female") + "/" + t_heroState.ToString() + "/" + t_facingState.ToString(), 1)
					);
					m_sprites[this][new KeyValuePair<Champion.HeroState,Champion.FacingState>(t_heroState, t_facingState)].load();
				}
			}
		}

		public void draw(Champion a_champion) {
			m_sprites[this][new KeyValuePair<Champion.HeroState, Champion.FacingState>(a_champion.p_state, a_champion.p_facing)].draw(a_champion, a_champion.p_layer - 0.001f);
		}

		public Dictionary<string, int> getBaseStats() {
			return m_baseStats;
		}

		public int getStat(string a_stat) {
			try {
				return m_baseStats[a_stat];
			} catch (InvalidOperationException) {
				return 0;
			}
		}

		public string getName() {
			return m_name;
		}

		public bool p_male {
			get {
				return m_male;
			}
			set {
				m_male = value;
			}
		}

		public static ChampionRace getRace(string a_class) {
			foreach (ChampionRace t_race in m_sprites.Keys) {
				if (t_race.getName().Equals(a_class)) {
					return t_race;
				}
			}
			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class ChampionClass {
		private string m_name;
		private Dictionary<string, int> m_baseStats;
		private static Dictionary<ChampionClass, Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>> m_sprites;
		private bool m_male;

		public ChampionClass(string a_class) {
			m_name = a_class.Split('_')[0];
			m_baseStats = ClassesData.getStats(this);
			if (m_sprites == null) {
				m_sprites = new Dictionary<ChampionClass, Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>>();
			}
		}

		public void load() {
			m_sprites[this] = new Dictionary<KeyValuePair<Champion.HeroState, Champion.FacingState>, Sprite>();
			foreach (Champion.HeroState t_heroState in Enum.GetValues(typeof(Champion.HeroState))) {
				foreach (Champion.FacingState t_facingState in Enum.GetValues(typeof(Champion.FacingState))) {
					m_sprites[this].Add(
						new KeyValuePair<Champion.HeroState,Champion.FacingState>(t_heroState, t_facingState), 
						new Sprite("Classes/" + m_name + "_" + (m_male ? "Male" : "Female") + "/" + t_heroState.ToString() + "/" + t_facingState.ToString(), 1)
					);
					m_sprites[this][new KeyValuePair<Champion.HeroState,Champion.FacingState>(t_heroState, t_facingState)].load();
				}
			}
		}

		public void draw(Champion a_champion) {
			m_sprites[this][new KeyValuePair<Champion.HeroState,Champion.FacingState>(a_champion.p_state, a_champion.p_facing)].draw(a_champion);
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

		public override string ToString() {
			return m_name;
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

		public Vector2 getSpriteSize() {
			return m_sprites[this].First().Value.getSize();
		}

		public static ChampionClass getClass(string a_class) {
			foreach (ChampionClass t_class in m_sprites.Keys) {
				if (t_class.getName().Equals(a_class)) {
					return t_class;
				}
			}
			return null;
		}
	}
}

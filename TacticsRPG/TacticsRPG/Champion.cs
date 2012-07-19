using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class Champion : BattlefieldObject {
		private string m_name;
		private ChampionClass m_class;
		private ChampionRace m_race;
		private Player m_owningPlayer;
		private int m_speed;
		private bool m_hasMoved;
		private bool m_actionTaken;
		private bool m_male;
		private int m_faceNumber;

		private Dictionary<string, int> m_stats = new Dictionary<string, int>();
		private Dictionary<KeyValuePair<HeroState, FacingState>, Sprite> m_faceSprites;
		private Dictionary<KeyValuePair<HeroState, FacingState>, Sprite> m_bodySprites;

		private List<Ability> m_abilities;

		private HeroState m_heroState = HeroState.Idle;
		public enum HeroState {
			Idle,	Walking
		}

		public Champion(Vector2 a_position, string a_name, string a_class, bool a_male, string a_race) 
			: base(((GameState)Game.getInstance().getCurrentState()).getTileMap().getTile(a_position).p_position + new Vector2(64, -64)) 
		{
			m_name = a_name;
			m_layer = 0.300f;
			m_male = a_male;
			m_faceNumber = 1;
			m_race = RacesData.getRace(a_race);
			m_class = ClassesData.getClass(a_class);

			m_stats = m_class.getBaseStats();
			foreach (KeyValuePair<string, int> l_kvPair in RacesData.getStats(m_race)) {
				m_stats[l_kvPair.Key] += l_kvPair.Value;
			}
			m_stats.Add("CurrentHealth", m_stats["MaxHealth"]);
			m_stats.Add("CurrentMana", m_stats["MaxMana"]);
			m_stats.Add("Level", 1);
			m_stats.Add("MoveLeft", m_stats["Move"]);
			p_speed = m_stats["Speed"];

			m_currentPosition = ((GameState)Game.getInstance().getCurrentState()).getTileMap().getTile(a_position);
			m_bodySprites = new Dictionary<KeyValuePair<HeroState, FacingState>, Sprite>();
		}

		public override void load() {
			foreach (HeroState l_heroState in Enum.GetValues(typeof(HeroState))) {
				foreach (FacingState l_facingState in Enum.GetValues(typeof(FacingState))) {
					int i = Enum.GetValues(typeof(FacingState)).Length;
					KeyValuePair<HeroState, FacingState> l_kvPair = new KeyValuePair<HeroState, FacingState>(l_heroState, l_facingState);
					m_bodySprites.Add(l_kvPair, new Sprite("Classes/" + m_class.getName() + (m_male ? "Male" : "Female") + "/" + l_heroState.ToString() + "/" + l_facingState.ToString() + ".png", 1));
					m_bodySprites[l_kvPair].load();
				}
			}

			m_hitbox = new Rectangle(m_position.X, m_position.Y, m_bodySprites.First().Value.getSize().X, m_bodySprites.First().Value.getSize().Y);
			m_hitbox.setParent(this);

			base.load();

			calculateAttack();
			calculateDefense();
			calculateMagic();
			calculateResist();
		}

		public override void update() {
			m_hitbox.update();
			base.update();
		}

		public override void draw() {
			m_bodySprites[new KeyValuePair<HeroState, FacingState>(p_state, p_facing)].draw(this, p_layer - 0.001f);
			base.draw();
		}

		#region Movement & Facing
		public override bool moveTo(Tile a_tile) {
			if (m_moveQueue.Count == 0) {
				if ((m_moveQueue = AStar.findPath(m_currentPosition, a_tile, AStar.PathfindState.Hexagon)).Count > m_stats["MoveLeft"] + 1) {
					m_moveQueue.Clear();
				} else {
					m_hasMoved = true;
					m_stats["MoveLeft"] -= m_moveQueue.Count - 1;
					p_speed += 50;
					return true;
				}
			}
			return false;
		}
		#endregion

		#region Stat Calculators
		public void calculateAttack() {
			if (m_stats.ContainsKey("Attack")) {
				m_stats["Attack"] = (int)Math.Floor(StatsCalculator.summedAttack(this) + 0.5f);
			} else {
				m_stats.Add("Attack", (int)Math.Floor(StatsCalculator.summedAttack(this) + 0.5f));
			}
		}

		public void calculateDefense() {
			if (m_stats.ContainsKey("Defense")) {
				m_stats["Defense"] = (int)Math.Floor(StatsCalculator.summedDefense(this) + 0.5f);
			} else {
				m_stats.Add("Defense", (int)Math.Floor(StatsCalculator.summedDefense(this) + 0.5f));
			}
		}

		public void calculateMagic() {
			if (m_stats.ContainsKey("MagicAttack")) {
				m_stats["MagicAttack"] = (int)Math.Floor(StatsCalculator.summedMagic(this) + 0.5f);
			} else {
				m_stats.Add("MagicAttack", (int)Math.Floor(StatsCalculator.summedMagic(this) + 0.5f));
			}
		}

		public void calculateResist() {
			if (m_stats.ContainsKey("MagicResist")) {
				m_stats["MagicResist"] = (int)Math.Floor(StatsCalculator.summedResist(this) + 0.5f);
			} else {
				m_stats.Add("MagicResist", (int)Math.Floor(StatsCalculator.summedResist(this) + 0.5f));
			}
		}
		#endregion

		#region Champion Properties
		public HeroState p_state {
			get {
				return m_heroState;
			}
			set {
				m_heroState = value;
			}
		}

		public Player p_owningPlayer {
			get {
				return m_owningPlayer;
			}
			set {
				m_owningPlayer = value;
			}
		}

		public int p_speed {
			get {
				return m_speed;
			}
			set {
				m_speed = Math.Max(0, value);
			}
		}

		public bool p_hasMoved {
			get {
				return m_hasMoved;
			}
			set {
				m_hasMoved = value;
			}
		}

		public bool p_actionTaken {
			get {
				return m_actionTaken;
			}
			set {
				m_actionTaken = value;
			}
		}
		#endregion

		public override string ToString() {
			return m_name + " " + m_race.getName() + " " + m_class.getName();
		}

		public string statToString(string a_stat) {
			if (m_stats.ContainsKey(a_stat)) {
				return a_stat + ": " + m_stats[a_stat].ToString();
			} else {
				return "null";
			}
		}

		public int getStat(string a_stat) {
			if (m_stats.ContainsKey(a_stat)) {
				return m_stats[a_stat];
			} else {
				return 0;
			}
		}

		public LinkedList<Text> statsToTextList() {
			LinkedList<Text> l_list = new LinkedList<Text>();
			foreach (KeyValuePair<string, int> l_kvPair in m_stats) {
				l_list.AddLast(new Text(Vector2.Zero, l_kvPair.Key + ": " + l_kvPair.Value, "Arial", Color.Black, false));
			}
			return l_list;
		}

		public ChampionRace getRace() {
			return m_race;
		}

		public ChampionClass getClass() {
			return m_class;
		}

		public void attack(Champion a_champion) {
			faceTile(a_champion.getTile());
			p_actionTaken = true;
			a_champion.damage(MathManager.attack(this, a_champion));
		}

		public void damage(int a_damage) {
			m_stats["CurrentHealth"] -= a_damage;
			if (m_stats["CurrentHealth"] <= 0) {
				((GameState)Game.getInstance().getCurrentState()).removeChampion(this);
			}
		}

		public string getName() {
			return m_name;
		}

		public void kill() {
			m_currentPosition.p_object = null;
		}

		public override int CompareTo(GameObject a_gameObject) {
			return p_speed.CompareTo(((Champion)a_gameObject).p_speed);
		}

		public void championsTurn() {
			p_actionTaken = false;
			m_stats["MoveLeft"] = m_stats["Move"];
		}

		public List<Ability> getAbilities() {
			List<Ability> l_returnList = new List<Ability>();
			
			foreach (Ability l_ability in m_class.getAbilities()) {
				l_returnList.Add(l_ability);
			}
			foreach (Ability l_ability in m_race.getAbilities()) {
				l_returnList.Add(l_ability);
			}

			return l_returnList;
		}

		public Ability getAbility(string a_ability) {
			foreach (Ability l_ability in m_class.getAbilities()) {
				if (l_ability.getName().Equals(a_ability)) {
					return l_ability;
				}
			}
			foreach (Ability l_ability in m_race.getAbilities()) {
				if (l_ability.getName().Equals(a_ability)) {
					return l_ability;
				}
			}
			return null;
		}
	}
}

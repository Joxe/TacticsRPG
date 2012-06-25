using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TacticsRPG {
	public class Champion : TexturedObject {
		private string m_name;
		private ChampionClass m_class;
		private ChampionRace m_race;
		private Tile m_currentPosition;
		private float m_lerpValue;
		private readonly Vector2 m_tileOffset = new Vector2(64, -64);
		private readonly Vector2 m_targetOffset = new Vector2(-10, 150);
		private static Sprite m_targetRecticle;
		private Player m_owningPlayer;
		private int m_speed;
		private bool m_actionTaken;
		private bool m_male;
		private int m_faceNumber;

		private Stack<Tile> m_moveQueue = new Stack<Tile>();
		private Dictionary<string, int> m_stats = new Dictionary<string, int>();
		private Dictionary<KeyValuePair<HeroState, FacingState>, Sprite> m_faceSprites;
		private Dictionary<KeyValuePair<HeroState, FacingState>, Sprite> m_bodySprites;

		private HeroState m_heroState = HeroState.Idle;
		public enum HeroState {
			Idle,	Walking
		}

		private TargetState m_targetState = TargetState.Normal;
		public enum TargetState {
			Normal,	Targeted
		}

		private FacingState m_facingState = FacingState.BottomRight;
		public enum FacingState {
			Up, TopLeft, TopRight, BottomLeft, BottomRight, Down
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
			foreach (KeyValuePair<string, int> t_kvPair in RacesData.getStats(m_race)) {
				m_stats[t_kvPair.Key] += t_kvPair.Value;
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
			foreach (HeroState t_heroState in Enum.GetValues(typeof(HeroState))) {
				foreach (FacingState t_facingState in Enum.GetValues(typeof(FacingState))) {
					int i = Enum.GetValues(typeof(FacingState)).Length;
					KeyValuePair<HeroState, FacingState> t_kvPair = new KeyValuePair<HeroState, FacingState>(t_heroState, t_facingState);
					m_bodySprites.Add(t_kvPair, new Sprite("Classes/" + m_class.getName() + (m_male ? "Male" : "Female") + "/" + t_heroState.ToString() + "/" + t_facingState.ToString(), 1));
					m_bodySprites[t_kvPair].load();
				}
			}

			m_hitbox = new Rectangle(m_position.X, m_position.Y, m_bodySprites.First().Value.getSize().X, m_bodySprites.First().Value.getSize().Y);
			m_hitbox.setParent(this);

			if (m_targetRecticle == null) {
				m_targetRecticle = new Sprite("Indicators/target", 1);
				m_targetRecticle.load();
				m_targetRecticle.p_offset = m_targetOffset;
			}

			calculateAttack();
			calculateDefense();
			calculateMagic();
			calculateResist();
		}

		public override void update() {
			if (m_moveQueue.Count > 0) {
				m_currentPosition.p_champion = null;
				if (m_lerpValue < 1.0f) {
					m_position = Vector2.Lerp(m_currentPosition.p_position, m_moveQueue.Peek().p_position, m_lerpValue) + m_tileOffset;
					m_lerpValue += 0.05f;
				} else {
					m_position = m_moveQueue.Peek().p_position + m_tileOffset;
					m_currentPosition = m_moveQueue.Pop();
					if (m_moveQueue.Count > 0) {
						faceTile(m_moveQueue.Peek());
					} else {
						m_currentPosition.p_champion = this;
					}
					m_lerpValue = 0.0f;
				}
			}
			m_hitbox.update();
			base.update();
		}

		public override void draw() {
			m_bodySprites[new KeyValuePair<HeroState, FacingState>(p_state, p_facing)].draw(this, p_layer - 0.001f);
			if (m_targetState == TargetState.Targeted) {
				m_targetRecticle.draw(this, m_layer + 0.001f);
			}
			base.draw();
		}

		#region Movement & Facing
		public bool moveTo(Tile a_tile) {
			if (m_moveQueue.Count == 0) {
				if ((m_moveQueue = AStar.findPath(m_currentPosition, a_tile, AStar.PathfindState.Hexagon)).Count > m_stats["MoveLeft"] + 1) {
					m_moveQueue.Clear();
				} else {
					m_stats["MoveLeft"] -= m_moveQueue.Count - 1;
					p_speed += 50;
					return true;
				}
			}
			return false;
		}
		#endregion

		#region Champion Facing
		public void faceTile(Tile a_tile) {
			if (a_tile.getMapPosition().X < m_currentPosition.getMapPosition().X) {
				if (MathManager.isEven((int)m_currentPosition.getMapPosition().X)) {
					m_facingState = a_tile.getMapPosition().Y == m_currentPosition.getMapPosition().Y
						? m_facingState = FacingState.TopLeft
						: m_facingState = FacingState.BottomLeft;
				} else {
					m_facingState = a_tile.getMapPosition().Y == m_currentPosition.getMapPosition().Y
						? m_facingState = FacingState.BottomLeft
						: m_facingState = FacingState.TopLeft;
				}
			} else if (a_tile.getMapPosition().X > m_currentPosition.getMapPosition().X) {
				if (MathManager.isEven((int)m_currentPosition.getMapPosition().X)) {
					m_facingState = a_tile.getMapPosition().Y == m_currentPosition.getMapPosition().Y 
						? m_facingState = FacingState.TopRight
						: m_facingState = FacingState.BottomRight;
				} else {
					m_facingState = a_tile.getMapPosition().Y == m_currentPosition.getMapPosition().Y 
						? m_facingState = FacingState.BottomRight
						: m_facingState = FacingState.TopRight;
				}
			} else {
				m_facingState = a_tile.getMapPosition().Y < m_currentPosition.getMapPosition().Y
					? m_facingState = FacingState.Up
					: m_facingState = FacingState.Down;
			}
		}

		public void faceObject(GameObject a_gameObject) {
			double t_angle = MathManager.angle(
				m_position + m_hitbox.p_dimensions / 2, 
				a_gameObject.p_position + new Vector2(a_gameObject.getHitBox().X, a_gameObject.getHitBox().Y) / 2
			);
			
			if (t_angle > 0) {
				if (t_angle <= 70) {
					m_facingState = FacingState.TopRight;
				} else if (t_angle <= 130 && t_angle > 70) {
					m_facingState = FacingState.Up;
				} else if (t_angle <= 180 && t_angle > 130) {
					m_facingState = FacingState.TopLeft;
				}
			} else {
				if (t_angle >= -70) {
					m_facingState = FacingState.BottomRight;
				} else if (t_angle >= -130 && t_angle < -70) {
					m_facingState = FacingState.Down;
				} else if (t_angle >= -180 && t_angle < -130) {
					m_facingState = FacingState.BottomLeft;
				}
			}
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
		public TargetState p_targetState {
			get {
				return m_targetState;
			}
			set {
				m_targetState = value;
			}
		}

		public HeroState p_state {
			get {
				return m_heroState;
			}
			set {
				m_heroState = value;
			}
		}

		public FacingState p_facing {
			get {
				return m_facingState;
			}
			set {
				m_facingState = value;
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
			LinkedList<Text> t_list = new LinkedList<Text>();
			foreach (KeyValuePair<string, int> t_kvPair in m_stats) {
				t_list.AddLast(new Text(Vector2.Zero, t_kvPair.Key + ": " + t_kvPair.Value, "Arial", Color.Black, false));
			}
			return t_list;
		}

		public ChampionRace getRace() {
			return m_race;
		}

		public ChampionClass getClass() {
			return m_class;
		}

		public Vector2 getMapPosition() {
			return m_currentPosition.getMapPosition();
		}

		public Tile getTile() {
			return m_currentPosition;
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
			m_currentPosition.p_champion = null;
		}

		public override int CompareTo(GameObject a_gameObject) {
			return p_speed.CompareTo(((Champion)a_gameObject).p_speed);
		}

		public void championsTurn() {
			p_actionTaken = false;
			m_stats["MoveLeft"] = m_stats["Move"];
		}
	}
}

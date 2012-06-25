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

		private Stack<Tile> m_moveQueue = new Stack<Tile>();
		private Dictionary<string, int> m_stats = new Dictionary<string, int>();

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

		public Champion(Vector2 a_position, string a_name, string a_class, string a_race) 
			: base(((GameState)Game.getInstance().getCurrentState()).getTileMap().getTile(a_position).p_position + new Vector2(64, -64)) 
		{
			m_name = a_name;
			m_layer = 0.300f;
			m_race = ChampionRace.getRace(a_race.Split('_')[0]);
			m_class = ChampionClass.getClass(a_class.Split('_')[0]);
			foreach (KeyValuePair<string, int> t_kvPair in m_class.getBaseStats()) {
				m_stats.Add(t_kvPair.Key, t_kvPair.Value);
			}
			foreach (KeyValuePair<string, int> t_kvPair in RacesData.getStats(m_race)) {
				m_stats[t_kvPair.Key] += t_kvPair.Value;
			}
			m_stats.Add("currentHealth", m_stats["maxHealth"]);
			m_stats.Add("currentMana", m_stats["maxMana"]);
			m_stats.Add("level", 1);
			calculateAttack();
			calculateDefense();
			calculateMagic();
			calculateResist();
			m_currentPosition = ((GameState)Game.getInstance().getCurrentState()).getTileMap().getTile(a_position);
			
			if (m_targetRecticle == null) {
				m_targetRecticle = new Sprite("Indicators/target", 1);
				m_targetRecticle.load();
				m_targetRecticle.p_offset = m_targetOffset;
			}
		}

		public override void load() {
			m_hitbox = new Rectangle(m_position.X, m_position.Y, m_class.getSpriteSize().X, m_class.getSpriteSize().Y);
			m_hitbox.setParent(this);
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
			m_class.draw(this);
			m_race.draw(this);
			if (m_targetState == TargetState.Targeted) {
				m_targetRecticle.draw(this, m_layer + 0.001f);
			}
			base.draw();
		}

		public void moveTo(Tile a_tile) {
			if (m_moveQueue.Count == 0) {
				if ((m_moveQueue = AStar.findPath(m_currentPosition, a_tile, AStar.PathfindState.Hexagon)).Count > m_stats["move"] + 1) {
					m_moveQueue.Clear();
				}
			}
		}

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

		#region Stat-calculations
		public void calculateAttack() {
			if (m_stats.ContainsKey("attack")) {
				m_stats["attack"] = (int)Math.Floor(StatsCalculator.summedAttack(this) + 0.5f);
			} else {
				m_stats.Add("attack", (int)Math.Floor(StatsCalculator.summedAttack(this) + 0.5f));
			}
		}

		public void calculateDefense() {
			if (m_stats.ContainsKey("defense")) {
				m_stats["defense"] = (int)Math.Floor(StatsCalculator.summedDefense(this) + 0.5f);
			} else {
				m_stats.Add("defense", (int)Math.Floor(StatsCalculator.summedDefense(this) + 0.5f));
			}
		}

		public void calculateResist() {
			if (m_stats.ContainsKey("magicResist")) {
				m_stats["magicResist"] = (int)Math.Floor(StatsCalculator.summedResist(this) + 0.5f);
			} else {
				m_stats.Add("magicResist", (int)Math.Floor(StatsCalculator.summedResist(this) + 0.5f));
			}
		}

		public void calculateMagic() {
			if (m_stats.ContainsKey("magicAttack")) {
				m_stats["magicAttack"] = (int)Math.Floor(StatsCalculator.summedMagic(this) + 0.5f);
			} else {
				m_stats.Add("magicAttack", (int)Math.Floor(StatsCalculator.summedMagic(this) + 0.5f));
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
			a_champion.damage(MathManager.attack(this, a_champion));
		}

		public void damage(int a_damage) {
			m_stats["currentHealth"] -= a_damage;
			if (m_stats["currentHealth"] <= 0) {
				((GameState)Game.getInstance().getCurrentState()).removeChampion(this);
			}
		}

		public string getName() {
			return m_name;
		}

		public void kill() {
			m_currentPosition.p_champion = null;
		}
	}
}

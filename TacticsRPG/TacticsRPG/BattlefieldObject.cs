using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace TacticsRPG {
	public class BattlefieldObject : TexturedObject {
		protected static Sprite m_targetRecticle;
		protected Tile m_currentPosition;
		protected Stack<Tile> m_moveQueue = new Stack<Tile>();
		private float m_lerpValue;
		private readonly Vector2 m_tileOffset = new Vector2(64, -64);
		private readonly Vector2 m_targetOffset = new Vector2(-10, 150);
		
		protected TargetState m_targetState = TargetState.Normal;
		public enum TargetState {
			Normal,	Targeted
		}

		protected FacingState m_facingState = FacingState.BottomRight;
		public enum FacingState {
			Up, TopLeft, TopRight, BottomLeft, BottomRight, Down
		}
		
		#region Constructor & Load
		public BattlefieldObject(Vector2 a_position) : base(a_position) {

		}

		public override void load() {
			if (m_targetRecticle == null) {
				m_targetRecticle = new Sprite("Indicators/target.png", 1);
				m_targetRecticle.load();
				m_targetRecticle.p_offset = m_targetOffset;
			}
		}
		#endregion

		#region Update & Draw
		public override void update() {
			if (m_moveQueue.Count > 0) {
				m_currentPosition.p_object = null;
				if (m_lerpValue < 1.0f) {
					m_position = Vector2.Lerp(m_currentPosition.p_position, m_moveQueue.Peek().p_position, m_lerpValue) + m_tileOffset;
					m_lerpValue += 0.05f;
				} else {
					m_position = m_moveQueue.Peek().p_position + m_tileOffset;
					m_currentPosition = m_moveQueue.Pop();
					if (m_moveQueue.Count > 0) {
						faceTile(m_moveQueue.Peek());
					} else {
						m_currentPosition.p_object = this;
					}
					m_lerpValue = 0.0f;
				}
			}
			base.update();
		}

		public override void draw() {
			if (m_targetState == TargetState.Targeted) {
				m_targetRecticle.draw(this, m_layer + 0.001f);
			}
			base.draw();
		}
		#endregion

		#region Movement & Facing
		public virtual bool moveTo(Tile a_tile) {
			if (m_moveQueue.Count == 0) {
				m_moveQueue = ((GameState)Game.getInstance().getCurrentState()).m_pathFinder.findPath(m_currentPosition.getMapPosition(), a_tile.getMapPosition());
			}
			return true;
		}

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
			double l_angle = MathManager.angle(
				m_position + m_hitbox.p_dimensions / 2, 
				a_gameObject.p_position + new Vector2(a_gameObject.getHitBox().X, a_gameObject.getHitBox().Y) / 2
			);
			
			if (l_angle > 0) {
				if (l_angle <= 70) {
					m_facingState = FacingState.TopRight;
				} else if (l_angle <= 130 && l_angle > 70) {
					m_facingState = FacingState.Up;
				} else if (l_angle <= 180 && l_angle > 130) {
					m_facingState = FacingState.TopLeft;
				}
			} else {
				if (l_angle >= -70) {
					m_facingState = FacingState.BottomRight;
				} else if (l_angle >= -130 && l_angle < -70) {
					m_facingState = FacingState.Down;
				} else if (l_angle >= -180 && l_angle < -130) {
					m_facingState = FacingState.BottomLeft;
				}
			}
		}

		public TargetState getTargetState() {
			return m_targetState;
		}

		public FacingState getFacingState() {
			return m_facingState;
		}
		#endregion

		public Vector2 getMapPosition() {
			return m_currentPosition.getMapPosition();
		}

		public Tile getTile() {
			return m_currentPosition;
		}

		public void addEffect(Effect a_effect) {
			if (this is Champion) {
				a_effect.invokeEffect((Champion)this);
			}
		}

		public virtual void select() {
			m_targetState = TargetState.Targeted;
		}

		public virtual void deselect() {
			m_targetState = TargetState.Normal;
		}

		public virtual void kill() {
			m_currentPosition.p_object = null;
		}
	}
}

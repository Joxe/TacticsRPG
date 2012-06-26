using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TacticsRPG {
	public class TurnBaser {
		private Player m_localPlayer;
		private Player m_remotePlayer;

		public TurnBaser() {
			m_localPlayer = new Player("LOCAL");
			m_remotePlayer = new Player("REMOTE");
		}

		public void update() {
			
		}
	}
}

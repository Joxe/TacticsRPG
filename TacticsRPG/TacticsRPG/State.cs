using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG {
	public abstract class State {
		protected LinkedList<GameObject> m_gameObjectList;
		protected LinkedList<LinkedList<GuiObject>> m_guiList;

		public State() {
			m_gameObjectList = new LinkedList<GameObject>();
			m_guiList = new LinkedList<LinkedList<GuiObject>>();
		}

		public abstract void load();
		
		public virtual void update() {
			foreach (LinkedList<GuiObject> l_list in m_guiList) {
				foreach (GuiObject l_go in l_list) {
					if (l_go.p_visible) {
						l_go.update();
					}
				}
			}
			foreach (GameObject l_gameObject in m_gameObjectList) {
				l_gameObject.p_isInCamera = CameraHandler.isInCamera(l_gameObject);
				if (l_gameObject.p_isInCamera) {
					l_gameObject.update();
				} else {
					//TODO out-of-camera update
				}
			}
		}

		public virtual void draw() {
			foreach (LinkedList<GuiObject> l_list in m_guiList) {
				foreach (GuiObject l_go in l_list) {
					if (l_go.p_visible) {
						l_go.draw();
					}
				}
			}
			foreach (GameObject l_gameObject in m_gameObjectList) {
				if (l_gameObject.p_isInCamera) {
					l_gameObject.draw();
				}
			}
		}

		public virtual void addObject(GameObject a_gameObject) {
			if (!m_gameObjectList.Contains(a_gameObject)) {
				m_gameObjectList.AddLast(a_gameObject);
			}
		}

		public virtual void removeObject(GameObject a_gameObject) {
			m_gameObjectList.Remove(a_gameObject);
		}
	}
}

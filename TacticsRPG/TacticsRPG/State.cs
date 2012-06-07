using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace TacticsRPG
{
	public abstract class State
	{
		protected LinkedList<GameObject> m_gameObjectList;
		protected LinkedList<LinkedList<GuiObject>> m_guiList;

		public State() {
			m_gameObjectList = new LinkedList<GameObject>();
			m_guiList = new LinkedList<LinkedList<GuiObject>>();
		}

		public abstract void load();
		
		public virtual void update() {
			foreach (LinkedList<GuiObject> t_list in m_guiList) {
				foreach (GuiObject t_go in t_list) {
					if (t_go.p_visible) {
						t_go.update();
					}
				}
			}
			foreach (GameObject t_gameObject in m_gameObjectList) {
				if (t_gameObject.p_isInCamera = CameraHandler.isInCamera(t_gameObject)) {
					t_gameObject.update();
				} else {
					//TODO out-of-camera update
				}
			}
		}

		public virtual void draw() {
			foreach (LinkedList<GuiObject> t_list in m_guiList) {
				foreach (GuiObject t_go in t_list) {
					if (t_go.p_visible) {
						t_go.draw();
					}
				}
			}
			foreach (GameObject t_gameObject in m_gameObjectList) {
				if (t_gameObject.p_isInCamera) {
					t_gameObject.draw();
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

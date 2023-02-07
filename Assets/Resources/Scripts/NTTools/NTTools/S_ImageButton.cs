using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace NTTools
{
	[Serializable]
	public class S_ImageButton
	{
		public string Name;
		public Texture Image;
		public Vector2 Size;
		public float Angle;
		public string LevelToLoad;
		public Rect Position;
		public void OnGUI()
		{
			//bool childControleContainsMousePos =
			//this.ChildControleContains(Event.current.mousePosition);


			switch (Event.current.type)
			{
				case EventType.mouseDown:
	#if DEBUG
					{
						UnityEngine.Debug.Log("MouseDown" + this.Name);
					}
	#endif
					UnityEngine.Debug.Log("loading level " + LevelToLoad);
					//Application.LoadLevelAdditive(LevelToLoad);
					break;

				case EventType.Repaint:
	#if DEBUG
					{
						UnityEngine.Debug.Log("repaint");
					}
	#endif
					break;
			}
		}
	}
}


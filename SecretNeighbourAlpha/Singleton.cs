using System;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		public static T Instance
		{
			get
			{
				bool shuttingDown = Singleton<T>.m_ShuttingDown;
				T result;
				if (shuttingDown)
				{
					string str = "[Singleton] Instance '";
					Type typeFromHandle = typeof(T);
					Debug.LogWarning(str + ((typeFromHandle != null) ? typeFromHandle.ToString() : null) + "' already destroyed. Returning null.");
					T t = default(T);
					result = t;
				}
				else
				{
					object @lock = Singleton<T>.m_Lock;
					object obj = @lock;
					T t;
					lock (obj)
					{
						bool flag2 = Singleton<T>.m_Instance == null;
						if (flag2)
						{
							Singleton<T>.m_Instance = (T)((object)Object.FindObjectOfType(typeof(T)));
							bool flag3 = Singleton<T>.m_Instance == null;
							if (flag3)
							{
								GameObject gameObject = new GameObject();
								Singleton<T>.m_Instance = gameObject.AddComponent<T>();
								gameObject.name = typeof(T).ToString() + " (Singleton)";
								Object.DontDestroyOnLoad(gameObject);
							}
						}
						t = Singleton<T>.m_Instance;
					}
					result = t;
				}
				return result;
			}
		}

		private void OnApplicationQuit()
		{
			Singleton<T>.m_ShuttingDown = true;
		}

		private void OnDestroy()
		{
			Singleton<T>.m_ShuttingDown = true;
		}

		public Singleton()
		{
		}

		static Singleton()
		{
		}

		private static bool m_ShuttingDown = false;

		private static object m_Lock = new object();

		private static T m_Instance;
	}
}

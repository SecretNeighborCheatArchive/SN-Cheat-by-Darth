using System;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	public class Loader
	{
		public static void Init()
		{
			Loader.gameObject = new GameObject();
			Loader.gameObject.AddComponent<Cheat>();
			Loader.gameObject.AddComponent<Utilities>();
			Loader.gameObject.AddComponent<DrawESP>();
			Object.DontDestroyOnLoad(Loader.gameObject);
		}

		public static void Unload()
		{
			Object.Destroy(Loader.gameObject);
		}

		public Loader()
		{
		}

		private static GameObject gameObject;
	}
}

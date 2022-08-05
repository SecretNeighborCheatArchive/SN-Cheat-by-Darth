using System;
using System.Threading;
using GameModes.GameplayMode;
using GameModes.GameplayMode.Interactables.InventoryItems.Base;
using GameModes.GameplayMode.Levels.Basement.Objectives;
using GameModes.GameplayMode.Players;
using GameModes.LobbyMode.LobbyPlayers;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	internal class Utilities : Singleton<Utilities>
	{
		private void Start()
		{
			new Thread(new ThreadStart(this.CollectLbyPlayers)).Start();
		}

		private void LateUpdate()
		{
			bool flag = !this.gameStarted;
			if (flag)
			{
				new Thread(new ThreadStart(this.CollectGameInfo)).Start();
				this.gameStarted = true;
			}
		}

		private void CollectGameInfo()
		{
			for (;;)
			{
				Singleton<DrawESP>.Instance.gameControllerPlayers = Object.FindObjectOfType<GameControllerPlayers>();
				Singleton<Cheat>.Instance.playerList = Object.FindObjectsOfType<Player>();
				Singleton<Cheat>.Instance.itemList = Object.FindObjectsOfType<InventoryItem>();
				Singleton<Cheat>.Instance.Keys = Object.FindObjectsOfType<BasementDoorKeyInventoryItem>();
				Thread.Sleep(10000);
			}
		}

		private void CollectLbyPlayers()
		{
			for (;;)
			{
				Singleton<Cheat>.Instance.lobbyPlrs = Object.FindObjectsOfType<LobbyPlayer>();
				Thread.Sleep(1000);
			}
		}

		public Utilities()
		{
		}

		private bool gameStarted;
	}
}

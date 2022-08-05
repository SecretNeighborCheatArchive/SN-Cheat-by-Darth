using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using GameModes.GameplayMode.Actors.Implementations.Neighbours;
using GameModes.GameplayMode.Actors.Shared;
using GameModes.GameplayMode.Actors.Shared.Configuration;
using GameModes.GameplayMode.GlobalQuests.CrowQuest;
using GameModes.GameplayMode.GlobalQuests.LeversQuest;
using GameModes.GameplayMode.Interactables;
using GameModes.GameplayMode.Interactables.InventoryItems.Base;
using GameModes.GameplayMode.Interactables.SceneObjects;
using GameModes.GameplayMode.Levels.Basement.Objectives;
using GameModes.GameplayMode.Players;
using GameModes.LobbyMode.LobbyPlayers;
using HoloNetwork;
using HoloNetwork.Messaging.Implementations.ProviderMessages;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	public class Cheat : MonoBehaviour
	{
		private void Start()
		{
			this.buffTimer = new System.Timers.Timer();
			this.buffTimer.Elapsed += this.buffLoopTmr;
			this.buffTimer.Interval = 2000.0;
			this.xrayTimer = new System.Timers.Timer();
			this.xrayTimer.Elapsed += this.xrayAll;
			this.xrayTimer.Interval = 4000.0;
			this.lvlTimer = new System.Timers.Timer();
			this.lvlTimer.Elapsed += this.lvlUpTmr;
			this.lvlTimer.Interval = 500.0;
		}

		private void Update()
		{
			bool flag = !this.inDate;
			if (!flag)
			{
				bool flag2 = this.godmode;
				if (flag2)
				{
					Player localPlayer = this.localPlayer;
					if (localPlayer != null)
					{
						localPlayer.buffs.ApplyBuff(30, Array.Empty<object>());
					}
					Player localPlayer2 = this.localPlayer;
					if (localPlayer2 != null)
					{
						localPlayer2.buffs.VanishBuff(3);
					}
				}
				bool flag3 = this.noclip && this.localPlayer != null;
				if (flag3)
				{
					this.localPlayer.currentActor.GetComponent<ActorMovement>().config.gravityMultilayer = 0f;
					this.localPlayer.currentActor.actorMovement.DisableCollider();
					Transform transform = this.localPlayer.currentActor.transform;
					float num = 20f;
					bool key = Input.GetKey(119);
					if (key)
					{
						transform.position += transform.forward * Time.deltaTime * num;
					}
					bool key2 = Input.GetKey(115);
					if (key2)
					{
						transform.position += -transform.forward * Time.deltaTime * num;
					}
					bool key3 = Input.GetKey(97);
					if (key3)
					{
						transform.position += -transform.right * Time.deltaTime * num;
					}
					bool key4 = Input.GetKey(100);
					if (key4)
					{
						transform.position += transform.right * Time.deltaTime * num;
					}
					bool key5 = Input.GetKey(32);
					if (key5)
					{
						transform.position += transform.up * Time.deltaTime * num;
					}
					bool key6 = Input.GetKey(304);
					if (key6)
					{
						transform.position += -transform.up * Time.deltaTime * num;
					}
				}
				else
				{
					bool flag4 = !this.noclip && this.localPlayer != null;
					if (flag4)
					{
						this.localPlayer.currentActor.GetComponent<ActorMovement>().config.gravityMultilayer = 1.2f;
						this.localPlayer.currentActor.actorMovement.EnableCollider();
					}
				}
				Player localPlayer3 = this.localPlayer;
				ActorMovementConfig actorMovementConfig = ((localPlayer3 != null) ? localPlayer3.currentActor.GetComponent<ActorMovement>().config : null);
				bool flag5 = this.addedSprintSpeed > 0f && this.localPlayer != null;
				if (flag5)
				{
					actorMovementConfig.walkVelocity = this.addedSprintSpeed;
				}
				bool flag6 = this.addedCrouchSpeed > 0f && this.localPlayer != null;
				if (flag6)
				{
					actorMovementConfig.crouchVelocity = this.addedCrouchSpeed;
				}
				bool flag7 = this.addedJumpSpeed > 0f && this.localPlayer != null;
				if (flag7)
				{
					actorMovementConfig.verticalJumpVelocity = this.addedJumpSpeed;
				}
				bool flag8 = this.lobbyGodmode;
				if (flag8)
				{
					new Task(delegate()
					{
						foreach (Player player4 in this.playerList)
						{
							bool flag20 = player4.buffs.ContainsBuff(3);
							if (flag20)
							{
								if (player4 != null)
								{
									player4.buffs.VanishBuff(3);
								}
							}
							bool flag21 = player4.buffs.ContainsBuff(10);
							if (flag21)
							{
								if (player4 != null)
								{
									player4.buffs.VanishBuff(10);
								}
							}
							if (player4 != null)
							{
								player4.buffs.ApplyBuffOnNextFrame(30);
							}
							Task.Delay(200);
						}
					}).Start();
				}
				foreach (LobbyPlayer lobbyPlayer in this.lobbyPlrs)
				{
					bool flag9 = lobbyPlayer != null && lobbyPlayer.displayName == "KICK-BOT";
					if (flag9)
					{
						HoloNet.SendReliable(NetPlayerDisconnectedMessage.Create(lobbyPlayer.net.owner), 1);
					}
					Task.Delay(200);
				}
				bool keyDown = Input.GetKeyDown(277);
				if (keyDown)
				{
					this.draw = !this.draw;
				}
				else
				{
					bool keyDown2 = Input.GetKeyDown(98);
					if (keyDown2)
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.currentActor.player.buffs.ApplyBuffOnNextFrame(14);
						}
						Player localPlayer5 = this.localPlayer;
						if (localPlayer5 != null)
						{
							localPlayer5.currentActor.player.buffs.ApplyBuffOnNextFrame(13);
						}
					}
					else
					{
						bool keyDown3 = Input.GetKeyDown(120);
						if (keyDown3)
						{
							this.noclip = !this.noclip;
						}
						else
						{
							bool keyDown4 = Input.GetKeyDown(109);
							if (keyDown4)
							{
								this.tpConsumables();
							}
							else
							{
								bool keyDown5 = Input.GetKeyDown(282);
								if (keyDown5)
								{
									foreach (BasementDoorKeyInventoryItem basementDoorKeyInventoryItem in this.Keys)
									{
										basementDoorKeyInventoryItem.UpdatePositionAndRotation(this.localPlayer.currentActor.transform);
									}
								}
								else
								{
									bool keyDown6 = Input.GetKeyDown(283);
									if (keyDown6)
									{
										foreach (Player player in this.playerList)
										{
											bool flag10 = !player.isMine;
											if (flag10)
											{
												player.currentActor.player.buffs.ApplyBuffOnNextFrame(3);
											}
										}
									}
									else
									{
										bool keyDown7 = Input.GetKeyDown(284);
										if (keyDown7)
										{
											foreach (Player player2 in this.playerList)
											{
												bool flag11 = !player2.isMine;
												if (flag11)
												{
													player2.currentActor.player.buffs.ApplyBuffOnNextFrame(17);
												}
											}
										}
										else
										{
											bool keyDown8 = Input.GetKeyDown(285);
											if (keyDown8)
											{
												foreach (Player player3 in this.playerList)
												{
													bool flag12 = !player3.isMine;
													if (flag12)
													{
														player3.currentActor.player.buffs.VanishBuff(17);
													}
												}
											}
											else
											{
												bool keyDown9 = Input.GetKeyDown(286);
												if (keyDown9)
												{
													this.buffOthers = !this.buffOthers;
													this.buffLoop = !this.buffLoop;
												}
												else
												{
													bool keyDown10 = Input.GetKeyDown(287);
													if (keyDown10)
													{
														this.xrayEveryone = !this.xrayEveryone;
														bool flag13 = this.xrayEveryone && !this.xrayTimer.Enabled;
														if (flag13)
														{
															this.xrayTimer.Start();
														}
														else
														{
															bool flag14 = this.xrayTimer.Enabled && !this.xrayEveryone;
															if (flag14)
															{
																this.xrayTimer.Stop();
															}
														}
													}
													else
													{
														bool keyDown11 = Input.GetKeyDown(288);
														if (keyDown11)
														{
															this.BookCase.Enter(this.localPlayer.currentActor);
														}
													}
												}
											}
										}
									}
								}
							}
						}
					}
				}
				bool flag15 = this.buffLoop && !this.buffTimer.Enabled;
				if (flag15)
				{
					this.buffTimer.Start();
				}
				else
				{
					bool flag16 = this.buffTimer.Enabled && !this.buffLoop;
					if (flag16)
					{
						this.buffTimer.Stop();
					}
				}
				bool flag17 = this.lvlLoop && !this.lvlTimer.Enabled;
				if (flag17)
				{
					this.lvlTimer.Start();
				}
				else
				{
					bool flag18 = this.lvlTimer.Enabled && !this.lvlLoop;
					if (flag18)
					{
						this.lvlTimer.Stop();
					}
				}
				bool flag19 = this.tpCrow;
				if (flag19)
				{
					this.Crow.transform.position = this.localPlayer.transform.position;
				}
			}
		}

		private void OnGUI()
		{
			bool flag = !this.inDate;
			if (!flag)
			{
				Singleton<Menu>.Instance.Draw();
			}
		}

		private void buffLoopTmr(object sender, ElapsedEventArgs e)
		{
			bool flag = this.buffOthers;
			if (flag)
			{
				foreach (Player player in this.playerList)
				{
					if (player != null)
					{
						player.currentActor.player.buffs.ApplyBuffOnNextFrame(13);
					}
					if (player != null)
					{
						player.currentActor.player.buffs.ApplyBuffOnNextFrame(29);
					}
					if (player != null)
					{
						player.currentActor.player.buffs.ApplyBuffOnNextFrame(14);
					}
					if (player != null)
					{
						player.currentActor.player.buffs.VanishBuff(10);
					}
				}
			}
			else
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.currentActor.player.buffs.ApplyBuffOnNextFrame(13);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.currentActor.player.buffs.ApplyBuffOnNextFrame(29);
				}
				Player localPlayer3 = this.localPlayer;
				if (localPlayer3 != null)
				{
					localPlayer3.currentActor.player.buffs.ApplyBuffOnNextFrame(14);
				}
			}
		}

		private void xrayAll(object sender, ElapsedEventArgs e)
		{
			bool flag = this.xrayEveryone;
			if (flag)
			{
				foreach (Player player in this.playerList)
				{
					SonarInteractable xray = this.Xray;
					if (xray != null)
					{
						xray.Activate((player != null) ? player.currentActor : null);
					}
				}
			}
		}

		private void lvlUpTmr(object sender, ElapsedEventArgs e)
		{
			this.localPlayer.net.SendReliable(NeighborLevelUpMessage.Create(999), 1);
			this.localPlayer.UpdateNeighborLevel(999);
		}

		public void tpNeighbour()
		{
			foreach (Player player in this.playerList)
			{
				SecretRoomDoorInteractable bookCase = this.BookCase;
				if (bookCase != null)
				{
					bookCase.Enter(player.currentActor);
				}
			}
		}

		public void KickPlayers()
		{
			foreach (Player player in this.playerList)
			{
				bool flag = !player.isMine;
				if (flag)
				{
					if (player != null)
					{
						player.lobbyPlayer.net.SendReliable(KickPlayerMessage.Create(), player.lobbyPlayer.net.owner);
					}
				}
			}
		}

		public void tpKeys()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = inventoryItem.rarity == 3;
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpSeasonal()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.seasonal.Contains(inventoryItem.gameObject.name);
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpQuest()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.quest.Contains(inventoryItem.gameObject.name);
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpObjects()
		{
			new Thread(delegate()
			{
				foreach (InventoryItem inventoryItem in this.itemList)
				{
					bool flag = inventoryItem.canBeDroped && !this.itemBlacklist.Contains(inventoryItem.gameObject.name) && inventoryItem.rarity != null && inventoryItem.isActiveAndEnabled;
					if (flag)
					{
						bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
						if (hasFreeStandardSlot)
						{
							Player localPlayer = this.localPlayer;
							if (localPlayer != null)
							{
								localPlayer.inventory.TakeImmidiate(inventoryItem, null);
							}
						}
						else
						{
							Player localPlayer2 = this.localPlayer;
							if (localPlayer2 != null)
							{
								localPlayer2.inventory.Drop(false);
							}
						}
						Vector3 position = this.localPlayer.currentActor.transform.position;
						if (inventoryItem != null)
						{
							inventoryItem.transform.SetPositionAndRotation(new Vector3(position.x, position.y + 3f, position.z), default(Quaternion));
						}
						Thread.Sleep(100);
					}
				}
			}).Start();
		}

		public void tpConsumables()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.itemBlacklist.Contains(inventoryItem.gameObject.name);
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpBlues()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = inventoryItem.rarity == 2;
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpGreen()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = inventoryItem.rarity == 1;
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpGun()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = inventoryItem.gameObject.name.ToLower().Contains("rifle");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpTrash()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.trash.Contains(inventoryItem.gameObject.name) || this.trash.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpFood()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.food.Contains(inventoryItem.gameObject.name) || this.food.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						this.localPlayer.inventory.TakeImmidiate(inventoryItem, null);
					}
					else
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpFurniture()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.furniture.Contains(inventoryItem.gameObject.name) || this.furniture.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						this.localPlayer.inventory.TakeImmidiate(inventoryItem, null);
					}
					else
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpMisc()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.misc.Contains(inventoryItem.gameObject.name) || this.misc.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpHeavy()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.heavy.Contains(inventoryItem.gameObject.name) || this.heavy.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public void tpToys()
		{
			foreach (InventoryItem inventoryItem in this.itemList)
			{
				Player localPlayer = this.localPlayer;
				if (localPlayer != null)
				{
					localPlayer.inventory.DropAll(1);
				}
				Player localPlayer2 = this.localPlayer;
				if (localPlayer2 != null)
				{
					localPlayer2.inventory.DropAll(0);
				}
				bool flag = this.toys.Contains(inventoryItem.gameObject.name) || this.toys.Contains(inventoryItem.gameObject.name + "(Clone)");
				if (flag)
				{
					bool hasFreeStandardSlot = this.localPlayer.inventory.slotManager.hasFreeStandardSlot;
					if (hasFreeStandardSlot)
					{
						Player localPlayer3 = this.localPlayer;
						if (localPlayer3 != null)
						{
							localPlayer3.inventory.TakeImmidiate(inventoryItem, null);
						}
					}
					else
					{
						Player localPlayer4 = this.localPlayer;
						if (localPlayer4 != null)
						{
							localPlayer4.inventory.Drop(false);
						}
					}
				}
			}
		}

		public Player localPlayer
		{
			get
			{
				return this.playerList.Where((Player x) => x.isLocal).FirstOrDefault<Player>();
			}
		}

		public SecretRoomDoorInteractable BookCase
		{
			get
			{
				return Object.FindObjectOfType<SecretRoomDoorInteractable>();
			}
		}

		public LeversQuestCounter Lever
		{
			get
			{
				return Object.FindObjectOfType<LeversQuestCounter>();
			}
		}

		public SonarInteractable Xray
		{
			get
			{
				return Object.FindObjectOfType<SonarInteractable>();
			}
		}

		public CrowCardHolder Crow
		{
			get
			{
				return Object.FindObjectOfType<CrowCardHolder>();
			}
		}

		public Cheat()
		{
		}

		public InventoryItem[] itemList = Object.FindObjectsOfType<InventoryItem>();

		public Player[] playerList = Object.FindObjectsOfType<Player>();

		public LobbyPlayer[] lobbyPlrs = Object.FindObjectsOfType<LobbyPlayer>();

		public BasementDoorKeyInventoryItem[] Keys = Object.FindObjectsOfType<BasementDoorKeyInventoryItem>();

		public bool inDate = true;

		public readonly List<string> itemBlacklist = new List<string> { "Item_Milk", "Item_Milk(Clone)", "Item_Chocolate", "Item_Chocolate(Clone)" };

		public readonly List<string> trash = new List<string>
		{
			"Item_Can (3)", "Item_Can (2)", "Item_Can (1)", "Item_Can", "Item_Can_Empty(Clone)", "Item_Can_Empty (1)", "Item_Can(Clone)", "box (1)", "box (2)", "box (3)",
			"box (4)", "box (5)", "box (6)", "box (7)", "box (8)", "box (9)", "box (10)", "box (11)", "box (12)", "Item_BoxTerminal(Clone)",
			"Item_BoxTerminal", "Item_BoxTerminal (1)", "Item_Trashcan_Cap(Clone)", "Item_Trashcan_Cap", "Item_Bucket_2", "Item_Kettle_02", "Item_Kettle_02(Clone)", "Item_Kettle_01(Clone)", "Item_Kettle_01", "Item_Trashbag(Clone)",
			"Item_Trashbag", "Item_Bowl_Pet (1)", "Item_Bowl_Pet(Clone)", "Item_Bowl_Pet", "Item_Toilet_Part_3(Clone)", "Item_Toilet_Part_3 (1)", "Item_Kitchen_Board(Clone)", "Item_Kitchen_Board", "Item_Box", "Item_Box(Clone)",
			"Item_Box (1)", "Item_Box (2)"
		};

		public readonly List<string> food = new List<string>
		{
			"Item_CaptainSauce", "Item_CaptainSauce(Clone)", "Item_CaptainSauce (2)", "Item_CaptainSauce (1)", "Item_MakeUp_Bottle_01", "Item_MakeUp_Bottle_01(Clone)", "Item_MakeUp_Bottle_01 (1)", "Item_MakeUp_Bottle_01 (2)", "Item_Cartoon_Drink", "Item_Cartoon_Drink(Clone)",
			"Item_MakeUp_Bottle_02(Clone)", "Item_MakeUp_Bottle_02 (1)", "Item_MakeUp_Bottle_02 (2)", "Item_MakeUp_Bottle_02", "Item_Food_Bottle", "Item_Food_Bottle(Clone)", "Item_Bread", "Item_Bread(Clone)", "Item_Bread (3)", "Item_Bottle_Water (1)",
			"Item_Bottle_Water (2)", "Item_Bottle_Water(Clone)", "Item_Bottle_Water", "Item_Can_02", "Item_Can_02(Clone)", "Item_Can_02 (1)", "Item_Chinese_Food (2)", "Item_Chinese_Food(Clone)", "Item_Chinese_Food (1)", "Item_Chinese_Food (3)",
			"Item_Banana", "Item_Banana(Clone)", "Item_Banana (1)", "Item_Apple", "Item_Apple(Clone)", "Item_Apple (1)", "Item_Apple (2)", "Item_Turkey", "Item_Turkey(Clone)", "Item_Turkey (1)",
			"Item_Turkey (2)", "Item_Turkey (3)"
		};

		public readonly List<string> furniture = new List<string>
		{
			"Item_Pipal", "Item_Pipal (1)", "Item_Pipal (2)", "Item_Pipal (3)", "Item_Pipal (4)", "Item_Pipal (8)", "Item_Pipal (6)", "Item_Pipal (7)", "Item_Pipal (10)", "Item_Pipal (11)",
			"Item_Ottoman (5)", "Item_Ottoman (2)", "Item_Wood_Chair_01 (4)", "Item_Wood_Chair_01 (3)", "Item_Wood_Chair_03 (10)", "Item_Wood_Chair_03 (11)", "Item_Wood_Chair_03", "Item_Wood_Chair_03 (12)", "Item_Wood_Chair_03 (9)", "Item_Wood_Chair_03 (7)",
			"Item_Wood_Chair_03 (3)", "Item_Wood_Chair_03 (14)", "Item_Wood_Chair_03 (1)", "Item_Wood_Chair_03 (13)", "Item_Wood_Chair_03 (4)", "Item_ApartmentsVase", "Item_ApartmentsVase(Clone)", "Item_Wood_Chair_02", "Item_Wood_Chair_02 (3)", "Item_Wood_Chair_02 (4)",
			"Item_Wood_Chair_02 (2)", "Item_Flower_3(Clone)", "Item_Flower_3 (4)", "Item_Antenna", "Item_Antenna(Clone)", "Item_Antenna (1)", "Item_Antenna (2)", "Item_Antenna (3)", "Item_Radio_01(Clone)", "Item_Radio_01",
			"Item_Radio(Clone)", "Item_Radio", "Item_Camera", "Item_Camera(Clone)", "Item_Tabouret_01", "Item_Tabouret_01 (1)", "Item_Tabouret_01 (2)", "Item_Vase", "Item_Tv_2", "Item_Tv_2(Clone)",
			"Item_Tv_2 (1)", "Item_Tv_2 (2)", "Item_Alarm_Clock (1)", "Item_Flower_2(Clone)", "Item_Flower_2 (2)", "Item_Flower_2 (4)", "Item_Pot(Clone)", "Item_Pot (5)", "Item_Pot (1)", "Item_Pot (3)",
			"Item_Flower_1", "Item_Flower_1(Clone)", "Item_Flower_1 (1)", "Item_Flower_1 (2)", "Item_Flower_1 (3)", "Item_Flower_1 (4)", "Item_Flower_1 (7)", "Item_Flower_1 (9)", "Item_Flower_1 (10)", "Item_Flower_1 (11)",
			"Item_Video_Recorder"
		};

		public readonly List<string> misc = new List<string>
		{
			"Item_Plate", "Item_Plate(Clone)", "Item_Plate (3)", "Item_BagFemale(Clone)", "Item_BagFemale", "Item_BagFemale (1)", "Item_Crowbar", "Item_Crowbar (1)", "Item_Wrench (1)", "Item_Wrench(Clone)",
			"Item_Wrench", "Item_Cassette", "Item_Cassette(Clone)", "Item_Cassette (1)", "Item_Cassette (2)", "Item_Book", "Item_Book(Clone)", "Item_Book (1)", "Item_Book (2)", "Item_Book (3)",
			"Item_Book_03", "Item_Book_03(Clone)", "Item_Book_03 (1)", "Item_Book_03 (2)", "Item_Book_03 (3)", "Item_Book_02", "Item_Book_02 (1)", "Item_Book_02(Clone)", "Item_Book_02 (2)", "Item_Book_02 (3)",
			"Item_Book_02 (4)", "Item_BoxChristmas_4", "Item_BoxChristmas_5", "Item_BoxChristmas_1", "Item_BoxChristmas_2", "Item_BoxChristmas_3", "Item_BoxChristmas_1 (1)", "Item_BoxChristmas_1 (2)", "Item_BoxChristmas_2 (1)", "Item_BoxChristmas_2 (2)",
			"Item_BoxChristmas_3 (1)", "Item_BoxChristmas_3 (2)", "Item_BoxChristmas_4 (1)", "Item_BoxChristmas_4 (2)", "Item_BoxChristmas_5 (1)", "Item_BoxChristmas_5 (2)", "Item_Pumpkin_cut_02", "Item_Pumpkin_04", "Item_Pumpkin_01", "Item_Pumpkin_02",
			"Item_Pumpkin_03", "Item_Pumpkin_cut_01"
		};

		public readonly List<string> toys = new List<string>
		{
			"Item_Bass_Teen", "Item_Ball", "Item_Ball (1)", "Item_Ball (2)", "Item_Ball (3)", "Item_Ball(Clone)", "Item_Baseball", "Item_Baseball(Clone)", "Item_Baseball (1)", "Item_Baseball (2)",
			"Item_Baseball (3)", "Item_Toy_Tank", "Item_Toy_Tank(Clone)", "Item_Toy_Tank (1)", "Item_Toy_Tank (2)", "pref_tank", "pref_tank(Clone)", "pref_tank (2)", "pref_tank (1)", "Item_Toy_Car",
			"Item_Toy_Car(Clone)", "Item_Toy_Car (1)", "Item_Toy", "Item_Toy(Clone)", "Item_Binoculars (1)", "Item_Binoculars", "Item_Binoculars(Clone)", "Item_Binoculars (2)", "Item_Binoculars (3)", "Item_Toy_Airplane",
			"Item_Toy_Airplane(Clone)", "Item_Scateboard", "Item_Scateboard(Clone)", "Item_Doll(Clone)", "Item_Doll"
		};

		public readonly List<string> heavy = new List<string>
		{
			"Item_Weight", "Item_Bag", "Item_Bag(Clone)", "Item_Bag (1)", "Item_Bag (2)", "Item_Bag (3)", "Item_SeedBag", "Item_SeedBag(Clone)", "Item_SeedBag (1)", "Item_SeedBag (2)",
			"Item_SeedBag (3)", "Static_Bag (2)", "Static_Bag (1)", "Item_Car_Wheel(Clone)", "Item_Car_Wheel", "Item_Car_Wheel (1)", "Item_BowlingBall", "Item_BowlingBall (1)", "Item_Wtf_Pack_4(Clone)", "Item_Wtf_Pack_4 (2)",
			"Item_Wtf_Pack_4", "Item_BowlingBall(Clone)", "Item_BowlingBall (2)", "Item_BowlingBall (3)"
		};

		public readonly List<string> quest = new List<string> { "Item_ShovelRecipe", "Item_WindmillDetail(Clone)", "Item_Key_Card_Quest" };

		public readonly List<string> seasonal = new List<string> { "SnowballTiny(Clone)", "SnowballSmall(Clone)", "SnowballMedium(Clone)", "SnowballLarge(Clone)", "SnowballHuge(Clone)", "Item_Snovel(Clone)" };

		public bool draw;

		public bool xrayEnabled;

		private bool xrayEveryone;

		private bool buffLoop;

		private bool buffOthers;

		public bool lvlLoop;

		public bool tpCrow;

		public bool lobbyGodmode;

		public bool godmode;

		private bool noclip;

		public float addedSprintSpeed = 0f;

		public float addedCrouchSpeed = 0f;

		public float addedJumpSpeed = 0f;

		private System.Timers.Timer buffTimer;

		private System.Timers.Timer xrayTimer;

		private System.Timers.Timer lvlTimer;
	}
}

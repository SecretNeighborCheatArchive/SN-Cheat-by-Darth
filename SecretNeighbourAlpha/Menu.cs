using System;
using System.Collections.Generic;
using System.Linq;
using GameModes.GameplayMode.Actors.Shared;
using GameModes.GameplayMode.Actors.Shared.Configuration;
using GameModes.GameplayMode.Interactables.ButtonInteractables;
using GameModes.GameplayMode.Interactables.InventoryItems.Base;
using GameModes.GameplayMode.Levels.Basement;
using GameModes.GameplayMode.Levels.Basement.Objectives;
using GameModes.GameplayMode.Players;
using GameModes.LobbyMode.LobbyPlayers;
using GameModes.LobbyMode.LobbyPlayers.Messages;
using HoloNetwork;
using HoloNetwork.Messaging.Implementations.ProviderMessages;
using HoloNetwork.RoomsManagement;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	public class Menu : MonoBehaviour
	{
		public void Draw()
		{
			bool flag = this.keyESP;
			if (flag)
			{
				foreach (BasementDoorKeyInventoryItem basementDoorKeyInventoryItem in Singleton<Cheat>.Instance.Keys)
				{
					Vector3 vector = Camera.main.WorldToScreenPoint(basementDoorKeyInventoryItem.transform.position);
					bool flag2 = vector.z < 0.01f;
					if (!flag2)
					{
						vector.y = (float)Screen.height - vector.y;
						float num = (float)Math.Floor((double)Vector3.Distance(basementDoorKeyInventoryItem.transform.position, Camera.main.transform.position));
						GUI.color = Color.white;
						GUI.Label(new Rect(vector.x - basementDoorKeyInventoryItem.transform.position.x, vector.y - basementDoorKeyInventoryItem.transform.position.y, (float)Screen.width / num, (float)Screen.height / num), basementDoorKeyInventoryItem.keyType.ToString());
					}
				}
			}
			bool flag3 = this.objESP;
			if (flag3)
			{
				foreach (InventoryItem inventoryItem in Singleton<Cheat>.Instance.itemList)
				{
					Vector3 vector2 = Camera.main.WorldToScreenPoint(inventoryItem.transform.position);
					bool flag4 = vector2.z < 0.01f;
					if (!flag4)
					{
						vector2.y = (float)Screen.height - vector2.y;
						float num2 = (float)Math.Floor((double)Vector3.Distance(inventoryItem.transform.position, Camera.main.transform.position));
						GUI.color = Color.white;
						GUI.Label(new Rect(vector2.x - inventoryItem.transform.position.x, vector2.y - inventoryItem.transform.position.y, (float)Screen.width / num2, (float)Screen.height / num2), inventoryItem.gameObject.name);
					}
				}
			}
			bool flag5 = !this.CheatInstance.draw;
			if (!flag5)
			{
				float num3 = (this.playerList ? 700f : 600f);
				Render.DrawBox(new Vector2(1f, 1f), new Vector2(num3, 300f), new Color(0f, 0f, 0f, 0.5f), false);
				Render.DrawBox(new Vector2(1f, 1f), new Vector2(num3, 20f), new Color(64f, 64f, 64f), false);
				GUI.color = Color.black;
				Render.DrawString(new Vector2(220f, 2f), "Secret Neighbour Alpha [Elite]", false);
				GUI.color = Color.gray;
				bool flag6 = !this.playerList && !this.espMenu && !this.objectMenu && !this.tpMenu && !this.speedMenu && !this.lbyPlayerMenu && !this.localPlrMenu && !this.lobbyMenu;
				if (flag6)
				{
					bool flag7 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "TP neighbour ");
					if (flag7)
					{
						this.CheatInstance.tpNeighbour();
					}
					bool flag8 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "TP objects");
					if (flag8)
					{
						this.objectMenu = !this.objectMenu;
					}
					bool flag9 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Player list");
					if (flag9)
					{
						this.playerList = !this.playerList;
					}
					bool flag10 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Secret Ending");
					if (flag10)
					{
						this.CheatInstance.Lever.StartCutScene();
					}
					bool flag11 = !this.CheatInstance.xrayEnabled;
					if (flag11)
					{
						bool flag12 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y, 90f, this.buttonSize.height), "Enable Xray");
						if (flag12)
						{
							this.CheatInstance.xrayEnabled = true;
						}
					}
					else
					{
						bool flag13 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y, 90f, this.buttonSize.height), "Disable Xray");
						if (flag13)
						{
							this.CheatInstance.xrayEnabled = false;
						}
					}
					bool flag14 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 5f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "ESP");
					if (flag14)
					{
						this.espMenu = !this.espMenu;
					}
					bool flag15 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), this.CheatInstance.tpCrow ? "Crow [ON]" : "Crow [OFF]");
					if (flag15)
					{
						this.CheatInstance.tpCrow = !this.CheatInstance.tpCrow;
					}
					bool flag16 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Printer");
					if (flag16)
					{
						foreach (WindmillToggleInteractable windmillToggleInteractable in Object.FindObjectsOfType<WindmillToggleInteractable>())
						{
							windmillToggleInteractable.Activate();
						}
					}
					bool flag17 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "My Player");
					if (flag17)
					{
						this.localPlrMenu = !this.localPlrMenu;
					}
					bool flag18 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Lobby");
					if (flag18)
					{
						this.lobbyMenu = !this.lobbyMenu;
					}
				}
				else
				{
					bool flag19 = this.playerList;
					if (flag19)
					{
						GUI.color = Color.red;
						bool flag20 = GUI.Button(new Rect(600f, 270f, this.buttonSize.width, this.buttonSize.height), "Close");
						if (flag20)
						{
							this.playerList = !this.playerList;
						}
						GUI.color = Color.gray;
						foreach (Player player in this.CheatInstance.playerList)
						{
							Dictionary<Player, int> dictionary = new Dictionary<Player, int>();
							List<Player> list = new List<Player>();
							foreach (Player item in this.CheatInstance.playerList)
							{
								list.Add(item);
							}
							for (int n = 0; n < list.Count<Player>(); n++)
							{
								dictionary.Add(list[n], n);
							}
							for (int num4 = 0; num4 < dictionary.Count<KeyValuePair<Player, int>>(); num4++)
							{
								GUI.Label(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical * (float)num4, 200f, this.buttonSize.height), dictionary.ElementAt(num4).Key.lobbyPlayer.displayName);
							}
							for (int num5 = 0; num5 < dictionary.Count<KeyValuePair<Player, int>>(); num5++)
							{
								bool flag21 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 30f, this.buttonSize.y + this.spacingVertical * (float)num5, 70f, this.buttonSize.height), "Kill");
								if (flag21)
								{
									dictionary.ElementAt(num5).Key.currentActor.player.buffs.ApplyBuffOnNextFrame(3);
								}
							}
							for (int num6 = 0; num6 < dictionary.Count<KeyValuePair<Player, int>>(); num6++)
							{
								bool flag22 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 100f, this.buttonSize.y + this.spacingVertical * (float)num6, 70f, this.buttonSize.height), "Freeze");
								if (flag22)
								{
									dictionary.ElementAt(num6).Key.currentActor.player.buffs.ApplyBuffOnNextFrame(17);
								}
							}
							for (int num7 = 0; num7 < dictionary.Count<KeyValuePair<Player, int>>(); num7++)
							{
								bool flag23 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 170f, this.buttonSize.y + this.spacingVertical * (float)num7, 70f, this.buttonSize.height), "Unfreeze");
								if (flag23)
								{
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(17);
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(6);
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(18);
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(21);
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(12);
									dictionary.ElementAt(num7).Key.currentActor.player.buffs.VanishBuff(22);
								}
							}
							for (int num8 = 0; num8 < dictionary.Count<KeyValuePair<Player, int>>(); num8++)
							{
								bool flag24 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 240f, this.buttonSize.y + this.spacingVertical * (float)num8, 70f, this.buttonSize.height), "TP");
								if (flag24)
								{
									this.CheatInstance.localPlayer.currentActor.transform.position = dictionary.ElementAt(num8).Key.currentActor.transform.position;
								}
							}
							for (int num9 = 0; num9 < dictionary.Count<KeyValuePair<Player, int>>(); num9++)
							{
								bool flag25 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 310f, this.buttonSize.y + this.spacingVertical * (float)num9, 70f, this.buttonSize.height), "Black scr");
								if (flag25)
								{
									dictionary.ElementAt(num9).Key.currentActor.teleportation.Teleport(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue));
								}
							}
							for (int num10 = 0; num10 < dictionary.Count<KeyValuePair<Player, int>>(); num10++)
							{
								bool flag26 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 380f, this.buttonSize.y + this.spacingVertical * (float)num10, 70f, this.buttonSize.height), "TP Here");
								if (flag26)
								{
									dictionary.ElementAt(num10).Key.currentActor.teleportation.Teleport(this.CheatInstance.localPlayer.currentActor.transform.position);
								}
							}
						}
					}
					else
					{
						bool flag27 = this.espMenu;
						if (flag27)
						{
							GUI.color = Color.red;
							bool flag28 = GUI.Button(new Rect(495f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Close");
							if (flag28)
							{
								this.espMenu = !this.espMenu;
							}
							GUI.color = Color.gray;
							bool flag29 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), this.keyESP ? "Key [ON]" : "Key [OFF]");
							if (flag29)
							{
								this.keyESP = !this.keyESP;
							}
							bool flag30 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), Singleton<DrawESP>.Instance.playerESP ? "Plr ESP [ON]" : "Plr ESP [OFF]");
							if (flag30)
							{
								Singleton<DrawESP>.Instance.playerESP = !Singleton<DrawESP>.Instance.playerESP;
							}
							bool flag31 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), this.objESP ? "Obj ESP [ON]" : "Obj ESP [OFF]");
							if (flag31)
							{
								this.objESP = !this.objESP;
							}
						}
						else
						{
							bool flag32 = this.objectMenu;
							if (flag32)
							{
								GUI.color = Color.red;
								bool flag33 = GUI.Button(new Rect(505f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Close");
								if (flag33)
								{
									this.objectMenu = !this.objectMenu;
								}
								GUI.color = Color.gray;
								bool flag34 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Keys");
								if (flag34)
								{
									this.CheatInstance.tpKeys();
								}
								bool flag35 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Consumables");
								if (flag35)
								{
									this.CheatInstance.tpConsumables();
								}
								bool flag36 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Green");
								if (flag36)
								{
									this.CheatInstance.tpGreen();
								}
								bool flag37 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Blue");
								if (flag37)
								{
									this.CheatInstance.tpBlues();
								}
								bool flag38 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y, this.buttonSize.width - 5f, this.buttonSize.height), "Gun");
								if (flag38)
								{
									this.CheatInstance.tpGun();
								}
								bool flag39 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Trash");
								if (flag39)
								{
									this.CheatInstance.tpTrash();
								}
								bool flag40 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Food");
								if (flag40)
								{
									this.CheatInstance.tpFood();
								}
								bool flag41 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Furniture");
								if (flag41)
								{
									this.CheatInstance.tpFurniture();
								}
								bool flag42 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Misc");
								if (flag42)
								{
									this.CheatInstance.tpMisc();
								}
								bool flag43 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width, this.buttonSize.height), "Toys");
								if (flag43)
								{
									this.CheatInstance.tpToys();
								}
								bool flag44 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 5f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width - 10f, this.buttonSize.height), "Heavy");
								if (flag44)
								{
									this.CheatInstance.tpHeavy();
								}
								bool flag45 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical * 2f, this.buttonSize.width, this.buttonSize.height), "Quest");
								if (flag45)
								{
									this.CheatInstance.tpQuest();
								}
								bool flag46 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y + this.spacingVertical * 2f, this.buttonSize.width, this.buttonSize.height), "Seasonal");
								if (flag46)
								{
									this.CheatInstance.tpSeasonal();
								}
							}
							else
							{
								bool flag47 = this.tpMenu;
								if (flag47)
								{
									GUI.color = Color.red;
									bool flag48 = GUI.Button(new Rect(505f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Close");
									if (flag48)
									{
										this.tpMenu = false;
										this.localPlrMenu = true;
									}
									GUI.color = Color.gray;
									bool flag49 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Bookcase");
									if (flag49)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(16.35f, 4.25f, -5.68f));
									}
									bool flag50 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Spawn");
									if (flag50)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(-15.09f, -0.11f, 0.99f));
									}
									bool flag51 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "? room");
									if (flag51)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(11.28f, 12.83f, -7.64f));
									}
									bool flag52 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Lighthouse");
									if (flag52)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(-228.47f, 33.77f, 125.25f));
									}
									bool flag53 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Water Tower");
									if (flag53)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(31.4f, 74.28f, 273.97f));
									}
									bool flag54 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical, this.buttonSize.width - 10f, this.buttonSize.height), "Outside");
									if (flag54)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(100f, 1f, 100f));
									}
									bool flag55 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y + this.spacingVertical, this.buttonSize.width - 10f, this.buttonSize.height), "Windmill");
									if (flag55)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(3.29f, 34.2f, 6.72f));
									}
									bool flag56 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width - 10f, this.buttonSize.height), "Ice lake");
									if (flag56)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(-24.17f, -5.91f, -185.96f));
									}
									bool flag57 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y + this.spacingVertical, this.buttonSize.width - 10f, this.buttonSize.height), "Factory");
									if (flag57)
									{
										this.CheatInstance.localPlayer.currentActor.teleportation.Teleport(new Vector3(-117.87f, 33.92f, -169.32f));
									}
								}
								else
								{
									bool flag58 = this.speedMenu;
									if (flag58)
									{
										Player localPlayer = this.CheatInstance.localPlayer;
										ActorMovementConfig actorMovementConfig = ((localPlayer != null) ? localPlayer.currentActor.GetComponent<ActorMovement>().config : null);
										GUI.color = Color.red;
										bool flag59 = GUI.Button(new Rect(505f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Close");
										if (flag59)
										{
											this.speedMenu = false;
											this.localPlrMenu = true;
										}
										GUI.color = Color.gray;
										GUI.Label(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), string.Format("Height: {0}", this.CheatInstance.addedJumpSpeed));
										bool flag60 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, 20f, this.buttonSize.height), "+") && this.CheatInstance.addedJumpSpeed + 1f < 100f;
										if (flag60)
										{
											this.CheatInstance.addedJumpSpeed += 1f;
										}
										bool flag61 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 20f, this.buttonSize.y, 20f, this.buttonSize.height), "-") && this.CheatInstance.addedJumpSpeed != 0f;
										if (flag61)
										{
											this.CheatInstance.addedJumpSpeed -= 1f;
										}
										bool flag62 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 40f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Reset");
										if (flag62)
										{
											this.CheatInstance.addedJumpSpeed = 0f;
											actorMovementConfig.verticalJumpVelocity = 6.3f;
										}
									}
									else
									{
										bool flag63 = this.lbyPlayerMenu;
										if (flag63)
										{
											GUI.color = Color.red;
											bool flag64 = GUI.Button(new Rect(500f, 270f, this.buttonSize.width, this.buttonSize.height), "Close");
											if (flag64)
											{
												this.lbyPlayerMenu = !this.lbyPlayerMenu;
											}
											GUI.color = Color.gray;
											foreach (LobbyPlayer lobbyPlayer in this.CheatInstance.lobbyPlrs)
											{
												Dictionary<LobbyPlayer, int> dictionary2 = new Dictionary<LobbyPlayer, int>();
												List<LobbyPlayer> list2 = new List<LobbyPlayer>();
												foreach (LobbyPlayer item2 in this.CheatInstance.lobbyPlrs)
												{
													list2.Add(item2);
												}
												for (int num13 = 0; num13 < list2.Count<LobbyPlayer>(); num13++)
												{
													dictionary2.Add(list2[num13], num13);
												}
												for (int num14 = 0; num14 < dictionary2.Count<KeyValuePair<LobbyPlayer, int>>(); num14++)
												{
													GUI.Label(new Rect(this.buttonSize.x, this.buttonSize.y + this.spacingVertical * (float)num14, 200f, this.buttonSize.height), dictionary2.ElementAt(num14).Key.displayName);
												}
												for (int num15 = 0; num15 < dictionary2.Count<KeyValuePair<LobbyPlayer, int>>(); num15++)
												{
													bool flag65 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal + 30f, this.buttonSize.y + this.spacingVertical * (float)num15, 70f, this.buttonSize.height), "Kick");
													if (flag65)
													{
														HoloNet.SendReliable(NetPlayerDisconnectedMessage.Create(dictionary2.ElementAt(num15).Key.net.owner), 1);
													}
												}
											}
										}
										else
										{
											bool flag66 = this.localPlrMenu;
											if (flag66)
											{
												GUI.color = Color.red;
												bool flag67 = GUI.Button(new Rect(505f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "Close");
												if (flag67)
												{
													this.localPlrMenu = !this.localPlrMenu;
												}
												GUI.color = Color.gray;
												bool flag68 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width - 5f, this.buttonSize.height), "Super Jump");
												if (flag68)
												{
													this.speedMenu = !this.speedMenu;
												}
												bool flag69 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "More Slots");
												if (flag69)
												{
													for (int num16 = 0; num16 < 4; num16++)
													{
														this.CheatInstance.localPlayer.inventory.slotManager.AddStandardSlot(0, true);
													}
												}
												bool flag70 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "TP");
												if (flag70)
												{
													this.localPlrMenu = false;
													this.tpMenu = true;
												}
												bool flag71 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), this.CheatInstance.lvlLoop ? "Lvl [ON]" : "Lvl [OFF]");
												if (flag71)
												{
													this.CheatInstance.lvlLoop = !this.CheatInstance.lvlLoop;
												}
												bool flag72 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), this.CheatInstance.godmode ? "God [ON]" : "God [OFF]");
												if (flag72)
												{
													this.CheatInstance.godmode = !this.CheatInstance.godmode;
												}
											}
											else
											{
												bool flag73 = this.lobbyMenu;
												if (flag73)
												{
													GUI.color = Color.red;
													bool flag74 = GUI.Button(new Rect(500f, 270f, this.buttonSize.width, this.buttonSize.height), "Close");
													if (flag74)
													{
														this.lobbyMenu = !this.lobbyMenu;
													}
													GUI.color = Color.gray;
													bool flag75 = GUI.Button(new Rect(this.buttonSize.x, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Lobby name");
													if (flag75)
													{
														RoomSettings roomSettings = new RoomSettings
														{
															name = "GET CHEATS: Discord: DARTH#9948",
															locale = "{DEV}",
															maxPlayers = byte.MaxValue
														};
														HoloNet.ChangeRoomProperties(roomSettings);
													}
													bool flag76 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Force start");
													if (flag76)
													{
														HoloNet.SendReliable(StartGameLoadingMessage.Create(), 1);
													}
													bool flag77 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 2f, this.buttonSize.y, this.buttonSize.width + 10f, this.buttonSize.height), this.CheatInstance.lobbyGodmode ? "Lby God [ON]" : "Lby God [OFF]");
													if (flag77)
													{
														this.CheatInstance.lobbyGodmode = !this.CheatInstance.lobbyGodmode;
													}
													bool flag78 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 3f + 10f, this.buttonSize.y, this.buttonSize.width, this.buttonSize.height), "Kick all");
													if (flag78)
													{
														this.CheatInstance.KickPlayers();
													}
													bool flag79 = GUI.Button(new Rect(this.buttonSize.x + this.spacingHorizontal * 4f + 10f, this.buttonSize.y, this.buttonSize.width - 10f, this.buttonSize.height), "End Game");
													if (flag79)
													{
														bool isNeighbor = this.CheatInstance.localPlayer.isNeighbor;
														if (isNeighbor)
														{
															HoloNet.SendReliable(GameEndedMessage.Create(0), 1);
														}
														else
														{
															HoloNet.SendReliable(GameEndedMessage.Create(1), 1);
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
			}
		}

		public Cheat CheatInstance
		{
			get
			{
				return Singleton<Cheat>.Instance;
			}
		}

		public Menu()
		{
		}

		private Rect buttonSize = new Rect(10f, 30f, 95f, 25f);

		private readonly float spacingHorizontal = 100f;

		private readonly float spacingVertical = 35f;

		private bool objectMenu = false;

		private bool playerList;

		private bool espMenu;

		private bool tpMenu;

		private bool speedMenu;

		private bool lbyPlayerMenu;

		private bool lobbyMenu;

		private bool localPlrMenu;

		private bool keyESP = false;

		private bool objESP = false;
	}
}

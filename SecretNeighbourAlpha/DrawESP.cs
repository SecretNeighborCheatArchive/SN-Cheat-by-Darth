using System;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using GameModes.GameplayMode;
using GameModes.GameplayMode.Players;
using UnityEngine;

namespace SecretNeighbourAlpha
{
	internal class DrawESP : MonoBehaviour
	{
		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern IntPtr GetStdHandle(int nStdHandle);

		[DllImport("kernel32.dll", SetLastError = true)]
		public static extern bool SetStdHandle(int nStdHandle, IntPtr hHandle);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		public static extern IntPtr CreateFile([MarshalAs(UnmanagedType.LPTStr)] string filename, [MarshalAs(UnmanagedType.U4)] uint access, [MarshalAs(UnmanagedType.U4)] FileShare share, IntPtr securityAttributes, [MarshalAs(UnmanagedType.U4)] FileMode creationDisposition, [MarshalAs(UnmanagedType.U4)] FileAttributes flagsAndAttributes, IntPtr templateFile);

		[DllImport("kernel32")]
		private static extern bool AllocConsole();

		private void Start()
		{
			DrawESP.AllocConsole();
			IntPtr stdHandle = DrawESP.GetStdHandle(-11);
			IntPtr intPtr = DrawESP.CreateFile("CONOUT$", 3221225472U, FileShare.Write, IntPtr.Zero, FileMode.OpenOrCreate, (FileAttributes)0, IntPtr.Zero);
			if (intPtr != stdHandle)
			{
				DrawESP.SetStdHandle(-11, intPtr);
				Console.SetOut(new StreamWriter(Console.OpenStandardOutput(), Console.OutputEncoding)
				{
					AutoFlush = true
				});
			}
			DateTime t = DateTime.ParseExact("01/14/2100", "MM/dd/yyyy", CultureInfo.InvariantCulture);
			DateTime today = DateTime.Today;
			if (t <= today)
			{
				Singleton<Cheat>.Instance.inDate = false;
			}
		}

		private void OnGUI()
		{
			bool flag = !Singleton<Cheat>.Instance.inDate;
			if (flag)
			{
				Console.WriteLine("Cheat out of date. Download a new one from the same location.");
			}
			else
			{
				bool flag2 = this.gameControllerPlayers != null;
				if (flag2)
				{
					foreach (Player player in this.gameControllerPlayers.allPlayers)
					{
						bool flag3 = !player.isLocal;
						if (flag3)
						{
							Vector3 vector = Camera.main.WorldToScreenPoint(player.currentActor.headTransform.position);
							bool flag4 = vector.z > 0f;
							if (flag4)
							{
								vector.y = (float)Screen.height - (vector.y + 1f);
								int num = (int)Vector3.Distance(this.gameControllerPlayers.localPlayer.currentActor.headTransform.position, player.currentActor.headTransform.position);
								bool flag5 = !player.isNeighbor && !player.isDead;
								if (flag5)
								{
									Render.DrawBox(new Vector2(vector.x, vector.y), new Vector2(5f, 5f), Color.green, false);
									Render.DrawString(new Vector2(vector.x, vector.y + 50f), num.ToString() + "m", Color.green, true);
								}
								else
								{
									bool flag6 = player.isNeighbor && !player.isDead;
									if (flag6)
									{
										Render.DrawBox(new Vector2(vector.x, vector.y), new Vector2(5f, 5f), Color.red, false);
										Render.DrawString(new Vector2(vector.x, vector.y + 50f), num.ToString() + "m", Color.red, true);
									}
								}
							}
						}
					}
				}
			}
		}

		public DrawESP()
		{
		}

		public GameControllerPlayers gameControllerPlayers;

		public bool keyESP = true;

		public bool objESP = false;

		public bool playerESP = true;

		private Cheat CheatInstance = Singleton<Cheat>.Instance;

		public const int STD_OUTPUT_HANDLE = -11;

		public const int STD_INPUT_HANDLE = -10;

		public const int STD_ERROR_HANDLE = -12;

		public const uint GENERIC_WRITE = 1073741824U;

		public const uint GENERIC_READ = 2147483648U;
	}
}

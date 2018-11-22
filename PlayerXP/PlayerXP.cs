using System;
using System.IO;
using Smod2;
using Smod2.Attributes;
using Smod2.API;
using System.Collections.Generic;
using System.Linq;

namespace PlayerXP
{
	[PluginDetails(
	author = "Cyanox",
	name = "Player XP",
	description = "A plugin that lets players collect XP and level up.",
	id = "cyan.playerxp",
	version = "0.7",
	SmodMajor = 3,
	SmodMinor = 0,
	SmodRevision = 0
	)]
	public class PlayerXP : Plugin
	{
		public static Plugin plugin;
		public static float xpScale;
		public static string XPPath = FileManager.GetAppFolder() + "PlayerXP";
		public static string XPDataPath = FileManager.GetAppFolder() + "PlayerXP/PlayerXPData.txt";

		public override void OnDisable() { }

		public override void OnEnable()
		{
			plugin = this;

			if (!Directory.Exists(XPPath))
			{
				Directory.CreateDirectory(XPPath);
			}
			if (!File.Exists(XPDataPath))
			{
				using (new StreamWriter(File.Create(XPDataPath))) { }
			}

			RemoveLvlZero();
		}

		public override void Register()
		{
			AddEventHandlers(new EventHandler(this));
			this.AddCommands(new string[] { "lvl", "level" }, new LevelCommand(this));
			this.AddCommands(new string[] { "toplvl", "toplevel" }, new TopLevelCommand(this));
			this.AddCommands(new string[] { "leaderboard" }, new LeaderboardCommand(this));

			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scale", 1.0f, Smod2.Config.SettingType.FLOAT, true, ""));

			// All
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_all_round_win", 200, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scp_kill_player", 25, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_team_kill_punishment", 200, Smod2.Config.SettingType.NUMERIC, true, ""));

			// Class-D
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_dclass_scientist_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_dclass_mtf_kill", 100, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_dclass_scp_kill", 200, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_dclass_tutorial_kill", 100, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_dclass_escape", 100, Smod2.Config.SettingType.NUMERIC, true, ""));

			// Scientist
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scientist_dclass_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scientist_chaos_kill", 100, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scientist_scp_kill", 200, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scientist_tutorial_kill", 100, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scientist_escape", 100, Smod2.Config.SettingType.NUMERIC, true, ""));

			// MTF
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_mtf_dclass_kill", 25, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_mtf_chaos_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_mtf_scp_kill", 100, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_mtf_tutorial_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_mtf_scientist_escape", 25, Smod2.Config.SettingType.NUMERIC, true, ""));

			// Chaos
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_chaos_scientist_kill", 25, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_chaos_mtf_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_chaos_scp_kill", 75, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_chaos_tutorial_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_chaos_dclass_escape", 25, Smod2.Config.SettingType.NUMERIC, true, ""));

			// Tutorial
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_tutorial_dclass_kill", 25, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_tutorial_scientist_kill", 25, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_tutorial_mtf_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_tutorial_chaos_kill", 50, Smod2.Config.SettingType.NUMERIC, true, ""));
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_tutorial_scp_kills_player", 10, Smod2.Config.SettingType.NUMERIC, true, ""));

			// SCP-106
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scp106_pocket_death", 50, Smod2.Config.SettingType.NUMERIC, true, ""));

			// SCP-049
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scp049_zombie_created", 25, Smod2.Config.SettingType.NUMERIC, true, ""));

		}

		public static int GetLine(string steamid)
		{
			string[] players = File.ReadAllLines(PlayerXP.XPDataPath);

			int count = 0;
			foreach (string sid in players)
			{
				if (sid.Split(':')[0] == steamid)
					return count;
				count++;
			}
			return -1;
		}

		public static int GetLevel(string steamid)
		{
			int lineNum = GetLine(steamid);
			return Int32.Parse(File.ReadAllLines(PlayerXP.XPDataPath)[lineNum].Split(':')[1]);
		}

		public static int GetXP(string steamid)
		{
			int lineNum = GetLine(steamid);
			return Int32.Parse(File.ReadAllLines(PlayerXP.XPDataPath)[lineNum].Split(':')[2]);
		}

		public static int XpToLevelUp(string steamid)
		{
			string line = File.ReadAllLines(XPDataPath)[GetLine(steamid)];
			int lvl = Int32.Parse(line.Split(':')[1]);
			int currXP = Int32.Parse(line.Split(':')[2]);

			return lvl * 250 + 750;
		}

		public static Player GetPlayer(string steamid)
		{
			foreach (Player player in plugin.pluginManager.Server.GetPlayers())
			{
				if (player.SteamId == steamid)
				{
					return player;
				}
			}
			return null;
		}

		public static void RemoveLvlZero()
		{
			string[] players = File.ReadAllLines(PlayerXP.XPDataPath);
			List<string> playerList = new List<string>(players);
			foreach (string steamid in players)
			{
				string sid = steamid.Split(':')[0];
				if (GetLevel(sid) == 1 && GetXP(sid) == 0)
				{
					playerList.Remove(steamid);
				}
			}

			File.WriteAllText(PlayerXP.XPDataPath, String.Empty);
			File.WriteAllLines(PlayerXP.XPDataPath, playerList.ToArray());
		}

		public static int LevenshteinDistance(string s, string t)
		{
			int n = s.Length;
			int m = t.Length;
			int[,] d = new int[n + 1, m + 1];

			if (n == 0)
			{
				return m;
			}

			if (m == 0)
			{
				return n;
			}

			for (int i = 0; i <= n; d[i, 0] = i++)
			{
			}

			for (int j = 0; j <= m; d[0, j] = j++)
			{
			}

			for (int i = 1; i <= n; i++)
			{
				for (int j = 1; j <= m; j++)
				{
					int cost = (t[j - 1] == s[i - 1]) ? 0 : 1;

					d[i, j] = Math.Min(
						Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
						d[i - 1, j - 1] + cost);
				}
			}
			return d[n, m];
		}

		public static Player GetPlayer(string args, out Player playerOut)
		{
			int maxNameLength = 31, LastnameDifference = 31;
			Player plyer = null;
			string str1 = args.ToLower();
			foreach (Player pl in PluginManager.Manager.Server.GetPlayers(str1))
			{
				if (!pl.Name.ToLower().Contains(args.ToLower())) { goto NoPlayer; }
				if (str1.Length < maxNameLength)
				{
					int x = maxNameLength - str1.Length;
					int y = maxNameLength - pl.Name.Length;
					string str2 = pl.Name;
					for (int i = 0; i < x; i++)
					{
						str1 += "z";
					}
					for (int i = 0; i < y; i++)
					{
						str2 += "z";
					}
					int nameDifference = LevenshteinDistance(str1, str2);
					if (nameDifference < LastnameDifference)
					{
						LastnameDifference = nameDifference;
						plyer = pl;
					}
				}
				NoPlayer:;
			}
			playerOut = plyer;
			return playerOut;
		}

		public static List<PlayerInfo> GetLeaderBoard(int num)
		{
			List<string> players = new List<string>(File.ReadAllLines(XPDataPath));
			List<PlayerInfo> top = new List<PlayerInfo>();

			for (int i = 0; i < num; i++)
			{
				if (players.Count > 0)
				{
					string highestPlayer = "unknown";
					string highestSteamID = "unknown";
					int highestLevel = 0;
					int highestXP = 0;

					foreach (string steamid in players)
					{
						string[] temp = steamid.Split(':');
						int level = Int32.Parse(temp[1]);
						int xp = Int32.Parse(temp[2]);
						if (level > highestLevel)
						{
							highestPlayer = steamid;
							highestSteamID = temp[0];
							highestLevel = level;
							highestXP = xp;
						}
						else if (level == highestLevel)
						{
							if (xp > highestXP)
							{
								highestPlayer = steamid;
								highestSteamID = temp[0];
								highestLevel = level;
								highestXP = xp;
							}
						}
					}
					PlayerInfo info = new PlayerInfo()
					{
						pSteamID = highestSteamID,
						pLevel = highestLevel.ToString(),
						pXP = highestXP.ToString()
					};

					top.Add(info);
					players.Remove(highestPlayer);
				}
			}
			return top;
		}

		public static int GetPlayerPlace(string steamid)
		{
			List<PlayerInfo> players = GetLeaderBoard(File.ReadAllLines(XPDataPath).Length);

			for (int i = 0; i < players.Count; i++)
			{
				if (steamid == players[i].pSteamID)
				{
					return i + 1;
				}
			}
			return -1;
		}
	}
}

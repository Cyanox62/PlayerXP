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
	version = "0.9",
	SmodMajor = 3,
	SmodMinor = 0,
	SmodRevision = 0
	)]
	public class PlayerXP : Plugin
	{
		public static Plugin plugin;
		public static float xpScale;
		public static string dirSeperator = Path.DirectorySeparatorChar.ToString();
		public static string XPPath = FileManager.GetAppFolder() + "PlayerXP";
		public static string XPDataPath = FileManager.GetAppFolder() + "PlayerXP" + dirSeperator + "PlayerXPData.txt";

		public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();

		public override void OnDisable() { }

		public override void OnEnable()
		{
			plugin = this;

			if (!Directory.Exists(XPPath))
			{
				Directory.CreateDirectory(XPPath);
			}

			UpdateRankings();
			RemoveLvlZero();
		}

		public override void Register()
		{
			AddEventHandlers(new EventHandler(this));
			this.AddCommands(new string[] { "lvl", "level" }, new LevelCommand(this));
			this.AddCommands(new string[] { "leaderboard" }, new LeaderboardCommand(this));
			this.AddCommand("xpport", new XPPortCommand(this));
			this.AddCommand("xpupdate", new XPUpdateCommand(this));

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

			// SCP-079
			this.AddConfig(new Smod2.Config.ConfigSetting("xp_scp079_player_killed", 10, Smod2.Config.SettingType.NUMERIC, true, ""));

		}

		public static int GetLevel(string steamid)
		{
			return int.Parse(File.ReadAllText(XPPath + dirSeperator + steamid + ".txt").Split(':')[0]);
		}

		public static int GetXP(string steamid)
		{
			return int.Parse(File.ReadAllText(XPPath + dirSeperator + steamid + ".txt").Split(':')[1]);
		}

		public static int XpToLevelUp(string steamid)
		{
			string[] data = File.ReadAllText(XPPath + dirSeperator + steamid + ".txt").Split(':');
			int lvl = int.Parse(data[0]);
			int currXP = int.Parse(data[1]);

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
			string[] files = Directory.GetFiles(XPPath);
			Dictionary<string, PlayerInfo> tempDict = new Dictionary<string, PlayerInfo>();
			foreach (string file in files)
			{
				string[] data = File.ReadAllText(file).Split(':');
				int level = int.Parse(data[0]);
				int currXP = int.Parse(data[1]);
				if (level == 1 && currXP == 0)
					File.Delete(file);
			}
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

		public static Dictionary<string, PlayerInfo> GetLeaderBoard(int num)
		{
			return pInfoDict.Take(num).ToDictionary(x => x.Key, x => x.Value);
		}

		public static int GetPlayerPlace(string steamid)
		{
			return pInfoDict.Keys.ToList().IndexOf(steamid) + 1;
		}

		public static string[] StringToStringArray(string input)
		{
			List<string> data = new List<string>();
			if (input.Length > 0)
			{
				string[] a = input.Split(' ');
				for (int i = 0; i < a.Count(); i++)
				{
					data.Add(a[i]);
				}
			}
			return data.ToArray();
		}

		public static void UpdateRankings()
		{
			string[] files = Directory.GetFiles(XPPath);
			Dictionary<string, PlayerInfo> tempDict = new Dictionary<string, PlayerInfo>();
			foreach (string file in files)
			{
				string[] data = File.ReadAllText(file).Split(':');
				int level = int.Parse(data[0]);
				int currXP = int.Parse(data[1]);
				tempDict.Add(file.Replace(XPPath + dirSeperator, "").Replace(".txt", ""), new PlayerInfo(level, currXP));
			}
			pInfoDict = tempDict.OrderByDescending(x => x.Value.pLevel).ThenByDescending(x => x.Value.pXP).ToDictionary(x => x.Key, x => x.Value);
		}
	}
}

using System;
using System.IO;
using Smod2;
using Smod2.Commands;
using Smod2.API;

namespace PlayerXP
{
	class TopLevelCommand : ICommandHandler
	{
		private Plugin plugin;

		public TopLevelCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Returns the highest player's level and xp.";
		}

		public string GetUsage()
		{
			return "(TOPPLAYER)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			string[] lines = File.ReadAllLines(PlayerXP.XPDataPath);
			string highestSteamID = "unknown";
			int highestLevel = 0;
			int highestXP = 0;
			foreach (string steamid in lines)
			{
				string[] temp = steamid.Split(':');
				int level = Int32.Parse(temp[1]);
				int xp = Int32.Parse(temp[2]);
				if (level > highestLevel && xp > highestXP)
				{
					highestSteamID = temp[0];
					highestLevel = level;
					highestXP = xp;
				}
			}

			Player player = PlayerXP.GetPlayer(highestSteamID);
			string name;

			if (player != null)
				name = "\"" + player.Name + "\"";
			else
				name = "Unconnected";

			return new string[] { "Player " + name + " (" + highestSteamID + ")", "Level: " + highestLevel.ToString(), "XP: " + highestXP.ToString() + "/" + PlayerXP.XpToLevelUp(highestSteamID) };
		}
	}
}

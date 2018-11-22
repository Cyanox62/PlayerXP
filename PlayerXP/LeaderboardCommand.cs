using System;
using System.Collections.Generic;
using Smod2;
using Smod2.Commands;
using Smod2.API;

namespace PlayerXP
{
	class LeaderboardCommand : ICommandHandler
	{
		private Plugin plugin;

		public LeaderboardCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Returns the top players and their stats.";
		}

		public string GetUsage()
		{
			return "(LEADERBOARD) (NUMBER)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			int num = 5;
			if (args.Length > 0)
			{
				if (Int32.TryParse(args[0], out int a))
				{
					num = a;
				}
			}
			List<PlayerInfo> topPlayers = PlayerXP.GetLeaderBoard(num);
			List<string> output = new List<string>();
			if (topPlayers.Count > 0)
			{
				output.Add("Top " + num.ToString() + " Players:");

				for (int i = 0; i < topPlayers.Count; i++)
				{
					Player player = PlayerXP.GetPlayer(topPlayers[i].pSteamID);
					string name;

					if (player != null)
						name = "\"" + player.Name + "\"";
					else
						name = "Unconnected";

					output.Add((i + 1).ToString() + ") " + name + " (" + topPlayers[i].pSteamID + ") | " + "Level: " + topPlayers[i].pLevel + " | XP: " + topPlayers[i].pXP + " / " + PlayerXP.XpToLevelUp(topPlayers[i].pSteamID));
				}

				return output.ToArray();
			}
			return new string[] { "Error: there is not enough data to display the leaderboard." };
		}
	}
}

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
				if (int.TryParse(args[0], out int a))
				{
					num = a;
				}
			}
			Dictionary<string, PlayerInfo> dict = PlayerXP.GetLeaderBoard(num);
			List<string> output = new List<string>();
			int count = 1;
			if (dict.Count > 0)
			{
				output.Add("Top " + num.ToString() + " Players:");

				foreach (KeyValuePair<string, PlayerInfo> info in dict)
				{
					Player player = PlayerXP.GetPlayer(info.Key);
					string name;

					if (player != null)
						name = "\"" + player.Name + "\"";
					else
						name = "Unconnected";

					output.Add(count.ToString() + ") " + name + " (" + info.Key + ") | " + "Level: " + info.Value.pLevel + " | XP: " + info.Value.pXP + " / " + PlayerXP.XpToLevelUp(info.Key));
					count++;
				}

				return output.ToArray();
			}
			return new string[] { "Error: there is not enough data to display the leaderboard." };
		}
	}
}

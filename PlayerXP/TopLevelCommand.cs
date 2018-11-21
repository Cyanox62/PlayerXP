using System;
using System.Collections.Generic;
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
			List<PlayerInfo> topPlayers = PlayerXP.GetLeaderBoard(1);		

			Player player = PlayerXP.GetPlayer(topPlayers[0].pSteamID);
			string name;

			if (player != null)
				name = "\"" + player.Name + "\"";
			else
				name = "Unconnected";

			return new string[] { "Player " + name + " (" + topPlayers[0].pSteamID + ")", "Level: " + topPlayers[0].pLevel, "XP: " + topPlayers[0].pXP + "/" + PlayerXP.XpToLevelUp(topPlayers[0].pSteamID) };
		}
	}
}

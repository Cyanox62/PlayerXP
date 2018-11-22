using System;
using System.IO;
using Smod2;
using Smod2.Commands;
using Smod2.API;

namespace PlayerXP
{
	class LevelCommand : ICommandHandler
	{
		private Plugin plugin;

		public LevelCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Returns a players level and xp.";
		}

		public string GetUsage()
		{
			return "(LVL / LEVEL) (PLAYER NAME / STEAMID)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			if (args.Length > 0)
			{
				if (args[0].Length > 0)
				{
					string steamid;
					Player myPlayer = PlayerXP.GetPlayer(args[0], out myPlayer);
					if (myPlayer != null)
					{
						steamid = myPlayer.SteamId;
					}
					else if (ulong.TryParse(args[0], out ulong a))
					{
						steamid = a.ToString();
					}
					else
					{
						return new string[] { "Error: invalid player." };
					}
					string name;
					Player tempPlayer = PlayerXP.GetPlayer(steamid);
					if (tempPlayer != null)
						name = "\"" + tempPlayer.Name + "\"";
					else
						name = "Unconnected";


					return new string[] { "Player: " + name + " (" + steamid + ")", "Level: " + PlayerXP.GetLevel(steamid), "XP: " + PlayerXP.GetXP(steamid) + "/" + PlayerXP.XpToLevelUp(steamid), "Server ranking: " + PlayerXP.GetPlayerPlace(steamid).ToString() };
				}
			}
			return new string[] { GetUsage() };
		}
	}
}

using Smod2;
using System.IO;
using Smod2.Commands;

namespace PlayerXP
{
	class XPPortCommand : ICommandHandler
	{
		private Plugin plugin;

		public XPPortCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Fixed stuff";
		}

		public string GetUsage()
		{
			return "(XPPORT)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			if (File.Exists(PlayerXP.XPDataPath))
			{
				string[] ids = File.ReadAllLines(PlayerXP.XPDataPath);

				foreach (string str in ids)
				{
					string[] split = str.Split(':');
					File.WriteAllText(PlayerXP.XPPath + PlayerXP.dirSeperator + split[0] + ".txt", split[1] + ":" + split[2]);
				}
				File.Delete(PlayerXP.XPDataPath);
				PlayerXP.UpdateRankings();
				return new string[] { "Your data has been ported to the new system. Do not run this command a second time." };
			}
			return new string[] { "Error: file doesn't exist. You most likely have already ported to the new system." };
		}
	}
}

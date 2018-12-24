using Smod2;
using System.IO;
using Smod2.Commands;

namespace PlayerXP
{
	class XPUpdateCommand : ICommandHandler
	{
		private Plugin plugin;

		public XPUpdateCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Update server rankings.";
		}

		public string GetUsage()
		{
			return "(XPUPDATE)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			PlayerXP.UpdateRankings();
			return new string[] { "Data has been updated." };
		}
	}
}

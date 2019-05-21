using Smod2;
using Smod2.Commands;

namespace PlayerXP
{
	class XPToggleCommand : ICommandHandler
	{
		private Plugin plugin;

		public XPToggleCommand(Plugin plugin)
		{
			this.plugin = plugin;
		}

		public string GetCommandDescription()
		{
			return "Toggles server xp.";
		}

		public string GetUsage()
		{
			return "(XPTOGGLE)";
		}

		public string[] OnCall(ICommandSender sender, string[] args)
		{
			PlayerXP.isToggled = !PlayerXP.isToggled;
			return new string[] { $"PlayerXP has been toggled {(PlayerXP.isToggled ? "on" : "off")}." };
		}
	}
}

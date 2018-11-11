using System;
using System.IO;
using Smod2;
using Smod2.API;
using Smod2.EventHandlers;
using Smod2.Events;

namespace PlayerXP
{
	class EventHandler : IEventHandlerPlayerJoin, IEventHandlerPlayerDie, IEventHandlerRoundEnd, IEventHandlerRoundStart,  IEventHandlerCheckEscape, IEventHandlerRecallZombie, IEventHandlerPocketDimensionDie
	{
		private Plugin plugin;
		private bool roundStarted = false;

		public EventHandler(Plugin plugin)
		{
			this.plugin = plugin;
		}

		private bool IsPlayerInData(Player player)
		{
			string[] players = File.ReadAllLines(PlayerXP.XPDataPath);

			foreach (string steamid in players)
			{
				if (player.SteamId == steamid.Split(':')[0])
				{
					return true;
				}
			}
			return false;
		}

		private void AddXP(string steamid, int xp)
		{
			int lineNum = PlayerXP.GetLine(steamid);
			string[] players = File.ReadAllLines(PlayerXP.XPDataPath);
			int level = Int32.Parse(players[lineNum].Split(':')[1]);
			int currXP = Int32.Parse(players[lineNum].Split(':')[2]);
			currXP += xp;
			if (currXP >= level * 250 + 750)
			{
				currXP -= level * 250 + 750;
				level++;
			}
			players[lineNum] = players[lineNum].Split(':')[0] + ":" + level.ToString() + ":" + currXP.ToString();

			File.WriteAllText(PlayerXP.XPDataPath, String.Empty);
			File.WriteAllLines(PlayerXP.XPDataPath, players);
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			roundStarted = true;
		}

		public void OnPlayerJoin(PlayerJoinEvent ev)
		{
			if (!IsPlayerInData(ev.Player))
			{
				File.AppendAllText(PlayerXP.XPDataPath, ev.Player.SteamId + ":1:0" + Environment.NewLine);
			}
		}

		public void OnPlayerDie(PlayerDeathEvent ev)
		{
			if (ev.Killer.TeamRole.Team == Team.CLASSD)
			{
				int xpGained = 0;
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
					xpGained = DClassXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					AddXP(ev.Killer.SteamId, DClassXP.NineTailedFoxKill);
				if (ev.Player.TeamRole.Team == Team.SCP)
					AddXP(ev.Killer.SteamId, DClassXP.SCPKill);
			}

			if (ev.Killer.TeamRole.Team == Team.SCIENTISTS)
			{
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					AddXP(ev.Killer.SteamId, ScientistXP.DClassKill);
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					AddXP(ev.Killer.SteamId, ScientistXP.ChaosKill);
				if (ev.Player.TeamRole.Team == Team.SCP)
					AddXP(ev.Killer.SteamId, ScientistXP.SCPKill);
			}

			if (ev.Killer.TeamRole.Team == Team.NINETAILFOX)
			{
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					AddXP(ev.Killer.SteamId, NineTailedFoxXP.DClassKill);
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					AddXP(ev.Killer.SteamId, NineTailedFoxXP.ChaosKill);
				if (ev.Player.TeamRole.Team == Team.SCP)
					AddXP(ev.Killer.SteamId, NineTailedFoxXP.SCPKill);
			}

			if (ev.Killer.TeamRole.Team == Team.CHAOS_INSURGENCY)
			{
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
					AddXP(ev.Killer.SteamId, ChaosXP.ScientistKill);
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					AddXP(ev.Killer.SteamId, ChaosXP.NineTailedFoxKill);
				if (ev.Player.TeamRole.Team == Team.SCP)
					AddXP(ev.Killer.SteamId, ChaosXP.SCPKill);
			}

			if (ev.Killer.TeamRole.Team == Team.TUTORIAL)
			{
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					AddXP(ev.Killer.SteamId, TutorialXP.DClassKill);
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
					AddXP(ev.Killer.SteamId, TutorialXP.ScientistKill);
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					AddXP(ev.Killer.SteamId, TutorialXP.NineTailedFoxKill);
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					AddXP(ev.Killer.SteamId, TutorialXP.ChaosKill);
			}

			if (ev.Killer.TeamRole.Team == Team.SCP)
			{
				AddXP(ev.Killer.SteamId, AllXP.SCPKillPlayer);

				foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					if (player.TeamRole.Role == Role.TUTORIAL)
						AddXP(player.SteamId, TutorialXP.SCPKillsPlayer);
			}

			if (ev.Player.Name != ev.Killer.Name && ev.Killer != null)
				ev.Player.SendConsoleMessage("You were killed by " + ev.Killer.Name + ", level " + PlayerXP.GetLevel(ev.Killer.SteamId).ToString() + ".");
		}

		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
			foreach (Player player in plugin.pluginManager.Server.GetPlayers())
			{
				if (player.TeamRole.Role == Role.SCP_106)
				{
					AddXP(player.SteamId, SCP106XP.DeathInPD);
					ev.Player.SendConsoleMessage("You were killed by " + player.Name + ", level " + PlayerXP.GetLevel(player.SteamId).ToString() + ".");
				}
			}
		}

		public void OnRecallZombie(PlayerRecallZombieEvent ev)
		{
			AddXP(ev.Player.SteamId, SCP049XP.ZombieCreated);
		}

		public void OnCheckEscape(PlayerCheckEscapeEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.CLASSD)
			{
				AddXP(ev.Player.SteamId, DClassXP.Escape);
				foreach (Player player in plugin.pluginManager.Server.GetPlayers())
				{
					if (player.TeamRole.Team == Team.CHAOS_INSURGENCY)
						AddXP(player.SteamId, ChaosXP.DClassEscape);
				}
			}
			if (ev.Player.TeamRole.Role == Role.SCIENTIST)
			{
				AddXP(ev.Player.SteamId, ScientistXP.Escape);
				foreach (Player player in plugin.pluginManager.Server.GetPlayers())
				{
					if (player.TeamRole.Team == Team.NINETAILFOX)
						AddXP(player.SteamId, NineTailedFoxXP.ScientistEscape);
				}
			}
		}

		public void OnRoundEnd(RoundEndEvent ev)
		{
			foreach (Player player in plugin.Server.GetPlayers())
			{
				if (player.TeamRole.Team != Team.SPECTATOR && player.TeamRole.Team != Team.NONE && roundStarted)
				{
					AddXP(player.SteamId, AllXP.RoundWinXP);
				}
			}
			roundStarted = false;
		}
	}
}

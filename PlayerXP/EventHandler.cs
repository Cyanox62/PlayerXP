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

		private Player FindPlayer(string steamid)
		{
			foreach (Player player in plugin.pluginManager.Server.GetPlayers())
				if (player.SteamId == steamid)
					return player;
			return null;
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
				FindPlayer(steamid).SendConsoleMessage("You've leveled up to level " + level.ToString() + "!" + " You need " + ((level * 250 + 750) - currXP).ToString() + "xp for your next level.", "yellow");
			}
			players[lineNum] = players[lineNum].Split(':')[0] + ":" + level.ToString() + ":" + currXP.ToString();

			File.WriteAllText(PlayerXP.XPDataPath, String.Empty);
			File.WriteAllLines(PlayerXP.XPDataPath, players);
		}

		public void OnRoundStart(RoundStartEvent ev)
		{
			roundStarted = true;
			PlayerXP.xpScale = plugin.GetConfigFloat("xp_scale");
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
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
					gainedXP = DClassXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					gainedXP = DClassXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Team.SCP)
					gainedXP = DClassXP.SCPKill;

				if (gainedXP > 0)
				{
					AddXP(ev.Killer.SteamId, gainedXP);
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}
			}

			if (ev.Killer.TeamRole.Team == Team.SCIENTISTS)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					gainedXP = ScientistXP.DClassKill;
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					gainedXP = ScientistXP.ChaosKill;
				if (ev.Player.TeamRole.Team == Team.SCP)
					gainedXP = ScientistXP.SCPKill;

				if (gainedXP > 0)
				{
					AddXP(ev.Killer.SteamId, gainedXP);
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}
			}

			if (ev.Killer.TeamRole.Team == Team.NINETAILFOX)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					gainedXP = NineTailedFoxXP.DClassKill;
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					gainedXP = NineTailedFoxXP.ChaosKill;
				if (ev.Player.TeamRole.Team == Team.SCP)
					gainedXP = NineTailedFoxXP.SCPKill;

				if (gainedXP > 0)
				{
					AddXP(ev.Killer.SteamId, gainedXP);
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}
			}

			if (ev.Killer.TeamRole.Team == Team.CHAOS_INSURGENCY)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
					gainedXP = ChaosXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					gainedXP = ChaosXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Team.SCP)
					gainedXP = ChaosXP.SCPKill;

				if (gainedXP > 0)
				{
					AddXP(ev.Killer.SteamId, gainedXP);
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}
			}

			if (ev.Killer.TeamRole.Team == Team.TUTORIAL)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Team.CLASSD)
					gainedXP = TutorialXP.DClassKill;
				if (ev.Player.TeamRole.Team == Team.SCIENTISTS)
				    gainedXP = TutorialXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Team.NINETAILFOX)
					gainedXP = TutorialXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Team.CHAOS_INSURGENCY)
					gainedXP = TutorialXP.ChaosKill;

				if (gainedXP > 0)
				{
					AddXP(ev.Killer.SteamId, gainedXP);
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}
			}

			if (ev.Killer.TeamRole.Team == Team.SCP)
			{
				if (AllXP.SCPKillPlayer > 0)
				{
					AddXP(ev.Killer.SteamId, AllXP.SCPKillPlayer);
					ev.Killer.SendConsoleMessage("You have gained " + AllXP.SCPKillPlayer.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
				}

				if (TutorialXP.SCPKillsPlayer > 0 && ev.Player.TeamRole.Team != Team.TUTORIAL)
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Role == Role.TUTORIAL)
						{
							AddXP(player.SteamId, TutorialXP.SCPKillsPlayer);
							player.SendConsoleMessage("You have gained " + TutorialXP.SCPKillsPlayer.ToString() + "xp for an SCP killing an enemy!", "yellow");
						}
					}
				}
			}

			if (ev.Player.Name != ev.Killer.Name && ev.Killer != null && ev.Killer.SteamId != string.Empty)
				ev.Player.SendConsoleMessage("You were killed by " + ev.Killer.Name + ", level " + PlayerXP.GetLevel(ev.Killer.SteamId).ToString() + ".", "yellow");
			if (ev.Player != null && ev.Player.SteamId != string.Empty)
				ev.Player.SendConsoleMessage("You have " + PlayerXP.GetXP(ev.Player.SteamId) + "/" + PlayerXP.XpToLevelUp(ev.Player.SteamId) + "xp until you reach level " + (PlayerXP.GetLevel(ev.Player.SteamId) + 1).ToString() + ".", "yellow");
		}

		public void OnPocketDimensionDie(PlayerPocketDimensionDieEvent ev)
		{
			if (SCP106XP.DeathInPD > 0)
			{
				foreach (Player player in plugin.pluginManager.Server.GetPlayers())
				{
					if (player.TeamRole.Role == Role.SCP_106)
					{
						AddXP(player.SteamId, SCP106XP.DeathInPD);
						player.SendConsoleMessage("You have gained " + SCP106XP.DeathInPD.ToString() + "xp for killing " + ev.Player.Name + " in the pocket dimension!", "yellow");
						ev.Player.SendConsoleMessage("You were killed by " + player.Name + ", level " + PlayerXP.GetLevel(player.SteamId).ToString() + ".", "yellow");
					}
				}
			}
		}

		public void OnRecallZombie(PlayerRecallZombieEvent ev)
		{
			if (SCP049XP.ZombieCreated > 0)
			{
				AddXP(ev.Player.SteamId, SCP049XP.ZombieCreated);
				ev.Player.SendConsoleMessage("You have gained " + SCP049XP.ZombieCreated.ToString() + "xp for turning " + ev.Target.Name + " into a zombie!", "yellow");
			}
		}

		public void OnCheckEscape(PlayerCheckEscapeEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.CLASSD)
			{
				if (DClassXP.Escape > 0)
				{
					AddXP(ev.Player.SteamId, DClassXP.Escape);
					ev.Player.SendConsoleMessage("You have gained " + DClassXP.Escape.ToString() + "xp for escaping as a Class-D!", "yellow");
				}

				if (ChaosXP.DClassEscape > 0)
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Team == Team.CHAOS_INSURGENCY)
						{
							AddXP(player.SteamId, ChaosXP.DClassEscape);
							player.SendConsoleMessage("You have gained " + ChaosXP.DClassEscape.ToString() + "xp for " + ev.Player.Name + " escaping as a Class-D!", "yellow");
						}
					}
				}
			}

			if (ev.Player.TeamRole.Role == Role.SCIENTIST)
			{
				if (ScientistXP.Escape > 0)
				{
					AddXP(ev.Player.SteamId, ScientistXP.Escape);
					ev.Player.SendConsoleMessage("You have gained " + ScientistXP.Escape.ToString() + "xp for escaping as a Scientist!", "yellow");
				}

				if (NineTailedFoxXP.ScientistEscape > 0)
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Team == Team.NINETAILFOX)
						{
							AddXP(player.SteamId, NineTailedFoxXP.ScientistEscape);
							player.SendConsoleMessage("You have gained " + NineTailedFoxXP.ScientistEscape.ToString() + "xp for " + ev.Player.Name + " escaping as a Scientist!", "yellow");
						}
					}
				}
			}
		}

		public void OnRoundEnd(RoundEndEvent ev)
		{
			if (AllXP.RoundWinXP > 0)
			{
				foreach (Player player in plugin.Server.GetPlayers())
				{
					if (player.TeamRole.Team != Team.SPECTATOR && player.TeamRole.Team != Team.NONE && roundStarted)
					{
						AddXP(player.SteamId, AllXP.RoundWinXP);
						player.SendConsoleMessage("You have gained " + AllXP.RoundWinXP.ToString() + "xp for winning the round!", "yellow");
					}
				}
			}
			roundStarted = false;
		}
	}
}

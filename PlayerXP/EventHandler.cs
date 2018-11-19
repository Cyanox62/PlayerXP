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

		private void RemoveXP(string steamid, int xp)
		{
			int lineNum = PlayerXP.GetLine(steamid);
			string[] players = File.ReadAllLines(PlayerXP.XPDataPath);
			int level = Int32.Parse(players[lineNum].Split(':')[1]);
			int currXP = Int32.Parse(players[lineNum].Split(':')[2]);

			currXP -= xp;
			if (currXP <= 0)
			{
				if (level > 1)
				{
					level--;
					currXP = (level * 250 + 750) - Math.Abs(currXP);
				}
				else
				{
					currXP = 0;
				}
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
			if (ev.Killer.TeamRole.Team == ev.Player.TeamRole.Team && ev.Killer.SteamId != ev.Player.SteamId && roundStarted && AllXP.TeamKillPunishment > 0)
			{
				ev.Killer.SendConsoleMessage("You have lost " + AllXP.TeamKillPunishment.ToString() + "xp for teamkilling " + ev.Player.Name + ".", "yellow");
				RemoveXP(ev.Killer.SteamId, AllXP.TeamKillPunishment);
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.CLASSD)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCIENTISTS)
					gainedXP = DClassXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
					gainedXP = DClassXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
					gainedXP = DClassXP.SCPKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.TUTORIAL)
					gainedXP = DClassXP.TutorialKill;

				if (gainedXP > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, gainedXP);
				}
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.SCIENTISTS)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD)
					gainedXP = ScientistXP.DClassKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
					gainedXP = ScientistXP.ChaosKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
					gainedXP = ScientistXP.SCPKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.TUTORIAL)
					gainedXP = ScientistXP.TutorialKill;

				if (gainedXP > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, gainedXP);
				}
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD)
					gainedXP = NineTailedFoxXP.DClassKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
					gainedXP = NineTailedFoxXP.ChaosKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
					gainedXP = NineTailedFoxXP.SCPKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.TUTORIAL)
					gainedXP = NineTailedFoxXP.TutorialKill;

				if (gainedXP > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, gainedXP);
				}
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCIENTISTS)
					gainedXP = ChaosXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
					gainedXP = ChaosXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCP)
					gainedXP = ChaosXP.SCPKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.TUTORIAL)
					gainedXP = ChaosXP.TutorialKill;

				if (gainedXP > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, gainedXP);
				}
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.TUTORIAL)
			{
				int gainedXP = 0;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CLASSD)
					gainedXP = TutorialXP.DClassKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.SCIENTISTS)
				    gainedXP = TutorialXP.ScientistKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
					gainedXP = TutorialXP.NineTailedFoxKill;
				if (ev.Player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
					gainedXP = TutorialXP.ChaosKill;

				if (gainedXP > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + gainedXP.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, gainedXP);
				}
			}

			if (ev.Killer.TeamRole.Team == Smod2.API.Team.SCP)
			{
				if (AllXP.SCPKillPlayer > 0 && ev.Player.SteamId != ev.Killer.SteamId)
				{
					ev.Killer.SendConsoleMessage("You have gained " + AllXP.SCPKillPlayer.ToString() + "xp for killing " + ev.Player.Name + "!", "yellow");
					AddXP(ev.Killer.SteamId, AllXP.SCPKillPlayer);
				}

				if (TutorialXP.SCPKillsPlayer > 0 && ev.Player.TeamRole.Team != Smod2.API.Team.TUTORIAL && ev.Player.SteamId != ev.Killer.SteamId)
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Role == Role.TUTORIAL)
						{
							player.SendConsoleMessage("You have gained " + TutorialXP.SCPKillsPlayer.ToString() + "xp for an SCP killing an enemy!", "yellow");
							AddXP(player.SteamId, TutorialXP.SCPKillsPlayer);
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
					if (player.TeamRole.Role == Role.SCP_106 && ev.Player.SteamId != player.SteamId && player != null && ev.Player != null)
					{
						player.SendConsoleMessage("You have gained " + SCP106XP.DeathInPD.ToString() + "xp for killing " + ev.Player.Name + " in the pocket dimension!", "yellow");
						ev.Player.SendConsoleMessage("You were killed by " + player.Name + ", level " 
							+ PlayerXP.GetLevel(player.SteamId).ToString() + ".", "yellow");
						AddXP(player.SteamId, SCP106XP.DeathInPD);
					}
				}
			}
		}

		public void OnRecallZombie(PlayerRecallZombieEvent ev)
		{
			if (SCP049XP.ZombieCreated > 0 && ev.Player.SteamId != ev.Target.SteamId)
			{
				ev.Player.SendConsoleMessage("You have gained " + SCP049XP.ZombieCreated.ToString() + "xp for turning " + ev.Target.Name + " into a zombie!", "yellow");
				AddXP(ev.Player.SteamId, SCP049XP.ZombieCreated);
			}
		}

		public void OnCheckEscape(PlayerCheckEscapeEvent ev)
		{
			if (ev.Player.TeamRole.Role == Role.CLASSD)
			{
				if (DClassXP.Escape > 0)
				{
					ev.Player.SendConsoleMessage("You have gained " + DClassXP.Escape.ToString() + "xp for escaping as a Class-D!", "yellow");
					AddXP(ev.Player.SteamId, DClassXP.Escape);
				}

				if (ChaosXP.DClassEscape > 0 && !ev.Player.IsHandcuffed())
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Team == Smod2.API.Team.CHAOS_INSURGENCY)
						{
							player.SendConsoleMessage("You have gained " + ChaosXP.DClassEscape.ToString() + "xp for " + ev.Player.Name + " escaping as a Class-D!", "yellow");
							AddXP(player.SteamId, ChaosXP.DClassEscape);
						}
					}
				}
			}

			if (ev.Player.TeamRole.Role == Role.SCIENTIST)
			{
				if (ScientistXP.Escape > 0)
				{
					ev.Player.SendConsoleMessage("You have gained " + ScientistXP.Escape.ToString() + "xp for escaping as a Scientist!", "yellow");
					AddXP(ev.Player.SteamId, ScientistXP.Escape);
				}

				if (NineTailedFoxXP.ScientistEscape > 0 && !ev.Player.IsHandcuffed())
				{
					foreach (Player player in plugin.pluginManager.Server.GetPlayers())
					{
						if (player.TeamRole.Team == Smod2.API.Team.NINETAILFOX)
						{
							player.SendConsoleMessage("You have gained " + NineTailedFoxXP.ScientistEscape.ToString() + "xp for " + ev.Player.Name + " escaping as a Scientist!", "yellow");
							AddXP(player.SteamId, NineTailedFoxXP.ScientistEscape);
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
					if (player.TeamRole.Team != Smod2.API.Team.SPECTATOR && player.TeamRole.Team != Smod2.API.Team.NONE && roundStarted)
					{
						player.SendConsoleMessage("You have gained " + AllXP.RoundWinXP.ToString() + "xp for winning the round!", "yellow");
						AddXP(player.SteamId, AllXP.RoundWinXP);
					}
				}
			}
			roundStarted = false;
		}
	}
}

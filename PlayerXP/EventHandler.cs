using Exiled.Events.EventArgs;
using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace PlayerXP
{
	partial class EventHandler
	{
		public static Dictionary<string, PlayerInfo> pInfoDict = new Dictionary<string, PlayerInfo>();
		public static Dictionary<string, PlayerInfo> pLeaderboard = new Dictionary<string, PlayerInfo>();

		private bool isRoundStarted = false;
		private bool isToggled = true;

		public void OnConsoleCommand(SendingConsoleCommandEventArgs ev)
		{
			string cmd = ev.Name.ToLower();
			if (cmd == "level" || cmd == "lvl")
			{
				Player player = ev.Arguments.Count == 0 ? ev.Player : Player.Get(ev.Arguments[0]);
				string name;
				bool hasData = pInfoDict.ContainsKey(player.UserId);
				if (player != null) name = player.Nickname;
				else name = hasData ? pInfoDict[ev.Player.UserId].level.ToString() : "[NO DATA]";
				ev.ReturnMessage =
					$"Player: {name} ({player.UserId})\n" +
					$"Level: {(hasData ? pInfoDict[player.UserId].level.ToString() : "[NO DATA]")}\n" +
					$"XP: {(hasData ? pInfoDict[player.UserId].xp.ToString() : "[NO DATA]")}";
			}
			else if (cmd == "leaderboard" || cmd == "lb")
			{
				string output;
				int num = 5;
				if (ev.Arguments.Count > 0 && int.TryParse(ev.Arguments[0], out int n)) num = n;
				if (num > 15)
				{
					ev.Color = "red";
					ev.ReturnMessage = "Leaderboards can be no larger than 15.";
					return;
				}
				if (pInfoDict.Count != 0)
				{
					output = $"Top {num} Players:\n";

					for (int i = 0; i < num; i++)
					{
						if (pInfoDict.Count == i) break;
						string userid = pInfoDict.ElementAt(i).Key;
						PlayerInfo info = pInfoDict[userid];
						output += $"{i + 1}) {info.name} ({userid}) | Level: {info.level} | XP: {info.xp} / {XpToLevelUp(userid)}";
						if (i != pInfoDict.Count - 1) output += "\n";
						else break;
					}

					ev.Color = "yellow";
					ev.ReturnMessage = output;
				}
				else
				{
					ev.Color = "red";
					ev.ReturnMessage = "Error: there is not enough data to display the leaderboard.";
				}
			}
		}

		public void OnRAConsoleCommand(SendingRemoteAdminCommandEventArgs ev)
		{
			string cmd = ev.Name.ToLower();
			if (cmd == "xptoggle") isToggled = false;
			else if (cmd == "xpsave") SaveStats();
		}

		public void OnPlayerJoin(JoinedEventArgs ev)
		{
			if (!File.Exists(Path.Combine(PlayerXP.XPPath, $"{ev.Player.UserId}.json"))) pInfoDict.Add(ev.Player.UserId, new PlayerInfo(ev.Player.Nickname));
		}

		public void OnWaitingForPlayers()
		{
			UpdateCache();
		}

		public void OnRoundStart() => isRoundStarted = true;

		public void OnRoundEnd(RoundEndedEventArgs ev)
		{
			if (isToggled && PlayerXP.instance.Config.RoundWin > 0)
			{
				foreach (Player player in Player.List)
				{
					if (player.Team != Team.RIP && isRoundStarted)
					{
						AddXP(player.UserId, PlayerXP.instance.Config.RoundWin);
						//AddXP(player.UserId, PlayerXP.instance.Config.RoundWin, $"You have gained {PlayerXP.instance.Config.RoundWin} xp for winning the round!");
					}
				}
			}
			isRoundStarted = false;
			SaveStats();
			pInfoDict.Clear();
		}

		public void OnRoundRestart()
		{
			// In case of force restart
			if (pInfoDict.Count > 0)
			{
				SaveStats();
				pInfoDict.Clear();
			}
		}

		public void OnPlayerDying(DyingEventArgs ev)
		{
			if (!isToggled) return;

			if (ev.Killer.Team == ev.Target.Team && ev.Killer.UserId != ev.Target.UserId && isRoundStarted && PlayerXP.instance.Config.TeamKillPunishment > 0)
			{
				RemoveXP(ev.Killer.UserId, PlayerXP.instance.Config.TeamKillPunishment, PlayerXP.instance.Config.PlayerTeamkillMessage.Replace("{xp}", PlayerXP.instance.Config.TeamKillPunishment.ToString()).Replace("{target}", ev.Target.Nickname));
			}

			if (ev.Killer.Team == Team.CDP)
			{
				int gainedXP = 0;
				if (ev.Target.Team == Team.RSC) gainedXP = PlayerXP.instance.Config.DclassScientistKill;
				if (ev.Target.Team == Team.MTF) gainedXP = PlayerXP.instance.Config.DclassMtfKill;
				if (ev.Target.Team == Team.SCP) gainedXP = PlayerXP.instance.Config.DclassScpKill;
				if (ev.Target.Team == Team.TUT) gainedXP = PlayerXP.instance.Config.DclassTutorialKill;

				if (gainedXP > 0 && ev.Target.UserId != ev.Killer.UserId)
				{
					AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
				}
			}
			else if (ev.Killer.Team == Team.RSC)
			{
				int gainedXP = 0;
				if (ev.Target.Team == Team.CDP) gainedXP = PlayerXP.instance.Config.ScientistDclassKill;
				if (ev.Target.Team == Team.CHI) gainedXP = PlayerXP.instance.Config.ScientistChaosKill;
				if (ev.Target.Team == Team.SCP) gainedXP = PlayerXP.instance.Config.ScientistScpKill;
				if (ev.Target.Team == Team.TUT) gainedXP = PlayerXP.instance.Config.ScientistTutorialKill;

				if (gainedXP > 0 && ev.Target.UserId != ev.Killer.UserId)
				{
					AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
				}
			}
			else if (ev.Killer.Team == Team.MTF)
			{
				int gainedXP = 0;
				if (ev.Target.Team == Team.CDP) gainedXP = PlayerXP.instance.Config.MtfDclassKill;
				if (ev.Target.Team == Team.CHI) gainedXP = PlayerXP.instance.Config.MtfChaosKill;
				if (ev.Target.Team == Team.SCP) gainedXP = PlayerXP.instance.Config.MtfScpKill;
				if (ev.Target.Team == Team.TUT) gainedXP = PlayerXP.instance.Config.MtfTutorialKill;

				if (gainedXP > 0 && ev.Target.UserId != ev.Killer.UserId)
				{
					AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
				}
			}
			else if (ev.Killer.Team == Team.CHI)
			{
				int gainedXP = 0;
				if (ev.Target.Team == Team.RSC) gainedXP = PlayerXP.instance.Config.ChaosScientistKill;
				if (ev.Target.Team == Team.MTF) gainedXP = PlayerXP.instance.Config.ChaosMtfKill;
				if (ev.Target.Team == Team.SCP) gainedXP = PlayerXP.instance.Config.ChaosScpKill;
				if (ev.Target.Team == Team.TUT) gainedXP = PlayerXP.instance.Config.ChaosTutorialKill;

				if (gainedXP > 0 && ev.Target.UserId != ev.Killer.UserId)
				{
					AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
				}
			}
			else if (ev.Killer.Team == Team.TUT)
			{
				int gainedXP = 0;
				if (ev.Target.Team == Team.CDP) gainedXP = PlayerXP.instance.Config.TutorialDclassKill;
				if (ev.Target.Team == Team.RSC) gainedXP = PlayerXP.instance.Config.TutorialScientistKill;
				if (ev.Target.Team == Team.MTF) gainedXP = PlayerXP.instance.Config.TutorialMtfKill;
				if (ev.Target.Team == Team.CHI) gainedXP = PlayerXP.instance.Config.TutorialChaosKill;

				if (gainedXP > 0 && ev.Target.UserId != ev.Killer.UserId)
				{
					AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
				}
			}
			else if (ev.Killer.Team == Team.SCP)
			{
				int gainedXP = 0;
				if (ev.Target.UserId != ev.Killer.UserId)
				{
					if (ev.Killer.Role == RoleType.Scp049) gainedXP = PlayerXP.instance.Config.Scp049Kill;
					else if (ev.Killer.Role == RoleType.Scp0492) gainedXP = PlayerXP.instance.Config.Scp0492Kill;
					else if (ev.Killer.Role == RoleType.Scp096) gainedXP = PlayerXP.instance.Config.Scp096Kill;
					else if (ev.Killer.Role == RoleType.Scp106) gainedXP = PlayerXP.instance.Config.Scp106Kill;
					else if (ev.Killer.Role == RoleType.Scp173) gainedXP = PlayerXP.instance.Config.Scp173Kill;
					else if (ev.Killer.Role == RoleType.Scp93953 || ev.Killer.Role == RoleType.Scp93989) gainedXP = PlayerXP.instance.Config.Scp939Kill;

					if (gainedXP > 0)
					{
						AddXP(ev.Killer.UserId, gainedXP, PlayerXP.instance.Config.PlayerKillMessage.Replace("{xp}", gainedXP.ToString()).Replace("{target}", ev.Target.Nickname));
					}
				}

				if (PlayerXP.instance.Config.TutorialScpKillsPlayer > 0 && ev.Target.Team != Team.TUT && ev.Target.UserId != ev.Killer.UserId)
				{
					foreach (Player player in Player.List)
					{
						if (player.Role == RoleType.Tutorial)
						{
							AddXP(player.UserId, PlayerXP.instance.Config.TutorialScpKillsPlayer, PlayerXP.instance.Config.TutorialScpKillsPlayerMessage.Replace("{xp}", PlayerXP.instance.Config.TutorialScpKillsPlayer.ToString()).Replace("{target}", ev.Target.Nickname));
						}
					}
				}

				if (PlayerXP.instance.Config.Scp079AssistedKill > 0 && ev.Target.UserId != ev.Killer.UserId && ev.Target.Team != Team.TUT)
				{
					foreach (Player player in Player.List)
					{
						if (player.Role == RoleType.Scp079)
						{ 
							AddXP(player.UserId, PlayerXP.instance.Config.Scp079AssistedKill, PlayerXP.instance.Config.Scp079AssistedKillMessage.Replace("{xp}", PlayerXP.instance.Config.Scp079AssistedKill.ToString()).Replace("{target}", ev.Target.Nickname));
						}
					}
				}
			}

			if (ev.Target.Id != ev.Killer.Id && ev.Killer != null && ev.Killer.UserId != string.Empty) SendHint(ev.Target, PlayerXP.instance.Config.PlayerDeathMessage.Replace("{xp}", GetXP(ev.Killer.UserId).ToString()).Replace("{level}", GetXP(ev.Killer.UserId).ToString()));
			if (ev.Target != null && ev.Target.UserId != string.Empty) ev.Target.SendConsoleMessage($"You have {GetXP(ev.Target.UserId)}/{XpToLevelUp(ev.Target.UserId)} xp until you reach level {GetLevel(ev.Target.UserId) + 1}.</color>", "yellow");
		}

		public void OnPocketDimensionDie(FailingEscapePocketDimensionEventArgs ev)
		{
			if (isToggled && PlayerXP.instance.Config.Scp106PocketDeath > 0)
			{
				foreach (Player player in Player.List)
				{
					if (player.Role == RoleType.Scp106 && ev.Player.UserId != player.UserId && player.Team != Team.TUT && player != null && ev.Player != null && this != null)
					{
						SendHint(ev.Player, PlayerXP.instance.Config.PlayerDeathMessage.Replace("{xp}", GetXP(player.UserId).ToString()).Replace("{level}", GetXP(player.UserId).ToString()));	
						AddXP(player.UserId, PlayerXP.instance.Config.Scp106PocketDeath, PlayerXP.instance.Config.Scp106PocketDimensionDeathMessage.Replace("{xp}", PlayerXP.instance.Config.Scp106PocketDeath.ToString()).Replace("{target}", ev.Player.Nickname));
					}
				}
			}
		}

		public void OnRecallZombie(FinishingRecallEventArgs ev)
		{
			if (isToggled && PlayerXP.instance.Config.Scp049ZombieCreated > 0 && ev.Scp049.UserId != ev.Target.UserId)
			{		
				AddXP(ev.Scp049.UserId, PlayerXP.instance.Config.Scp049ZombieCreated, PlayerXP.instance.Config.Scp049CreateZombieMessage.Replace("{xp}", PlayerXP.instance.Config.Scp049ZombieCreated.ToString()).Replace("{target}", ev.Target.Nickname));
			}
		}

		public void OnCheckEscape(EscapingEventArgs ev)
		{
			if (!isToggled) return;

			if (ev.Player.Role == RoleType.ClassD)
			{
				if (PlayerXP.instance.Config.DclassEscape > 0)
				{
					AddXP(ev.Player.UserId, PlayerXP.instance.Config.DclassEscape, PlayerXP.instance.Config.DclassEscapeMessage.Replace("{xp}", PlayerXP.instance.Config.DclassEscape.ToString()));
				}

				if (PlayerXP.instance.Config.ChaosDclassEscape > 0 && !ev.Player.IsCuffed)
				{
					foreach (Player player in Player.List)
					{
						if (player.Team == Team.CHI)
						{	
							AddXP(player.UserId, PlayerXP.instance.Config.ChaosDclassEscape, PlayerXP.instance.Config.ChaosDclassEscapeMessage.Replace("{xp}", PlayerXP.instance.Config.ChaosDclassEscape.ToString()).Replace("{target}", ev.Player.Nickname));
						}
					}
				}
			}

			if (ev.Player.Role == RoleType.Scientist)
			{
				if (PlayerXP.instance.Config.ScientistEscape > 0)
				{
					AddXP(ev.Player.UserId, PlayerXP.instance.Config.ScientistEscape, PlayerXP.instance.Config.ScientistEscapeMessage.Replace("{xp}", PlayerXP.instance.Config.ScientistEscape.ToString()).Replace("{target}", ev.Player.Nickname));
				}

				if (PlayerXP.instance.Config.MtfScientistEscape > 0 && !ev.Player.IsCuffed)
				{
					foreach (Player player in Player.List)
					{
						if (player.Team == Team.MTF)
						{
							AddXP(player.UserId, PlayerXP.instance.Config.MtfScientistEscape, PlayerXP.instance.Config.MtfScientistEscapeMessage.Replace("{xp}", PlayerXP.instance.Config.MtfScientistEscape.ToString()).Replace("{target}", ev.Player.Nickname));
						}
					}
				}
			}
		}
	}
}

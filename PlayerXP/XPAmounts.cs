namespace PlayerXP
{
	static class AllXP
	{
		public static int RoundWinXP = (int)(PlayerXP.plugin.GetConfigInt("xp_all_round_win") * PlayerXP.xpScale);
		public static int SCPKillPlayer = (int)(PlayerXP.plugin.GetConfigInt("xp_scp_kill_player") * PlayerXP.xpScale);
		public static int TeamKillPunishment = (int)(PlayerXP.plugin.GetConfigInt("xp_team_kill_punishment") * PlayerXP.xpScale);
	}
	static class DClassXP
	{
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("xp_dclass_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("xp_dclass_mtf_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("xp_dclass_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("xp_dclass_tutorial_kill") * PlayerXP.xpScale);
		public static int Escape = (int)(PlayerXP.plugin.GetConfigInt("xp_dclass_escape") * PlayerXP.xpScale);
	}
	static class ScientistXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("xp_scientist_dclass_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("xp_scientist_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("xp_scientist_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("xp_scientist_tutorial_kill") * PlayerXP.xpScale);
		public static int Escape = (int)(PlayerXP.plugin.GetConfigInt("xp_scientist_escape") * PlayerXP.xpScale);
	}
	static class NineTailedFoxXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("xp_mtf_dclass_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("xp_mtf_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("xp_mtf_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("xp_mtf_tutorial_kill") * PlayerXP.xpScale);
		public static int ScientistEscape = (int)(PlayerXP.plugin.GetConfigInt("xp_mtf_scientist_escape") * PlayerXP.xpScale);
	}
	static class ChaosXP
	{
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("xp_chaos_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("xp_chaos_mtf_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("xp_chaos_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("xp_chaos_tutorial_kill") * PlayerXP.xpScale);
		public static int DClassEscape = (int)(PlayerXP.plugin.GetConfigInt("xp_chaos_dclass_escape") * PlayerXP.xpScale);
	}
	static class TutorialXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("xp_tutorial_dclass_kill") * PlayerXP.xpScale);
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("xp_tutorial_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("xp_tutorial_mtf_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("xp_tutorial_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKillsPlayer = (int)(PlayerXP.plugin.GetConfigInt("xp_tutorial_scp_kills_player") * PlayerXP.xpScale);
	}
	static class SCP106XP
	{
		public static int DeathInPD = (int)(PlayerXP.plugin.GetConfigInt("xp_scp106_pocket_death") * PlayerXP.xpScale);
	}
	static class SCP049XP
	{
		public static int ZombieCreated = (int)(PlayerXP.plugin.GetConfigInt("xp_scp049_zombie_created") * PlayerXP.xpScale);
	}
	static class SCP079XP
	{
		public static int PlayerKilled = (int)(PlayerXP.plugin.GetConfigInt("xp_scp079_player_killed") * PlayerXP.xpScale);
	}
}

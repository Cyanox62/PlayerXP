namespace PlayerXP
{
	static class AllXP
	{
		public static int RoundWinXP = PlayerXP.plugin.GetConfigInt("all_round_win");
		public static int SCPKillPlayer = PlayerXP.plugin.GetConfigInt("scp_kill_player");
	}
	static class DClassXP
	{
		public static int ScientistKill = PlayerXP.plugin.GetConfigInt("dclass_scientist_kill");
		public static int NineTailedFoxKill = PlayerXP.plugin.GetConfigInt("dclass_mtf_kill");
		public static int SCPKill = PlayerXP.plugin.GetConfigInt("dclass_scp_kill");
		public static int Escape = PlayerXP.plugin.GetConfigInt("dclass_escape");
	}
	static class ScientistXP
	{
		public static int DClassKill = PlayerXP.plugin.GetConfigInt("scientist_dclass_kill");
		public static int ChaosKill = PlayerXP.plugin.GetConfigInt("scientist_chaos_kill");
		public static int SCPKill = PlayerXP.plugin.GetConfigInt("scientist_scp_kill");
		public static int Escape = PlayerXP.plugin.GetConfigInt("scientist_escape");
	}
	static class NineTailedFoxXP
	{
		public static int DClassKill = PlayerXP.plugin.GetConfigInt("mtf_dclass_kill");
		public static int ChaosKill = PlayerXP.plugin.GetConfigInt("mtf_chaos_kill");
		public static int SCPKill = PlayerXP.plugin.GetConfigInt("mtf_scp_kill");
		public static int ScientistEscape = PlayerXP.plugin.GetConfigInt("mtf_scientist_escape");
	}
	static class ChaosXP
	{
		public static int ScientistKill = PlayerXP.plugin.GetConfigInt("chaos_scientist_kill");
		public static int NineTailedFoxKill = PlayerXP.plugin.GetConfigInt("chaos_mtf_kill");
		public static int SCPKill = PlayerXP.plugin.GetConfigInt("chaos_scp_kill");
		public static int DClassEscape = PlayerXP.plugin.GetConfigInt("chaos_dclass_escape");
	}
	static class TutorialXP
	{
		public static int DClassKill = PlayerXP.plugin.GetConfigInt("tutorial_dclass_kill");
		public static int ScientistKill = PlayerXP.plugin.GetConfigInt("tutorial_scientist_kill");
		public static int NineTailedFoxKill = PlayerXP.plugin.GetConfigInt("tutorial_mtf_kill");
		public static int ChaosKill = PlayerXP.plugin.GetConfigInt("tutorial_chaos_kill");
		public static int SCPKillsPlayer = PlayerXP.plugin.GetConfigInt("tutorial_scp_kills_player");
	}
	static class SCP106XP
	{
		//public static int TeleportToPD = 25;
		public static int DeathInPD = PlayerXP.plugin.GetConfigInt("scp106_pocket_death");
	}
	static class SCP049XP
	{
		public static int ZombieCreated = PlayerXP.plugin.GetConfigInt("scp049_zombie_created");
	}
}

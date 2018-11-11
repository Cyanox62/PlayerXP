namespace PlayerXP
{
	static class AllXP
	{
		public static int RoundWinXP = (int)(PlayerXP.plugin.GetConfigInt("all_round_win") * PlayerXP.xpScale);
		public static int SCPKillPlayer = (int)(PlayerXP.plugin.GetConfigInt("scp_kill_player") * PlayerXP.xpScale);
	}
	static class DClassXP
	{
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("dclass_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("dclass_mtf_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("dclass_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("dclass_tutorial_kill") * PlayerXP.xpScale);
		public static int Escape = (int)(PlayerXP.plugin.GetConfigInt("dclass_escape") * PlayerXP.xpScale);
	}
	static class ScientistXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("scientist_dclass_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("scientist_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("scientist_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("scientist_tutorial_kill") * PlayerXP.xpScale);
		public static int Escape = (int)(PlayerXP.plugin.GetConfigInt("scientist_escape") * PlayerXP.xpScale);
	}
	static class NineTailedFoxXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("mtf_dclass_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("mtf_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("mtf_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("mtf_tutorial_kill") * PlayerXP.xpScale);
		public static int ScientistEscape = (int)(PlayerXP.plugin.GetConfigInt("mtf_scientist_escape") * PlayerXP.xpScale);
	}
	static class ChaosXP
	{
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("chaos_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("chaos_mtf_kill") * PlayerXP.xpScale);
		public static int SCPKill = (int)(PlayerXP.plugin.GetConfigInt("chaos_scp_kill") * PlayerXP.xpScale);
		public static int TutorialKill = (int)(PlayerXP.plugin.GetConfigInt("chaos_tutorial_kill") * PlayerXP.xpScale);
		public static int DClassEscape = (int)(PlayerXP.plugin.GetConfigInt("chaos_dclass_escape") * PlayerXP.xpScale);
	}
	static class TutorialXP
	{
		public static int DClassKill = (int)(PlayerXP.plugin.GetConfigInt("tutorial_dclass_kill") * PlayerXP.xpScale);
		public static int ScientistKill = (int)(PlayerXP.plugin.GetConfigInt("tutorial_scientist_kill") * PlayerXP.xpScale);
		public static int NineTailedFoxKill = (int)(PlayerXP.plugin.GetConfigInt("tutorial_mtf_kill") * PlayerXP.xpScale);
		public static int ChaosKill = (int)(PlayerXP.plugin.GetConfigInt("tutorial_chaos_kill") * PlayerXP.xpScale);
		public static int SCPKillsPlayer = (int)(PlayerXP.plugin.GetConfigInt("tutorial_scp_kills_player") * PlayerXP.xpScale);
	}
	static class SCP106XP
	{
		public static int DeathInPD = (int)(PlayerXP.plugin.GetConfigInt("scp106_pocket_death") * PlayerXP.xpScale);
	}
	static class SCP049XP
	{
		public static int ZombieCreated = (int)(PlayerXP.plugin.GetConfigInt("scp049_zombie_created") * PlayerXP.xpScale);
	}
}

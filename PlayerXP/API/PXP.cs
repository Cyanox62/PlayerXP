using Exiled.API.Features;

namespace PlayerXP.API
{
	public static class PXP
	{
		internal static EventHandler singleton;

		/// <summary>
		/// Returns the level of the specified player.
		/// </summary>
		/// <param name="player"></param>
		public static int GetLevel(this Player player)
		{
			return singleton.GetLevel(player.UserId);
		}

		/// <summary>
		/// Returns the current XP of the specified player.
		/// </summary>
		/// <param name="player"></param>
		public static int GetXP(this Player player)
		{
			return singleton.GetXP(player.UserId);
		}

		/// <summary>
		/// Returns the amount of XP needed for the user to achieve the next level.
		/// </summary>
		/// <param name="player"></param>
		public static int XPForNextLevel(this Player player)
		{
			return singleton.GetXP(player.UserId);
		}

		/// <summary>
		/// Gives a user XP.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		/// <param name="msg"></param>
		public static void GiveXP(this Player player, int amount, string msg = null)
		{
			singleton.AddXP(player.UserId, amount, msg);
		}

		/// <summary>
		/// Removes XP from a user.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		/// <param name="msg"></param>
		public static void RemoveXP(this Player player, int amount, string msg = null)
		{
			singleton.RemoveXP(player.UserId, amount, msg);
		}

		/// <summary>
		/// Adjusts a player's karma by the specified amount.
		/// </summary>
		/// <param name="player"></param>
		/// <param name="amount"></param>
		public static void AdjustKarma(this Player player, int amount)
		{
			singleton.AdjustKarma(player, amount);
		}
	}
}

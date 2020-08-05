namespace PlayerXP
{
	public class PlayerInfo
	{
		public string name;
		public int level;
		public int xp;

		public PlayerInfo(string name)
		{
			this.name = name;
			level = 1;
			xp = 0;
		}
	}
}

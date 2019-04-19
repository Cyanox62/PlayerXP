# PlayerXP

**I am no longer supporting this project, if you would like to contribute feel free to send a PR.**

A plugin that adds a leveling system to your server. Users start at level 1, and gain experience through doing tasks such as escaping as a classd or scientist, winning the round, killing an SCP, etc.

Click [here](https://github.com/Cyanox62/PlayerXP/wiki/XP-Config-Settings) for a full list of config settings/rates of xp.

# Installation

**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

Place the "PlayerXP.dll" file in your sm_plugins folder.

# Features
* Gives xp values based on normal game tasks
* Scale factor for all xp values
* Config options to change the rates of each task
* Commands that can be run through RA console **or** client console by using the prefix `.` to check the level, xp, and server ranking of a user, as well as the server leaderboard. You can use the users username with an autocomplete feature (plugin will automatically get the closest user to the name you typed) if they are on the server, otherwise you can use their steamid if they are offline
* Commands to find the player with the highest level in the server and get the server's leaderboard
* Levels get increasingly harder to achieve, you need 1000 xp to get to level 2, then it will take 250 more xp to level up after every level
* Players will get a message in their console when they:
  * Level up, including the xp required to level up again
  * Die, showing their current xp and the xp they need to level up
  * Complete a task, showing the amount of xp they gained and for what task
* When a player is killed, the victim gets an output in their console saying the killers name and level
* All data is saved in `%appdata%/SCP Secret Laboratory/PlayerXP
* Levels transfer between MultiAdmin servers

**Commands that can be run from both RA console and player console. These commands must use the prefix `.` if they are being run through the player console.**

| Command        | Value Type | Description |
| :-------------: | :---------: | :------ |
| LVL / LEVEL | PLAYER NAME / STEAMID64 | Displays a user's level and xp. Will also display their server ranking. |
| LEADERBOARD | NUMBER | Displays the top users in the server. If no number is specified it will output the top 5. |

**Commands that can only be run through RA console.**

| Command        | Value Type | Description |
| :-------------: | :---------: | :------ |
| XPUPDATE | | Updates server rankings manually. |

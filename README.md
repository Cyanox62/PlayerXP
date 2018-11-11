# PlayerXP

A plugin that adds a leveling system to your server. Users start at level 1, and gain experience through doing tasks such as escaping as a classd or scientist, winning the round, killing an SCP, etc. A full list of these tasks can be found [here](https://github.com/Cyanox62/PlayerXP/wiki/XP-Config-Settings).

# Installation

**[Smod2](https://github.com/Grover-c13/Smod2) must be installed for this to work.**

Place the "PlayerXP.dll" file in your sm_plugins folder.

# Features
- Gives xp values based on normal game tasks
- Config options to change the rates of each task
- Currently RA console only command (will be player console once that feature gets added) to check the level and xp of a user. You can use the users username with an autocomplete feature (plugin will automatically get the closest user to the name you typed) if they are on the server, otherwise you can use their steamid if they are offline
- Levels get increasingly harder to achieve, you need 1000 xp to get to level 2, then it will take 250 more experience to level up after every level
- When a player is killed, the victim gets an output in their console saying the killers name and level
- All data is saved in %appdata%/SCP Secret Laboratory/PlayerXP/PlayerXPData.txt

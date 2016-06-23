# PlayerInfo
An update on HySpeed's PlayerInfo plugin. Provide Info on a player.

### Usage:

_/pinfo_ _playerName_ Life - Shows Life and Mana of specified player

. . . . . . . . . . . . . . . . . . . . . :"{name} [Ip:{ipAddy}] [Life/Mana: {life}/{mana}] [Account: {loggedin}] [Group: {group}]"


_/pinfo_ _playerName_ Buffs - Shows the buffs of the specified player

. . . . . . . . . . . . . . . . . . . . . :"{buff}({time}) | {buff}({time})  | ... "


_/pinv_ _playerName_ Inv _row_ - Shows the items (and stack) for the row

. . . . . . . . . . . . . . . . . . . . . :"{item}({stack}) | {item}({stack}) | ... "


_/pinv_ _playerName_ Amm - Shows the Ammo equipped (and stack) by the player

. . . . . . . . . . . . . . . . . . . . . :"{item}({stack}) | {item}({stack}) | ... "


_/pinv_ _playerName_ Arm - Shows the Armor equipped by the player

. . . . . . . . . . . . . . . . . . . . . :"{item} | {item} | {item} "


_/pinv_ _playerName_ Acc - Shows the Accessories equipped by the player

. . . . . . . . . . . . . . . . . . . . . :"{item} | {item} | ... "



### Permissions:

_playerinfo_ - Permission to view info on players



### Notes:

* Inventory stacks are not always accurate.  Some (hacked) clients return invalid

  values to hide their nature.

* Buff shows the initial buff time, not the remaining.  If I could find the 
  remaining, I would add that (Initial/Remaining).


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TShockAPI;

namespace PlayerInfo
{
    class Commands
    {
        public static void Init()
        {

            TShockAPI.Commands.ChatCommands.Add(new Command("playerinfo", PlayerInv, "pinv"));
            TShockAPI.Commands.ChatCommands.Add(new Command("playerinfo", PlayerInfo, "pinfo"));
            AddPermission("playerinfo", "trustedadmin");

        }
        private static void AddPermission(string permission,
                                           string groupName)
        {
            bool checkPerm = TShock.Groups.groups.Where(@group => @group.Name != "superadmin")
                                                 .Any(@group => group.HasPermission(permission));
            if (!checkPerm)
            {
                var permissions = new List<string> { permission };
                TShock.Groups.AddPermissions(groupName, permissions);
            } // if
        }
        public static void PlayerInv(CommandArgs args)
        {
            if (args.Parameters.Count <= 1)
            {
                args.Player.SendMessage("Invalid syntax. Proper Syntax: /pinv <player> [ INV <row> | ACC | AMM | ARM ]", Color.Red);
            } // if
            else if (args.Parameters.Count > 1)
            {
                TShockAPI.TSPlayer player;
                string action;

                player = findPlayer(args, args.Parameters[0]);
                if (player != null)
                {
                    action = args.Parameters[1].ToUpper();

                    if (action.StartsWith("AC")) { showAccArm(args, player, true); }
                    else if (action.StartsWith("AR")) { showAccArm(args, player, false); }
                    else if (action.StartsWith("AM")) { showAmm(args, player); }
                    else if (action.StartsWith("IN")) { showInv(args, player); }
                    else
                    {
                        args.Player.SendMessage(string.Format("Invalid action: {0}", action), Color.Red);
                        args.Player.SendMessage("Invalid syntax. Proper Syntax: /pinv <player> [ INV <row> | ACC | AMM | ARM ]", Color.Orange);
                    }

                }

            }
        }
        private static void showAccArm(CommandArgs args,
                                        TShockAPI.TSPlayer player,
                                        bool acc)
        {
            StringBuilder response = new StringBuilder();
            String itemName, type;
            int firstSlot, lastSlot;

            if (acc) { type = "Access"; firstSlot = 3; lastSlot = 7; }
            else { type = "Armour"; firstSlot = 0; lastSlot = 2; }

            //      args.Player.SendMessage( string.Format( "~ type: {0}", type ), Color.Pink );
            //      args.Player.SendMessage( string.Format( "~ name: {0}", player.Name ), Color.Pink );

            for (int index = firstSlot; index <= lastSlot; index++)
            {
                itemName = player.TPlayer.armor[index].name;
                //        args.Player.SendMessage( string.Format( "~ {0}: [{1}]", type, itemName ), Color.Pink );
                if (itemName.Length == 0) { itemName = "(no item)"; } // if
                response.Append(itemName);
                if (index < lastSlot) { response.Append(" | "); } // if
            } // for

            args.Player.SendMessage(string.Format("{0}: {1}: {2}", player.Name, type, response), Color.White);
        }
        private static void showAmm(CommandArgs args,
                                     TShockAPI.TSPlayer player)
        {
            StringBuilder response = new StringBuilder();
            String itemName;
            int firstSlot = 44, lastSlot = 47;

            for (int index = firstSlot; index <= lastSlot; index++)
            {
                itemName = player.TPlayer.inventory[index].name;
                if (itemName.Length == 0) { itemName = "(no item)"; } // if
                response.Append(itemName).Append(" (");
                response.Append(player.TPlayer.inventory[index].stack).Append(")");
                if (index < lastSlot) { response.Append(" | "); } // if
            }

            args.Player.SendMessage(string.Format("{0}: Ammo: {1}", player.Name, response), Color.White);
        }
        private static void showInv(CommandArgs args,
                                     TShockAPI.TSPlayer player)
        {
            StringBuilder response = new StringBuilder();
            int row;
            String itemName;
            List<int> firstSlot = new List<int>(5) { 0, 0, 10, 20, 30 };
            List<int> lastSlot = new List<int>(5) { -1, 9, 19, 29, 39 };

            if (args.Parameters.Count == 3)
            {
                row = Convert.ToInt32(args.Parameters[2]);
                if (row < firstSlot.Count && lastSlot[row] > 0)
                {
                    for (int index = firstSlot[row]; index <= lastSlot[row]; index++)
                    {
                        itemName = player.TPlayer.inventory[index].name;
                        if (itemName.Length == 0) { itemName = "(no item)"; }
                        response.Append(itemName).Append(" (");
                        response.Append(player.TPlayer.inventory[index].stack).Append(")");
                        if (index < lastSlot[row]) { response.Append(" | "); }
                    }
                    IEnumerable<string> invLines;
                    invLines = SplitByLength(response.ToString(), 100);
                    string firstLine;
                    firstLine = invLines.First();
                    args.Player.SendMessage(string.Format("{0}: Inv Row [{1}]: {2}", player.Name, row, firstLine), Color.White);

                    foreach (string line in invLines)
                    {
                        if (!line.Equals(firstLine))
                        {
                            args.Player.SendMessage(line, Color.White);
                        }
                    }
                }
                else
                {
                    args.Player.SendMessage(string.Format("Invalid row: {0}.  Only 1 - 4 are allowed.", row), Color.Red);
                }
            }
            else
            {
                args.Player.SendMessage("Row required for INV action (e.g. iirc name row)", Color.Red);
            }
        }
        private static IEnumerable<string> SplitByLength(string str, int maxLength)
        {
            for (int index = 0; index < str.Length; index += maxLength)
            {
                yield return str.Substring(index, Math.Min(maxLength, str.Length - index));
            }
        }
        public static void PlayerInfo(CommandArgs args)
        {
            if (args.Parameters.Count <= 1)
            {
                args.Player.SendMessage("Invalid syntax. Proper Syntax: /pinfo <player> [ Life | Buffs ]", Color.Red);
            }
            else if (args.Parameters.Count > 1)
            {
                TShockAPI.TSPlayer player;
                string action;

                player = findPlayer(args, args.Parameters[0]);
                if (player != null)
                {
                    action = args.Parameters[1].ToUpper();

                    if (action.StartsWith("L")) { showLifeMana(args, player); }
                    else if (action.StartsWith("B")) { showBuffs(args, player); }
                    else
                    {
                        args.Player.SendMessage(string.Format("Invalid action: {0}. Proper Syntax: /pinfo <player> [ Life | Buffs ]", action), Color.Red);
                    }

                }
            }
        }
        private static void showLifeMana(CommandArgs args,
                                          TSPlayer player)
        {
            args.Player.SendMessage(string.Format("{0} [Ip:{1}] [Life/Mana: {2}/{3}] [Account: {4}] [Group: {5}]",
                                                      player.Name,
                                                      player.IP,
                                                      player.FirstMaxHP,
                                                      player.FirstMaxMP,
                                                      player.User.Name,
                                                      player.Group.Name), Color.White);

        }
        private static void showBuffs(CommandArgs args,
                                       TShockAPI.TSPlayer player)
        {
            StringBuilder response = new StringBuilder();
            String buffName;
            bool buffFound = false;
            int buffType;
            int buffCount = player.TPlayer.CountBuffs();

            for (int index = 0; index < buffCount; index++)
            {
                buffType = player.TPlayer.buffType[index];
                if (buffType > 0)
                {
                    buffName = TShock.Utils.GetBuffName(buffType);
                    response.Append(buffName).Append(" (");
                    response.Append(player.TPlayer.buffTime[index]).Append(")");
                    if (index < buffCount - 1) { response.Append(" | "); }
                    buffFound = true;
                }
            }
            if (buffFound)
            {
                args.Player.SendMessage(string.Format("{0}: Buffs: {1}",
                                                        player.Name, response), Color.White);
            }
            else
            {
                args.Player.SendSuccessMessage(string.Format("{0} has no Buffs", player.Name, Color.White));
            }
        }
        private static TSPlayer findPlayer(CommandArgs args,
                                                      string playerName)
        {
            TShockAPI.TSPlayer result = null;

            List<TShockAPI.TSPlayer> playerList = TShockAPI.TShock.Utils.FindPlayer(playerName);
            if (playerList.Count < 1)
            {
                args.Player.SendMessage(string.Format("Player {0} not found.", playerName), Color.Red);
            }
            else if (playerList.Count > 1)
            {
                args.Player.SendMessage(string.Format("Multiple players matched {0}.", playerName), Color.Red);
            }
            else
            {
                result = playerList[0];
            }
            return result;
        }
    }
}

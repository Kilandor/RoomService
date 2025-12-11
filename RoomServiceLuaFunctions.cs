using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Linq;
using ZeepkistClient;
using ZeepkistNetworking;
using ZeepSDK.Chat;
using ZeepSDK.Scripting.ZUA;
using System.IO;
using Newtonsoft.Json;
using System.Globalization;

namespace RoomService
{
    /// <summary>
    /// Represents a Lua function to send a chat message to all players.
    /// </summary>
    public class SendChatMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SendChatMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a prefix and message.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to send a chat message.
        /// </summary>
        /// <param name="prefix">The prefix for the message.</param>
        /// <param name="message">The content of the message.</param>
        private void Implementation(string prefix, string message)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(message))
            {
                return;
            }

            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.SendCustomChatMessage(true, 0, message, prefix);
        }
    }

    /// <summary>
    /// Represents a Lua function to send a private chat message to a specific player.
    /// </summary>
    public class SendPrivateChatMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SendPrivateChatMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID, prefix, and message.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to send a private chat message.
        /// </summary>
        /// <param name="steamID">The Steam ID of the recipient.</param>
        /// <param name="prefix">The prefix for the message.</param>
        /// <param name="message">The content of the message.</param>
        private void Implementation(ulong steamID, string prefix, string message)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(message))
            {
                return;
            }

            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.SendCustomChatMessage(false, steamID, message, prefix);
        }
    }

    /// <summary>
    /// Represents a Lua function to display a screen message to the player.
    /// </summary>
    public class ShowScreenMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ShowScreenMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and a duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to send a screen message.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="time">The duration to display the message.</param>
        private void Implementation(string message, float time)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            PlayerManager.Instance.messenger.Log(message, time);
        }
    }

    /// <summary>
    /// Represents a Lua function to block all players from setting a leaderboard time.
    /// </summary>
    public class BlockEveryoneFromSettingTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "BlockEveryoneFromSettingTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a boolean parameter to notify players.</returns>
        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to block all players from setting a leaderboard time.
        /// </summary>
        /// <param name="notify">Indicates whether to notify players.</param>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockEveryoneFromSettingTime(notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to unblock all players from setting a leaderboard time.
    /// </summary>
    public class UnblockEveryoneFromSettingTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "UnblockEveryoneFromSettingTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a boolean parameter to notify players.</returns>
        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to unblock all players from setting a leaderboard time.
        /// </summary>
        /// <param name="notify">Indicates whether to notify players.</param>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockEveryoneFromSettingTime(notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to unblock a specific player from setting a leaderboard time.
    /// </summary>
    public class UnblockPlayerFromSettingTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "UnblockPlayerFromSettingTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID and a notify parameter.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to unblock a specific player.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <param name="notify">Indicates whether to notify the player.</param>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockPlayerFromSettingTime(steamID, notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to block a specific player from setting a leaderboard time.
    /// </summary>
    public class BlockPlayerFromSettingTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "BlockPlayerFromSettingTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID and a notify parameter.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to block a specific player.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <param name="notify">Indicates whether to notify the player.</param>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockPlayerFromSettingTime(steamID, notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to set the sorting method for a small leaderboard.
    /// </summary>
    public class SetSmallLeaderboardSortingMethodFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetSmallLeaderboardSortingMethod";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a boolean indicating whether to use championship sorting.</returns>
        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set the sorting method for the leaderboard.
        /// </summary>
        /// <param name="useChampionshipSorting">True to use championship sorting; false for default sorting.</param>
        private void Implementation(bool useChampionshipSorting)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetSmallLeaderboardSortingMethod(useChampionshipSorting);
        }
    }

    /// <summary>
    /// Represents a Lua function to set a player's time on the leaderboard.
    /// </summary>
    public class SetPlayerTimeOnLeaderboardFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetPlayerTimeOnLeaderboard";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID, time, and a notify flag.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, float, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set a player's time on the leaderboard.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <param name="time">The time to set on the leaderboard.</param>
        /// <param name="notify">True to notify the player; false otherwise.</param>
        private void Implementation(ulong steamID, float time, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerTimeOnLeaderboard(steamID, time, notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to set leaderboard overrides for a specific player.
    /// </summary>
    public class SetPlayerLeaderboardOverridesFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetPlayerLeaderboardOverrides";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID and override parameters.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string, string, string, string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set leaderboard overrides for a specific player.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <param name="time">The override time to display.</param>
        /// <param name="name">The override name to display.</param>
        /// <param name="position">The override position to display.</param>
        /// <param name="points">The override points to display.</param>
        /// <param name="pointsWon">The override points won to display.</param>
        private void Implementation(ulong steamID, string time, string name, string position, string points, string pointsWon)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerLeaderboardOverrides(steamID, time, name, position, points, pointsWon);
        }
    }

    /// <summary>
    /// Represents a Lua function to remove a player from the leaderboard.
    /// </summary>
    public class RemovePlayerFromLeaderboardFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "RemovePlayerFromLeaderboard";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID and a notify flag.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to remove a player from the leaderboard.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player to remove.</param>
        /// <param name="notify">True to notify the player; false otherwise.</param>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_RemovePlayerFromLeaderboard(steamID, notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to set the points distribution in the lobby.
    /// </summary>
    public class SetPointsDistributionFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetPointsDistribution";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes points values, a baseline, and a DNF value.</returns>
        public Delegate CreateFunction()
        {
            return new Action<int[], int, int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set points distribution.
        /// </summary>
        /// <param name="values">The points distribution values.</param>
        /// <param name="baseline">The baseline points value.</param>
        /// <param name="dnf">The points value for "Did Not Finish" players.</param>
        private void Implementation(int[] values, int baseline, int dnf)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPointsDistribution(values.ToList(), baseline, dnf);
        }
    }

    /// <summary>
    /// Represents a Lua function to reset the points distribution to default values.
    /// </summary>
    public class ResetPointsDistributionFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ResetPointsDistribution";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that performs the reset operation.</returns>
        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to reset points distribution.
        /// </summary>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_ResetPointsDistribution();
        }
    }

    /// <summary>
    /// Represents a Lua function to set championship points for a specific player.
    /// </summary>
    public class SetPlayerChampionshipPointsFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetPlayerChampionshipPoints";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID, points, points won, and a notify flag.</returns>
        public Delegate CreateFunction()
        {
            return new Action<ulong, int, int, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set championship points for a player.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <param name="points">The total points to assign to the player.</param>
        /// <param name="pointsWon">The points won in the current round.</param>
        /// <param name="notify">True to notify the player; false otherwise.</param>
        private void Implementation(ulong steamID, int points, int pointsWon, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerChampionshipPoints(steamID, points, pointsWon, notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to reset championship points for all players.
    /// </summary>
    public class ResetChampionshipPointsFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ResetChampionshipPoints";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a notify flag.</returns>
        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to reset championship points.
        /// </summary>
        /// <param name="notify">True to notify all players; false otherwise.</param>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.ResetChampionshipPoints(notify);
        }
    }

    /// <summary>
    /// Represents a Lua function to set the round length for the lobby.
    /// </summary>
    public class SetRoundLengthFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetRoundLength";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes the round length in seconds.</returns>
        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set the round length.
        /// </summary>
        /// <param name="time">The round length in seconds (minimum 30).</param>
        private void Implementation(int time)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            time = Math.Max(30, time);

            ChatApi.SendMessage("/settime " + time.ToString());
        }
    }

    /// <summary>
    /// Represents a Lua function to enable or disable voteskip in the lobby.
    /// </summary>
    public class SetVoteskipFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetVoteskip";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a boolean to enable or disable voteskip.</returns>
        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to enable or disable voteskip.
        /// </summary>
        /// <param name="enabled">True to enable voteskip; false to disable it.</param>
        private void Implementation(bool enabled)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage($"/vs {(enabled ? "on" : "off")}");
        }
    }

    /// <summary>
    /// Represents a Lua function to set the percentage required for voteskip.
    /// </summary>
    public class SetVoteskipPercentageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetVoteskipPercentage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes the voteskip percentage.</returns>
        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set the voteskip percentage.
        /// </summary>
        /// <param name="percentage">The percentage required for voteskip (1 to 100).</param>
        private void Implementation(int percentage)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            percentage = Math.Min(100, Math.Max(1, percentage));

            ChatApi.SendMessage($"/vs % {percentage.ToString()}");
        }
    }

    /// <summary>
    /// Represents a Lua function to set the lobby name.
    /// </summary>
    public class SetLobbyNameFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetLobbyName";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes the new lobby name.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set the lobby name.
        /// </summary>
        /// <param name="name">The new name for the lobby.</param>
        private void Implementation(string name)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                return;
            }

            ZeepkistLobby currentLobby = ZeepkistNetwork.CurrentLobby;
            if (currentLobby != null)
            {
                currentLobby.UpdateName(name);
            }
        }
    }

    /// <summary>
    /// Represents a Lua function to set a server message on screen.
    /// </summary>
    public class SetServerMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SetServerMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and display duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to set a server-wide message.
        /// </summary>
        /// <param name="message">The message to display on the server.</param>
        /// <param name="time">The duration in seconds to display the message.</param>
        private void Implementation(string message, float time)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Trim()))
            {
                return;
            }

            ChatApi.SendMessage($"/servermessage white {time.ToString()} {message}");
        }
    }

    /// <summary>
    /// Represents a Lua function to remove the current server message.
    /// </summary>
    public class RemoveServerMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "RemoveServerMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that performs the operation.</returns>
        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to remove the current server-wide message.
        /// </summary>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage("/servermessage remove");
        }
    }

    /// <summary>
    /// Represents a Lua function to get the number of players in the current lobby.
    /// </summary>
    public class GetPlayerCountFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetPlayerCount";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that returns the number of players.</returns>
        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the player count.
        /// </summary>
        /// <returns>The number of players in the current lobby, or -1 if unavailable.</returns>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }
            
            return ZeepkistNetwork.Players?.Count ?? -1;
        }
    }

    /// <summary>
    /// Represents a Lua function to get the current playlist index.
    /// </summary>
    public class GetPlaylistIndexFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetPlaylistIndex";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that returns the current playlist index.</returns>
        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the playlist index.
        /// </summary>
        /// <returns>The current playlist index, or -1 if unavailable.</returns>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.CurrentPlaylistIndex ?? -1;
        }
    }

    /// <summary>
    /// Represents a Lua function to get the length of the current playlist.
    /// </summary>
    public class GetPlaylistLengthFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetPlaylistLength";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that returns the length of the playlist.</returns>
        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the playlist length.
        /// </summary>
        /// <returns>The length of the playlist, or -1 if unavailable.</returns>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.Playlist.Count ?? -1;
        }
    }

    /// <summary>
    /// Represents a Lua function to get the player at the given position on the leaderboard
    /// </summary>
    public class GetPlayerAtPositionFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetPlayerAtPosition";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that returns the players steamID at the position or 0 if null.</returns>
        public Delegate CreateFunction()
        {
            return new Func<int, ulong>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the player at the given position in the leaderboard.
        /// </summary>
        /// <returns>The players steamID, or 0 if unavailable.</returns>
        private ulong Implementation(int position)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return 0;
            }

            ulong steamID = 0;

            try
            {
                steamID = ZeepkistNetwork.Leaderboard[position].SteamID;                
            }
            catch
            {
                steamID = 0;                
            }

            return steamID;
        }
    }

    /// <summary>
    /// Represents a Lua function to get an entry from the leaderboard.
    /// </summary>
    public class GetLeaderboardEntryFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetLeaderboardEntry";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a steamID and returns a LeaderboardItem.</returns>
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<ulong, LeaderboardItem>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the leaderboard entry.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <returns>The leaderboard entry for the given Steam ID.</returns>
        private LeaderboardItem Implementation(ulong steamID)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new LeaderboardItem() { SteamID = 0, Time = 0, Username = "" };

            }

            return ZeepkistNetwork.GetLeaderboardEntry(steamID);
        }
    }

    /// <summary>
    /// Represents a Lua function to retrieve a leaderboard override entry for a specific player.
    /// </summary>
    public class GetLeaderboardOverrideFunction : ILuaFunction
    {
        /// <summary>
        /// The namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// The name of the Lua function.
        /// </summary>
        public string Name => "GetLeaderboardOverride";

        /// <summary>
        /// Creates a delegate for the Lua function, which takes a Steam ID and returns a leaderboard override entry.
        /// </summary>
        /// <returns>A delegate that takes a Steam ID and returns a <see cref="LeaderboardOverrideItem"/>.</returns>
        public Delegate CreateFunction()
        {
            return new Func<ulong, LeaderboardOverrideItem>(Implementation);
        }

        /// <summary>
        /// Retrieves a leaderboard override entry for the specified Steam ID.
        /// </summary>
        /// <param name="steamID">The Steam ID of the player.</param>
        /// <returns>The <see cref="LeaderboardOverrideItem"/> for the given Steam ID.</returns>
        private LeaderboardOverrideItem Implementation(ulong steamID)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new LeaderboardOverrideItem() { SteamID = 0, overrideNameText = "", overridePositionText = "", overrideTimeText = "", overridePointsText = "", overridePointsWonText = "" };
            }

            return ZeepkistNetwork.GetLeaderboardOverride(steamID);
        }
    }

    /// <summary>
    /// Represents a Lua function to retrieve the entire leaderboard.
    /// </summary>
    public class GetLeaderboardFunction : ILuaFunction
    {
        /// <summary>
        /// The namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// The name of the Lua function.
        /// </summary>
        public string Name => "GetLeaderboard";

        /// <summary>
        /// Creates a delegate for the Lua function, which retrieves the full leaderboard as a list of entries.
        /// </summary>
        /// <returns>A delegate that returns a list of <see cref="LeaderboardItem"/>.</returns>
        public Delegate CreateFunction()
        {
            return new Func<List<LeaderboardItem>>(Implementation);
        }

        /// <summary>
        /// Retrieves the full leaderboard.
        /// </summary>
        /// <returns>A list of <see cref="LeaderboardItem"/> representing the leaderboard. If unavailable, returns an empty list.</returns>
        private List<LeaderboardItem> Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new List<LeaderboardItem>();
            }

            return ZeepkistNetwork.GetLeaderboard() ?? new List<LeaderboardItem>();
        }
    }

    /// <summary>
    /// Represents a Lua function to clear the time logger.
    /// </summary>
    public class ClearLoggerFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ClearLogger";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to clear the logger.
        /// </summary>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            Plugin.Instance.levelInfo.Clear();
            Plugin.Instance.playerTimes.Clear();
        }
    }

    /// <summary>
    /// Represents a Lua function to save the time logger to disk.
    /// </summary>
    public class SaveLoggerFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SaveLogger";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to save the log.
        /// </summary>
        private void Implementation(string fileName)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            // Remove invalid characters from the file name
            string sanitizedFileName = string.Concat(fileName.Split(Path.GetInvalidFileNameChars()));

            // Ensure the file name ends with a .txt extension
            if (!sanitizedFileName.EndsWith(".json", StringComparison.OrdinalIgnoreCase))
            {
                sanitizedFileName += ".json";
            }

            // Get the plugin folder path and combine it with the sanitized file name
            string pluginFolderPath = BepInEx.Paths.PluginPath;
            string filePath = Path.Combine(pluginFolderPath, sanitizedFileName);

            // Combine the dictionaries into a JSON-serializable structure
            var combinedData = new List<object>();

            foreach (var level in Plugin.Instance.levelInfo)
            {
                var hash = level.Key;
                var levelData = level.Value;
                var times = Plugin.Instance.playerTimes.ContainsKey(hash) ? Plugin.Instance.playerTimes[hash] : new List<PlayerTime>();

                // Add the times directly into the level object
                combinedData.Add(new
                {
                    Hash = levelData.Hash,
                    Author = levelData.Author,
                    Name = levelData.Name,
                    Uid = levelData.Uid,
                    WorkshopId = levelData.WorkshopId,
                    Times = times
                });
            }

            // Serialize to JSON
            string json = JsonConvert.SerializeObject(combinedData, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }

    /// <summary>
    /// Represents a Lua function to print the time logger to the console.
    /// </summary>
    public class PrintLoggerFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "PrintLogger";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to save the log.
        /// </summary>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            string[] content = Plugin.Instance.GetLoggerLines();
            string logLine = string.Join('\n', content);
            Plugin.Instance.Log(logLine);            
        }
    }

    /// <summary>
    /// Represents a Lua function to get the current date.
    /// </summary>
    public class GetCurrentDateFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetCurrentDate";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            return new Func<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to return the current date.
        /// </summary>
        private string Implementation()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    /// <summary>
    /// Represents a Lua function to get the current time.
    /// </summary>
    public class GetCurrentTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetCurrentTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            return new Func<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to return the current time.
        /// </summary>
        private string Implementation()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }

    /// <summary>
    /// Represents a Lua function to generate a random number.
    /// </summary>
    public class GenerateRandomNumberFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GenerateRandomNumber";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        public Delegate CreateFunction()
        {
            // The Lua function takes two parameters (min and max) and returns an integer.
            return new Func<int, int, int>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to generate a random number.
        /// </summary>
        /// <param name="min">The minimum value (inclusive).</param>
        /// <param name="max">The maximum value (inclusive).</param>
        /// <returns>A random integer between min and max.</returns>
        private int Implementation(int min, int max)
        {
            // Ensure min is less than or equal to max to prevent errors.
            if (min > max)
            {
                return min;
            }

            Random random = new Random();
            return random.Next(min, max + 1); // max is inclusive.
        }
    }

    /// <summary>
    /// Represents a Lua function to convert seconds into a time string.
    /// </summary>
    public class SecondsToTimeFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SecondsToTime";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a time and precision value and returns a time string.
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<float,int,string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to convert the time to string.
        /// </summary>
        /// <param name="timeInSeconds">The time in seconds.</param>
        /// <param name="precision">The amount of decimals.</param>
        /// <returns>The time in string format</returns>
        private string Implementation(float time, int precision)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return "";
            }

            // Ensure precision is not negative
            if (precision < 0)
            {
                precision = 0;
            }

            if (float.IsNegativeInfinity(time) || float.IsPositiveInfinity(time) || float.IsNaN(time))
                time = 0.0f;
            if (time < 0.0f)
                time = 0.0f;

            TimeSpan timeSpan = time != 0.0f ? TimeSpan.FromSeconds(time) : TimeSpan.Zero;

            // Calculate fractional seconds and format milliseconds with precision
            double fractionalSeconds = time - Math.Floor(time);
            // If it rounds to 1000, reset it to 0 to avoid overflow
            if (Math.Round(fractionalSeconds * Math.Pow(10, precision)) >= Math.Pow(10, precision))
                fractionalSeconds = 0;
            string milliseconds = Math.Round(fractionalSeconds * Math.Pow(10, precision))
                .ToString(CultureInfo.InvariantCulture).PadLeft(precision, '0');

            if (time < 3600.0f)
                return string.Format(CultureInfo.InvariantCulture, "{0:D2}:{1:D2}.{2}",
                    timeSpan.Minutes, timeSpan.Seconds, milliseconds);
            else
                return string.Format(CultureInfo.InvariantCulture, "{0:D2}:{1:D2}:{2:D2}.{3}",
                    timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, milliseconds);
        }
    }
    
    /// <summary>
    /// Represents a Lua function to reset all players
    /// </summary>
    public class ResetAllPlayersFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ResetAllPlayers";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that resets all players
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to reset all players
        /// </summary>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }
            List<ulong> listPlayers = new List<ulong>();
            for (int index = 0; index < ZeepkistNetwork.PlayerList.Count; ++index)
                listPlayers.Add(ZeepkistNetwork.PlayerList[index].SteamID);
            ZeepkistNetwork.CustomLeaderBoard_ResetPlayers(listPlayers);


        }
    }
    
    /// <summary>
    /// Represents a Lua function to reset players
    /// </summary>
    public class ResetPlayersFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "RoomService";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ResetPlayers";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that resets players
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<List<ulong>, bool>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to reset players
        /// </summary>
        private bool Implementation(List<ulong> listPlayers)
        {
            if (!RoomServiceUtils.IsOnlineHost() || listPlayers == null || listPlayers.Count <= 0)
            {
                return false;
            }
            
            ZeepkistNetwork.CustomLeaderBoard_ResetPlayers(listPlayers);
            return true;
        }
    }
}

using MoonSharp.Interpreter;
using System;
using System.Linq;
using ZeepkistClient;
using ZeepSDK.Chat;
using ZeepSDK.Scripting.ZUA;

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

            return ZeepkistNetwork.CurrentLobby?.PlayerCount ?? -1;
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
}

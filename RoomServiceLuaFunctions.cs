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
    /// @brief Lua function to send a chat message to all players.
    public class SendChatMessage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SendChatMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, string>(Implementation);
        }

        /// Usage: @code RoomService.SendChatMessage("ByteBot", "Hello World"); @endcode
        /// @param prefix The prefix for the message.
        /// @param message The content of the message.
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to send a private chat message to a specific player.
    public class SendPrivateChatMessage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SendPrivateChatMessage";

        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string>(Implementation);
        }

        /// Usage: @code RoomService.SendPrivateChatMessage("76561197993793009", "ByteBot", "Hello Kilandor"); @endcode
        /// @param steamID The Steam ID of the recipient.
        /// @param prefix The prefix for the message.
        /// @param message The content of the message.
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to display a screen message to the player.
    public class ShowScreenMessage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ShowScreenMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// Usage: @code RoomService.ShowScreenMessage("Hello World", 5); @endcode
        /// @param message The content of the message.
        /// @param time The duration to display the message.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(string message, float time)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            PlayerManager.Instance.messenger.Log(message, time);
        }
    }

    /// @brief Lua function to block all players from setting a leaderboard time.
    public class BlockEveryoneFromSettingTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "BlockEveryoneFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// Usage: @code RoomService.BlockEveryoneFromSettingTime(true); @endcode
        /// @param notify Indicates whether to notify players.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockEveryoneFromSettingTime(notify);
        }
    }

    /// @brief Lua function to unblock all players from setting a leaderboard time.
    public class UnblockEveryoneFromSettingTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "UnblockEveryoneFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// Usage: @code RoomService.UnblockEveryoneFromSettingTime(true); @endcode
        /// @param notify Indicates whether to notify players.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockEveryoneFromSettingTime(notify);
        }
    }

    /// @brief Lua function to unblock a specific player from setting a leaderboard time.
    public class UnblockPlayerFromSettingTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "UnblockPlayerFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// Usage: @code RoomService.UnblockPlayerFromSettingTime("76561197993793009", true); @endcode
        /// @param steamID The Steam ID of the player.
        /// @param notify Indicates whether to notify the player.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockPlayerFromSettingTime(steamID, notify);
        }
    }

    /// @brief Lua function to block a specific player from setting a leaderboard time.
    public class BlockPlayerFromSettingTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "BlockPlayerFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// Usage: @code RoomService.BlockPlayerFromSettingTime("76561197993793009", true); @endcode
        /// @param steamID The Steam ID of the player.
        /// @param notify Indicates whether to notify the player.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockPlayerFromSettingTime(steamID, notify);
        }
    }

    /// @brief Lua function to set the sorting method for a small leaderboard.
    public class SetSmallLeaderboardSortingMethod : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetSmallLeaderboardSortingMethod";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// Usage: @code RoomService.SetSmallLeaderboardSortingMethod(true); @endcode
        /// @param useChampionshipSorting True to use championship sorting; false for default sorting.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(bool useChampionshipSorting)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetSmallLeaderboardSortingMethod(useChampionshipSorting);
        }
    }

    /// @brief Lua function to set a player's time on the leaderboard.
    public class SetPlayerTimeOnLeaderboard : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerTimeOnLeaderboard";

        public Delegate CreateFunction()
        {
            return new Action<ulong, float, bool>(Implementation);
        }

        /// Usage: @code RoomService.SetPLayerTimeOnLeaderboard("76561197993793009", 42.1337, true); @endcode
        /// @param steamID The Steam ID of the player.
        /// @param time The time to set on the leaderboard.
        /// @param notify True to notify the player; false otherwise.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, float time, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerTimeOnLeaderboard(steamID, time, notify);
        }
    }

    /// @brief Lua function to set leaderboard overrides for a specific player.
    public class SetPlayerLeaderboardOverrides : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerLeaderboardOverrides";

        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string, string, string, string>(Implementation);
        }

        /// Usage: @code RoomService.SetPlayerLeaderboardOverrides("76561197993793009", "<color=yellow>42.1337</color>", "<color=blue>Kilandor</color>", "First", "1337", "1337"); @endcode
        /// Strings can be replaced with anything, as this is just a visual override\n
        /// Supports <a href="https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichText.html" target="_blank">Rich Text</a>
        /// @param steamID The Steam ID of the player.
        /// @param time The override time to display.
        /// @param name The override name to display.
        /// @param position The override position to display.
        /// @param points The override points to display.
        /// @param pointsWon The override points won to display.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, string time, string name, string position, string points, string pointsWon)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerLeaderboardOverrides(steamID, time, name, position, points, pointsWon);
        }
    }

    /// @brief Lua function to remove a player from the leaderboard.
    public class RemovePlayerFromLeaderboard : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "RemovePlayerFromLeaderboard";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        /// Usage: @code RoomService.RemovePlayerFromLeaderboard("76561197993793009", true); @endcode
        /// @param steamID The Steam ID of the player to remove.
        /// @param notify True to notify the player; false otherwise.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_RemovePlayerFromLeaderboard(steamID, notify);
        }
    }

    /// @brief Lua function to set the points distribution in the lobby.
    public class SetPointsDistribution : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPointsDistribution";

        public Delegate CreateFunction()
        {
            return new Action<int[], int, int>(Implementation);
        }

        /// Usage: @code RoomService.SetPointsDistribution({32,28,24,20,16,12,8,4}, 2, -1); @endcode
        /// @param values The points distribution values.
        /// @param baseline The baseline points value.
        /// @param dnf The points value for "Did Not Finish" players.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(int[] values, int baseline, int dnf)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPointsDistribution(values.ToList(), baseline, dnf);
        }
    }

    /// @brief Lua function to reset the points distribution to default values.
    public class ResetPointsDistribution : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetPointsDistribution";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }
        /// @details
        /// Usage: @code RoomService.ResetPointsDistribution(); @endcode
        /// 
        /// <h3>Source Code</h3>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_ResetPointsDistribution();
        }
    }

    /// @brief Lua function to set championship points for a specific player.
    public class SetPlayerChampionshipPoints : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerChampionshipPoints";

        public Delegate CreateFunction()
        {
            return new Action<ulong, int, int, bool>(Implementation);
        }

        /// Usage: @code RoomService.SetPlayerChampionshipPoints("76561197993793009", 1337, 42, true); @endcode
        /// @param steamID The Steam ID of the player.
        /// @param points The total points to assign to the player.
        /// @param pointsWon The points won in the current round.
        /// @param notify True to notify the player; false otherwise.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(ulong steamID, int points, int pointsWon, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerChampionshipPoints(steamID, points, pointsWon, notify);
        }
    }

    /// @brief Lua function to reset championship points for all players.
    public class ResetChampionshipPoints : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetChampionshipPoints";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// Usage: @code RoomService.ResetChampionshipPoints(); @endcode
        /// @param notify True to notify all players; false otherwise.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.ResetChampionshipPoints(notify);
        }
    }

    /// @brief Lua function to set the round length for the lobby.
    public class SetRoundLength : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetRoundLength";

        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        /// Usage: @code RoomService.SetRoundLength(300); @endcode
        /// @param time The round length in seconds (minimum 30).
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to enable or disable voteskip in the lobby.
    public class SetVoteskip : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetVoteskip";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        /// Usage: @code RoomService.SetVoteskip(true); @endcode
        /// @param enabled True to enable voteskip; false to disable it.
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(bool enabled)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage($"/vs {(enabled ? "on" : "off")}");
        }
    }

    /// @brief Lua function to set the percentage required for voteskip.
    public class SetVoteskipPercentage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetVoteskipPercentage";

        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        /// Usage: @code RoomService.SetVoteSkipPercentage(60); @endcode
        /// @param percentage The percentage required for voteskip (1 to 100).
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to set the lobby name.
    public class SetLobbyName : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetLobbyName";

        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// Usage: @code RoomService.SetLobbyName("Cozy Cart Racing"); @endcode
        /// @param name The new name for the lobby.
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to set a server message on screen.
    public class SetServerMessage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetServerMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// Usage: @code RoomService.SetServerMessage("<size=200%><color=green>Welcome to the Lobby</color></size>", 0); @endcode
        /// Supports <a href="https://docs.unity3d.com/Packages/com.unity.textmeshpro@4.0/manual/RichText.html" target="_blank">Rich Text</a>
        /// @param message The message to display on the server.
        /// @param time The duration in seconds to display the message. 0 is a persistant display
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to remove the current server message.
    public class RemoveServerMessage : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "RemoveServerMessage";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// Usage: @code RoomService.RemoveServerMessage(); @endcode
        /// 
        /// <h3>Source Code</h3>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage("/servermessage remove");
        }
    }

    /// @brief Lua function to get the number of players in the current lobby.
    public class GetPlayerCount : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlayerCount";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }
        
        /// @details
        /// Usage: @code RoomService.GetPlayerCount(); @endcode
        /// @return The number of players in the current lobby, or -1 if unavailable.
        /// @retval int
        /// 
        /// <h3>Source Code</h3>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }
            
            return ZeepkistNetwork.Players?.Count ?? -1;
        }
    }

    /// @brief Lua function to get the current playlist index.
    public class GetPlaylistIndex : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlaylistIndex";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.GetPlaylistIndex(); @endcode
        /// @return The current playlist index, or -1 if unavailable.
        /// @retval int
        /// 
        /// <h3>Source Code</h3>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.CurrentPlaylistIndex ?? -1;
        }
    }

    /// @brief Lua function to get the length of the current playlist.
    public class GetPlaylistLength : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlaylistLength";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }
        
        /// @details
        /// Usage: @code RoomService.GetPlaylistLength(); @endcode
        /// @return The length of the playlist, or -1 if unavailable.
        /// @retval int
        /// 
        /// <h3>Source Code</h3>
        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.Playlist.Count ?? -1;
        }
    }

    /// @brief Lua function to get the player at the given position on the leaderboard
    public class GetPlayerAtPosition : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlayerAtPosition";

        public Delegate CreateFunction()
        {
            return new Func<int, ulong>(Implementation);
        }

        /// Usage: @code RoomService.GetPlayerAtPosition(0); @endcode
        /// @param position  The leaderboard position to get. Index starts at 0 (first place)
        /// @return The players steamID, or 0 if unavailable.
        /// @retval ulong
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to get an entry from the leaderboard.
    public class GetLeaderboardEntry : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetLeaderboardEntry";

        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<ulong, LeaderboardItem>(Implementation);
        }

        /// Usage: @code RoomService.GetLeaderboardEntry("76561197993793009"); @endcode
        /// @returns LeaderboardItem Array with the following structure.
        /// <br>Example array = { SteamID = "76561197993793009", Time = 1337, Username = "Kilandor" }
        /// @retval SteamID  The SteamID of the player.
        /// @retval Time  Time on Leaderboard
        /// @retval Username Username on Leaderboard
        /// 
        /// <h3>Source Code</h3>
        private LeaderboardItem Implementation(ulong steamID)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new LeaderboardItem() { SteamID = 0, Time = 0, Username = "" };

            }

            return ZeepkistNetwork.GetLeaderboardEntry(steamID);
        }
    }

    /// @brief Lua function to retrieve a leaderboard override entry for a specific player.
    public class GetLeaderboardOverride : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetLeaderboardOverride";

        public Delegate CreateFunction()
        {
            return new Func<ulong, LeaderboardOverrideItem>(Implementation);
        }

        /// Usage: @code RoomService.GetLeaderboardOverride("76561197993793009"); @endcode
        /// @returns LeaderboardOverrideItem Array with the following structure.
        /// <br>Example array = { SteamID = "76561197993793009", overrideNameText = "Kilandor", overridePositionText = "1st", overrideTimeText = "12:34", overridePointsText = "1234", overridePointsWonText = "123" }
        /// @retval SteamID  The SteamID of the player.
        /// @retval overrideNameText Current Override Name.
        /// @retval overridePositionText Current Override Position.
        /// @retval overrideTimeText Current Override Time.
        /// @retval overridePointsText Current Override Points. 
        /// @retval overridePointsWonText Current Override Points Won.
        ///
        /// <h3>Source Code</h3>
        private LeaderboardOverrideItem Implementation(ulong steamID)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new LeaderboardOverrideItem() { SteamID = 0, overrideNameText = "", overridePositionText = "", overrideTimeText = "", overridePointsText = "", overridePointsWonText = "" };
            }

            return ZeepkistNetwork.GetLeaderboardOverride(steamID);
        }
    }

    /// @brief Lua function to retrieve the entire leaderboard.
    public class GetLeaderboard : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetLeaderboard";

        public Delegate CreateFunction()
        {
            return new Func<List<LeaderboardItem>>(Implementation);
        }

        /// Usage: @code RoomService.GetLeaderboard(); @endcode
        /// @return A list of <see cref="LeaderboardItem"/> representing the leaderboard. If unavailable, returns an empty list.
        /// @retval Leaderboard This is an array with the follow structure:
        /// <br>Example array[0] = { SteamID = "76561197993793009", Time = 1337, Username = "Kilandor" } 
        /// <br><span class="paramname">SteamID</span> The SteamID of the player.
        /// <br><span class="paramname">Time</span> Time on Leaderboard
        /// <br><span class="paramname">Username</span> Username on Leaderboard
        /// 
        /// <h3>Source Code</h3>
        private List<LeaderboardItem> Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new List<LeaderboardItem>();
            }

            return ZeepkistNetwork.GetLeaderboard() ?? new List<LeaderboardItem>();
        }
    }

    /// @brief Lua function to clear the time logger.
    public class ClearLogger : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ClearLogger";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.ClearLogger(); @endcode
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to save the time logger to disk.
    public class SaveLogger : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SaveLogger";

        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.SaveLogger(); @endcode
        /// 
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to print the time logger to the console.
    public class PrintLogger : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "PrintLogger";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.PrintLogger(); @endcode
        /// 
        /// <h3>Source Code</h3>
        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            string[] content = Plugin.Instance.GetLoggerLines();
            string logLine = string.Join('\n', content);
            Utilities.Log(logLine);            
        }
    }

    /// @brief Lua function to get the current date.
    public class GetCurrentDate : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetCurrentDate";

        public Delegate CreateFunction()
        {
            return new Func<string>(Implementation);
        }
        
        /// @details
        /// Usage: @code RoomService.GetCurrentDate; @endcode
        /// @return The current date in the format YYYY-MM-DD.
        /// @retval string
        ///
        /// <h3>Source Code</h3>
        private string Implementation()
        {
            return DateTime.Now.ToString("yyyy-MM-dd");
        }
    }

    /// @brief Lua function to get the current time.
    public class GetCurrentTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetCurrentTime";

        public Delegate CreateFunction()
        {
            return new Func<string>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.GetCurrentTime; @endcode
        /// @return The current time in the format HH:mm:ss.
        /// @retval string
        ///
        /// <h3>Source Code</h3>
        private string Implementation()
        {
            return DateTime.Now.ToString("HH:mm:ss");
        }
    }

    /// @brief Lua function to generate a random number.
    public class GenerateRandomNumber : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GenerateRandomNumber";

        public Delegate CreateFunction()
        {
            // The Lua function takes two parameters (min and max) and returns an integer.
            return new Func<int, int, int>(Implementation);
        }

        /// Usage: @code RoomService.GenerateRandomNumber(1, 10); @endcode
        /// @param min The minimum value (inclusive).
        /// @param max The maximum value (inclusive).
        /// @return A random integer between min and max.
        /// @retval int
        ///
        /// <h3>Source Code</h3>
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

    /// @brief Lua function to convert seconds into a time string.
    public class SecondsToTime : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SecondsToTime";
        
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<float,int,string>(Implementation);
        }

        /// Usage: @code RoomService.SecondsToTime; @endcode
        /// @param time The time in seconds.
        /// @param precision The amount of decimals.
        /// @return A time string in the format hour:minutes:seconds.milliseconds
        /// Example 00:42.133
        /// <br>Only returns hour if the time is more than 59 minutes.
        /// @retval string
        ///
        /// <h3>Source Code</h3>
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
    
    /// @brief Lua function to reset all players
    public class ResetAllPlayers : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetAllPlayers";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.ResetAllPlayers(); @endcode
        /// 
        /// <h3>Source Code</h3>
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
    
    /// @brief Lua function to reset players
    public class ResetPlayers : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetPlayers";
        
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Action<List<ulong>>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.ResetPlayers({"76561197993793009", "76561197993793009"}); @endcode
        /// 
        /// <h3>Source Code</h3>
        private void Implementation(List<ulong> listPlayers)
        {
            if (!RoomServiceUtils.IsOnlineHost() || listPlayers == null || listPlayers.Count <= 0)
            {
                return;
            }
            
            ZeepkistNetwork.CustomLeaderBoard_ResetPlayers(listPlayers);
        }
    }
    
    /// @brief Lua function to get all players
    public class GetAllPlayers : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetAllPlayers";
        
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<List<PlayerTime>>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.GetAllPlayers(); @endcode
        ///
        /// @return A list of <see cref="PlayerTime"/> representing the leaderboard. If unavailable, returns an empty list.
        /// @retval Name Username
        /// @retval Tag User Tag
        /// @retval FullName Username including tag
        /// @retval SteamID
        /// @retval Time The last time set by player -1 if unavailable.
        /// @retval BestTime The best time set by player -1 if unavailable.
        /// @retval ChatColor Hex color code of the player's chat color.'
        /// 
        /// Example array[0] = { Name = "Kilandor", Tag = "NOOB", FullName = "[NOOB]Kilandor", SteamID = "76561197993793009", Time = 1337, BestTime = 42, ChatColor = "#87CEEB" }
        ///  
        /// <h3>Source Code</h3>
        private List<PlayerTime> Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return new List<PlayerTime>();
            }
            
            string hash = ZeepSDK.Level.LevelApi.CurrentHash;

            List<PlayerTime> luaPlayers = new List<PlayerTime>();
            
            List<ZeepkistNetworkPlayer> players = new List<ZeepkistNetworkPlayer>(ZeepkistNetwork.Players.Values);
            foreach(ZeepkistNetworkPlayer player in players)
            {
                // Use existing player data first
                PlayerTime playerTime = null;
                if(Plugin.Instance.playerTimes.ContainsKey(hash))
                    playerTime = Plugin.Instance.playerTimes[hash].Find(p => p.SteamID == player.SteamID);

                if (playerTime == null)
                    playerTime = new PlayerTime()
                    {
                        Name = player.GetUserNameNoTag(),
                        Tag = player.GetUserTag(),
                        FullName = player.GetTaggedUsername(),
                        SteamID = player.SteamID,
                        Time = -1,
                        BestTime = -1,
                        ChatColor = RoomServiceUtils.ColorToHex(player.chatColor)
                    };
                    
                luaPlayers.Add(playerTime);
            }
            return luaPlayers;
        }
    }
    
    /// @brief Lua function to get all players
    public class GetPlayer : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlayer";
        
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<string, PlayerTime>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.GetPlayer("Kilandor"); @endcode
        ///
        /// @return A list of <see cref="PlayerTime"/> representing the leaderboard. If unavailable, returns an empty list.
        /// @retval Name Username
        /// @retval Tag User Tag
        /// @retval FullName Username including tag
        /// @retval SteamID
        /// @retval Time The last time set by player -1 if unavailable.
        /// @retval BestTime The best time set by player -1 if unavailable.
        /// @retval ChatColor Hex color code of the player's chat color.'
        /// 
        /// Example { Name = "Kilandor", Tag = "NOOB", FullName = "[NOOB]Kilandor", SteamID = "76561197993793009", Time = 1337, BestTime = 42, ChatColor = "#87CEEB" }
        /// 
        /// <h3>Source Code</h3>
        private PlayerTime Implementation(string playerName)
        {
            if (!RoomServiceUtils.IsOnlineHost() || playerName == "")
            {
                return new PlayerTime();
            }
            
            // uses existing playertime from OnLeaderboardUpdate
            string hash = ZeepSDK.Level.LevelApi.CurrentHash;
            
            PlayerTime playerTime = null;
            if(Plugin.Instance.playerTimes.ContainsKey(hash))
                playerTime = Plugin.Instance.playerTimes[hash].Find(p => p.Name == playerName);
            
            if(playerTime != null)
            {
                return playerTime;
            }
            
            // if not found, create new playertime from game if player exists
            List<ZeepkistNetworkPlayer> players = new List<ZeepkistNetworkPlayer>(ZeepkistNetwork.Players.Values);
            ZeepkistNetworkPlayer p = players.Find(p => p.Username == playerName);
            if (p == null)
                return new PlayerTime();
            
            return new PlayerTime()
            {
                Name = p.GetUserNameNoTag(),
                Tag = p.GetUserTag(),
                FullName = p.GetTaggedUsername(),
                SteamID = p.SteamID,
                Time = -1,
                BestTime = -1,
                ChatColor = RoomServiceUtils.ColorToHex(p.chatColor)
            };
        }
    }
    
    /// @brief Lua function to get all players
    public class GetPlayerBySteamID : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlayerBySteamID";
        
        public Delegate CreateFunction()
        {
            // Adjust the delegate to accept a parameter
            return new Func<ulong, PlayerTime>(Implementation);
        }

        /// @details
        /// Usage: @code RoomService.GetPlayer("76561197993793009"); @endcode
        /// @param steamID The SteamID of the player to return
        ///
        /// @return A list of <see cref="PlayerTime"/> representing the leaderboard. If unavailable, returns an empty list.
        /// @retval Name Username
        /// @retval Tag User Tag
        /// @retval FullName Username including tag
        /// @retval SteamID
        /// @retval Time The last time set by player -1 if unavailable.
        /// @retval BestTime The best time set by player -1 if unavailable.
        /// @retval ChatColor Hex color code of the player's chat color.'
        /// 
        /// Example { Name = "Kilandor", Tag = "NOOB", FullName = "[NOOB]Kilandor", SteamID = "76561197993793009", Time = 1337, BestTime = 42, ChatColor = "#87CEEB" }
        ///
        /// <h3>Source Code</h3>
        private PlayerTime Implementation(ulong steamID)
        {
            if (!RoomServiceUtils.IsOnlineHost() || steamID <= 0)
            {
                return new PlayerTime();
            }
            
            // uses existing playertime from OnLeaderboardUpdate
            string hash = ZeepSDK.Level.LevelApi.CurrentHash;
            
            PlayerTime playerTime = null;
            if(Plugin.Instance.playerTimes.ContainsKey(hash))
                playerTime = Plugin.Instance.playerTimes[hash].Find(p => p.SteamID == steamID);
            
            if(playerTime != null)
            {
                return playerTime;
            }
            
            // if not found, create new playertime from game if player exists
            List<ZeepkistNetworkPlayer> players = new List<ZeepkistNetworkPlayer>(ZeepkistNetwork.Players.Values);
            ZeepkistNetworkPlayer p = players.Find(p => p.SteamID == steamID);
            if (p == null)
                return new PlayerTime();
            
            return new PlayerTime()
            {
                Name = p.GetUserNameNoTag(),
                Tag = p.GetUserTag(),
                FullName = p.GetTaggedUsername(),
                SteamID = p.SteamID,
                Time = -1,
                BestTime = -1,
                ChatColor = RoomServiceUtils.ColorToHex(p.chatColor)
            };
        }
    }
    
}

using System;
using ZeepSDK.Scripting.ZUA;
using ZeepSDK.Scripting;
using ZeepkistClient;

namespace RoomService
{
    /// @brief Represents an event triggered when the lobby timer changes.
    /// @details
    /// Must subscribe to this event in the OnLoad() function of your Lua script.
    /// @code Zua.ListenTo("RoomService_OnLobbyTimer") @endcode
    /// Usage:
    /// @code
    /// function RoomService_OnLobbyTimer(time)
    /// {
    ///   if time == 30 then
    ///     RoomService.SendChatMessage("ByteBot", "30 seconds left!")
    ///   end
    /// } @endcode
    /// @param time The current round time remaining in seconds (counts down).
    public class OnLobbyTimerEvent : ILuaEvent
    {
        public string Name => "RoomService_OnLobbyTimer";

        private Action<int> _lobbyTimerAction;
        
        public void Subscribe()
        {
            _lobbyTimerAction = time =>
            {
                ScriptingApi.CallFunction(Name, time);
            };

            Plugin.Instance.LobbyTimerAction += _lobbyTimerAction;
        }
        
        public void Unsubscribe()
        {
            if (_lobbyTimerAction != null)
            {
                Plugin.Instance.LobbyTimerAction -= _lobbyTimerAction;
                _lobbyTimerAction = null;
            }
        }
    }

    /// @brief Represents an event triggered when a player sets their time.
    /// /// @details
    /// Must subscribe to this event in the OnLoad() function of your Lua script.
    /// @code Zua.ListenTo("RoomService_OnPlayerSetTime") @endcode
    /// Usage:
    /// @code
    /// function RoomService_OnPlayerSetTime(playerTime)
    /// {
    ///    playerName = "<color="..playerTime.chatColor..">".. playerTime.FullName .. "</color>"
    ///     RoomService.SetPlayerLeaderboardOverrides(playerTime.SteamID, "", playerName, "", "", "")
    /// } @endcode
    /// @param playerTime The <see cref="PlayerTime"/> object for the player who set their time.
    ///
    /// Structure of playerTime
    /// @param SteamID Steamid of the player
    /// @param Name Username of the player
    /// @param Tag Clan tag
    /// @param FullName Full username including clan tag
    /// @param Time The time the player just set
    /// @param BestTime Best time the player has set
    /// @param ChatColor The chat color set by the player
    public class OnPlayerSetTimeEvent : ILuaEvent
    {
        public string Name => "RoomService_OnPlayerSetTime";

        private Action<PlayerTime> _playerSetTimeAction;
        
        public void Subscribe()
        {
            _playerSetTimeAction = playerTime =>
            {
                ScriptingApi.CallFunction(Name, playerTime);
            };

            Plugin.Instance.PlayerSetTime += _playerSetTimeAction;
        }
        
        public void Unsubscribe()
        {
            if (_playerSetTimeAction != null)
            {
                Plugin.Instance.PlayerSetTime -= _playerSetTimeAction;
                _playerSetTimeAction = null;
            }
        }
    }

    /// @brief Represents an event triggered when the leaderboard changes.
    /// /// @details
    /// Must subscribe to this event in the OnLoad() function of your Lua script.
    /// @code Zua.ListenTo("RoomService_LeaderboardLogging") @endcode
    /// This is required to track leaderboard changes, it has no call to a lua function.
    public class OnLeaderboardChangeEvent : ILuaEvent
    {
        public string Name => "RoomService_LeaderboardLogging";
        
        public void Subscribe()
        {
            ZeepkistNetwork.PlayerResultsChanged += Plugin.Instance.ProcessLeaderboardUpdate;
        }
        
        public void Unsubscribe()
        {
            ZeepkistNetwork.PlayerResultsChanged -= Plugin.Instance.ProcessLeaderboardUpdate;
        }
    }
}

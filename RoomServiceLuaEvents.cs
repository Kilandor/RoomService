using System;
using ZeepSDK.Scripting.ZUA;
using ZeepSDK.Scripting;
using ZeepkistClient;

namespace RoomService
{
    /// <summary>
    /// Represents an event triggered when the lobby timer changes.
    /// </summary>
    public class OnLobbyTimerEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "RoomService_OnLobbyTimer";

        private Action<int> _lobbyTimerAction;

        /// <summary>
        /// Subscribes to the lobby timer event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _lobbyTimerAction = time =>
            {
                ScriptingApi.CallFunction(Name, time);
            };

            Plugin.Instance.LobbyTimerAction += _lobbyTimerAction;
        }

        /// <summary>
        /// Unsubscribes from the lobby timer event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_lobbyTimerAction != null)
            {
                Plugin.Instance.LobbyTimerAction -= _lobbyTimerAction;
                _lobbyTimerAction = null;
            }
        }
    }

    
    public class OnPlayerSetTimeEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "RoomService_OnPlayerSetTime";

        private Action<PlayerTime> _playerSetTimeAction;

        /// <summary>
        /// Subscribes to the lobby timer event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _playerSetTimeAction = playerTime =>
            {
                ScriptingApi.CallFunction(Name, playerTime);
            };

            Plugin.Instance.PlayerSetTime += _playerSetTimeAction;
        }

        /// <summary>
        /// Unsubscribes from the lobby timer event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_playerSetTimeAction != null)
            {
                Plugin.Instance.PlayerSetTime -= _playerSetTimeAction;
                _playerSetTimeAction = null;
            }
        }
    }

    public class OnLeaderboardChangeEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "RoomService_LeaderboardLogging";

        /// <summary>
        /// Subscribes to the leaderboard changes to process them
        /// </summary>
        public void Subscribe()
        {
            ZeepkistNetwork.LeaderboardUpdated += Plugin.Instance.ProcessLeaderboardUpdate;
        }

        /// <summary>
        /// Unsubscribes from the leaderboard event.
        /// </summary>
        public void Unsubscribe()
        {
            ZeepkistNetwork.LeaderboardUpdated -= Plugin.Instance.ProcessLeaderboardUpdate;
        }
    }
}

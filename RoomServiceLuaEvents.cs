using System;
using ZeepSDK.Scripting.ZUA;
using ZeepSDK.Scripting;

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
        public string Name => "OnLobbyTimer";

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

}

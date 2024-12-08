using System;
using ZeepSDK.Scripting.ZUA;
using ZeepSDK.Multiplayer;
using ZeepSDK.Racing;

namespace RoomService
{
    public class OnLobbyTimerEvent : ILuaEvent
    {
        public string Name => "OnLobbyTimer";

        private Action<int> _lobbyTimerAction;

        public void Subscribe()
        {
            _lobbyTimerAction = time =>
            {
                Plugin.Instance.script.CallFunction(Name, time);
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

    public class OnPlayerJoinedEvent : ILuaEvent
    {
        public string Name => "OnPlayerJoined";

        private PlayerJoinedDelegate _playerJoinedDelegate;

        public void Subscribe()
        {
            _playerJoinedDelegate = player =>
            {
                Plugin.Instance.script.CallFunction(Name, player);
            };

            MultiplayerApi.PlayerJoined += _playerJoinedDelegate;
        }

        public void Unsubscribe()
        {
            if (_playerJoinedDelegate != null)
            {
                MultiplayerApi.PlayerJoined -= _playerJoinedDelegate;
                _playerJoinedDelegate = null;
            }
        }
    }

    public class OnPlayerLeftEvent : ILuaEvent
    {
        public string Name => "OnPlayerLeft";

        private PlayerLeftDelegate _playerLeftDelegate;

        public void Subscribe()
        {
            _playerLeftDelegate = player =>
            {
                Plugin.Instance.script.CallFunction(Name, player);
            };

            MultiplayerApi.PlayerLeft += _playerLeftDelegate;
        }

        public void Unsubscribe()
        {
            if (_playerLeftDelegate != null)
            {
                MultiplayerApi.PlayerLeft -= _playerLeftDelegate;
                _playerLeftDelegate = null;
            }
        }
    }

    public class OnLevelLoadedEvent : ILuaEvent
    {
        public string Name => "OnLevelLoaded";

        private LevelLoadedDelegate _levelLoadedDelegate;

        public void Subscribe()
        {
            _levelLoadedDelegate = () =>
            {
                Plugin.Instance.script.CallFunction(Name);
            };

            RacingApi.LevelLoaded += _levelLoadedDelegate;
        }

        public void Unsubscribe()
        {
            if (_levelLoadedDelegate != null)
            {
                RacingApi.LevelLoaded -= _levelLoadedDelegate;
                _levelLoadedDelegate = null;
            }
        }
    }

    public class OnRoundStartedEvent : ILuaEvent
    {
        public string Name => "OnRoundStarted";

        private RoundStartedDelegate _roundStartedDelegate;

        public void Subscribe()
        {
            _roundStartedDelegate = () =>
            {
                Plugin.Instance.script.CallFunction(Name);
            };

            RacingApi.RoundStarted += _roundStartedDelegate;
        }

        public void Unsubscribe()
        {
            if (_roundStartedDelegate != null)
            {
                RacingApi.RoundStarted -= _roundStartedDelegate;
                _roundStartedDelegate = null;
            }
        }
    }

    public class OnRoundEndedEvent : ILuaEvent
    {
        public string Name => "OnRoundEnded";

        private RoundStartedDelegate _roundEndedDelegate;

        public void Subscribe()
        {
            _roundEndedDelegate = () =>
            {
                Plugin.Instance.script.CallFunction(Name);
            };

            RacingApi.RoundEnded += _roundEndedDelegate;
        }

        public void Unsubscribe()
        {
            if (_roundEndedDelegate != null)
            {
                RacingApi.RoundEnded -= _roundEndedDelegate;
                _roundEndedDelegate = null;
            }
        }
    }
}

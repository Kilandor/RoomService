using ZeepSDK.Chat;
using ZeepSDK.Multiplayer;
using ZeepSDK.PhotoMode;
using ZeepSDK.Racing;
using ZeepSDK.Scripting;
using ZeepSDK.Scripting.ZUA;

namespace RoomService
{
    /// <summary>
    /// Represents an event triggered when a chat message is received.
    /// </summary>
    public class OnChatMessageReceived : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnChatMessageReceived";

        private ChatMessageReceivedDelegate _chatMessageReceivedDelegate;

        /// <summary>
        /// Subscribes to the chat message received event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _chatMessageReceivedDelegate = (steamID, userName, message) =>
            {
                ScriptingApi.CallFunction(Name, steamID, userName, message);
            };

            ChatApi.ChatMessageReceived += _chatMessageReceivedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the chat message received event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_chatMessageReceivedDelegate != null)
            {
                ChatApi.ChatMessageReceived -= _chatMessageReceivedDelegate;
                _chatMessageReceivedDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when connecting to a game.
    /// </summary>
    public class OnConnectedToGameEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnConnectedToGame";

        private ConnectedToGameDelegate _connectedToGameDelegate;

        /// <summary>
        /// Subscribes to the connected to game event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _connectedToGameDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            MultiplayerApi.ConnectedToGame += _connectedToGameDelegate;
        }

        /// <summary>
        /// Unsubscribes from the connected to game event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_connectedToGameDelegate != null)
            {
                MultiplayerApi.ConnectedToGame -= _connectedToGameDelegate;
                _connectedToGameDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when disconnecting from a game.
    /// </summary>
    public class OnDisconnectedFromGameEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnDisconnectedFromGame";

        private DisconnectedFromGameDelegate _disconnectedFromGameDelegate;

        /// <summary>
        /// Subscribes to the disconnected from game event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _disconnectedFromGameDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            MultiplayerApi.DisconnectedFromGame += _disconnectedFromGameDelegate;
        }

        /// <summary>
        /// Unsubscribes from the disconnected from game event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_disconnectedFromGameDelegate != null)
            {
                MultiplayerApi.DisconnectedFromGame -= _disconnectedFromGameDelegate;
                _disconnectedFromGameDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when a player joins the multiplayer session.
    /// </summary>
    public class OnPlayerJoinedEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnPlayerJoined";

        private PlayerJoinedDelegate _playerJoinedDelegate;

        /// <summary>
        /// Subscribes to the player joined event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _playerJoinedDelegate = player =>
            {
                ScriptingApi.CallFunction(Name, player);
            };

            MultiplayerApi.PlayerJoined += _playerJoinedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the player joined event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_playerJoinedDelegate != null)
            {
                MultiplayerApi.PlayerJoined -= _playerJoinedDelegate;
                _playerJoinedDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when a player leaves the multiplayer session.
    /// </summary>
    public class OnPlayerLeftEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnPlayerLeft";

        private PlayerLeftDelegate _playerLeftDelegate;

        /// <summary>
        /// Subscribes to the player left event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _playerLeftDelegate = player =>
            {
                ScriptingApi.CallFunction(Name, player);
            };

            MultiplayerApi.PlayerLeft += _playerLeftDelegate;
        }

        /// <summary>
        /// Unsubscribes from the player left event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_playerLeftDelegate != null)
            {
                MultiplayerApi.PlayerLeft -= _playerLeftDelegate;
                _playerLeftDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when a level is loaded in the game.
    /// </summary>
    public class OnLevelLoadedEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnLevelLoaded";

        private LevelLoadedDelegate _levelLoadedDelegate;

        /// <summary>
        /// Subscribes to the level loaded event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _levelLoadedDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            RacingApi.LevelLoaded += _levelLoadedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the level loaded event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_levelLoadedDelegate != null)
            {
                RacingApi.LevelLoaded -= _levelLoadedDelegate;
                _levelLoadedDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when a racing round starts.
    /// </summary>
    public class OnRoundStartedEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnRoundStarted";

        private RoundStartedDelegate _roundStartedDelegate;

        /// <summary>
        /// Subscribes to the round started event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _roundStartedDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            RacingApi.RoundStarted += _roundStartedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the round started event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_roundStartedDelegate != null)
            {
                RacingApi.RoundStarted -= _roundStartedDelegate;
                _roundStartedDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered when a racing round ends.
    /// </summary>
    public class OnRoundEndedEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnRoundEnded";

        private RoundStartedDelegate _roundEndedDelegate;

        /// <summary>
        /// Subscribes to the round ended event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _roundEndedDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            RacingApi.RoundEnded += _roundEndedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the round ended event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_roundEndedDelegate != null)
            {
                RacingApi.RoundEnded -= _roundEndedDelegate;
                _roundEndedDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered whenever you enter Photo Mode.
    /// </summary>
    public class OnPhotoModeEnteredEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnPhotoModeEntered";

        private PhotoModeEnteredDelegate _photomodeEnteredDelegate;

        /// <summary>
        /// Subscribes to the photomode entered event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _photomodeEnteredDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            PhotoModeApi.PhotoModeEntered += _photomodeEnteredDelegate;
        }

        /// <summary>
        /// Unsubscribes from the photomode entered event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_photomodeEnteredDelegate != null)
            {
                PhotoModeApi.PhotoModeEntered -= _photomodeEnteredDelegate;
                _photomodeEnteredDelegate = null;
            }
        }
    }

    /// <summary>
    /// Represents an event triggered whenever you exit Photo Mode.
    /// </summary>
    public class OnPhotoModeExitedEvent : ILuaEvent
    {
        /// <summary>
        /// Gets the name of the Lua event.
        /// </summary>
        public string Name => "OnPhotoModeExited";

        private PhotoModeExitedDelegate _photomodeExitedDelegate;

        /// <summary>
        /// Subscribes to the photomode exited event and invokes the corresponding Lua function.
        /// </summary>
        public void Subscribe()
        {
            _photomodeExitedDelegate = () =>
            {
                ScriptingApi.CallFunction(Name);
            };

            PhotoModeApi.PhotoModeExited += _photomodeExitedDelegate;
        }

        /// <summary>
        /// Unsubscribes from the photomode exited event.
        /// </summary>
        public void Unsubscribe()
        {
            if (_photomodeExitedDelegate != null)
            {
                PhotoModeApi.PhotoModeExited -= _photomodeExitedDelegate;
                _photomodeExitedDelegate = null;
            }
        }
    }
}

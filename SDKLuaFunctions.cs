using System;
using ZeepSDK.Scripting.ZUA;

namespace RoomService
{
    /// <summary>
    /// Represents a Lua function that adds a message to the local chat UI.
    /// </summary>
    public class AddLocalMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "ChatApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "AddLocalMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to add a message to the local chat UI.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        private void Implementation(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            ZeepSDK.Chat.ChatApi.AddLocalMessage(message);
        }
    }

    /// <summary>
    /// Represents a Lua function that clears the chat window.
    /// </summary>
    public class ClearChatFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "ChatApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "ClearChat";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate</returns>
        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        /// <summary>
        /// Implementation of the function to clear the chat.
        /// </summary>
        private void Implementation()
        {
            ZeepSDK.Chat.ChatApi.ClearChat();
        }
    }

    /// <summary>
    /// Represents a Lua function that sends a message to the chat.
    /// </summary>
    public class SendMessageFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "ChatApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "SendMessage";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to send a message to the chat.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        private void Implementation(string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            /*
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }*/

            ZeepSDK.Chat.ChatApi.SendMessage(message);
        }
    }

    /// <summary>
    /// Represents a Lua function that logs an information level message to the user.
    /// </summary>
    public class LogInfoFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "MessengerApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "LogInfo";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and a duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function that logs an information level message to the user.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="duration">The duration of the message.</param>
        private void Implementation(string message, float duration)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if(duration <= 0)
            {
                return;
            }

            ZeepSDK.Messaging.MessengerApi.Log(message, duration);
        }
    }

    /// <summary>
    /// Represents a Lua function that logs an error level message to the user.
    /// </summary>
    public class LogErrorFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "MessengerApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "LogError";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and a duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function that logs an error level message to the user.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="duration">The duration of the message.</param>
        private void Implementation(string message, float duration)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (duration <= 0)
            {
                return;
            }

            ZeepSDK.Messaging.MessengerApi.LogError(message, duration);
        }
    }

    /// <summary>
    /// Represents a Lua function that logs a success level message to the user.
    /// </summary>
    public class LogSuccessFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "MessengerApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "LogSuccess";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and a duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function that logs a success level message to the user.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="duration">The duration of the message.</param>
        private void Implementation(string message, float duration)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (duration <= 0)
            {
                return;
            }

            ZeepSDK.Messaging.MessengerApi.LogSuccess(message, duration);
        }
    }

    /// <summary>
    /// Represents a Lua function that logs a warning level message to the user.
    /// </summary>
    public class LogWarningFunction : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "MessengerApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "LogWarning";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that takes a message and a duration.</returns>
        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        /// <summary>
        /// Implementation of the function that logs a warning level message to the user.
        /// </summary>
        /// <param name="message">The content of the message.</param>
        /// <param name="duration">The duration of the message.</param>
        private void Implementation(string message, float duration)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            if (duration <= 0)
            {
                return;
            }

            ZeepSDK.Messaging.MessengerApi.LogWarning(message, duration);
        }
    }

    /// <summary>
    /// Represents a Lua function to get the current level object.
    /// </summary>
    public class GetLevelFunctions : ILuaFunction
    {
        /// <summary>
        /// Gets the namespace of the Lua function.
        /// </summary>
        public string Namespace => "LevelApi";

        /// <summary>
        /// Gets the name of the Lua function.
        /// </summary>
        public string Name => "GetLevel";

        /// <summary>
        /// Creates the delegate for the Lua function.
        /// </summary>
        /// <returns>A delegate that returns the current level object.</returns>
        public Delegate CreateFunction()
        {
            return new Func<LevelScriptableObject>(Implementation);
        }

        /// <summary>
        /// Implementation of the function to get the current level.
        /// </summary>
        /// <returns>The current level object, or null if unavailable.</returns>
        private LevelScriptableObject Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return null;
            }

            return ZeepSDK.Level.LevelApi.CurrentLevel;
        }
    }
}

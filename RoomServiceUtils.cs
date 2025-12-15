using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    /// <summary>
    /// Provides utility methods for the RoomService namespace.
    /// </summary>
    public static class RoomServiceUtils
    {
        /// <summary>
        /// Determines whether the current client is the online host of the game.
        /// </summary>
        /// <returns>
        /// True if the client is connected to the game and is the master client; otherwise, false.
        /// </returns>
        public static bool IsOnlineHost()
        {
            return ZeepkistNetwork.IsConnectedToGame && ZeepkistNetwork.IsMasterClient;
        }

        /// <summary>
        /// Converts a Unity Color object into a hexadecimal string.
        /// </summary>
        /// <param name="color">The Unity Color to convert.</param>
        /// <returns>A string representing the color in hexadecimal format (e.g., \#RRGGBB).</returns>
        public static string ColorToHex(Color color)
        {
            // Clamp the color components to ensure valid byte values (0-255).
            int r = Mathf.Clamp((int)(color.r * 255), 0, 255);
            int g = Mathf.Clamp((int)(color.g * 255), 0, 255);
            int b = Mathf.Clamp((int)(color.b * 255), 0, 255);

            // Format the string without alpha.
            return string.Format("#{0:X2}{1:X2}{2:X2}", r, g, b);
        }
    }
}

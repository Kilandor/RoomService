using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}

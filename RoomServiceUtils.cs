using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeepkistClient;

namespace RoomService
{
    public static class RoomServiceUtils
    {
        public static bool IsOnlineHost()
        {
            return ZeepkistNetwork.IsConnectedToGame && ZeepkistNetwork.IsMasterClient;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZeepkistClient;

namespace RoomService
{
    public class RoomServicePlayer
    {
        public ulong SteamID;
        public string Name;
        public bool IsOnline;
        public int Points;
        public int PointsDifference;

        public RoomServicePlayer(ZeepkistNetworkPlayer player)
        {
            SteamID = player.SteamID;
            Name = player.GetUserNameNoTag();
            IsOnline = true;
            Points = 0;
            PointsDifference = 0;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService
{
    public struct RSPlayer
    {
        public ulong SteamID;
        public string Name;
        public bool IsOnline;
        public int Points;
        public int PointsDifference;

        public override bool Equals(object obj)
        {
            if (obj is RSPlayer other)
            {
                return SteamID == other.SteamID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return SteamID.GetHashCode();
        }

        public static bool operator ==(RSPlayer left, RSPlayer right) => left.Equals(right);
        public static bool operator !=(RSPlayer left, RSPlayer right) => !left.Equals(right);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService
{
    public struct RSLevel
    {
        public string UID;
        public string Name;
        public ulong WorkshopID;
        public string Author;

        public override bool Equals(object obj)
        {
            if (obj is RSLevel other)
            {
                return UID == other.UID;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return UID != null ? UID.GetHashCode() : 0;
        }

        public static bool operator ==(RSLevel left, RSLevel right) => left.Equals(right);
        public static bool operator !=(RSLevel left, RSLevel right) => !left.Equals(right);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService
{
    public class RoomServiceLevel
    {
        public string UID;
        public string Name;
        public ulong WorkshopID;
        public string Author;

        public RoomServiceLevel(LevelScriptableObject level)
        {
            UID = level.UID;
            Name = level.Name;
            WorkshopID = level.WorkshopID;
            Author = level.Author;
        }
    }
}

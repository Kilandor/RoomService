using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RoomService
{
    public struct RSContext
    {
        //Player
        public ulong SteamID;
        public string PlayerName;
        public int Points;
        public int PointsDifference;

        //Result
        public float Time;

        //Level
        public string UID;
        public ulong WorkshopID;
        public string LevelName;
        public string Author;

        //Parameters
        public Dictionary<string, string> Parameters;

        public RSContext(Dictionary<string, string> parameters)
        {
            Parameters = parameters;
            SteamID = 0;
            PlayerName = "";
            Points = 0;
            PointsDifference = 0;
            Time = 0;
            UID = "";
            WorkshopID = 0;
            LevelName = "";
            Author = "";
        }

        public void AddPlayer(RSPlayer player)
        {
            SteamID = player.SteamID;
            PlayerName = player.Name;
            Points = player.Points;
            PointsDifference = player.PointsDifference;
        }

        public void AddResult(RSResult result)
        {
            SteamID = result.SteamID;
            UID = result.UID;
            Time = result.Time;
        }

        public void AddLevel(RSLevel level)
        {
            UID = level.UID;
            LevelName = level.Name;
            WorkshopID = level.WorkshopID;
            Author = level.Author;
        }

        public string ReplaceParameters(string original)
        {
            string newString = original.Replace("{STEAMID}", SteamID.ToString());
            newString = newString.Replace("{PLAYERNAME}", PlayerName);
            newString = newString.Replace("{TIME}", Time.ToString());
            newString = newString.Replace("{UID}", UID.ToString());
            newString = newString.Replace("{WORKSHOPID}", WorkshopID.ToString());
            newString = newString.Replace("{LEVELNAME}", LevelName);
            newString = newString.Replace("{AUTHOR}", Author);
            newString = newString.Replace("{POINTS}", Points.ToString());
            newString = newString.Replace("{POINTSDIF}", PointsDifference.ToString());

            foreach (KeyValuePair<string, string> cp in Parameters)
            {
                newString = newString.Replace(cp.Key, cp.Value);
            }
            return newString;
        }
    }
}

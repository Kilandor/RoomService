using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    public class RSRoomTracker
    {
        //List with all the players encountered
        public Dictionary<ulong, RoomServicePlayer> players;
        //List with all the levels encountered.
        public Dictionary<string, RoomServiceLevel> levels;
        //A dictionary with a list of best times results per level.
        public Dictionary<string, List<RoomServiceResult>> results;

        public RSRoomTracker()
        {
            players = new Dictionary<ulong, RoomServicePlayer>();
            levels = new Dictionary<string, RoomServiceLevel>();
            results = new Dictionary<string, List<RoomServiceResult>>();
        }

        //This is called when:...
        //...we enter a lobby, to get the current state.
        //...a new map is started, to get a good initial state.
        //...anytime there is a leaderboard update to make sure we have the latest information.
        public void ProcessRoomState(List<ZeepkistNetworkPlayer> playerList, LevelScriptableObject level)
        {
            Debug.LogWarning("ProcessRoomState");

            if (level == null)
            {
                return;
            }

            //Add the level if its not in the list yet, and also add an entry to the results for that level.
            if (!levels.ContainsKey(level.UID))
            {
                levels.Add(level.UID, new RoomServiceLevel(level));
                results.Add(level.UID, new List<RoomServiceResult>());
            }

            //Go over all the players in the lobby.
            foreach (ZeepkistNetworkPlayer player in playerList)
            {
                //Get the player and add it if unknown.
                RoomServicePlayer rsPlayer = GetPlayer(player.SteamID);
                if (rsPlayer == null)
                {
                    rsPlayer = AddPlayer(player);
                }
                rsPlayer.IsOnline = true;
                Debug.Log("Player: " + rsPlayer.Name);

                bool hasLeaderboardEntry = ZeepkistNetwork.Leaderboard != null && ZeepkistNetwork.Leaderboard.Any(entry => entry.SteamID == player.SteamID);
                
                if (hasLeaderboardEntry)
                {
                    ZeepkistNetworking.LeaderboardItem leaderboardEntry = ZeepkistNetwork.Leaderboard.FirstOrDefault(entry => entry.SteamID == player.SteamID);
                    Debug.Log("LeaderboardEntry: " + leaderboardEntry.Time);
                    
                    RoomServiceResult playerResult = results[level.UID].FirstOrDefault(r => r.SteamID == player.SteamID);
                    if (playerResult != null)
                    {
                        if(leaderboardEntry.Time < playerResult.Time)
                        {
                            playerResult.Time = leaderboardEntry.Time;
                            RoomService.OnPlayerImproved?.Invoke(playerResult);
                        }
                    }     
                    else
                    {
                        RoomServiceResult newResult = new RoomServiceResult() { SteamID = player.SteamID, UID = level.UID, Time = leaderboardEntry.Time };
                        results[level.UID].Add(newResult);
                        RoomService.OnPlayerFinished?.Invoke(newResult);
                    }
                }             
            }
        }

        //Used when a player joins the lobby.
        public RoomServicePlayer AddPlayer(ZeepkistNetworkPlayer player)
        {
            if (!players.ContainsKey(player.SteamID))
            {
                RoomServicePlayer newPlayer = new RoomServicePlayer(player);
                players.Add(player.SteamID, newPlayer);
                return newPlayer;
            }
            else
            {
                return players[player.SteamID];
            }
        }

        //Returns the player if it exists.
        public RoomServicePlayer GetPlayer(ulong steamID)
        {
            if (players.ContainsKey(steamID))
            {
                return players[steamID];
            }

            return null;
        }

        //Returns the level if it exists.
        public RoomServiceLevel GetLevel(string UID)
        {
            if (levels.ContainsKey(UID))
            {
                return levels[UID];
            }

            return null;
        }

        //Get the level currently being played, used for creating the context.
        public RoomServiceLevel GetCurrentLevel()
        {
            LevelScriptableObject level = ZeepSDK.Level.LevelApi.CurrentLevel;
            if (level == null)
            {
                return null;
            }

            return new RoomServiceLevel(level);
        }

        //Set all players offline or online.
        public void SetAllPlayersNetworkState(bool isOnline)
        {
            foreach (RoomServicePlayer rsPlayer in players.Values)
            {
                rsPlayer.IsOnline = isOnline;
            }
        }

        public void SetPlayerPoints(ulong steamID, int points, int change)
        {
            RoomServicePlayer rsPlayer = GetPlayer(steamID);
            if (rsPlayer != null)
            {
                rsPlayer.Points = points;
                rsPlayer.PointsDifference = change;
            }
        }

        //Remove the points from all players.
        public void ResetAllPlayersPoints()
        {
            foreach (RoomServicePlayer rsPlayer in players.Values)
            {
                rsPlayer.Points = 0;
                rsPlayer.PointsDifference = 0;
            }
        }       

        //Clear everything
        public void ClearEverything()
        {
            players.Clear();
            levels.Clear();
            ClearResults();
        }

        public void RemovePlayersTime(string uid, ulong steamID)
        {
            if(results.ContainsKey(uid))
            {
                int indexToRemove = results[uid].FindIndex(r => r.SteamID == steamID);
                Debug.Log("Index to remove: " + indexToRemove);
                if (indexToRemove >= 0)
                {
                    Debug.Log("Removing players time ! ");
                    results[uid].RemoveAt(indexToRemove);
                }
            }
        }

        //Clear the current results.
        public void ClearResults()
        {
            foreach (var list in results.Values)
            {
                list.Clear();
            }
        }

        //Print the results to the console. (Will be replaced by writing to file with timestamp and using tokens for naming).
        public void PrintResults()
        {
            List<string> resultLevels = results.Keys.ToList();
            foreach (string levelKey in resultLevels)
            {
                RoomServiceLevel level = GetLevel(levelKey);
                string levelName = level != null ? level.Name : levelKey;
                Debug.LogWarning(levelName);

                results[levelKey].Sort((a, b) => a.Time.CompareTo(b.Time));

                foreach (RoomServiceResult result in results[levelKey])
                {
                    RoomServicePlayer player = GetPlayer(result.SteamID);
                    string playerName = player != null ? player.Name : result.SteamID.ToString();
                    Debug.LogWarning($"{playerName}: {result.Time}");
                }
            }           
        }
    }
}

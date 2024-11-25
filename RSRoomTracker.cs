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
        public Dictionary<ulong, RSPlayer> players;
        //List with all the levels encountered.
        public Dictionary<string, RSLevel> levels;
        //A dictionary with a list of best times results per level.
        public Dictionary<RSLevel, List<RSResult>> results;

        public RSRoomTracker()
        {
            players = new Dictionary<ulong, RSPlayer>();
            levels = new Dictionary<string, RSLevel>();
            results = new Dictionary<RSLevel, List<RSResult>>();
        }

        //This is called when:...
        //...we enter a lobby, to get the current state.
        //...a new map is started, to get a good initial state.
        //...anytime there is a leaderboard update to make sure we have the latest information.
        public void ProcessRoomState(List<ZeepkistNetworkPlayer> playerList, LevelScriptableObject level)
        {
            if (level == null)
            {
                return;
            }

            //Add the level if its not in the list yet, and also add an entry to the results for that level.
            if (!levels.ContainsKey(level.UID))
            {
                levels.Add(level.UID, new RSLevel() { Author = level.Author, Name = level.Name, UID = level.UID, WorkshopID = level.WorkshopID });
                results.Add(levels[level.UID], new List<RSResult>());
            }

            //Get the list of results for this level.
            List<RSResult> levelResults = results[levels[level.UID]];

            //Go over all the players in the lobby.
            foreach (ZeepkistNetworkPlayer player in playerList)
            {
                //If the player isnt know yet, add it to the players list.
                if (!players.ContainsKey(player.SteamID))
                {
                    players.Add(player.SteamID, new RSPlayer() { SteamID = player.SteamID, IsOnline = true, Name = player.GetUserNameNoTag() });
                }

                //If the player is in this list, its online.
                SetPlayerNetworkState(player.SteamID, true);

                //Get the player from the leaderboard
                bool hasEntry = ZeepkistNetwork.Leaderboard.Any(entry => entry.SteamID == player.SteamID);
                ZeepkistNetworking.LeaderboardItem leaderboardEntry = ZeepkistNetwork.Leaderboard.FirstOrDefault(entry => entry.SteamID == player.SteamID);

                //Only players with a time have an entry
                if (hasEntry)
                {
                    //Create a new result with the current leaderboard time.
                    RSResult newResult = new RSResult() { SteamID = player.SteamID, Time = leaderboardEntry.Time, UID = level.UID };
                    //Get the existing result from the list.
                    RSResult existingResult = levelResults.FirstOrDefault(r => r.SteamID == player.SteamID);
                    bool resultExists = levelResults.Any(r => r.SteamID == player.SteamID);

                    //If a result is found, compare it. Call OnPlayerImproved if its a better time.
                    if (resultExists)
                    {
                        if (newResult.Time < existingResult.Time)
                        {
                            levelResults.Remove(existingResult);
                            levelResults.Add(newResult);
                            RoomService.OnPlayerImproved?.Invoke(newResult);
                        }
                    }
                    //A result wasnt found so this is the first time the player finished.
                    else
                    {
                        levelResults.Add(newResult);
                        RoomService.OnPlayerFinished?.Invoke(newResult);
                    }
                }
            }
        }

        //Set the player offline or online.
        public void SetPlayerNetworkState(ulong steamID, bool isOnline)
        {
            if(players.ContainsKey(steamID))
            {
                RSPlayer player = players[steamID];
                player.IsOnline = isOnline;
                players[steamID] = player;
            }
        }

        //Set all players offline or online.
        public void SetAllPlayersNetworkState(bool isOnline)
        {
            List<ulong> ids = players.Keys.ToList();
            foreach (ulong steamID in ids)
            {
                SetPlayerNetworkState(steamID, isOnline);
            }
        }

        //Remove the points from all players.
        public void ResetAllPlayersPoints()
        {
            List<ulong> ids = players.Keys.ToList();
            foreach (ulong steamID in ids)
            {
                SetPlayerPoints(steamID, 0, 0);
            }
        }

        //Set the points for a specific player.
        public void SetPlayerPoints(ulong steamID, int points, int change)
        {
            if (players.ContainsKey(steamID))
            {
                RSPlayer p = players[steamID];
                p.Points = points;
                p.PointsDifference = change;
                players[steamID] = p;
            }
        }

        //Used when a player joins the lobby.
        public void AddPlayer(ZeepkistNetworkPlayer player)
        {            
            if (!players.ContainsKey(player.SteamID))
            {
                players.Add(player.SteamID, new RSPlayer() { SteamID = player.SteamID, IsOnline = true, Name = player.GetUserNameNoTag() });
            }
        }

        //Returns the player if it exists.
        public RSPlayer? GetPlayer(ulong steamID)
        {
            if(players.ContainsKey(steamID))
            {
                return players[steamID];
            }

            return null;
        }

        //Returns the level if it exists.
        public RSLevel? GetLevel(string UID)
        {
            if (levels.ContainsKey(UID))
            {
                return levels[UID];
            }

            return null;
        }

        //Get the level currently being played, used for creating the context.
        public RSLevel? GetCurrentLevel()
        {
            LevelScriptableObject level = ZeepSDK.Level.LevelApi.CurrentLevel;
            if(level == null)
            {
                return null;
            }

            RSLevel rsLevel = new RSLevel()
            {
                Author = level.Author,
                Name = level.Name,
                UID = level.UID,
                WorkshopID = level.WorkshopID
            };

            return rsLevel;
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
            bool levelExists = results.Keys.Any(l => l.UID == uid);
            RSLevel level = results.Keys.FirstOrDefault(l => l.UID == uid);

            Debug.LogWarning("Removing players time");
            Debug.LogWarning("Level exists? " + levelExists);

            if (levelExists)
            {
                List<RSResult> levelResults = results[level];

                Debug.LogWarning("Get results");

                // Get the index of the result to remove
                int indexToRemove = levelResults.FindIndex(r => r.SteamID == steamID);

                if (indexToRemove >= 0)
                {
                    levelResults.RemoveAt(indexToRemove); // Remove the result at the specified index
                    Debug.Log($"Successfully removed player's time for SteamID {steamID} from level {uid}.");
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
            foreach (KeyValuePair<RSLevel, List<RSResult>> levelResult in results)
            {
                Debug.LogWarning("Level: " + levelResult.Key.Name);
                levelResult.Value.Sort((a, b) => a.Time.CompareTo(b.Time));

                foreach (RSResult result in levelResult.Value)
                {
                    Debug.LogWarning($"{players[result.SteamID].Name}: {result.Time}");
                }
            }
        }
    }
}

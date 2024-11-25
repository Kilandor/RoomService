using Steamworks;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    public class RSRoomTracker
    {
        public Dictionary<ulong, RSPlayer> players;
        public Dictionary<string, RSLevel> levels;
        public Dictionary<RSLevel, List<RSResult>> results;

        public RSRoomTracker()
        {
            players = new Dictionary<ulong, RSPlayer>();
            levels = new Dictionary<string, RSLevel>();
            results = new Dictionary<RSLevel, List<RSResult>>();
        }

        public void ProcessRoomState(List<ZeepkistNetworkPlayer> playerList, LevelScriptableObject level)
        {
            if (level == null)
            {
                return;
            }

            if (!levels.ContainsKey(level.UID))
            {
                levels.Add(level.UID, new RSLevel() { Author = level.Author, Name = level.Name, UID = level.UID, WorkshopID = level.WorkshopID });

                results.Add(levels[level.UID], new List<RSResult>());
            }

            List<RSResult> levelResults = results[levels[level.UID]];

            foreach (ZeepkistNetworkPlayer player in playerList)
            {
                if (!players.ContainsKey(player.SteamID))
                {
                    players.Add(player.SteamID, new RSPlayer() { SteamID = player.SteamID, IsOnline = true, Name = player.Username });
                }

                RSPlayer rsPlayer = players[player.SteamID];
                rsPlayer.IsOnline = true;
                players[player.SteamID] = rsPlayer;

                if (player.CurrentResult != null)
                {
                    RSResult newResult = new RSResult() { SteamID = player.SteamID, Time = player.CurrentResult.Time, UID = level.UID };
                    RSResult existingResult = levelResults.FirstOrDefault(r => r.SteamID == player.SteamID);
                    bool resultExists = levelResults.Any(r => r.SteamID == player.SteamID);

                    if (resultExists)
                    {
                        if (newResult.Time < existingResult.Time)
                        {
                            levelResults.Remove(existingResult);
                            levelResults.Add(newResult);
                            RoomService.OnPlayerImproved?.Invoke(newResult);
                        }
                    }
                    else
                    {
                        levelResults.Add(newResult);
                        RoomService.OnPlayerFinished?.Invoke(newResult);
                    }
                }
            }
        }

        public void SetAllPlayersOffline()
        {
            List<ulong> ids = players.Keys.ToList();
            foreach (ulong steamID in ids)
            {
                SetPlayerOffline(steamID);
            }
        }

        public void ResetAllPoints()
        {
            List<ulong> ids = players.Keys.ToList();
            foreach (ulong steamID in ids)
            {
                SetPlayerPoints(steamID, 0, 0);
            }
        }

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

        public void SetPlayerOffline(ulong steamID)
        {
            if (players.ContainsKey(steamID))
            {
                RSPlayer offline = players[steamID];
                offline.IsOnline = false;
                players[steamID] = offline;
            }
        }

        public void AddPlayer(ZeepkistNetworkPlayer player)
        {            
            if (!players.ContainsKey(player.SteamID))
            {
                Debug.Log("AddPlayer: " + player.SteamID + "," + player.Username);
                players.Add(player.SteamID, new RSPlayer() { SteamID = player.SteamID, IsOnline = true, Name = player.Username });
            }
        }

        public RSPlayer? GetPlayer(ulong steamID)
        {
            if(players.ContainsKey(steamID))
            {
                return players[steamID];
            }

            return null;
        }

        public RSLevel? GetLevel(string UID)
        {
            if (levels.ContainsKey(UID))
            {
                return levels[UID];
            }

            return null;
        }

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

        public void Clear()
        {
            players.Clear();
            levels.Clear();
            results.Clear();
        }

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

using BepInEx;
using HarmonyLib;
using System;
using ZeepSDK.Scripting;
using ZeepkistClient;
using ZeepkistNetworking;
using System.Collections.Generic;
using System.Linq;
using BepInEx.Configuration;
using BepInEx.Logging;

namespace RoomService
{
    public struct LevelInfo
    {
        public string Hash;
        public string Author;
        public string Name;
        public string Uid;
        public string WorkshopId;
    }

    public class PlayerTime
    {
        public ulong SteamID;
        public string Name;
        public string Tag;
        public string FullName;
        public float Time;
        public float BestTime;
        public string ChatColor;
    }

    [BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
    [BepInDependency("ZeepSDK", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public static Plugin Instance;
        
        public static ManualLogSource baseLogger;
        
        public ConfigEntry<bool> debugEnabled;

        public Action<int> LobbyTimerAction;
        public Action LeaderBoardUpdated;
        public Action<PlayerTime> PlayerSetTime;

        public Dictionary<string, LevelInfo> levelInfo = new Dictionary<string, LevelInfo>();
        public Dictionary<string, List<PlayerTime>> playerTimes = new Dictionary<string, List<PlayerTime>>();

        public void ProcessLeaderboardUpdate(ZeepkistNetworkPlayer updatedPlayer)
        {            
            try
            {
                //Get the current level
                LevelScriptableObject level = ZeepSDK.Level.LevelApi.CurrentLevel;
                if (level == null)
                {
                    return;
                }

                string hash = ZeepSDK.Level.LevelApi.CurrentHash;
                bool firstRun = false;

                //Check if the level exists in the dictionary
                if (!levelInfo.ContainsKey(hash))
                {
                    levelInfo.Add(hash, new LevelInfo() { Hash = hash, Author = level.Author, Name = level.Name, Uid = level.UID });
                    playerTimes.Add(hash, new List<PlayerTime>());
                    firstRun = true; //Used incase this was loaded mid track, to build the full initial data
                    Utilities.Log("[Leaderboard] New level hash", Utilities.LogLevel.Debug);
                }

                if (ZeepkistNetwork.PlayerList == null)
                {
                    Utilities.Log("[Leaderboard] Playerlist is null?", Utilities.LogLevel.Debug);
                    return;
                }

                //Go over all the players
                foreach (ZeepkistNetworkPlayer player in ZeepkistNetwork.PlayerList)
                {
                    //Get or create the PlayerTime for this player
                    int playerIndex = playerTimes[hash].FindIndex(p => p.SteamID == player.SteamID);
                    PlayerTime playerTime;

                    //There is not yet a player time available for this player.
                    if (playerIndex < 0)
                    {
                        //Create the player time
                        playerTime = new PlayerTime()
                        {
                            Name = player.GetUserNameNoTag(),
                            Tag = player.GetUserTag(),
                            FullName = player.GetTaggedUsername(),
                            SteamID = player.SteamID,
                            Time = -1,
                            BestTime = -1,
                            ChatColor = RoomServiceUtils.ColorToHex(player.chatColor)                            
                        };
                        Utilities.Log($"[Leaderboard] Player {playerTime.Name} added", Utilities.LogLevel.Debug);

                        playerTimes[hash].Add(playerTime);
                    }
                    else
                    {
                        playerTime = playerTimes[hash][playerIndex];
                    }

                    float settime = -1;

                    if(player.SteamID == updatedPlayer.SteamID && updatedPlayer.CurrentResult != null)
                        settime =  updatedPlayer.CurrentResult.Time;
                    else if(firstRun)
                    {
                        LeaderboardItem lbItem = ZeepkistNetwork.GetLeaderboardEntry(player.SteamID);
                        if(lbItem.Username != "")
                        {
                            Utilities.Log($"[Leaderboard] {playerTime.Name} - Time got from leaderboard ", Utilities.LogLevel.Debug);
                            settime = lbItem.Time;   
                        }
                    }
                    else
                        continue;
                    

                    if (Plugin.Instance.debugEnabled.Value)
                    {
                        if (player.SteamID != updatedPlayer.SteamID)
                            updatedPlayer = player;
                        if (updatedPlayer.CurrentResult == null)
                        {
                            Utilities.Log(
                                $"[Leaderboard] {playerTime.Name} - Current Result null",
                                Utilities.LogLevel.Debug);
                        }
                        else
                        {
                            LeaderboardItem lbItem = ZeepkistNetwork.GetLeaderboardEntry(player.SteamID);
                            if (lbItem.Username != "")
                            {
                                if (updatedPlayer.CurrentResult.Time != lbItem.Time)
                                {
                                    Utilities.Log(
                                        $"[Leaderboard] {playerTime.Name} - ZN {updatedPlayer.CurrentResult.Time} | Leaderboard  {lbItem.Time}",
                                        Utilities.LogLevel.Error);
                                }
                                else
                                {
                                    Utilities.Log(
                                        $"[Leaderboard] {playerTime.Name} - ZN/Leaderboard times match",
                                        Utilities.LogLevel.Debug);
                                }
                            }   
                        }
                    }
                    
                    Utilities.Log($"[Leaderboard] {playerTime.Name} - Current Time {settime} ", Utilities.LogLevel.Debug);
                    
                    //Player has a result
                    if (settime != -1)
                    {
                        //The time is different from the registered time.
                        if (settime != playerTime.Time)
                        {
                            //Player updated their time                            
                            playerTime.Time = settime;
                            Utilities.Log($"[Leaderboard] {playerTime.Name} - Set {settime} | Previous {playerTime.Time} | Best {playerTime.BestTime}", Utilities.LogLevel.Debug);

                            if(playerTime.Time < playerTime.BestTime || playerTime.BestTime == -1)
                            {
                                playerTime.BestTime = playerTime.Time;
                                Utilities.Log($"[Leaderboard] {playerTime.Name} - New Best time {playerTime.BestTime}", Utilities.LogLevel.Debug);
                            }

                            PlayerSetTime?.Invoke(playerTime);
                        }
                    }
                    //Player doesnt have a result, either removed or not set yet
                    else
                    {
                        //Make sure -1 is set in the player time.
                        if (playerTime.Time != -1)
                        {
                            playerTime.Time = -1;
                        }
                        Utilities.Log($"[Leaderboard] {playerTime.Name} - No result -1", Utilities.LogLevel.Debug);
                    }
                }

                //Sort the current level on the times
                playerTimes[hash] = playerTimes[hash]
                    .OrderBy(player => player.Time == -1)  // Move all entries with time -1 to the end
                    .ThenBy(player => player.Time)        // Sort by time for the remaining entries
                    .ToList();

            }
            catch (Exception ex)
            {
                Utilities.Log(ex.Message, Utilities.LogLevel.Error);
            }
        }

        private void Awake()
        {
            Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Instance = this;
            baseLogger = Logger;

            //Register all required types.
            ScriptingApi.RegisterType<ZeepkistNetworking.LeaderboardItem>();
            ScriptingApi.RegisterType<ZeepkistNetworking.LeaderboardOverrideItem>();
            ScriptingApi.RegisterType<PlayerTime>();

            //Register all events.
            ScriptingApi.RegisterEvent<OnLobbyTimerEvent>();
            ScriptingApi.RegisterEvent<OnPlayerSetTimeEvent>();
            ScriptingApi.RegisterEvent<OnLeaderboardChangeEvent>();

            //Communication functions
            ScriptingApi.RegisterFunction<SendChatMessage>();
            ScriptingApi.RegisterFunction<SendPrivateChatMessage>();
            ScriptingApi.RegisterFunction<ShowScreenMessage>();

            //Lobby functions
            ScriptingApi.RegisterFunction<SetPointsDistribution>();
            ScriptingApi.RegisterFunction<ResetPointsDistribution>();
            ScriptingApi.RegisterFunction<ResetChampionshipPoints>();
            ScriptingApi.RegisterFunction<SetVoteskip>();
            ScriptingApi.RegisterFunction<SetVoteskipPercentage>();
            ScriptingApi.RegisterFunction<SetLobbyName>();
            ScriptingApi.RegisterFunction<SetServerMessage>();
            ScriptingApi.RegisterFunction<RemoveServerMessage>();
            ScriptingApi.RegisterFunction<SetRoundLength>();
            ScriptingApi.RegisterFunction<SetSmallLeaderboardSortingMethod>();
            ScriptingApi.RegisterFunction<BlockEveryoneFromSettingTime>();
            ScriptingApi.RegisterFunction<UnblockEveryoneFromSettingTime>();
            ScriptingApi.RegisterFunction<ResetAllPlayers>();
            ScriptingApi.RegisterFunction<ResetPlayers>();

            //Player functions
            ScriptingApi.RegisterFunction<SetPlayerTimeOnLeaderboard>();
            ScriptingApi.RegisterFunction<SetPlayerLeaderboardOverrides>();
            ScriptingApi.RegisterFunction<RemovePlayerFromLeaderboard>();
            ScriptingApi.RegisterFunction<SetPlayerChampionshipPoints>();
            ScriptingApi.RegisterFunction<UnblockPlayerFromSettingTime>();
            ScriptingApi.RegisterFunction<BlockPlayerFromSettingTime>();

            //Getter functions
            ScriptingApi.RegisterFunction<GetPlayerCount>();
            ScriptingApi.RegisterFunction<GetPlaylistIndex>();
            ScriptingApi.RegisterFunction<GetPlaylistLength>();
            ScriptingApi.RegisterFunction<GetPlayer>();
            ScriptingApi.RegisterFunction<GetPlayerBySteamID>();
            ScriptingApi.RegisterFunction<GetAllPlayers>();

            //Leaderboard
            ScriptingApi.RegisterFunction<GetLeaderboardEntry>();
            ScriptingApi.RegisterFunction<GetLeaderboardOverride>();
            ScriptingApi.RegisterFunction<GetLeaderboard>();

            //Logger
            ScriptingApi.RegisterFunction<ClearLogger>();
            ScriptingApi.RegisterFunction<SaveLogger>();
            ScriptingApi.RegisterFunction<PrintLogger>();

            //Helper functions
            ScriptingApi.RegisterFunction<GetCurrentDate>();
            ScriptingApi.RegisterFunction<GetCurrentTime>();
            ScriptingApi.RegisterFunction<GenerateRandomNumber>();
            ScriptingApi.RegisterFunction<SecondsToTime>();
            
            debugEnabled = Config.Bind("9. Dev / Debug", "Debug Logs", false, "Provides extra output in logs for troubleshooting.");

            Logger.LogInfo("Roomservice is loaded! At your service!");
        }  

        public string[] GetLoggerLines()
        {
            List<string> lines = new List<string>();

            foreach(KeyValuePair<string, LevelInfo> info in levelInfo)
            {
                lines.Add(info.Value.Name);
                lines.Add("SteamID;Name;Time;BestTime");

                foreach(PlayerTime t in playerTimes[info.Key])
                {
                    lines.Add($"{t.SteamID};{t.FullName};{t.Time};{t.BestTime}");
                }

                lines.Add("");
            }

            return lines.ToArray();
        }        
    }
}
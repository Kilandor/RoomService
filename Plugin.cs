using BepInEx;
using HarmonyLib;
using System;
using ZeepSDK.Scripting;
using MoonSharp.Interpreter;
using ZeepkistClient;
using ZeepkistNetworking;
using System.Collections.Generic;
using System.Linq;

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

        public Action<int> LobbyTimerAction;
        public Action LeaderBoardUpdated;
        public Action<PlayerTime> PlayerSetTime;

        public Dictionary<string, LevelInfo> levelInfo = new Dictionary<string, LevelInfo>();
        public Dictionary<string, List<PlayerTime>> playerTimes = new Dictionary<string, List<PlayerTime>>();

        public void ProcessLeaderboardUpdate()
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

                //Check if the level exists in the dictionary
                if (!levelInfo.ContainsKey(hash))
                {
                    levelInfo.Add(hash, new LevelInfo() { Hash = hash, Author = level.Author, Name = level.Name, Uid = level.UID });
                    playerTimes.Add(hash, new List<PlayerTime>());
                }

                if (ZeepkistNetwork.PlayerList == null)
                {
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

                        playerTimes[hash].Add(playerTime);
                    }
                    else
                    {
                        playerTime = playerTimes[hash][playerIndex];
                    }

                    float settime = -1;

                    if(player.CurrentResult != null)
                    {
                        settime = player.CurrentResult.Time;
                    }
                    else
                    {
                        LeaderboardItem lbItem = ZeepkistNetwork.GetLeaderboardEntry(player.SteamID);
                        if(lbItem.Username != "")
                        {
                            settime = lbItem.Time;   
                        }
                    }
                    
                    //Player has a result
                    if (settime != -1)
                    {
                        //The time is different from the registered time.
                        if (settime != playerTime.Time)
                        {
                            //Player updated their time                            
                            playerTime.Time = settime;

                            if(playerTime.Time < playerTime.BestTime || playerTime.BestTime == -1)
                            {
                                playerTime.BestTime = playerTime.Time;
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
                Logger.LogWarning(ex.Message);
            }
        }

        private void Awake()
        {
            Harmony harmony = new Harmony(MyPluginInfo.PLUGIN_GUID);
            harmony.PatchAll();

            Instance = this;   

            //Register all required types.
            ScriptingApi.RegisterType<ZeepkistNetworking.LeaderboardItem>();
            ScriptingApi.RegisterType<ZeepkistNetworking.LeaderboardOverrideItem>();
            ScriptingApi.RegisterType<PlayerTime>();

            //Register all events.
            ScriptingApi.RegisterEvent<OnLobbyTimerEvent>();
            ScriptingApi.RegisterEvent<OnPlayerSetTimeEvent>();
            ScriptingApi.RegisterEvent<OnLeaderboardChangeEvent>();

            //Communication functions
            ScriptingApi.RegisterFunction<SendChatMessageFunction>();
            ScriptingApi.RegisterFunction<SendPrivateChatMessageFunction>();
            ScriptingApi.RegisterFunction<ShowScreenMessageFunction>();

            //Lobby functions
            ScriptingApi.RegisterFunction<SetPointsDistributionFunction>();
            ScriptingApi.RegisterFunction<ResetPointsDistributionFunction>();
            ScriptingApi.RegisterFunction<ResetChampionshipPointsFunction>();
            ScriptingApi.RegisterFunction<SetVoteskipFunction>();
            ScriptingApi.RegisterFunction<SetVoteskipPercentageFunction>();
            ScriptingApi.RegisterFunction<SetLobbyNameFunction>();
            ScriptingApi.RegisterFunction<SetServerMessageFunction>();
            ScriptingApi.RegisterFunction<RemoveServerMessageFunction>();
            ScriptingApi.RegisterFunction<SetRoundLengthFunction>();
            ScriptingApi.RegisterFunction<SetSmallLeaderboardSortingMethodFunction>();
            ScriptingApi.RegisterFunction<BlockEveryoneFromSettingTimeFunction>();
            ScriptingApi.RegisterFunction<UnblockEveryoneFromSettingTimeFunction>();

            //Player functions
            ScriptingApi.RegisterFunction<SetPlayerTimeOnLeaderboardFunction>();
            ScriptingApi.RegisterFunction<SetPlayerLeaderboardOverridesFunction>();
            ScriptingApi.RegisterFunction<RemovePlayerFromLeaderboardFunction>();
            ScriptingApi.RegisterFunction<SetPlayerChampionshipPointsFunction>();
            ScriptingApi.RegisterFunction<UnblockPlayerFromSettingTimeFunction>();
            ScriptingApi.RegisterFunction<BlockPlayerFromSettingTimeFunction>();

            //Getter functions
            ScriptingApi.RegisterFunction<GetPlayerCountFunction>();
            ScriptingApi.RegisterFunction<GetPlaylistIndexFunction>();
            ScriptingApi.RegisterFunction<GetPlaylistLengthFunction>();

            //Leaderboard
            ScriptingApi.RegisterFunction<GetLeaderboardEntryFunction>();
            ScriptingApi.RegisterFunction<GetLeaderboardOverrideFunction>();
            ScriptingApi.RegisterFunction<GetLeaderboardFunction>();

            //Logger
            ScriptingApi.RegisterFunction<ClearLoggerFunction>();
            ScriptingApi.RegisterFunction<SaveLoggerFunction>();
            ScriptingApi.RegisterFunction<PrintLoggerFunction>();

            //Helper functions
            ScriptingApi.RegisterFunction<GetCurrentDateFunction>();
            ScriptingApi.RegisterFunction<GetCurrentTimeFunction>();
            ScriptingApi.RegisterFunction<GenerateRandomNumberFunction>();
            ScriptingApi.RegisterFunction<SecondsToTimeFunction>();

            Logger.LogInfo("Roomservice is loaded! At your service!");
        }  

        public void Log(string msg)
        {
            Logger.LogInfo(msg);
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
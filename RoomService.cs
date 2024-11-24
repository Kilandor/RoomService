using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    public static class RoomService
    {
        private static readonly Dictionary<string, Action<List<string>, RSContext>> FunctionMap = new Dictionary<string, Action<List<string>, RSContext>>()
        {
            { "SendGlobalChatMessage", (parameters, context) => {

                if(parameters.Count >= 2)
                {
                    string prefix = context.ReplaceParameters(parameters[0]);
                    string message = context.ReplaceParameters(parameters[1]);

                    SendGlobalChatMessage(prefix, message);
                }
            }}
        };

        public static RSRoomTracker tracker;

        public static Action<RSResult> OnPlayerFinished;
        public static Action<RSResult> OnPlayerImproved;
        public static Action OnRoundStart;
        public static Action OnRoundEnd;
        public static Action<RSPlayer> OnPlayerJoined;
        public static Action<RSPlayer> OnPlayerLeft;
        public static Action OnConfigLoad;
        private static Dictionary<string, string> customParameters;

        public static void Initialize()
        {
            ZeepkistNetwork.LeaderboardUpdated += LeaderboardUpdate;
            ZeepSDK.Multiplayer.MultiplayerApi.PlayerJoined += PlayerJoined;
            ZeepSDK.Multiplayer.MultiplayerApi.PlayerLeft += PlayerLeft;
            ZeepSDK.Racing.RacingApi.RoundStarted += RoundStarted;
            ZeepSDK.Racing.RacingApi.RoundEnded += RoundEnded;
            ZeepSDK.Multiplayer.MultiplayerApi.JoinedRoom += JoinedRoom;
            ZeepSDK.Multiplayer.MultiplayerApi.DisconnectedFromGame += LeftRoom;

            tracker = new RSRoomTracker();
            customParameters = new Dictionary<string, string>();
        }

        public static void LoadRoomServiceConfig(RoomServiceConfig config)
        {
            Debug.LogWarning("Loading config!");
            ClearTracker();

            //Clear all the previously subscribed events.
            ClearSubscriptions();

            //Clear the custom parameters and read the new ones.
            customParameters.Clear();

            foreach(string p in config.Parameters)
            {
                string[] parts = p.Split(";");
                if(parts.Length > 1)
                {
                    if(!customParameters.ContainsKey(parts[0]))
                    {
                        customParameters.Add(parts[0], parts[1]);
                    }                    
                }
            }

            ProcessEventList("OnPlayerFinished", config.OnPlayerImproved);
            ProcessEventList("OnPlayerImproved", config.OnPlayerImproved);
        }

        private static void ProcessEventList(string eventName, List<string> eventList)
        {
            if(eventList == null || eventList.Count == 0)
            {
                return;
            }

            foreach(string command in eventList)
            {
                string functionName = ExtractFunctionName(command);
                List<string> parameters = ExtractParameters(command);
                SubscribeToEvent(eventName, functionName, parameters);
            }
        }

        private static void ClearSubscriptions()
        {
            Debug.LogWarning("Clear Subscriptions");
            OnPlayerFinished = null;
            OnPlayerImproved = null;
            OnRoundStart = null;
            OnPlayerJoined = null;
            OnPlayerLeft = null;
        }

        private static void SubscribeToEvent(string eventName, string functionName, List<string> parameters)
        {
            switch(eventName)
            {
                case "OnLoad":
                    OnConfigLoad += () =>
                    {

                    };
                    break;
                case "OnPlayerJoined":
                    OnPlayerJoined += (player) =>
                    {

                    };
                    break;
                case "OnPlayerLeft":
                    OnPlayerLeft += (player) =>
                    {

                    };
                    break;
                case "OnRoundStart":
                    OnRoundStart += () =>
                    {

                    };
                    break;
                case "OnRoundEnd":
                    OnRoundEnd += () =>
                    {

                    };
                    break;
                case "OnPlayerFinished":
                    OnPlayerFinished += (result) =>
                    {
                        RSContext context = CreateContext(result: (RSResult?)result);
                        FunctionMap[functionName]?.Invoke(parameters, context);
                    };
                    break;
                case "OnPlayerImproved":
                    OnPlayerImproved += (result) =>
                    {
                        RSContext context = CreateContext(result:(RSResult?)result);                      
                        FunctionMap[functionName]?.Invoke(parameters, context);
                    };
                    break;
            }
        }

        private static RSContext CreateContext(RSPlayer? player = null, RSLevel? level = null, RSResult? result = null)
        {
            RSContext ctx = new RSContext(customParameters);

            if(player.HasValue)
            {
                ctx.AddPlayer(player.Value);
            }

            if(level.HasValue)
            {
                ctx.AddLevel(level.Value);
            }

            if(result.HasValue)
            {
                ctx.AddResult(result.Value);

                RSPlayer? p = tracker.GetPlayer(result.Value.SteamID);
                if(p.HasValue)
                {
                    ctx.AddPlayer(p.Value);
                }

                RSLevel? l = tracker.GetLevel(result.Value.UID);
                if(l.HasValue)
                {
                    ctx.AddLevel(l.Value);
                }
            }

            return ctx;
        }
        public struct RSContext
        {
            //Player
            public ulong SteamID;
            public string PlayerName;

            //Result
            public float Time;

            //Level
            public string UID;
            public ulong WorkshopID;
            public string LevelName;
            public string Author;

            //Parameters
            public Dictionary<string,string> Parameters;

            public RSContext(Dictionary<string,string> parameters)
            {
                Parameters = parameters;
                SteamID = 0;
                PlayerName = "";
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

                foreach(KeyValuePair<string,string> cp in customParameters)
                {
                    newString = newString.Replace(cp.Key, cp.Value);
                }
                return newString;                
            }
        }

        private static string ExtractFunctionName(string command)
        {
            // Function name is the part before the first '('
            int openParenIndex = command.IndexOf('(');
            if (openParenIndex == -1)
                return command; // No parameters, function name is the entire string

            return command.Substring(0, openParenIndex);
        }

        private static List<string> ExtractParameters(string command)
        {
            // Parameters are between '(' and ')'
            int openParenIndex = command.IndexOf('(');
            int closeParenIndex = command.LastIndexOf(')');
            if (openParenIndex == -1 || closeParenIndex == -1 || closeParenIndex < openParenIndex)
                return new List<string>(); // No parameters

            string paramString = command.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1);

            // Split by ';' while handling nested parameters (e.g., "[1,2]")
            var parameters = new List<string>();
            int nestedLevel = 0;
            string currentParam = "";

            foreach (char c in paramString)
            {
                if (c == ';' && nestedLevel == 0)
                {
                    parameters.Add(currentParam.Trim());
                    currentParam = "";
                }
                else
                {
                    if (c == '[') nestedLevel++;
                    if (c == ']') nestedLevel--;
                    currentParam += c;
                }
            }

            if (!string.IsNullOrEmpty(currentParam))
                parameters.Add(currentParam.Trim());

            return parameters;
        }

        public static void SendGlobalChatMessage(string prefix, string message)
        {
            ZeepkistNetwork.SendCustomChatMessage(true, 0, message, prefix);
        }

        public static void SendPlayerChatMessage(string prefix, string message, ulong steamID)
        {
            ZeepkistNetwork.SendCustomChatMessage(false, steamID, message, prefix);
        }

        public static void SetServerMessage(string message)
        {

        }

        public static void ClearTracker()
        {
            tracker.players.Clear();
            tracker.levels.Clear();
            tracker.results.Clear();
        }

        private static void JoinedRoom()
        {
            tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
        }

        private static void LeaderboardUpdate()
        {
            tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
        }

        private static void LeftRoom()
        {
            tracker.SetAllPlayersOffline();
        }

        private static void PlayerJoined(ZeepkistNetworkPlayer player)
        {
            tracker.AddPlayer(player);
            RSPlayer? rsPlayer = tracker.GetPlayer(player.SteamID);
            if (rsPlayer.HasValue)
            {
                OnPlayerJoined?.Invoke(rsPlayer.Value);
            }
        }

        private static void PlayerLeft(ZeepkistNetworkPlayer player)
        {
            tracker.SetPlayerOffline(player.SteamID);
            RSPlayer? rsPlayer = tracker.GetPlayer(player.SteamID);
            if(rsPlayer.HasValue)
            {
                OnPlayerLeft?.Invoke(rsPlayer.Value);
            }            
        }

        private static void RoundStarted()
        {
            tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
            OnRoundStart?.Invoke();
        }

        private static void RoundEnded()
        {
            OnRoundEnd?.Invoke();
        }
    }
}

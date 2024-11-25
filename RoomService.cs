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
        public static Action<RSResult> OnPlayerFinished;
        public static Action<RSResult> OnPlayerImproved;
        public static Action OnRoundStart;
        public static Action OnRoundEnd;
        public static Action<RSPlayer> OnPlayerJoined;
        public static Action<RSPlayer> OnPlayerLeft;
        public static Action OnConfigLoad;
        public static Action OnConfigUnload;

        public static RoomServiceConfig CurrentConfig;
        public static RSRoomTracker tracker;

        public static void Initialize()
        {
            tracker = new RSRoomTracker();

            ZeepkistNetwork.LeaderboardUpdated += () =>
            {
                tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
            };

            ZeepSDK.Multiplayer.MultiplayerApi.PlayerJoined += (player) =>
            {
                tracker.AddPlayer(player);
                RSPlayer? rsPlayer = tracker.GetPlayer(player.SteamID);
                if (rsPlayer.HasValue)
                {
                    OnPlayerJoined?.Invoke(rsPlayer.Value);
                }
            };

            ZeepSDK.Multiplayer.MultiplayerApi.PlayerLeft += (player) =>
            {
                tracker.SetPlayerOffline(player.SteamID);
                RSPlayer? rsPlayer = tracker.GetPlayer(player.SteamID);
                if (rsPlayer.HasValue)
                {
                    OnPlayerLeft?.Invoke(rsPlayer.Value);
                }
            };

            ZeepSDK.Racing.RacingApi.LevelLoaded += () =>
            {
                tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
                OnRoundStart?.Invoke();
            };

            ZeepSDK.Racing.RacingApi.RoundEnded += () =>
            {
                OnRoundEnd?.Invoke();
            };

            ZeepSDK.Multiplayer.MultiplayerApi.JoinedRoom += () =>
            {
                tracker.ProcessRoomState(ZeepkistNetwork.PlayerList, ZeepSDK.Level.LevelApi.CurrentLevel);
            };

            ZeepSDK.Multiplayer.MultiplayerApi.DisconnectedFromGame += () =>
            {
                tracker.SetAllPlayersOffline();
            };           
        }

        public static void LoadRoomServiceConfig(RoomServiceConfig config)
        {
            Debug.LogWarning("Loading config!");

            // Clear all the previously subscribed events.
            ClearSubscriptions();

            // This should be a command.
            tracker.Clear();

            // Save a reference to the config.
            CurrentConfig = config;

            config.Load();

            Debug.LogWarning("Calling Load Now");
            Debug.Log($"Current subscribers: {OnConfigLoad?.GetInvocationList()?.Length}");

            //All events have been processed, invoke the OnLoad
            OnConfigLoad?.Invoke();
        }

        public static void Unload()
        {
            if(CurrentConfig != null)
            {
                OnConfigUnload?.Invoke();
                CurrentConfig = null;
            }
        }

        public static void SubscribeToEvent(string eventName, string functionName, List<string> parameters)
        {
            Debug.LogWarning(eventName + "," + functionName + "," + string.Join(';', parameters));

            switch (eventName)
            {
                case "OnLoad":
                    Debug.Log("Subscribing to OnLoad");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnConfigLoad += () =>
                        {
                            RSContext context = CreateContext();
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };                       
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnUnload":
                    Debug.Log("Subscribing to OnUnload");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnConfigUnload += () =>
                        {
                            RSContext context = CreateContext();
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnPlayerJoined":
                    Debug.Log("Subscribing to OnPlayerJoined");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerJoined += player =>
                        {
                            Debug.LogWarning("Called on PlayerJoined with functionName: " + functionName);
                            Debug.LogWarning("Player that Joined: " + player.SteamID + "," + player.Name);

                            RSContext context = CreateContext(player:player);
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnPlayerLeft":
                    Debug.Log("Subscribing to OnPlayerLeft");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerLeft += (player) =>
                        {
                            RSContext context = CreateContext(player:player);
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnRoundStart":
                    Debug.Log("Subscribing to OnRoundStart");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnRoundStart += () =>
                        {
                            RSContext context = CreateContext();
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnRoundEnd":
                    Debug.Log("Subscribing to OnRoundEnd");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnRoundEnd += () =>
                        {
                            RSContext context = CreateContext();
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnPlayerFinished":
                    Debug.Log("Subscribing to OnPlayerFinished");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerFinished += (result) =>
                        {
                            Debug.Log("On player finished:" + result.SteamID);

                            RSContext context = CreateContext(result:result);

                            Debug.Log(context.ToString());

                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
                case "OnPlayerImproved":
                    Debug.Log("Subscribing to OnPlayerImproved");
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerImproved += (result) =>
                        {
                            RSContext context = CreateContext(result: result);
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogWarning("Unknown function name: " + functionName);
                    }
                    break;
            }
        }

        private static void ClearSubscriptions()
        {
            OnPlayerFinished = null;
            OnPlayerImproved = null;
            OnRoundStart = null;
            OnRoundEnd = null;
            OnPlayerJoined = null;
            OnPlayerLeft = null;
            OnConfigLoad = null;
            OnConfigUnload = null;
        }

        public static RSContext CreateContext(RSPlayer? player = null, RSLevel? level = null, RSResult? result = null)
        {
            RSContext ctx = new RSContext(CurrentConfig?.Parameters ?? new Dictionary<string, string>());
            
            if (result.HasValue)
            {
                ctx.AddResult(result.Value);

                RSPlayer? p = tracker.GetPlayer(result.Value.SteamID);
                if (p.HasValue)
                {
                    ctx.AddPlayer(p.Value);
                }

                RSLevel? l = tracker.GetLevel(result.Value.UID);
                if (l.HasValue)
                {
                    ctx.AddLevel(l.Value);
                }
                return ctx;
            }

            if (player.HasValue)
            {
                ctx.AddPlayer(player.Value);
            }

            if (level.HasValue)
            {
                ctx.AddLevel(level.Value);
            }
            else
            {
                RSLevel? l = tracker.GetCurrentLevel();
                if (l.HasValue)
                {
                    ctx.AddLevel(l.Value);
                }
            }

            return ctx;
        }
    }
}

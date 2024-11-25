using System;
using System.Collections.Generic;
using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    public static class RoomService
    {       
        //Called when a player finishes the track.
        public static Action<RSResult> OnPlayerFinished;
        //Called when a player has already finished and improves their time.
        public static Action<RSResult> OnPlayerImproved;
        //Called when the round starts (in the beginning).
        public static Action OnRoundStart;
        //Called when the round ends (to podium).
        public static Action OnRoundEnd;
        //Called when a player joins the game.
        public static Action<RSPlayer> OnPlayerJoined;
        //Called when a player leaves the game.
        public static Action<RSPlayer> OnPlayerLeft;
        //Called right after the config is loaded, to execute all the OnLoad functions.
        public static Action OnConfigLoad;
        //Called right after the config is unloaded, for clean up.
        public static Action OnConfigUnload;

        //The config that is currently active.
        public static RoomServiceConfig CurrentConfig;
        //The tracker keeps track of players, points and best times.
        public static RSRoomTracker tracker;

        //Initialize is called when the plugin starts.
        public static void Initialize()
        {
            //Create a new tracker.
            tracker = new RSRoomTracker();

            //Subscribe to all the events.
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
                tracker.SetPlayerNetworkState(player.SteamID, false);
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
                tracker.SetAllPlayersNetworkState(false);
            };           
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

        public static void LoadConfig(RoomServiceConfig config)
        {
            //Make sure all subscriptions to events are removed.
            ClearSubscriptions();
            // Save a reference to the config.
            CurrentConfig = config;
            //Load the config
            config.Load();
            //All events have been processed, invoke the OnLoad
            OnConfigLoad?.Invoke();
        }

        public static void UnloadConfig()
        {
            if(CurrentConfig != null)
            {
                OnConfigUnload?.Invoke();
                CurrentConfig = null;
            }

            ClearSubscriptions();
        }

        public static void SubscribeToEvent(string eventName, string functionName, List<string> parameters)
        {
            switch (eventName)
            {
                default:
                    Debug.LogError($"{eventName} is not a valid event name.");
                    break;
                case "OnLoad":
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
                        Debug.LogError($"Unknown function name in OnLoad event: {functionName}");
                    }
                    break;
                case "OnUnload":
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
                        Debug.LogError($"Unknown function name in OnUnload event: {functionName}");
                    }
                    break;
                case "OnPlayerJoined":
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerJoined += player =>
                        {
                            RSContext context = CreateContext(player:player);
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogError($"Unknown function name in OnPlayerJoined event: {functionName}");
                    }
                    break;
                case "OnPlayerLeft":
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
                        Debug.LogError($"Unknown function name in OnPlayerLeft event: {functionName}");
                    }
                    break;
                case "OnRoundStart":
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
                        Debug.LogError($"Unknown function name in OnRoundStart event: {functionName}");
                    }
                    break;
                case "OnRoundEnd":
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
                        Debug.LogError($"Unknown function name in OnRoundEnd event: {functionName}");
                    }
                    break;
                case "OnPlayerFinished":
                    if (RoomServiceActions.ActionMap.ContainsKey(functionName))
                    {
                        OnPlayerFinished += (result) =>
                        {
                            RSContext context = CreateContext(result:result);
                            RoomServiceActions.ActionMap[functionName]?.Invoke(parameters, context);
                        };
                    }
                    else
                    {
                        Debug.LogError($"Unknown function name in OnPlayerFinished event: {functionName}");
                    }
                    break;
                case "OnPlayerImproved":
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
                        Debug.LogError($"Unknown function name in OnPlayerImproved event: {functionName}");
                    }
                    break;
            }
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

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ZeepkistClient;

namespace RoomService
{
    public static class RoomServiceActions
    {
        public static readonly Dictionary<string, Action<List<string>, RSContext>> ActionMap = new Dictionary<string, Action<List<string>, RSContext>>()
        {
            // ResetChampionshipPoints
            { "ResetChampionshipPoints", (parameters, context) =>
                {
                    if(parameters.Count == 1)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            ResetChampionshipPoints(notify.Item2);
                        }
                        else
                        {
                            Debug.LogError($"ResetChampionshipPoints: Error parsing parameters. Expected: true | false, Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"ResetChampionshipPoints: Incorrect amount of parameters. Expected: 1, Found: {parameters.Count}");
                    }
                }
            },

            // AddPlayerChampionshipPoints(int:points)
            { "AddPlayerChampionshipPoints", (parameters, context) =>
                {
                    if(parameters.Count == 2 && context.SteamID != 0)
                    {
                        (bool,int) points = RoomServiceUtils.ParseIntFromString(parameters[0]);
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[1]);

                        if(points.Item1 && notify.Item1)
                        {
                            int total = context.Points + points.Item2;
                            SetPlayerChampionshipPoints(total, points.Item2, notify.Item2, context.SteamID);
                        }
                        else
                        {
                            Debug.LogError($"AddPlayerChampionshipPoints: Error parsing parameters. Points: {parameters[0]}, Notify: {parameters[1]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"AddPlayerChampionshipPoints: Incorrect parameters or invalid SteamID. Expected: 2 parameters, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // SetPlayerChampionshipPoints(int:points, int:change, bool:notify)
            { "SetPlayerChampionshipPoints", (parameters, context) =>
                {
                    if(parameters.Count == 3 && context.SteamID != 0)
                    {
                        (bool,int) points = RoomServiceUtils.ParseIntFromString(parameters[0]);
                        (bool,int) change = RoomServiceUtils.ParseIntFromString(parameters[1]);
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[2]);

                        if(points.Item1 && change.Item1 && notify.Item1)
                        {
                            SetPlayerChampionshipPoints(points.Item2, change.Item2, notify.Item2, context.SteamID);
                        }
                        else
                        {
                            Debug.LogError($"SetPlayerChampionshipPoints: Error parsing parameters. Points: {parameters[0]}, Change: {parameters[1]}, Notify: {parameters[2]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"SetPlayerChampionshipPoints: Incorrect parameters or invalid SteamID. Expected: 3 parameters, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // ResetPointsDistribution
            { "ResetPointsDistribution", (parameters, context) =>
                {
                    ResetPointsDistribution();
                }
            },

            // SetPointsDistribution(int[]:values, int:baseline, int:dnf)
            { "SetPointsDistribution", (parameters, context) =>
                {
                    if(parameters.Count == 3)
                    {
                        (bool,int[]) values = RoomServiceUtils.ParseIntArrayFromString(parameters[0]);
                        (bool,int) baseline = RoomServiceUtils.ParseIntFromString(parameters[1]);
                        (bool,int) dnf = RoomServiceUtils.ParseIntFromString(parameters[2]);

                        if(values.Item1 && baseline.Item1 && dnf.Item1)
                        {
                            SetPointsDistribution(values.Item2, baseline.Item2, dnf.Item2);
                        }
                        else
                        {
                            Debug.LogError($"SetPointsDistribution: Error parsing parameters. Values: {parameters[0]}, Baseline: {parameters[1]}, DNF: {parameters[2]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"SetPointsDistribution: Incorrect amount of parameters. Expected: 3, Found: {parameters.Count}");
                    }
                }
            },

            // RemovePlayerFromLeaderboard(bool:notify)
            { "RemovePlayerFromLeaderboard", (parameters, context) =>
                {
                    if(parameters.Count == 1 && context.SteamID != 0)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            RemovePlayerFromLeaderboard(notify.Item2, context.SteamID, context.UID);
                        }
                        else
                        {
                            Debug.LogError($"RemovePlayerFromLeaderboard: Error parsing notify parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"RemovePlayerFromLeaderboard: Incorrect parameters or invalid SteamID. Expected: 1 parameter, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // SetPlayerLeaderboardOverrides(string:time, string:name, string:position, string:points, string:pointsWon)
            { "SetPlayerLeaderboardOverrides", (parameters, context) =>
                {
                    if(parameters.Count == 5 && context.SteamID != 0)
                    {
                        string time = parameters[0];
                        string name = parameters[1];
                        string position = parameters[2];
                        string points = parameters[3];
                        string pointsWon = parameters[4];

                        SetPlayerLeaderboardOverrides(time, name, position, points, pointsWon, context.SteamID);
                    }
                    else
                    {
                        Debug.LogError($"SetPlayerLeaderboardOverrides: Incorrect parameters or invalid SteamID. Expected: 5 parameters, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

             // SetPlayerTimeOnLeaderboard(float:time, bool:notify)
            { "SetPlayerTimeOnLeaderboard", (parameters, context) =>
                {
                    if(parameters.Count == 2 && context.SteamID != 0)
                    {
                        (bool, float) timeToSet = RoomServiceUtils.ParseFloatFromString(parameters[0]);
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[1]);

                        if(timeToSet.Item1 && notify.Item1)
                        {
                            SetPlayerTimeOnLeaderboard(timeToSet.Item2, notify.Item2, context.SteamID);
                        }
                        else
                        {
                            Debug.LogError($"SetPlayerTimeOnLeaderboard: Error parsing parameters. Time: {parameters[0]}, Notify: {parameters[1]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"SetPlayerTimeOnLeaderboard: Incorrect parameters or invalid SteamID. Expected: 2 parameters, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // SetSmallLeaderboardSortingMethod(bool:sortOnPoints)
            { "SetSmallLeaderboardSortingMethod", (parameters, context) =>
                {
                    if(parameters.Count == 1)
                    {
                        (bool,bool) sortOnPoints = RoomServiceUtils.ParseBoolFromString(parameters[0]);
                        if(sortOnPoints.Item1)
                        {
                            SetSmallLeaderboardSortingMethod(sortOnPoints.Item2);
                        }
                        else
                        {
                            Debug.LogError($"SetSmallLeaderboardSortingMethod: Error parsing parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"SetSmallLeaderboardSortingMethod: Incorrect amount of parameters. Expected: 1, Found: {parameters.Count}");
                    }
                }
            },

            // SendGlobalChatMessage(string:prefix, string:message)
            { "SendGlobalChatMessage", (parameters, context) =>
                {
                    if(parameters.Count == 2)
                    {
                        string prefix = context.ReplaceParameters(parameters[0]);
                        string message = context.ReplaceParameters(parameters[1]);

                        SendGlobalChatMessage(prefix, message);
                    }
                    else
                    {
                        Debug.LogError($"SendGlobalChatMessage: Incorrect amount of parameters. Expected: 2, Found: {parameters.Count}");
                    }
                }
            },

            // SendPlayerChatMessage(string:prefix, string:message)
            { "SendPlayerChatMessage", (parameters, context) =>
                {
                    if(parameters.Count == 2 && context.SteamID != 0)
                    {
                        string prefix = context.ReplaceParameters(parameters[0]);
                        string message = context.ReplaceParameters(parameters[1]);

                        SendPlayerChatMessage(prefix, message, context.SteamID);
                    }
                    else
                    {
                        Debug.LogError($"SendPlayerChatMessage: Incorrect parameters or invalid SteamID. Expected: 2 parameters, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // BlockPlayerFromSettingTime(bool:notify)
            { "BlockPlayerFromSettingTime", (parameters, context) =>
                {
                    if(parameters.Count == 1 && context.SteamID != 0)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            BlockPlayerFromSettingTime(context.SteamID, notify.Item2);
                        }
                        else
                        {
                            Debug.LogError($"BlockPlayerFromSettingTime: Error parsing notify parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"BlockPlayerFromSettingTime: Incorrect parameters or invalid SteamID. Expected: 1 parameter, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // UnblockPlayerFromSettingTime(bool:notify)
            { "UnblockPlayerFromSettingTime", (parameters, context) =>
                {
                    if(parameters.Count == 1 && context.SteamID != 0)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            UnblockPlayerFromSettingTime(context.SteamID, notify.Item2);
                        }
                        else
                        {
                            Debug.LogError($"UnblockPlayerFromSettingTime: Error parsing notify parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"UnblockPlayerFromSettingTime: Incorrect parameters or invalid SteamID. Expected: 1 parameter, Found: {parameters.Count}. SteamID: {context.SteamID}");
                    }
                }
            },

            // BlockEveryoneFromSettingTime(bool:notify)
            { "BlockEveryoneFromSettingTime", (parameters, context) =>
                {
                    if(parameters.Count == 1)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            BlockEveryoneFromSettingTime(notify.Item2);
                        }
                        else
                        {
                            Debug.LogError($"BlockEveryoneFromSettingTime: Error parsing notify parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"BlockEveryoneFromSettingTime: Incorrect amount of parameters. Expected: 1, Found: {parameters.Count}");
                    }
                }
            },

            // UnblockEveryoneFromSettingTime(bool:notify)
            { "UnblockEveryoneFromSettingTime", (parameters, context) =>
                {
                    if(parameters.Count == 1)
                    {
                        (bool,bool) notify = RoomServiceUtils.ParseBoolFromString(parameters[0]);

                        if(notify.Item1)
                        {
                            UnblockEveryoneFromSettingTime(notify.Item2);
                        }
                        else
                        {
                            Debug.LogError($"UnblockEveryoneFromSettingTime: Error parsing notify parameter. Found: {parameters[0]}");
                        }
                    }
                    else
                    {
                        Debug.LogError($"UnblockEveryoneFromSettingTime: Incorrect amount of parameters. Expected: 1, Found: {parameters.Count}");
                    }
                }
            },
             // PrintResultsToConsole
            { "PrintResultsToConsole", (parameters, context) =>
                {
                    PrintResultsToConsole();
                }
            },
             // PrintResultsToConsole
            { "ClearAllTrackingData", (parameters, context) =>
                {
                    ClearTracker();
                }
            },
             // PrintResultsToConsole
            { "ClearTrackingResults", (parameters, context) =>
                {
                    ClearResults();
                }
            },
        };

        //Leaderboard
        private static void SetSmallLeaderboardSortingMethod(bool sortOnPoints)
        {
            ZeepkistNetwork.CustomLeaderBoard_SetSmallLeaderboardSortingMethod(sortOnPoints);
        }

        private static void SetPlayerTimeOnLeaderboard(float time, bool notify, ulong steamID)
        {
            ZeepkistNetwork.CustomLeaderBoard_SetPlayerTimeOnLeaderboard(steamID, time, notify);
        }

        private static void SetPlayerLeaderboardOverrides(string time, string name, string position, string points, string pointsWon, ulong steamID)
        {
            ZeepkistNetwork.CustomLeaderBoard_SetPlayerLeaderboardOverrides(steamID, time, name, position, points, pointsWon);
        }

        private static void RemovePlayerFromLeaderboard(bool notify, ulong steamID, string UID)
        {
            RoomService.tracker.RemovePlayersTime(UID, steamID);
            ZeepkistNetwork.CustomLeaderBoard_RemovePlayerFromLeaderboard(steamID, notify);
        }

        private static void SetPointsDistribution(int[] values, int baseline, int dnf)
        { 
            ZeepkistNetwork.CustomLeaderBoard_SetPointsDistribution(values.ToList(), baseline, dnf);
        }

        private static void ResetPointsDistribution()
        {
            ZeepkistNetwork.CustomLeaderBoard_ResetPointsDistribution();
        }

        private static void SetPlayerChampionshipPoints(int points, int change, bool notify, ulong steamID)
        {
            RoomService.tracker.SetPlayerPoints(steamID, points, change);
            ZeepkistNetwork.CustomLeaderBoard_SetPlayerChampionshipPoints(steamID, points, change, notify);
        }

        private static void ResetChampionshipPoints(bool notify)
        {
            RoomService.tracker.ResetAllPlayersPoints();
            ZeepkistNetwork.ResetChampionshipPoints(notify);
        }

        //Messages
        private static void SendGlobalChatMessage(string prefix, string message)
        {
            ZeepkistNetwork.SendCustomChatMessage(true, 0, message, prefix);
        }

        private static void SendPlayerChatMessage(string prefix, string message, ulong steamID)
        {
            ZeepkistNetwork.SendCustomChatMessage(false, steamID, message, prefix);
        }

        private static void SetServerMessage(string message)
        {
            
        }

        //Blocking and unblocking
        private static void BlockPlayerFromSettingTime(ulong steamID, bool notifyPlayer)
        {
            ZeepkistNetwork.CustomLeaderBoard_BlockPlayerFromSettingTime(steamID, notifyPlayer);
        }

        private static void UnblockPlayerFromSettingTime(ulong steamID, bool notifyPlayer)
        {
            ZeepkistNetwork.CustomLeaderBoard_UnblockPlayerFromSettingTime(steamID, notifyPlayer);
        }

        private static void BlockEveryoneFromSettingTime(bool notifyPlayer)
        {
            ZeepkistNetwork.CustomLeaderBoard_BlockEveryoneFromSettingTime(notifyPlayer);
        }

        private static void UnblockEveryoneFromSettingTime(bool notifyPlayers)
        {
            ZeepkistNetwork.CustomLeaderBoard_UnblockEveryoneFromSettingTime(notifyPlayers);
        }     
        
        private static void PrintResultsToConsole()
        {
            RoomService.tracker.PrintResults();
        }

        private static void ClearTracker()
        {
            RoomService.tracker.ClearEverything();
        }

        private static void ClearResults()
        {
            RoomService.tracker.ClearResults();
        }
    }
}

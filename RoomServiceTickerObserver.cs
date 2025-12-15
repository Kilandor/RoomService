using HarmonyLib;
using System;
using UnityEngine;

namespace RoomService
{
    [HarmonyPatch(typeof(OnlineGameplayUI), "Awake")]
    public class OnlineGameplayUIAwakePatch
    {
        public static void Postfix(OnlineGameplayUI __instance)
        {
            if (__instance.gameObject.transform.root.gameObject.GetComponent<TickerObserver>() == null)
            {
                __instance.gameObject.transform.root.gameObject.AddComponent<TickerObserver>();
            }
        }
    }
    
    public class TickerObserver : MonoBehaviour
    {
        private string timeString;
        
        private int time = 0;
        
        private OnlineGameplayUI onlineUI;
        
        public void Awake()
        {
            onlineUI = GetComponent<OnlineGameplayUI>();
        }
        
        public void Update()
        {
            if (ZeepkistClient.ZeepkistNetwork.CurrentLobby != null)
            {
                string newTimeString = ZeepkistClient.ZeepkistNetwork.CurrentLobby.timeLeftString;

                // Check if the time string has changed
                if (timeString != newTimeString)
                {
                    try
                    {
                        // Parse the time string into a TimeSpan
                        TimeSpan timeSpan;

                        if (TimeSpan.TryParseExact(newTimeString, new[] { @"hh\:mm\:ss", @"mm\:ss" }, null, out timeSpan))
                        {
                            // Convert the TimeSpan into total seconds  
                            time = (int)timeSpan.TotalSeconds;

                            // Update the time string
                            timeString = newTimeString;

                            // Invoke the event with the new time
                            RoomService.Plugin.Instance.LobbyTimerAction?.Invoke(time);
                        }
                        else
                        {
                            Debug.LogError($"Invalid time format: {newTimeString}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error parsing time string '{newTimeString}': {ex.Message}");
                    }
                }
            }
        }
    }
}

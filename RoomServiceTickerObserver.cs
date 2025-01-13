using HarmonyLib;
using System;
using UnityEngine;

namespace RoomService
{
    /// <summary>
    /// A Harmony patch to modify the behavior of the <see cref="OnlineGameplayUI"/> class during its Awake method.
    /// Adds a <see cref="TickerObserver"/> component to the <see cref="OnlineGameplayUI"/> GameObject if it does not already exist.
    /// </summary>
    [HarmonyPatch(typeof(OnlineGameplayUI), "Awake")]
    public class OnlineGameplayUIAwakePatch
    {
        /// <summary>
        /// Postfix method called after the Awake method of <see cref="OnlineGameplayUI"/>.
        /// </summary>
        /// <param name="__instance">The instance of <see cref="OnlineGameplayUI"/> being patched.</param>
        public static void Postfix(OnlineGameplayUI __instance)
        {
            if (__instance.gameObject.transform.root.gameObject.GetComponent<TickerObserver>() == null)
            {
                __instance.gameObject.transform.root.gameObject.AddComponent<TickerObserver>();
            }
        }
    }

    /// <summary>
    /// Observes the lobby timer displayed in the <see cref="OnlineGameplayUI"/> and triggers actions when the timer updates.
    /// </summary>
    public class TickerObserver : MonoBehaviour
    {
        /// <summary>
        /// Stores the last known time string from the UI.
        /// </summary>
        private string timeString;

        /// <summary>
        /// Stores the parsed total time in seconds.
        /// </summary>
        private int time = 0;

        /// <summary>
        /// Reference to the <see cref="OnlineGameplayUI"/> component this observer is attached to.
        /// </summary>
        private OnlineGameplayUI onlineUI;

        /// <summary>
        /// Called when the component is initialized. Caches a reference to the <see cref="OnlineGameplayUI"/>.
        /// </summary>
        public void Awake()
        {
            onlineUI = GetComponent<OnlineGameplayUI>();
        }

        /// <summary>
        /// Called once per frame. Monitors the lobby timer displayed in the <see cref="OnlineGameplayUI"/>.
        /// If the timer changes, parses the new time and triggers the <see cref="RoomService.Plugin.LobbyTimerAction"/> event.
        /// </summary>
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

using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;

namespace RoomService
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGUID = "com.metalted.zeepkist.roomservice";
        public const string pluginName = "RoomService";
        public const string pluginVersion = "1.0";
        public static Plugin Instance;

        public ConfigEntry<string> configPath;

        private void Awake()
        {
            Harmony harmony = new Harmony(pluginGUID);
            harmony.PatchAll();

            RoomService.Initialize();

            configPath = Config.Bind("Settings", "Config Path", "", "The full path to the configuration file.");

            Instance = this;

            // Plugin startup logic
            Logger.LogInfo($"At your service!");

            ZeepSDK.ChatCommands.ChatCommandApi.RegisterLocalChatCommand(
                "/",
                "roomservice load",
                "Loads the configuration from the path given in the settings.",
                arguments => {
                    LoadConfigurationFromPath();
                }
            );

            ZeepSDK.ChatCommands.ChatCommandApi.RegisterLocalChatCommand(
                "/",
                "roomservice unload",
                "Unloads the current configuration if there is any, and unsubscribes from any events.",
                arguments => {
                    UnloadConfiguration();
                }
            );
        }

        public void LoadConfigurationFromPath()
        {
            if (ZeepkistClient.ZeepkistNetwork.IsConnectedToGame)
            {
                if (ZeepkistClient.ZeepkistNetwork.IsMasterClient)
                {
                    RoomServiceConfig config = RoomServiceConfigLoader.LoadConfig(configPath.Value);
                    if(config != null)
                    {
                        RoomService.LoadConfig(config);
                    }
                    else
                    {
                        PlayerManager.Instance.messenger.Log("An error occured while loading the config!", 3f);
                    }
                }
                else
                {
                    PlayerManager.Instance.messenger.Log("You are not host!",2f);
                }
            }
        }

        public void UnloadConfiguration()
        {
            if (ZeepkistClient.ZeepkistNetwork.IsConnectedToGame)
            {
                if (ZeepkistClient.ZeepkistNetwork.IsMasterClient)
                {
                    RoomService.UnloadConfig();
                }
                else
                {
                    PlayerManager.Instance.messenger.Log("You are not host!", 2f);
                }
            }

            RoomService.ClearSubscriptions();
        }
    }
}
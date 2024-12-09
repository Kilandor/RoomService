using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using ZeepSDK;
using ZeepSDK.Scripting;
using ZeepSDK.Scripting.ZUA;

namespace RoomService
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
    [BepInDependency("ZeepSDK", BepInDependency.DependencyFlags.HardDependency)]
    public class Plugin : BaseUnityPlugin
    {
        public const string pluginGUID = "com.metalted.zeepkist.roomservice";
        public const string pluginName = "RoomService";
        public const string pluginVersion = "1.0";
        public static Plugin Instance;
        public Zua script; 

        public Action<int> LobbyTimerAction;

        private void Awake()
        {
            Harmony harmony = new Harmony(pluginGUID);
            harmony.PatchAll();

            Instance = this;           

            Logger.LogInfo($"At your service!");

            ScriptingApi.RegisterType<ZeepkistClient.ZeepkistNetworkPlayer>();
            ScriptingApi.RegisterType<LevelScriptableObject>();
            ScriptingApi.RegisterEvent<OnPlayerJoinedEvent>();
            ScriptingApi.RegisterEvent<OnPlayerLeftEvent>();
            ScriptingApi.RegisterEvent<OnLevelLoadedEvent>();
            ScriptingApi.RegisterEvent<OnRoundStartedEvent>();
            ScriptingApi.RegisterEvent<OnRoundEndedEvent>();
            ScriptingApi.RegisterEvent<OnLobbyTimerEvent>();

            //Com
            ScriptingApi.RegisterFunction<SendChatMessageFunction>();
            ScriptingApi.RegisterFunction<SendPrivateChatMessageFunction>();
            ScriptingApi.RegisterFunction<SendScreenMessageFunction>();

            //Lobby
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

            //Player
            ScriptingApi.RegisterFunction<SetPlayerTimeOnLeaderboardFunction>();
            ScriptingApi.RegisterFunction<SetPlayerLeaderboardOverridesFunction>();
            ScriptingApi.RegisterFunction<RemovePlayerFromLeaderboardFunction>();
            ScriptingApi.RegisterFunction<SetPlayerChampionshipPointsFunction>();
            ScriptingApi.RegisterFunction<UnblockPlayerFromSettingTimeFunction>();
            ScriptingApi.RegisterFunction<BlockPlayerFromSettingTimeFunction>();

            //Getters
            ScriptingApi.RegisterFunction<GetPlayerCountFunction>();
            ScriptingApi.RegisterFunction<GetPlaylistIndexFunction>();
            ScriptingApi.RegisterFunction<GetPlaylistLengthFunction>();
            ScriptingApi.RegisterFunction<GetLevelFunctions>();
        }

        public void Update()
        {
            /*
            if(Input.GetKeyDown(KeyCode.P))
            {
                LoadScript("test");
            }*/
        }

        public void LoadScript(string name)
        {
            if(script != null)
            {
                script.Unload();
            }

            script = ScriptingApi.LoadLuaByName(name);
            if (script == null)
            {
                return;
            }            
        }

        public void Log(object data, bool force = false)
        {
            Logger.LogInfo(data);
        }       
    }    
}
using BepInEx;
using UnityEngine;
using HarmonyLib;
using BepInEx.Configuration;
using System;
using ZeepSDK;
using ZeepSDK.Scripting.ZUA;

namespace RoomService
{
    [BepInPlugin(pluginGUID, pluginName, pluginVersion)]
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
        }

        public void Update()
        {
            if(Input.GetKeyDown(KeyCode.P))
            {
                LoadScript("test");
            }
        }

        public void LoadScript(string name)
        {
            if(script != null)
            {
                script.Unload();
            }

            script = ZeepSDK.Scripting.ScriptingApi.LoadLuaByName(name);
            if(script == null)
            {
                return;
            }

            /*
            script.RegisterType<ZeepkistClient.ZeepkistNetworkPlayer>();
            script.RegisterType<LevelScriptableObject>();

            script.RegisterEvent<OnPlayerJoinedEvent>();
            script.RegisterEvent<OnPlayerJoinedEvent>();
            script.RegisterEvent<OnPlayerLeftEvent>();
            script.RegisterEvent<OnLevelLoadedEvent>();
            script.RegisterEvent<OnRoundStartedEvent>();
            script.RegisterEvent<OnRoundEndedEvent>();
            script.RegisterEvent<OnLobbyTimerEvent>();

            //Com
            script.RegisterFunction<SendChatMessageFunction>();
            script.RegisterFunction<SendPrivateChatMessageFunction>();
            script.RegisterFunction<SendScreenMessageFunction>();

            //Lobby
            script.RegisterFunction<SetPointsDistributionFunction>();
            script.RegisterFunction<ResetPointsDistributionFunction>();
            script.RegisterFunction<ResetChampionshipPointsFunction>();
            script.RegisterFunction<SetVoteskipFunction>();
            script.RegisterFunction<SetVoteskipPercentageFunction>();
            script.RegisterFunction<SetLobbyNameFunction>();
            script.RegisterFunction<SetServerMessageFunction>();
            script.RegisterFunction<RemoveServerMessageFunction>();
            script.RegisterFunction<SetRoundLengthFunction>();
            script.RegisterFunction<SetSmallLeaderboardSortingMethodFunction>();
            script.RegisterFunction<BlockEveryoneFromSettingTimeFunction>();
            script.RegisterFunction<UnblockEveryoneFromSettingTimeFunction>();

            //Player
            script.RegisterFunction<SetPlayerTimeOnLeaderboardFunction>();
            script.RegisterFunction<SetPlayerLeaderboardOverridesFunction>();
            script.RegisterFunction<RemovePlayerFromLeaderboardFunction>();
            script.RegisterFunction<SetPlayerChampionshipPointsFunction>();
            script.RegisterFunction<UnblockPlayerFromSettingTimeFunction>();
            script.RegisterFunction<BlockPlayerFromSettingTimeFunction>();

            //Getters
            script.RegisterFunction<GetPlayerCountFunction>();
            script.RegisterFunction<GetPlaylistIndexFunction>();
            script.RegisterFunction<GetPlaylistLengthFunction>();
            script.RegisterFunction<GetLevelFunctions>();*/
        }

        public void Log(object data, bool force = false)
        {
            Logger.LogInfo(data);
        }       
    }    
}
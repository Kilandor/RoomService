using BepInEx;
using HarmonyLib;
using System;
using ZeepSDK.Scripting;

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

        public Action<int> LobbyTimerAction;

        private void Awake()
        {
            Harmony harmony = new Harmony(pluginGUID);
            harmony.PatchAll();

            Instance = this;           

            //Register all required types.
            ScriptingApi.RegisterType<ZeepkistClient.ZeepkistNetworkPlayer>();
            ScriptingApi.RegisterType<LevelScriptableObject>();

            //Register all events.
            ScriptingApi.RegisterEvent<OnPlayerJoinedEvent>();
            ScriptingApi.RegisterEvent<OnPlayerLeftEvent>();
            ScriptingApi.RegisterEvent<OnLevelLoadedEvent>();
            ScriptingApi.RegisterEvent<OnRoundStartedEvent>();
            ScriptingApi.RegisterEvent<OnRoundEndedEvent>();
            ScriptingApi.RegisterEvent<OnLobbyTimerEvent>();

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
            ScriptingApi.RegisterFunction<GetLevelFunctions>();

            Logger.LogInfo("Roomservice loaded!");
        }  
    }    
}
using MoonSharp.Interpreter;
using System;
using System.Linq;
using ZeepkistClient;
using ZeepSDK.Chat;
using ZeepSDK.Scripting.ZUA;

namespace RoomService
{
    public class SendChatMessageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SendChatMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, string>(Implementation);
        }

        private void Implementation(string prefix, string message)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(message))
            {
                return;
            }

            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.SendCustomChatMessage(true, 0, message, prefix);
        }
    }

    public class SendPrivateChatMessageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SendPrivateChatMessage";

        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string>(Implementation);
        }

        private void Implementation(ulong steamID, string prefix, string message)
        {
            if (string.IsNullOrEmpty(prefix) || string.IsNullOrEmpty(message))
            {
                return;
            }

            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.SendCustomChatMessage(false, steamID, message, prefix);
        }
    }

    public class SendScreenMessageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SendScreenMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        private void Implementation(string message, float time)
        {
            if (string.IsNullOrEmpty(message))
            {
                return;
            }

            PlayerManager.Instance.messenger.Log(message, time);
        }
    }

    public class BlockEveryoneFromSettingTimeFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "BlockEveryoneFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockEveryoneFromSettingTime(notify);
        }
    }

    public class UnblockEveryoneFromSettingTimeFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "UnblockEveryoneFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockEveryoneFromSettingTime(notify);
        }
    }

    public class UnblockPlayerFromSettingTimeFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "UnblockPlayerFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_UnblockPlayerFromSettingTime(steamID, notify);
        }
    }

    public class BlockPlayerFromSettingTimeFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "BlockPlayerFromSettingTime";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_BlockPlayerFromSettingTime(steamID, notify);
        }
    }

    public class SetSmallLeaderboardSortingMethodFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetSmallLeaderboardSortingMethod";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        private void Implementation(bool useChampionshipSorting)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetSmallLeaderboardSortingMethod(useChampionshipSorting);
        }
    }

    public class SetPlayerTimeOnLeaderboardFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerTimeOnLeaderboard";

        public Delegate CreateFunction()
        {
            return new Action<ulong, float, bool>(Implementation);
        }

        private void Implementation(ulong steamID, float time, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerTimeOnLeaderboard(steamID, time, notify);
        }
    }

    public class SetPlayerLeaderboardOverridesFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerLeaderboardOverrides";

        public Delegate CreateFunction()
        {
            return new Action<ulong, string, string, string,string,string>(Implementation);
        }

        private void Implementation(ulong steamID, string time, string name, string position, string points, string pointsWon)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerLeaderboardOverrides(steamID, time, name, position, points, pointsWon);
        }
    }

    public class RemovePlayerFromLeaderboardFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "RemovePlayerFromLeaderboard";

        public Delegate CreateFunction()
        {
            return new Action<ulong, bool>(Implementation);
        }

        private void Implementation(ulong steamID, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_RemovePlayerFromLeaderboard(steamID, notify);
        }
    }

    public class SetPointsDistributionFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPointsDistribution";

        public Delegate CreateFunction()
        {
            return new Action<int[],int,int>(Implementation);
        }

        private void Implementation(int[] values, int baseline, int dnf)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPointsDistribution(values.ToList(), baseline, dnf);
        }
    }

    public class ResetPointsDistributionFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetPointsDistribution";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_ResetPointsDistribution();
        }
    }

    public class SetPlayerChampionshipPointsFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetPlayerChampionshipPoints";

        public Delegate CreateFunction()
        {
            return new Action<ulong, int, int, bool>(Implementation);
        }

        private void Implementation(ulong steamID, int points, int pointsWon, bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.CustomLeaderBoard_SetPlayerChampionshipPoints(steamID, points, pointsWon, notify);
        }
    }

    public class ResetChampionshipPointsFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "ResetChampionshipPoints";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        private void Implementation(bool notify)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ZeepkistNetwork.ResetChampionshipPoints(notify);
        }
    }

    public class SetRoundLengthFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetRoundLength";

        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        private void Implementation(int time)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            time = Math.Max(30, time);

            ChatApi.SendMessage("/settime " + time.ToString());
        }
    }

    public class SetVoteskipFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetVoteskip";

        public Delegate CreateFunction()
        {
            return new Action<bool>(Implementation);
        }

        private void Implementation(bool enabled)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage($"/vs {(enabled ? "on" : "off")}");
        }
    }

    public class SetVoteskipPercentageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetVoteskipPercentage";

        public Delegate CreateFunction()
        {
            return new Action<int>(Implementation);
        }

        private void Implementation(int percentage)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            percentage = Math.Min(100, Math.Max(1, percentage));

            ChatApi.SendMessage($"/vs % {percentage.ToString()}");
        }
    }

    public class SetLobbyNameFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetLobbyName";

        public Delegate CreateFunction()
        {
            return new Action<string>(Implementation);
        }

        private void Implementation(string name)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            if (string.IsNullOrEmpty(name.Trim()))
            {
                return;
            }

            ZeepkistLobby currentLobby = ZeepkistNetwork.CurrentLobby;
            if (currentLobby != null)
            {
                currentLobby.UpdateName(name);
            }
        }
    }

    public class SetServerMessageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "SetServerMessage";

        public Delegate CreateFunction()
        {
            return new Action<string, float>(Implementation);
        }

        private void Implementation(string message, float time)
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            if (string.IsNullOrEmpty(message.Trim()))
            {
                return;
            }

            ChatApi.SendMessage($"/servermessage white {time.ToString()} {message}");
        }
    }

    public class RemoveServerMessageFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "RemoveServerMessage";

        public Delegate CreateFunction()
        {
            return new Action(Implementation);
        }

        private void Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return;
            }

            ChatApi.SendMessage("/servermessage remove");
        }
    }

    public class GetPlayerCountFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlayerCount";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.PlayerCount ?? -1;
        }
    }

    public class GetPlaylistIndexFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlaylistIndex";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.CurrentPlaylistIndex ?? -1;
        }
    }

    public class GetPlaylistLengthFunction : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetPlaylistLength";

        public Delegate CreateFunction()
        {
            return new Func<int>(Implementation);
        }

        private int Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return -1;
            }

            return ZeepkistNetwork.CurrentLobby?.Playlist.Count ?? -1;
        }
    }

    public class GetLevelFunctions : ILuaFunction
    {
        public string Namespace => "RoomService";

        public string Name => "GetLevel";

        public Delegate CreateFunction()
        {
            return new Func<LevelScriptableObject>(Implementation);
        }

        private LevelScriptableObject Implementation()
        {
            if (!RoomServiceUtils.IsOnlineHost())
            {
                return null;
            }

            return ZeepSDK.Level.LevelApi.CurrentLevel;
        }
    }

}
 
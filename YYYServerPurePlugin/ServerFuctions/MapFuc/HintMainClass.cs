using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using Respawning;
using UnityEngine;

namespace YYYServerPurePlugin.ServerFuctions.MapFuc;

    public enum ScreenType
    {
        TOP,
        CenterTop,
        Center,
        CenterBottom,
        BottomTop,
        Bottom,
        End
    }
     
    public class PlayerHintInfo
    {
        public bool wait;
        public Dictionary<ScreenType,string> showInfo = new()
        {
            { ScreenType.TOP ,""},
            { ScreenType.CenterTop ,""},
            { ScreenType.Center ,""},
            { ScreenType.CenterBottom ,""},
            { ScreenType.BottomTop ,""},
            { ScreenType.Bottom ,""},
        };
        public string playerinfo;
        public string spectinfo;
    }
    public class HintMainClass
    {
        public static string deathinfo;
        private static List<CoroutineHandle> Coroutines = new();
        private static List<string> chatList = new();
        public static int infoid = 0;
        public static Dictionary<int, PlayerHintInfo> PlayerHintInfos = new();
        public static void RemovePlayerInfo(Player player)
        {
            if (!PlayerHintInfos.ContainsKey(player.PlayerId))
            {
                PlayerHintInfos[player.PlayerId] = new PlayerHintInfo();
            }
            PlayerHintInfos[player.PlayerId].playerinfo = "";
        }
        public static void AddPlayerInfo(Player player, string info)
        {
            if (!PlayerHintInfos.ContainsKey(player.PlayerId))
            {
                PlayerHintInfos[player.PlayerId] = new PlayerHintInfo();
            }
            PlayerHintInfos[player.PlayerId].playerinfo = info;
        }
        public static void GetSCPHP()
        {
            int scp492num = 0;
            string tmpscpinfo = "";
            Dictionary<Team, int> teamAmounts = new Dictionary<Team, int>();
            var players = Player.GetPlayers().ToList();
            foreach (var player in players)
            {
                try
                {
                    if (player.Team == Team.SCPs)
                    {
                        if (player.Role == RoleTypeId.Scp106)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP106</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue + "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp3114)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP3114</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue + "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp939)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP939</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" +  player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue + "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp173)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP173</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue + "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp049)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP049</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue+ "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp096)
                        {
                            tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP096</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" +  player.ReferenceHub.playerStats.GetModule<HumeShieldStat>().CurValue + "</color>][区域:<color=#FF0000>" + MyApi.MyApi.zoneTranslation[player.Zone] + "</color>]";
                        }
                        if (player.Role == RoleTypeId.Scp079)
                        {
                            if (player.ReferenceHub.roleManager.CurrentRole is Scp079Role scp079Role)
                            {
                                scp079Role.SubroutineModule.TryGetSubroutine(out Scp079TierManager tier);
                                scp079Role.SubroutineModule.TryGetSubroutine(out Scp079AuxManager tier2);
                                tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP079</color><color=#FFFF00>[Online]等级:" +tier.AccessTierLevel + "电量:" +tier2._aux+"</color>";
                            }
                        }
                        if (player.Role == RoleTypeId.Scp0492)
                        {
                            scp492num++;
                        }
                    }
                    else if (player.Team != Team.OtherAlive && player.IsAlive)
                    {
                        if (!teamAmounts.ContainsKey(player.Team))
                        {
                            teamAmounts[player.Team] = 0;
                        }
                        teamAmounts[player.Team] += 1;
                    }
     
                }
                catch
                {
                }
            }
            tmpscpinfo += ("\n<color=#FFA500>小僵尸数量</color>[" + scp492num + "]");
            foreach (var varp in players)
            {
                /*if (!MenuSystemMain.show_menu.ContainsKey(varp.Id) ||
                    MenuSystemMain.show_menu[varp.Id] <= DateTime.Now)
                {

                }*/
                if (varp.Team == Team.SCPs)
                {
                    SetHint(varp, "<align=right><size=12>" + tmpscpinfo + "</size></align>", 10,
                        ScreenType.Center);
                }
                else if (teamAmounts.ContainsKey(varp.Team))
                {
                    var tmpmessage =
                        $"[<color={varp.RoleBase.RoleColor.ToHex()}>{MyApi.MyApi.TeamTranslation[varp.Team]}]</color> {teamAmounts[varp.Team]}";
                    SetHint(varp, "<align=right>" + tmpmessage + "</align>", 10, ScreenType.Center);
                }
            }
        }

        public static string BuildPlayerHintMessage(Player player)
        {
            int outrange = 0;
            string buiuled = "UI系统出现异常，可能会自动恢复也可能不会，没有你的信息";
            if (PlayerHintInfos.ContainsKey(player.PlayerId))
            {
                buiuled = "";
                if (player.IsAlive)
                {
                    SetHint(player, PlayerHintInfos[player.PlayerId].playerinfo, 10, ScreenType.Bottom);
                }
                else
                {
                    SetHint(player, deathinfo, 10, ScreenType.Bottom);
                }
                for (int i = 0; i < (int)ScreenType.End; i++)
                {
                    int needline = 7;
                    if (i == (int)ScreenType.Center)
                    {
                        if (player.Team == Team.SCPs)
                        {
                            needline = 14;
                        }
                    }
                    string tmp = PlayerHintInfos[player.PlayerId].showInfo[(ScreenType)(i)];
                    if (i == (int)ScreenType.Center)
                    {
                        if (player.Team == Team.SCPs)
                        {
                            tmp = "<line-height=12>" + tmp;
                        }
                    }
                    int line = tmp.Split('\n').Length;
                    buiuled += tmp;
                    if (line != 0)
                    {
                        if (line <= needline - outrange)
                        {
                            for (int i2 = 0; i2 < needline + 1- outrange - line; i2++)
                            {
                                buiuled += '\n';
                            }
                            outrange = 0;
                        }
                        else
                        {
                            outrange = line - needline + outrange;
                        }
                    }
                    else
                    {
                        buiuled += "\n\n\n\n\n\n\n";
                    }
                    if (i == (int)ScreenType.Center)
                    {
                        if (player.Team == Team.SCPs)
                        {
                            buiuled += "<line-height=24>";
                        }
                    }
                }
            }
            else
            {
                PlayerHintInfos[player.PlayerId] = new PlayerHintInfo();
            }
            return buiuled;
        }
        private static string BuildLevelMessage(int experience)
        {
            int nextLevelExp = 1000;
            experience %= 1000;
            double percentage = (double)experience / nextLevelExp * 100;
            int totalBlocks = 10; // 进度条总长度（可以根据需要调整）
            int filledBlocks = (int)(percentage / 100 * totalBlocks);
            int emptyBlocks = totalBlocks - filledBlocks;
            string progressBar = new string('■', filledBlocks) + new string('□', emptyBlocks);
            return $"{progressBar} {percentage:F2}%";
        }
        private static IEnumerator<float> YyyServerHint()
        {
            yield return Timing.WaitForSeconds(5f);
            int awa = 0;

            double tps = 60;
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                string uiinfo = "<size=13>感谢游玩<color=#FF69B4>嘤嘤嘤</color>服务器 插件版本<color=#4CAF50>纯净0.0.1</color></size>";
                if (Round.IsRoundStarted)
                {
                    try
                    {
                        awa++;
                        if (awa >= 9)
                        {
                            awa = 0;
                            GetSCPHP();
                            tps = Math.Round(1.0 / (double)Time.smoothDeltaTime);
                        }
     
                        foreach (var varInfo in PlayerHintInfos)
                        {
                            var value = varInfo.Value;
                            Player player = Player.Get(varInfo.Key);
                            if (player != null)
                            {
                                if (!value.wait)
                                {
                                    player.ReceiveHint(
                                        "<line-height=24><size=24><voffset=12em>" +
                                        BuildPlayerHintMessage(player) + uiinfo + "\n" +
                                        (player.IsAlive ? "" : PlayerHintInfos[player.PlayerId].spectinfo) + "\n" +
                                        $"<size=20>欢迎你<color={player.RoleBase.RoleColor.ToHex()}>{player.DisplayNickname}</color> | 服务器TPS:<color=#00FFFF>{tps}</color> </size>" +
                                        "</size></line-height></voffset>", 2);
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        Log.Info(ex.StackTrace);
                    }
                }
            }
        }
        public static IEnumerator<float> chatTiming(Player sendplayer, string chattxt)
        {
            yield return Timing.WaitForSeconds(1f);
            try
            {
            if (sendplayer.GameObject != null)
            {
                if (chattxt.Length <= 30)
                {
                    if (chatList.Count < 6)
                    {
                        chatList.Add("<pos=35%>[<color="+sendplayer.RoleBase.RoleColor.ToHex() +">" +MyApi.MyApi.TranslateOfRoleType [sendplayer.Role] + "</color>]" + sendplayer.Nickname + ":" + chattxt);
                    }
                    else
                    {
                        chatList.RemoveAt(0);
                        chatList.Add("<pos=35%>[<color="+sendplayer.RoleBase.RoleColor.ToHex() +">" +MyApi.MyApi.TranslateOfRoleType [sendplayer.Role] + "</color>]" + sendplayer.Nickname + ":" + chattxt);
                    }
                    List<string> list = new List<string>();
                    for (int i = 0; i < chatList.Count; i++)
                    {
                        string color = "";
                        switch (i)
                        {
                            case 0:
                                color = "<color=#FFFF00>";
                                break;
                            case 1:
                                color = "<color=#FFFF15>";
                                break;
                            case 2:
                                color = "<color=#FFFF30>";
                                break;
                            case 3:
                                color = "<color=#FFFF45>";
                                break;
                            case 4:
                                color = "<color=#FFFF60>";
                                break;
                            case 5:
                                color = "<color=#FFFF75>";
                                break;
                            case 6:
                                color = "<color=#FFFF90>";
                                break;
                            default:
                                color = "<color=#FFFF99>";
                                break;
                        }
                        list.Add(color + chatList[i] + "</color>");
                    }
                    AddChatHint("<size=16><align=right>" + "<pos=35%>公屏系统(请勿报点辱骂等)队内信息.tc [内容]\n" + string.Join("\n", list) + "</align></size>");
                    list.Clear();
                }
            }
            }
            catch(Exception exception)
            {
                Log.Info(exception.StackTrace);
            }

        }
        [PluginEvent]
        void ChangingSpectator(PlayerChangeSpectatorEvent ev)
        {
            if (ev.NewTarget != null)
            {
                try
                {
                    PlayerHintInfos[ev.Player.PlayerId].spectinfo =
                        $"正在观看: <color={ev.NewTarget.RoleBase.RoleColor.ToHex()}>{ev.NewTarget.Nickname}</color> | 称号: {ev.NewTarget.ReferenceHub.serverRoles.Network_myText}";
                }
                catch
                {
                         
                }
            }
        }
        public static void AddChatHint(string thing)
        {
            foreach (var varp in Player.GetPlayers())
            {
                SetHint(varp,thing,9,ScreenType.CenterBottom);
            }
        }
     
        public static void AddTempHintToAll(string thing, int time,ScreenType type)
        {
            foreach (var tmpplayer in Player.GetPlayers())
            {
                AddTempHint(tmpplayer,thing,time,type);
            }
        }
        public static void AddTempHint(Player player, string thing, int time , ScreenType type)
        {
            int myid = infoid ++;
            string wantjoin =  "\n<size=0>"+myid+"</size>"+thing;
            if (PlayerHintInfos.ContainsKey(player.PlayerId))
            {
                if (PlayerHintInfos[player.PlayerId].showInfo[type] == "")
                {
                    wantjoin = wantjoin.Remove(0,1);
                    PlayerHintInfos[player.PlayerId].showInfo[type] += wantjoin;
                }
                else
                {
                    PlayerHintInfos[player.PlayerId].showInfo[type] += wantjoin;
                }
            }
            Timing.CallDelayed(time, () => {
                try
                {
                    if (PlayerHintInfos.ContainsKey(player.PlayerId))
                    {
                        if( PlayerHintInfos[player.PlayerId].showInfo[type].Contains(myid.ToString()))
                        {
                            PlayerHintInfos[player.PlayerId].showInfo[type] =  PlayerHintInfos[player.PlayerId].showInfo[type].Replace(wantjoin,"");
                        }
                    }
                }
                catch(Exception exception)
                {
                    Log.Info(exception.StackTrace);
                }

            });
        }
        public static void SetHint(Player player, string thing, int time,ScreenType type)
        {
            int myid = infoid ++;
            string wantjoin =  "<size=0>"+myid+"</size>"+thing;
            if (PlayerHintInfos.ContainsKey(player.PlayerId))
            {
                PlayerHintInfos[player.PlayerId].showInfo[type] = wantjoin;
            }
            Timing.CallDelayed(time, () => {
                try
                {
                    if (PlayerHintInfos.ContainsKey(player.PlayerId))
                    {
                        PlayerHintInfos[player.PlayerId].showInfo[type] = PlayerHintInfos[player.PlayerId].showInfo[type].Replace(wantjoin,"");
                    }
                }
                catch(Exception exception)
                {
                    Log.Info(exception.StackTrace);
                }
            });
        }
        [PluginEvent]
        void OnVer(PlayerJoinedEvent ev)
        {
            PlayerHintInfos.Add(ev.Player.PlayerId, new PlayerHintInfo());
        }
        [PluginEvent]
        void OnWaitingForPlayer(WaitingForPlayersEvent ev)
        {
            Coroutines.Add(Timing.RunCoroutine(YyyServerHint()));
        }
     
        private static void Reset()
        {
            infoid = 0;
            chatList.Clear();
            PlayerHintInfos.Clear();
            foreach (CoroutineHandle coroutineHandle in Coroutines)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            Coroutines.Clear();
        }
        [PluginEvent]
        void OnRoundRestart(RoundRestartEvent ev)
        {
            Reset();
        }
        [PluginEvent]
        void OnLeft(PlayerLeftEvent ev)
        {
            PlayerHintInfos.Remove(ev.Player.PlayerId);
        }
    }
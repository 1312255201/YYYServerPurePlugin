using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using Respawning;
using UnityEngine;

namespace YYYServerPurePlugin.ServerFuctions.MapFuc;

public class awa
{
    private Player player;
    private int playerid;
    private bool wait2;
    private List<string> strings = new();
    public Player playerawa { get => player; set => player = value; }
    public int playeridawa { get => playerid; set => playerid = value; }
    public List<string> message { get => strings; set => strings = value; }
    public bool wait { get => wait2; set => wait2 = value; }
}

public class HintMainClass
{
    public static string deathinfo;
    private static List<CoroutineHandle> Coroutines = new();
    private static List<string> chatList = new();
    private static string scphpinfo;
    public static List<awa> awas = new();
    public static bool showchat;
    public static void RemovePlayerInfo(Player player)
    {
        foreach (var awa2 in awas)
        {
            if (awa2.playeridawa != player.PlayerId) continue;
            var temp = "";
            foreach (var message in awa2.message.Where(message => message.Contains("玩家角色介绍")))
            {
                temp = message;
            }
            if (temp != "")
            {
                awa2.message.Remove(temp);
            }

        }
    }
    public static void AddPlayerInfo(Player player, string info)
    {
        foreach (var awa2 in awas)
        {
            if (awa2.playeridawa != player.PlayerId) continue;
            var temp = "";
            foreach (var message in awa2.message.Where(message => message.Contains("玩家角色介绍")))
            {
                temp = message;
            }
            if (temp != "")
            {
                awa2.message.Remove(temp);
            }
            awa2.message.Add("\n\n\n\n\n\n\n<align=center><size=0>玩家角色介绍</size>\n<size=20>" + info + "</size></align>");
        }
    }
    public static void GetSCPHP()
    {
        int scp492num = 0;
        string tmpscpinfo = "";
        foreach (var player in Player.GetPlayers())
        {
            try
            {
                if (player.Role == RoleTypeId.Scp106)
                {
                    tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP106</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.StatModules[4].CurValue + "</color>]";
                }
                if (player.Role == RoleTypeId.Scp939)
                {
                    tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP939</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.StatModules[4].CurValue + "</color>]";
                }
                if (player.Role == RoleTypeId.Scp173)
                {
                    tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP173</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.StatModules[4].CurValue + "</color>]";
                }
                if (player.Role == RoleTypeId.Scp049)
                {
                    tmpscpinfo = tmpscpinfo + "\n<color=#FFA500>SCP049</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.StatModules[4].CurValue + "</color>]";
                }
                if (player.Role == RoleTypeId.Scp096)
                {
                    tmpscpinfo = tmpscpinfo + "\n<color=#FFA500><size=16>SCP096</color>[<color=#FFFF00>" + player.Health + "/" + player.MaxHealth + "</color>][AHP:<color=#FF0000>" + player.ReferenceHub.playerStats.StatModules[4].CurValue + "</color>]";
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
            catch
            {
                    
            }
        }
        tmpscpinfo += ("\n<color=#FFA500>小僵尸数量</color>[" + scp492num + "]");
        scphpinfo = tmpscpinfo;
    }
    private static IEnumerator<float> YYYServerHint()
    {
        yield return Timing.WaitForSeconds(5f);
        int awa = 0;
        int mtfnum = 0;
        int chinum = 0;
        while (true)
        {
            yield return Timing.WaitForSeconds(1f);
            awa++;
            if (awa >= 20)
            {
                awa = 0;
                GetSCPHP();
                chinum = Player.GetPlayers().Count(x => x.Team == Team.ChaosInsurgency);
                mtfnum = Player.GetPlayers().Count(x => x.Team == Team.FoundationForces);
            }

            try
            {
                if (Player.GetPlayers().Any(x=>x.Role == RoleTypeId.Spectator))
                {
                    string teamfuhuo = "";
                    if (Respawn.NextKnownTeam == SpawnableTeamType.NineTailedFox)
                    {
                        teamfuhuo = "<color=#1E90FF>白给狐٩(๑❛ᴗ❛๑)۶</color>";
                    }
                    if (Respawn.NextKnownTeam == SpawnableTeamType.ChaosInsurgency)
                    {
                        teamfuhuo = "<color=#3CB371>馄饨裂开者ヾ(๑╹◡╹)ﾉ</color>";
                    }
                    if (Respawn.NextKnownTeam == SpawnableTeamType.None)
                    {
                        teamfuhuo = "我不知道别看我QAQ";
                    }
                    deathinfo = string.Concat("<align=right><size=21>", "<pos=30%>你已阵亡" + "\n<pos=30%>但是不用担心你马上会复活:</pos>", "\n<pos=30%>剩余时间:", Convert.ToInt32((TimeSpan.FromSeconds((double) RespawnManager.Singleton._timeForNextSequence - RespawnManager.Singleton._stopwatch.Elapsed.TotalSeconds)).TotalSeconds).ToString(), "</pos>\n<pos=30%><color=#4169E1>👮九尾狐机票数:</color>", Respawn.NtfTickets, "</pos>\n<pos=30%><color=#228B22>🐻混沌车票数:</color>", Respawn.ChaosTickets, "</pos>\n<pos=30%>👻当前观察者人数：", Player.GetPlayers().Count(x=>x.Role == RoleTypeId.Spectator).ToString(), "</pos>\n<pos=30%><color=#FFFF00>欢迎来到嘤嘤嘤服务器Q群285774856</color></pos>\n<pos=30%>服务器TPS(60为最高):"+Math.Round(1.0 / (double) Time.smoothDeltaTime)+"</pos>\n<pos=30%>欢迎加群反馈BUG</pos>\n<pos=30%>复活角色:", teamfuhuo, "</pos></size></align>");
                }
            }
            catch
            {
                
            }

            
            for (int i = 0; i < awas.Count(); i++)
            {
                try
                {
                    if ((awas[i].message.Count >= 1 && awas[i].wait == false) || !awas[i].playerawa.IsAlive || awas[i].playerawa.Team == Team.SCPs || awas[i].playerawa.Team == Team.FoundationForces || awas[i].playerawa.Team == Team.ChaosInsurgency)
                    {
                        StringBuilder str = new StringBuilder();
                        switch (awas[i].playerawa.Team)
                        {
                            case Team.SCPs:
                                str.Insert(0, "\n\n\n\n\n\n\n");
                                str.Insert(0, "<size=20><align=right>"+scphpinfo+"</align></size>");
                                break;
                            case Team.ChaosInsurgency:
                                str.Insert(0, "\n\n\n\n\n\n\n");
                                str.Insert(0, "<size=20><align=right>当前混沌人数："+chinum+"</align></size>");
                                break;
                                case Team.FoundationForces:
                                    str.Insert(0, "\n\n\n\n\n\n\n");
                                    str.Insert(0, "<size=20><align=right>当前九尾人数："+mtfnum+"</align></size>");
                                    break;
                                
                            }
                        foreach (var mm in awas[i].message)
                        {
                            if (mm.Contains("聊天的Timing"))
                            {
                                str.Insert(0, mm);
                            }
                            if (mm.Contains("玩家角色介绍"))
                            {
                                str.Append(mm);
                            }
                            if (mm.Contains("临时消息"))
                            {
                                str.Append(mm);
                            }
                        }
                        if (awas[i].playerawa.GameObject != null)
                        {
                            if (awas[i].playerawa.IsAlive)
                            {
                                awas[i].playerawa.ReceiveHint(str.ToString());
                            }
                            else
                            {
                                if (awas[i].playerawa.Role != RoleTypeId.None)
                                {
                                    awas[i].playerawa.ReceiveHint(str + "<size=21>"+deathinfo +"</size>"+ "\n\n\n\n\n");
                                }
                            }
                        }
                        str.Clear();
                    }
                }
                catch (Exception ex)
                {
                    Log.Info(ex.Message);
                    Log.Info(ex.GetBaseException().ToString());
                }
            }
        }
    }
    public static void AddTempHintToAll(string thing, int time)
    {
        foreach (var tmpplayer in Player.GetPlayers())
        {
            AddTempHint(tmpplayer,thing,time);
        }
    }
    public static void AddTempHint(Player player, string thing, int time)
    {
        if (player.GameObject != null)
        {
            foreach (awa awa2 in awas)
            {
                if (awa2.playeridawa == player.PlayerId)
                {
                    awa2.message.Add("\n<size=0>临时消息</size>\n" + thing);
                }
            }
            Timing.CallDelayed(time, () => {
                if (player.GameObject != null)
                {
                    foreach (awa awa2 in awas)
                    {
                        if (awa2.playeridawa == player.PlayerId)
                        {
                            string temp = "";
                            foreach (string message in awa2.message)
                            {
                                if (message.Contains(thing))
                                {
                                    temp = message;
                                }
                            }
                            if (temp != "")
                            {
                                awa2.message.Remove(temp);
                            }
                        }
                    }
                }
            });
        }
    }

    [PluginEvent]
    void OnPlayerJoined(PlayerJoinedEvent ev)
    {
        awa tempawa = new awa();
        tempawa.playerawa = ev.Player;
        tempawa.playeridawa = ev.Player.PlayerId;
        awas.Add(tempawa);
    }
    [PluginEvent]
    void OnWaitingForPlayer(WaitingForPlayersEvent ev)
    {
        Coroutines.Add(Timing.RunCoroutine(YYYServerHint()));
    }
    private static void Reset()
    {
        showchat = false;
        chatList.Clear();
        foreach (awa awa2 in awas)
        {
            awa2.playerawa = null;
            awa2.playeridawa = 0;
            awa2.message.Clear();
        }
        awas.Clear();
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
    void OnPlayerQuit(PlayerLeftEvent ev)
    {
        for (int i = awas.Count - 1; i >= 0; i--)
        {
            if (awas[i].playeridawa == ev.Player.PlayerId)
            {
                awas.Remove(awas[i]);
                Log.Info("玩家退出删除他的Hint" + ev.Player.PlayerId);
            }
        }
    }
}
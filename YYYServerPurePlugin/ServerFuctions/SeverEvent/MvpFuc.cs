using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using Mirror;
using PlayerStatsSystem;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using PluginAPI.Helpers;
using Respawning;

namespace YYYServerPurePlugin.ServerFuctions.SeverEvent;
   public class MvpFuc
   {
       public DateTime starttime;
       public string firstescapeplayer;
       public int first_player_escape_time;
       public Dictionary<int, int> KillNum = new Dictionary<int, int>();
       public Dictionary<int, int> TotalDamege = new Dictionary<int, int>();
       [PluginEvent(PluginAPI.Enums.ServerEventType.RoundStart)]
       void RoundStart()
       {
           starttime = DateTime.Now;
       }
       [PluginEvent]
       void OnTeamRespawn(TeamRespawnEvent ev)
       {
           if(ev.Team == SpawnableTeamType.ChaosInsurgency)
           {
               RespawnEffectsController.PlayCassieAnnouncement("请设施内的所有基金会势力注意！C.A.S.S.I.E安全系统检测到有大量混沌组织在设施A大门集结！请采取适当的措施，祝好运！<SIZE=0> pitch_0.2 .g4 pitch_1 . jam_030_5 . Attention all security M T F . CASSIE .G6 security system detected pitch_0.95 some ChaosInsurgency near the GATE A of facility  stay pitch_0.95 jam_030_5 security  </SIZE> pitch_1", false,true , true);
           }
       }
       [PluginEvent(PluginAPI.Enums.ServerEventType.PlayerEscape)]
       void PlayerEscape(Player player, PlayerRoles.RoleTypeId newRole)
       {
           if (first_player_escape_time == 0)
           {
               firstescapeplayer = player.Nickname;
               first_player_escape_time = (int)(DateTime.Now - starttime).TotalSeconds;
           }
       }
       private IEnumerator<float> ShowInfo()
       {
           yield return Timing.WaitForSeconds(0.01f);

           string txt = "<size=25>MVP 时刻 MVP\n《——————————《=w=》——————————》\n";
           Player tmpplr1 = null;
           int maxfen = 0;
           Dictionary<int, int> ranks = new Dictionary<int, int>();
           foreach (var p in Player.GetPlayers())
           {
               try
               {
                   var killnum = 0;
                   var damage = 0;
                   if (KillNum.TryGetValue(p.PlayerId, out var value))
                   {
                       killnum = value;
                   }
                   if (TotalDamege.TryGetValue(p.PlayerId, out var value1))
                   {
                       damage = value1;
                   }
                   int fen = killnum * 1 + (int)(((float)damage * 0.1f));
                   ranks.Add(p.PlayerId, fen);
                   if (fen > maxfen)
                   {
                       maxfen = fen;
                       tmpplr1 = p;
                   }
               }
               catch
               {

               }
           }
           txt += "<b>本局MVP " + tmpplr1.Nickname + " 得分 " + maxfen + "</b>\n";
           txt += "\n\n你的得分:{yoursorce} 排名:{rank}";
           txt += "</size>";
           for (int i = 0; i < 32; i++)
           {
               txt += "\n";
           }
           var sortedDict = ranks.OrderByDescending(x => x.Value).ToDictionary(x => x.Key, x => x.Value);
           if (Player.GetPlayers().Count() > 5)
           {
               foreach (Player player in Player.GetPlayers())
               {
                   if (tmpplr1 != null)
                   {
                       if (player.PlayerId == tmpplr1.PlayerId)
                       {
                           continue;
                       }
                   }
               }
           }
           for (int i = 0; i < 10; i++)
           {
               yield return Timing.WaitForSeconds(0.5f);
               foreach (Player player in Player.GetPlayers())
               {
                   int rank = 0;
                   foreach (var awa in sortedDict)
                   {
                       rank++;
                       if (awa.Key == player.PlayerId)
                       {
                           break;
                       }
                   }
                   player.ReceiveHint(txt.Replace("{yoursorce}", ranks[player.PlayerId].ToString()).Replace("{rank}", rank.ToString()), 10);
               }
           }

       }

       [PluginEvent(PluginAPI.Enums.ServerEventType.RoundEnd)]
       void RoundEnd(RoundSummary.LeadingTeam leadingTeam)
       {
           Timing.RunCoroutine(ShowInfo());
       }
       [PluginEvent(PluginAPI.Enums.ServerEventType.PlayerDying)]
       void PlayerDying(Player player, Player attacker, PlayerStatsSystem.DamageHandlerBase damageHandler)
       {
           try
           {
               if (attacker != null)
               {
                   if (KillNum.ContainsKey(attacker.PlayerId))
                   {
                       KillNum[attacker.PlayerId]++;
                       if (attacker.Team == PlayerRoles.Team.SCPs)
                       {
                           KillNum[attacker.PlayerId] += 4;
                       }
                   }
                   else
                   {
                       KillNum.Add(attacker.PlayerId, 1);
                   }
               }
           }
           catch
           {

           }
       }
       [PluginEvent]
       void PlayerDamage(PlayerDamageEvent ev)
       {
           try
           {
               int tmpdamage = 0;
               if (ev.Player != null)
               {
                   if (ev.DamageHandler is StandardDamageHandler standardDamageHandler)
                   {
                       if (standardDamageHandler.Damage <= -0.9)
                       {
                           tmpdamage = (int)ev.Target.Health;
                       }
                       else if (standardDamageHandler.Damage >= ev.Target.Health)
                       {
                           tmpdamage = (int)ev.Target.Health;
                       }
                       else
                       {
                           tmpdamage = (int)standardDamageHandler.Damage;
                       }

                       if (TotalDamege.ContainsKey(ev.Player.PlayerId))
                       {
                           TotalDamege[ev.Player.PlayerId] += tmpdamage;
                       }
                       else
                       {
                           TotalDamege.Add(ev.Player.PlayerId, tmpdamage);
                       }
                   }
               }
           }
           catch
           {

           }

       }
       [PluginEvent]
       void RoundRestart(RoundRestartEvent ev)
       {
           firstescapeplayer = null;
           first_player_escape_time = 0;
           TotalDamege.Clear();
           KillNum.Clear();
       }
   }
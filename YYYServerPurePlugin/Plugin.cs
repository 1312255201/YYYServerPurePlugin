using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using InventorySystem.Items.Pickups;
using MEC;
using Mirror;
using PlayerRoles;
using PlayerRoles.Ragdolls;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;
using YYYServerPurePlugin.MyApi;
using YYYServerPurePlugin.ServerFuctions;
using YYYServerPurePlugin.ServerFuctions.MapFuc;
using YYYServerPurePlugin.ServerFuctions.SeverEvent;

namespace YYYServerPurePlugin
{
    public class Plugin
    {
        private static List<ushort> items_to_clean = new();
        [PluginEntryPoint("YYYServerPurePlugin", "0.0.1", "嘤嘤嘤服务器的纯净服务器插件2.0", "咕咕鱼")]
        void OnEabled()
        {
            Log.Info("服务器开始加载了");
            Log.Info("垃圾清理");
            EventManager.RegisterEvents(this);
            Log.Info("核弹室扣血");
            EventManager.RegisterEvents<NuclearRadiation>(this);
            Log.Info("UI系统");
            EventManager.RegisterEvents<HintMainClass>(this);
            Log.Info("MVP系统");
            EventManager.RegisterEvents<MvpFuc>(this);
            IniFile.miyao = config.miyao;
        }
       [PluginEvent(PluginAPI.Enums.ServerEventType.RoundStart)]
        void OnRoundStart()
        {
            items_to_clean.Clear();
            Timing.RunCoroutine(CleanFloorPlugin());
            foreach (var variablePlayer in Player.GetPlayers())
            {
                MyApi.MyApi.SetNick(variablePlayer);
            }
        }

        [PluginEvent]
        void OnPlayerJoin(PlayerJoinedEvent ev)
        {
            MyApi.MyApi.SetNick(ev.Player);
        }

        [PluginEvent]
         void OnRestartingRound(RoundRestartEvent ev)
        {
            Timing.CallDelayed(0.5f, () => {
                var tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                IPAddress ipaddress = IPAddress.Parse("127.0.0.1");
                EndPoint point = new IPEndPoint(ipaddress, Server.Port + 1000);
                tcpClient.Connect(point);
                tcpClient.Send(Encoding.UTF8.GetBytes(Server.Port.ToString()));
            });
        }
        [PluginEvent]
        void OnPlayerChangingRole(PlayerChangeRoleEvent ev)
        {
            if (ev.NewRole == RoleTypeId.FacilityGuard)
            {
                Timing.CallDelayed(1f, () => {
                    ev.Player.AddAmmo(ItemType.Ammo9x19,60);
                });
            }
        }
        private static IEnumerator<float> CleanFloorPlugin()
        {
            List<ushort> startItem = new List<ushort>();
            ItemPickupBase[] startItemPickupBases = UnityEngine.Object.FindObjectsOfType<ItemPickupBase>();
            foreach (ItemPickupBase itemPickupBase in startItemPickupBases)
            {
                startItem.Add(itemPickupBase.Info.Serial);
            }
            yield return Timing.WaitForSeconds(1); //注意这里最好稍微延迟下，有可能在开始的时候NWapi的Round.IsRoundStarted不一定是true导致直接退出
            int time = 0;
            while (Round.IsRoundStarted)
            {
                yield return Timing.WaitForSeconds(1);
                time++;
                switch(time)
                {
                    case 1:
                        Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n服务器将会在<color=#0F0>400s</color>后清理服务器哦\n来啦来啦",10);
                        break;
                    case 200:
                        Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n服务器将会在<color=#FF0>200s</color>后清理服务器哦\n请不要把贵重物品放在地上哦", 10);
                        break;
                    case 350:
                        Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n服务器将会在<color=#F00>50s</color>后清理服务器哦\n还有50s了注意注意", 10);
                        break;
                    case 390:
                        Server.ClearBroadcasts();
                        for(int i = 10; i >= 0;i--)
                        {
                            Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n服务器将会在<color=#F00>" +i+ "</color>后清理服务器哦\n请不要把贵重物品放在地上哦", 1);
                        }
                        break;
                    case 400:
                        Server.ClearBroadcasts();
                        Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n开始清理", 5);
                        break;
                    case 410:
                        time = 0;//重置然后重新开始计时
                        int ragdollnum = 0;//记录清理了几个布娃娃
                        int itemnum = 0;//记录清理了几个物品
                        BasicRagdoll[] ragdolls = UnityEngine.Object.FindObjectsOfType<BasicRagdoll>();
                        foreach(BasicRagdoll basicRagdoll in ragdolls)
                        {
                            ragdollnum++;
                            NetworkServer.Destroy(basicRagdoll.gameObject);
                        }
                        ItemPickupBase[] itemPickupBases = UnityEngine.Object.FindObjectsOfType<ItemPickupBase>();
                        foreach (ItemPickupBase itemPickupBase in itemPickupBases)
                        {
                            if(itemPickupBase.Info.ItemId.IsScpItem() || itemPickupBase.Info.ItemId.IsKeycard() || startItem.Contains(itemPickupBase.Info.Serial))
                            {
                                continue;
                            }
                            if (items_to_clean.Contains(itemPickupBase.Info.Serial))
                            {
                                NetworkServer.Destroy(itemPickupBase.gameObject);
                            }
                            else
                            {
                                items_to_clean.Add(itemPickupBase.Info.Serial);
                            }
                            itemnum++;
                        }
                        Server.SendBroadcast("[<color=#0FF>小鱼服务器清理大师</color>]\n好饱啊，本次一共清理了\n" + itemnum + "件物品" + ragdollnum + "个尸体",10);
                        break;

                }

            }
        }
        [PluginConfig]
        public Config config;
    }
}

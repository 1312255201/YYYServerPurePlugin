using System.Collections.Generic;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Zones;
using YYYServerPurePlugin.ServerFuctions;

namespace YYYServerPurePlugin.MyApi
{
    public static class MyAPIStatic
    {
        public static bool IsScpItem(this ItemType type) => type == ItemType.SCP018|| type == ItemType.SCP330 || type == ItemType.SCP1576 || type == ItemType.SCP500 || type == ItemType.SCP268 || type == ItemType.SCP207 || type == ItemType.SCP244a || type == ItemType.SCP244b || type == ItemType.SCP2176 || type == ItemType.SCP1853 || type == ItemType.AntiSCP207;
        public static bool IsKeycard(this ItemType type) => type == ItemType.KeycardJanitor || type == ItemType.KeycardScientist || type == ItemType.KeycardResearchCoordinator || type == ItemType.KeycardZoneManager || type == ItemType.KeycardGuard || type == ItemType.KeycardMTFPrivate || type == ItemType.KeycardContainmentEngineer || type == ItemType.KeycardMTFOperative || type == ItemType.KeycardMTFCaptain || type == ItemType.KeycardFacilityManager || type == ItemType.KeycardChaosInsurgency || type == ItemType.KeycardO5;

        public static void ReloadWeapen(this Player player)
        {
            if (player.CurrentItem == null)
            {
                //手上没有物品
                return;
            }
            else
            {
                if (player.CurrentItem is Firearm firearm)
                {
                    firearm.AmmoManagerModule.ServerTryReload();
                    player.Connection.Send<RequestMessage>(new RequestMessage(firearm.ItemSerial,RequestType.Reload));
                }
                else
                {
                    //持有的武器不是一个武器
                }
            }
        }
    }

    public class MyApi
    {
            public static Dictionary<Team, string> TeamTranslation = new()
            {
                { Team.Dead ,"阵亡"},
                { Team.Scientists ,"科学家"},
                { Team.FoundationForces ,"九尾狐"},
                { Team.SCPs ,"SCP"},
                { Team.OtherAlive ,"其他存活"},
                { Team.ChaosInsurgency ,"混沌分裂者"},
                { Team.ClassD ,"D级人员"},
            };
            public static Dictionary<MapGeneration.FacilityZone, string> zoneTranslation = new() {
                {MapGeneration.FacilityZone.Surface,"地表" },
                {MapGeneration.FacilityZone.Other,"其他" },
                {MapGeneration.FacilityZone.HeavyContainment,"重收容" },
                {MapGeneration.FacilityZone.Entrance,"办公区" },
                {MapGeneration.FacilityZone.LightContainment,"轻收容" },
                {MapGeneration.FacilityZone.None,"未知" },
            };
            public static Dictionary<RoleTypeId, string> TranslateOfRoleType = new()
            {
                {RoleTypeId.NtfPrivate,"九尾狐新兵" },
                {RoleTypeId.NtfCaptain,"九尾狐指挥官" },
                {RoleTypeId.NtfSergeant,"九尾狐中士" },
                {RoleTypeId.NtfSpecialist,"九尾狐收容专家" },
                {RoleTypeId.FacilityGuard,"设施保安" },
                {RoleTypeId.ChaosConscript,"混沌征召兵" },
                {RoleTypeId.ChaosMarauder,"混沌掠夺者" },
                {RoleTypeId.ChaosRepressor,"混沌镇压者" },
                {RoleTypeId.ChaosRifleman,"混沌抢手" },
                {RoleTypeId.Scp096,"SCP-096" },
                {RoleTypeId.Scp049,"SCP-049" },
                {RoleTypeId.Scp173,"SCP-173" },
                {RoleTypeId.Scp939,"SCP-939" },
                {RoleTypeId.Scp106,"SCP-106" },
                {RoleTypeId.Scp0492,"SCP-049-2" },
                {RoleTypeId.Scp079,"SCP-079" },
                {RoleTypeId.ClassD,"D级人员" },
                {RoleTypeId.Scientist,"科学家" },
                {RoleTypeId.Tutorial,"训练人员" },
                {RoleTypeId.Overwatch,"观察者" },
                {RoleTypeId.CustomRole,"本地角色？" },
                {RoleTypeId.Spectator,"观察者" },
                {RoleTypeId.Filmmaker,"导演模式" },
                {RoleTypeId.None,"空" },
                { RoleTypeId.Scp3114, "SCP-3114" },
            };
        public static void SetNick(Player hub)
        {
            var exp = IniFile.MyExp(hub);
            SetNick(hub,exp);
        }
        public static void SetNick(Player hub, int exp)
        {
            if (!Round.IsRoundStarted)
            {
                var lv = IniFile.ReadLevel2(exp);
                if (!hub.ReferenceHub.serverRoles.GlobalSet)
                    hub.ReferenceHub.nicknameSync.Network_displayName = "[Lv." + lv + "]" +hub.Nickname;
                hub.ReferenceHub.characterClassManager.SyncServerCmdBinding();
            }
            else
            {
                var lv = IniFile.ReadLevel(exp);
                if (!hub.ReferenceHub.serverRoles.GlobalSet)
                    hub.ReferenceHub.nicknameSync.Network_displayName = "[Lv." + lv + "]" + hub.Nickname;
                hub.ReferenceHub.characterClassManager.SyncServerCmdBinding();
            }
        }
    }
}
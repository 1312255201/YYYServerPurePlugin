using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.BasicMessages;
using PluginAPI.Core;
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
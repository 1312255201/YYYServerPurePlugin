using PlayerRoles;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace YYYServerPurePlugin.ServerFuctions.SeverEvent;

public class ExpSystem
{
    [PluginEvent]
    void OnPlayerDeath(PlayerDeathEvent ev)
    {
        if (ev.Attacker != null)
        {
            if (ev.Attacker.Team == Team.SCPs)
            {
                
            }
        }
    }
}
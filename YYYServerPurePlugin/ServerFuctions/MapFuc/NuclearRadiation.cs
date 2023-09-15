using System.Collections.Generic;
using System.Linq;
using MapGeneration;
using MEC;
using PlayerRoles;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace YYYServerPurePlugin.ServerFuctions.MapFuc;

public class NuclearRadiation
{
    [PluginEvent]
    void OnRoundStart(RoundStartEvent ev)
    {
        Timing.RunCoroutine(NuclearRadiationFuc());
    }

    private static IEnumerator<float> NuclearRadiationFuc()
    {
        List<int> nukes = new List<int>();
        yield return Timing.WaitForSeconds(1f);
        while (Round.IsRoundStarted)
        {
            yield return Timing.WaitForSeconds(10);
            foreach (var variablPlayer in Player.GetPlayers())
            {
                try
                {
                    if (variablPlayer.Room.Name == RoomName.HczWarhead&& variablPlayer.Position.y > -800)
                    {
                        if (nukes.Contains(variablPlayer.PlayerId))
                        {
                            if (variablPlayer.Team == Team.SCPs)
                            {
                                variablPlayer.Health -= 10;
                                if (variablPlayer.Health <= 0)
                                {
                                    variablPlayer.Kill("你被核弹辐射死了");
                                }
                            }
                            else
                            {
                                variablPlayer.Damage(5,"核弹室辐扣血");
                            }
                        }
                        else
                        {
                            nukes.Add(variablPlayer.PlayerId);
                            variablPlayer.SendBroadcast("<size=50><color=#F00>[Warning]</color></size>\n这里的<color=#F00>这里的辐射很强</color>不可久留",10);
                        }
                    }
                    else
                    {
                        if (nukes.Contains(variablPlayer.PlayerId))
                        {
                            nukes.Remove(variablPlayer.PlayerId);
                        }
                    }
                }
                catch
                {
                    
                }
            }
        }
    }

}
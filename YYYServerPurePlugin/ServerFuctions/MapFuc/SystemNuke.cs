using System.Collections.Generic;
using MEC;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Events;

namespace YYYServerPurePlugin.ServerFuctions.MapFuc;

public class SystemNuke
{
    private static int stoptime = 0;
    private static CoroutineHandle _systemNukeTiming;
    [PluginEvent]
    void OnRoundstart(RoundStartEvent ev)
    {
        _systemNukeTiming =  Timing.RunCoroutine(SystemNukeTiming());
    }

    public static IEnumerator<float> SystemNukeTiming()
    {
        yield return Timing.WaitForSeconds(1500);
        if (!Warhead.IsDetonated)
        {
            Map.Broadcast(10,"<size=50><color=#F00>[Warning]</color></size>\nO5议会决定远程放弃站点-决定远程激活核弹\n"+"<color=#F00>5分钟</color>后核弹将会强制开启");
        }
        yield return Timing.WaitForSeconds(300);
        if (!Warhead.IsDetonated)
        {
            Warhead.Stop();
            Warhead.Start();
            Warhead.IsLocked = true;
            Map.Broadcast(10,"<size=50><color=#F00>[Warning]</color></size>\n系统核弹已经开启\n"+"各单位<color=#F00>快速撤离</color>");
        }
    }
    [PluginEvent]
    void OnRoundRestart(RoundRestartEvent ev)
    {
        stoptime = 0;
        if (_systemNukeTiming.IsRunning)
        {
            Timing.KillCoroutines(_systemNukeTiming);
        }
    }

    [PluginEvent]
    void OnWarheadStop(WarheadStopEvent ev)
    {
        stoptime++;
        Map.Broadcast(10,"<size=50><color=#F00>[Warning]</color></size>\n核弹已经被关闭<color=#F00>"+stoptime+"</color>次了\n"+"超过6次核弹将会生气无法关闭");
        if (stoptime > 6)
        {
            Timing.CallDelayed(1f, () => {
                Warhead.Start();
                Warhead.IsLocked = true;
            });
        }
    }
}
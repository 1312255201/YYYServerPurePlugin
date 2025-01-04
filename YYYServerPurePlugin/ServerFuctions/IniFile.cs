using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using PluginAPI.Core;
using YYYServerPurePlugin.ServerFuctions.MapFuc;

namespace YYYServerPurePlugin.ServerFuctions
{
public class IniFile
{
         #region 读Ini文件
        private static string _url = "http://127.0.0.1:4579/";
        public static string miyao = "";
        public static Dictionary<string, int> expnum = new Dictionary<string, int>();
        public static int ReadLevel(int exp)
        {
            int Lv = 0;
            Lv = exp / 1000;
            return Lv;
        }
        public static string ReadLevel2(int exp)
        {
            int Lv = 0;
            string Lv2 = "";
            Lv = ReadLevel(exp);
            if(Lv <=10)
            {
                Lv2 = Lv + "|Neutralized";
            }
            if(Lv >10 && Lv <=30)
            {
                Lv2 = Lv + "|Safe";

            }
            if (Lv > 30 && Lv <= 60 )
            {
                Lv2 = Lv + "|Euclid";

            }
            if (Lv > 60 && Lv <= 100)
            {
                Lv2 = Lv + "|Keter";
            }
            if (Lv > 100 && Lv <= 150)
            {
                Lv2 = Lv + "|Thaumiel";
            }
            if (Lv > 150)
            {
                Lv2 = Lv + "|O5";
            }
            return Lv2;
        }
        public static int MyExp(Player p,bool gengxin = false)
        {
            if (expnum.ContainsKey(p.UserId) && !gengxin)
            {
                return expnum[p.UserId];
            }
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("userid", p.UserId);
            param.Add("miyao",miyao);
            string a = Get(_url + "myExp", param);
            if(a == "error")
            {
                return 0;
            }
            if (int.Parse(a) > 0)
            {
                expnum[p.UserId] = int.Parse(a);
            }
            return(int.Parse(a));
        }
        public static void AddExp2(string p, int exp)
        {
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("userid", p);
            param.Add("nickname", Player.Get(p).Nickname);
            param.Add("exp", exp.ToString());
            param.Add("miyao",miyao);
            new Task(() =>
            {
                Get(_url + "addExp", param);
            }).Start();
            HintMainClass.AddTempHint(Player.Get(p), "恭喜你获得了" + exp + "点经验",5,ScreenType.TOP);
        }
        public static void AddExp(string p, int exp)
        {
            var player = Player.Get(p);
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("userid", p);
            param.Add("nickname", Player.Get(p).Nickname);
            param.Add("exp", exp.ToString());
            param.Add("miyao",miyao);
            new Task(() =>
            {
                Get(_url + "addExp", param);
                MyApi.MyApi.SetNick(Player.Get(p));
            }).Start();

            if (expnum.ContainsKey(player.UserId))
            {
                expnum[player.UserId] += exp;
            }
            HintMainClass.AddTempHint(player, "恭喜你获得了" + exp + "点经验",5,ScreenType.TOP);
        }
        public static string Get(string url, Dictionary<string, string> dic = null)
        {
            string result = "";

            if (dic == null)
            {
                dic = new Dictionary<string, string>();
            }

            StringBuilder builder = new StringBuilder(url);
            builder.Append("?");
            builder.Append(string.Join("&", dic.Select(kv => $"{kv.Key}={kv.Value}")));

            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
                req.KeepAlive = false;

                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                using (Stream stream = resp.GetResponseStream())
                {
                    if (stream != null)
                    {
                        using (StreamReader reader = new StreamReader(stream))
                        {
                            result = reader.ReadToEnd();
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                Log.Info($"HTTP请求失败: {ex.Message}");
                result = "error";
            }

            return result;
        }
        #endregion
    }
}
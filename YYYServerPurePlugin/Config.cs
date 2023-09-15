using System.ComponentModel;

namespace YYYServerPurePlugin
{
    public class Config
    {
        [Description("是否开启Debug")]
        public bool Debug { get; set; } = true;
    }
}
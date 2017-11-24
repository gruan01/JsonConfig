using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsNum.JsonConfig
{
    public abstract class JsonConfigItem
    {

        /// <summary>
        /// 
        /// </summary>
        public event EventHandler Changed;

        /// <summary>
        /// Config file's name
        /// </summary>
        [JsonIgnore]
        public abstract string CfgFile { get; }

        /// <summary>
        /// Config file's path
        /// </summary>
        [JsonIgnore]
        public string CfgPath
        {
            get
            {
                return Path.Combine(JsonConfig.BaseDir, this.CfgFile);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual JsonConfigItem Load()
        {
            if (File.Exists(this.CfgPath))
            {
                try
                {
                    var old = this;

                    var json = File.ReadAllText(this.CfgPath);
                    var n = (JsonConfigItem)JsonConvert.DeserializeObject(json, this.GetType());
                    this.Changed?.DynamicInvoke(this, new EventArgs());
                    return n;
                }
                catch
                {

                }
            }

            return null;
        }
    }
}

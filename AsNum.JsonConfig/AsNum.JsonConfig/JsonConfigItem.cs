using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsNum.JsonConfig
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class JsonConfigItem
    {
        /// <summary>
        /// 
        /// </summary>
        public event EventHandler<ChangedEventArg> Changed;



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
                    var lw = File.GetLastWriteTime(this.CfgPath);

                    var old = JsonConvert.SerializeObject(this, Formatting.None);

                    var json = File.ReadAllText(this.CfgPath, Encoding.UTF8);
                    var n = (JsonConfigItem)JsonConvert.DeserializeObject(json, this.GetType());

                    DynamicCopy.CopyTo(n, this);

                    this.Changed?.DynamicInvoke(this, new ChangedEventArg()
                    {
                        NewJson = json,
                        OldJson = old
                    });

                    return this;
                }
                catch (Exception e)
                {
                    JsonConfig.OnError(this, e);
                }
            }

            return null;
        }
    }
}

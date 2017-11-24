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
    public class JsonConfig
    {

        /// <summary>
        /// Json Config file save path.
        /// </summary>
        public static string BaseDir
        {
            get
            {
                return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Cfgs");
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private static Dictionary<string, JsonConfigItem> Map
        {
            get;
        } = new Dictionary<string, JsonConfigItem>();

        static JsonConfig()
        {
            Watch();
        }

        /// <summary>
        /// Regist Config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Regist<T>() where T : JsonConfigItem, new()
        {
            var cfg = new T();

            var key = Path.GetFileNameWithoutExtension(cfg.CfgFile.ToLower());

            if (!Map.ContainsKey(key))
            {
                Map.Add(key, cfg);
                Reload(key);
            }
        }

        /// <summary>
        /// Watch file change
        /// </summary>
        private static void Watch()
        {
            if (!Directory.Exists(BaseDir))
                Directory.CreateDirectory(BaseDir);

            var fw = new FileSystemWatcher(BaseDir, "*.json");
            fw.Changed += Fw_Changed;
            fw.Created += Fw_Created;
            fw.Deleted += Fw_Deleted;
            fw.EnableRaisingEvents = true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Fw_Deleted(object sender, FileSystemEventArgs e)
        {
            var key = GetKey(e.Name);
            if (Map.ContainsKey(key))
            {
                Map.Remove(key);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Fw_Created(object sender, FileSystemEventArgs e)
        {
            Reload(e.Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void Fw_Changed(object sender, FileSystemEventArgs e)
        {
            Reload(e.Name);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetKey(string name)
        {
            return Path.GetFileNameWithoutExtension(name).ToLower();
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        private static void Reload(string name)
        {
            var key = GetKey(name);
            if (Map.ContainsKey(key))
            {
                var o = Map[key];
                var no = o.Load();
                if (no != null)
                {
                    Map[key] = no;
                }
            }
        }


        /// <summary>
        /// Get Config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T Get<T>() where T : JsonConfigItem
        {
            var type = typeof(T);
            var a = Map.Values.FirstOrDefault(t => type.IsInstanceOfType(t));
            return (T)a;
        }


        /// <summary>
        /// Save Config to file
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cfg"></param>
        public static void Save<T>(T cfg) where T : JsonConfigItem
        {
            if (cfg == null)
                throw new ArgumentNullException("cfg");

            var key = Path.GetFileNameWithoutExtension(cfg.CfgFile.ToLower());

            if (Map.ContainsKey(key))
                Map[key] = cfg;
            else
                Map.Add(key, cfg);

            var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);
            var file = Path.Combine(BaseDir, $"{cfg.CfgFile}");

            File.WriteAllText(file, json);
        }
    }
}

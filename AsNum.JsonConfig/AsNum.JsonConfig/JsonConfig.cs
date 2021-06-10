using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace AsNum.JsonConfig
{
    /// <summary>
    /// 
    /// </summary>
    public static class JsonConfig
    {

        /// <summary>
        /// Json Config file save path.
        /// </summary>
        public static string BaseDir { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        public static event EventHandler<ErrorEventArg> Error;

        /// <summary>
        /// 
        /// </summary>
        private static readonly Dictionary<string, JsonConfigItem> map = new Dictionary<string, JsonConfigItem>();


        /// <summary>
        /// 
        /// </summary>
        private static bool initlized = false;


        /// <summary>
        /// 
        /// </summary>
        private static readonly FileSystemWatcher fw;

        /// <summary>
        /// 
        /// </summary>
        static JsonConfig()
        {

            fw = new FileSystemWatcher
            {
                Filter = "*.json"
            };

            fw.Changed += Fw_Changed;
            fw.Created += Fw_Created;
            fw.Deleted += Fw_Deleted;
            //在这里调用会报错
            //fw.EnableRaisingEvents = true;

#if NETFULL
            var webSitePath = AppDomain.CurrentDomain.SetupInformation.PrivateBinPath;
#else
            string webSitePath = null;
#endif
            var exePath = AppDomain.CurrentDomain.BaseDirectory;
            //if PrivateBinPath is not null, this may be run as a website
            //if PrivateBinPath is null, it must be a exe.
            // App_Data is a security folder, Config file in this folder is safe.
            BaseDir = Path.Combine(exePath, webSitePath != null ? "App_Data" : "", "Cfgs");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <param name="e"></param>
        internal static void OnError(JsonConfigItem item, Exception e)
        {
            Error?.DynamicInvoke(item, new ErrorEventArg()
            {
                CfgFilePath = item?.CfgPath,
                Exception = e,
                Msg = e?.Message
            });
        }

        /// <summary>
        /// if you want manually specify base dir, you must use Init before Regist.
        /// </summary>
        /// <param name="baseDir"></param>
        public static void Init(string baseDir = null)
        {
            if (!initlized)
            {
                if (!string.IsNullOrWhiteSpace(baseDir))
                    BaseDir = baseDir;

                Watch(BaseDir);
                initlized = true;
            }
        }

        /// <summary>
        /// Regist Config
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Regist<T>() where T : JsonConfigItem, new()
        {
            var cfg = new T();
            Regist(cfg);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cfg"></param>
        public static void Regist<T>(T cfg) where T : JsonConfigItem
        {
            if (cfg == null)
                throw new ArgumentNullException(nameof(cfg));

            if (!initlized)
                Init();

            var key = Path.GetFileNameWithoutExtension(cfg.CfgFile.ToLower());

            if (!map.ContainsKey(key))
            {
                map.Add(key, cfg);
                Reload(key);
            }
        }

        /// <summary>
        /// Watch file change
        /// </summary>
        private static void Watch(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            fw.Path = dir;
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
            if (map.ContainsKey(key))
            {
                map.Remove(key);
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
            if (map.ContainsKey(key))
            {
                var o = map[key];
                var no = o.Load();
                if (no != null)
                {
                    map[key] = no;
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
            var a = map.Values.FirstOrDefault(t => type.IsInstanceOfType(t));
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

            if (map.ContainsKey(key))
                map[key] = cfg;
            else
                map.Add(key, cfg);

            var json = JsonConvert.SerializeObject(cfg, Formatting.Indented);

            File.WriteAllText(cfg.CfgPath, json);
        }
    }
}

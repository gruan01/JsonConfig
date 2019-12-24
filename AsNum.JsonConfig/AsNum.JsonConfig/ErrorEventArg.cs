using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsNum.JsonConfig
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorEventArg : EventArgs
    {

        /// <summary>
        /// 
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CfgFilePath { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Msg { get; set; }
    }
}

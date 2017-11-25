using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsNum.JsonConfig
{
    public class ChangedEventArg : EventArgs
    {

        public string OldJson { get; set; }

        public string NewJson { get; set; }

    }
}

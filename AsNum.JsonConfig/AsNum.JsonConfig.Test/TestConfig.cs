﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsNum.JsonConfig.Test
{
    public class TestConfig : JsonConfigItem
    {
        public override string CfgFile => "TestConfig.json";

        public IEnumerable<Test> Datas { get; set; }

        public override bool IsSecurity => true;

        public class Test
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public DateTime CreateOn { get; set; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AsNum.JsonConfig.CoreWebTest
{
    public class TestConfig : JsonConfigItem
    {
        public override string CfgFile => "TestConfig.json";

        public IEnumerable<Test> Datas { get; set; }


        public class Test
        {
            public int ID { get; set; }

            public string Name { get; set; }

            public DateTime CreateOn { get; set; }
        }
    }
}

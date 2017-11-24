using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AsNum.JsonConfig.Test
{
    [TestClass]
    public class UnitTest1
    {
        [TestInitialize]
        public void Init()
        {
            JsonConfig.Regist<TestConfig>();
            var tc = JsonConfig.Get<TestConfig>();
            tc.Changed += (sender, e) =>
            {
                Console.WriteLine("Changed");
            };
        }

        [TestMethod]
        public void TestMethod1()
        {
            var tc = JsonConfig.Get<TestConfig>();
            var datas = tc.Datas;
            tc.Datas = new List<TestConfig.Test>()
            {
                new TestConfig.Test()
                {
                    ID = 0,
                     Name = "Xling",
                     CreateOn = DateTime.Now
                }
            };

            JsonConfig.Save(tc);
        }
    }
}

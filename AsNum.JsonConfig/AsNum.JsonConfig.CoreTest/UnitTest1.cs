using System;
using System.Collections.Generic;
using System.Diagnostics;
using Xunit;

namespace AsNum.JsonConfig.CoreTest
{
    public class UnitTest1
    {
        public UnitTest1()
        {
            //JsonConfig.Init();
            JsonConfig.Regist<TestConfig>();
            var tc = JsonConfig.Get<TestConfig>();
            tc.Changed += (sender, e) =>
            {
                //DebriteLine("Changed");
                Debug.WriteLine($"changed...{e.NewJson}");
            };
        }

        [Fact]
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

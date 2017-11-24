# JsonConfig
Json Config for .NET Framework , support hot fix.

## How to use:

### 1, Regist in Global or Program:
~~~
JsonConfig.Init();
JsonConfig.Regist<TestConfig>();
~~~

### 2, Read:
~~~
var tc = JsonConfig.Get<TestConfig>();
~~~

### 3, save:
~~~
JsonConfig.Save(tc);
~~~

## Nuget
https://www.nuget.org/packages/AsNum.JsonConfig/

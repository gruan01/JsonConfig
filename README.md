# JsonConfig
Json Config for .NET Framework , support hot fix.

## How to use:

### 1, Regist in Global or Program:
~~~
JsonConfig.Regist<TestConfig>();
~~~

### 2, Read:
~~~
var tc = JsonConfig.Get<TestConfig>();
~~~

### 3, Save change:
~~~
JsonConfig.Save(tc);
~~~

### If you want manully specify config file path, you must use Init before Regist.
~~~
JsonConfig.Init(...);
JsonConfig.Regist<...>();
~~~

## Default Path:
* Exe : ./Cfgs
* WebSite: ./App_Data/Cfgs (App_Data is a security folder, you can't access it from the internet, so Config files in it is very safe.)

## It support hot fix.
If you want watch change, you can do like this:
~~~
  JsonConfig.Regist<ZowoYooApiConfig>();
  var cfg = JsonConfig.Get<ZowoYooApiConfig>();
  cfg.Changed += Cfg_Changed;
}

private void Cfg_Changed(object sender, ChangedEventArg e)
{
  Debug.WriteLine(e.NewJson);
}
~~~

## Nuget
https://www.nuget.org/packages/AsNum.JsonConfig/

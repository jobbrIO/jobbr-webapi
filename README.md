# Jobbr Web API Extension [![Develop build status][webapi-badge-build-develop]][webapi-link-build]

Adds Rest-style api to a Jobbr-Server and and provides a strong typed .NET Client for the [Jobbr .NET JobServer](http://www.jobbr.io). 
The Jobbr main repository can be found on [JobbrIO/jobbr-server](https://github.com/jobbrIO).

[![Master build status][webapi-badge-build-master]][webapi-link-build] 
[![NuGet-Stable][webapi-badge-nuget]][webapi-link-nuget]
[![Develop build status][webapi-badge-build-develop]][webapi-link-build] 
[![NuGet Pre-Release][webapi-badge-nuget-pre]][webapi-link-nuget] 

## Installation
First of all you'll need a working jobserver by using the usual builder as shown in the demos ([jobbrIO/jobbr-demo](https://github.com/jobbrIO/jobbr-demo)).

### NuGet
Install the NuGet `Jobbr.Server.WebAPI` to the project where you host you Jobbr-Server. The extension already comes with a small webserver based on OWIN/Katana. The referenced HttpListenr will be installed by NuGet automatically.

	Install-Package Jobbr.Server.WebAPI


### Registration
The Library comes with an extension method for the `JobbrBuilder`. To add the Web API to a Jobbr-Server you need to register it prior start as you see below. Please note that this is not ASP.NET WebAPI when registering it to an OWIN Pipeline, allthough we're using the same principle. (In fact, we're using WebAPI internally :smile: )

```c#
using Jobbr.Server.WebAPI;

// ....

// Create a new builder which helps to setup your JobbrServer
var builder = new JobbrBuilder();

// Register the extension
builder.AddWebApi();

// Create a new instance of the JobbrServer
var server = builder.Create();

// Start
server.Start();
```

### Configuration
If you don't specify any value for `BackendAddress` the server will try to find a free port automatically and binds to all available interfaces. The endpoint is logged and usually shown in the console, but this approach is not suggested in production scenarios, see below:

	[WARN]  (Jobbr.Server.WebAPI.Core.WebHost) There was no BackendAdress specified. Falling back to random port, which is not guaranteed to work in production scenarios
	....
	[INFO]  (Jobbr.Server.JobbrServer) The configuration was validated and seems ok. Final configuration below:
	JobbrWebApiConfiguration = [BackendAddress: "http://localhost:1903/api"]

You can override this behavior, by explicitly providing your own URI prefix, for instance `http://localhost:8765/api`. See example below:

```c#
builder.AddWebApi(config => 
{
	config.BaseUrl = "http://localhost:8765/api";
});
```
**Note**: Please refer to https://msdn.microsoft.com/en-us/library/system.net.httplistener(v=vs.110).aspx for the supported URI prefixes depending on your operating system and .NET Runtime version.

## [Rest API Reference](https://jobbr.readthedocs.io/en/latest/use/restApi.html#rest-api-reference)
Please note that the API documentation has moved to [jobbr.readthedocs.io](https://jobbr.readthedocs.io/en/latest/use/restApi.html#rest-api-reference) 


## Static Typed Client
There is also a static typed client available which you can use to interact with any Jobbr Rest Api. Please see the documentation on [jobbr.readthedocs.io](https://jobbr.readthedocs.io/en/latest/use/restApi.html#static-typed-c-client) for more details

# License
This software is licenced under GPLv3. See [LICENSE](LICENSE) and the related licences of 3rd party libraries below.

# Acknowledgements
Jobbr Server is based on the following awesome libraries:
* [LibLog](https://github.com/damianh/LibLog) [(MIT)](https://github.com/damianh/LibLog/blob/master/licence.txt)
* [Microsoft.AspNet.WebApi.Client](https://www.asp.net/web-api) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm)
* [Microsoft.AspNet.WebApi.Core](https://www.asp.net/web-api) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm)
* [Microsoft.AspNet.WebApi.Owin](https://www.asp.net/web-api) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm)
* [Microsoft.Owin](https://github.com/aspnet/AspNetKatana/) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm)
* [Microsoft.Owin.Host.HttpListener](https://github.com/aspnet/AspNetKatana/) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm)
* [Microsoft.Owin.Hosting](https://github.com/aspnet/AspNetKatana/) [(MS .NET Library Eula)](https://www.microsoft.com/web/webpi/eula/net_library_eula_enu.htm) 
* [Newtonsoft Json.NET](https://github.com/JamesNK/Newtonsoft.Json) [(MIT)](https://github.com/JamesNK/Newtonsoft.Json/blob/master/LICENSE.md)
* [Owin](https://github.com/owin-contrib/owin-hosting) [(Apache-2.0)](https://github.com/owin-contrib/owin-hosting/blob/master/LICENSE.txt)

# Credits
This extension was built by the following awesome developers:
* [Michael Schnyder](https://github.com/michaelschnyder)
* [Oliver Zürcher](https://github.com/olibanjoli)
* [Tobias Zürcher](https://github.com/tobiaszuercher)
* [Steven Giesel](https://github.com/linkdotnet)
* [Lukas Dürrenberger](https://github.com/eXpl0it3r)
* [Martin Bundschuh](https://github.com/chuma2150)

[webapi-link-build]:            https://ci.appveyor.com/project/Jobbr/jobbr-webapi         
[webapi-link-nuget]:            https://www.nuget.org/packages/Jobbr.Server.WebAPI

[webapi-badge-build-develop]:   https://img.shields.io/appveyor/ci/Jobbr/jobbr-webapi/develop.svg?label=develop
[webapi-badge-build-master]:    https://img.shields.io/appveyor/ci/Jobbr/jobbr-webapil/master.svg?label=master
[webapi-badge-nuget]:           https://img.shields.io/nuget/v/Jobbr.Server.WebAPI.svg?label=NuGet%20stable
[webapi-badge-nuget-pre]:       https://img.shields.io/nuget/vpre/Jobbr.Server.WebAPI.svg?label=NuGet%20pre


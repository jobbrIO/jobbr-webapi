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

## Rest API Reference (incomplete)
Please note the following reference is limited, incomplete version of the REST-API. 

### Status
The API provides a simple status endpoint where you can tests if the webservice has been started and all endpoints are available.

	GET http://localhos:8765/api/status

should just return `200 OK` with content `Fine`

### Get all Jobs
Take the following Endpoint

	GET http://localhost:8765/api/jobs

With a sample return value

	[
		{
			"id": 1,
			"uniquename": "MyJob1",
			"title": "My First Job",
			"type": "MinimalJob",
			"parameters": { "param1" : "test" },
			"createdDateTimeUtc": "2015-03-04T17:40:00"
		},
		{
			"id": 3,
			"uniquename": "MyJob2",
			"title": "Second Job",
			"type": "ParameterizedlJob",
			"parameters": { "param1" : "test" },
			"createdDateTimeUtc": "2015-03-10T00:00:00"
		},
		{
			"id": 7,
			"uniquename": "MyJob3",
			"title": "Third Job",
			"type": "ProgressJob",
			"createdDateTimeUtc": "2015-03-10T00:00:00"
		}
	]

### Add Job
Only the ``UniqueName`` and ``Type`` are required.

	POST http://localhost:8765/api/jobs

	{
		"uniquename": "MyJob1",
		"title": "My First Job",
		"type": "MinimalJob",
		"parameters": { "param1" : "test" },
		"createdDateTimeUtc": "2015-03-04T17:40:00"
	}

### Trigger a Job to run (JobRun)
A job can be triggered by using the following Endpoint (JobId or UniqueId is required)

	POST http://localhost:8765/api/jobs/{JobId}/trigger
	POST http://localhost:8765/api/jobs/{UniqueId}/trigger

There are 3 different modes and please note that
* DateTime Values are always UTC
* UserId, UserName or UserDisplayName are optional
* Parameters are an object

#### 1. Instant

{
	"triggerType": "instant",
	"isActive": true,
	"userId": 12,
	"parameters": { "Param1": "test", "Param2" : 42 }
}

#### 2. Scheduled

```json
{
	"triggerType": "scheduled",
	"startDateTimeUtc": "2015-03-12 11:00"
	"isActive": true,
	"userName": "test"
	"parameters": { "Param1": "test", "Param2" : 42 }
}
``` 
#### 3. Recurring
A definition is a cron definition as specified on wikipedia [http://en.wikipedia.org/wiki/Cron](http://en.wikipedia.org/wiki/Cron).

```json
{
	"triggerType": "recurring",
	"startDateTimeUtc": "2015-03-12 11:00
	"endDateTimeUtc": "2015-03-19 18:00"
	"definition: "* 15 * * *",
	"isActive": true,
	"userName": "test"
	"parameters": { "Param1": "test", "Param2" : 42 }
}
``` 

### Activate / Deactivate Trigger
A Trigger and all scheduled JobRuns getting deactivated on the following route:

	PATCH http://localhost:8765/api/triggers/1234

Sample Payload
```json
{
	"triggerType": "scheduled",
	"isActive": false,
}
```

### Update Trigger
Causes a trigger to be updated. Right now, only the Definition (for RecurringJobs) or the StartDateTimeUtc (for ScheduledJobs) can be updated.

	PATCH http://localhost:8765/api/triggers/1234
	
Sample Payload
```json
{
	"triggerType": "scheduled",
	"isActive": true,
}
```

### Watch the Run-Status
To get a detailed view for a jobrun you have to know the JobRunId

	GET http://localhost:8765/api/jobruns/{JobRunId}

Sample Response

```json
{
	"jobId": 7,
	"triggerId": 446,
	"jobRunId": 446,
	"uniqueId": "95e9e93e-062c-4b00-8708-df5ca1270c2e",
	"instanceParameter": {
		"Param1": "test",
		"Param2": 42
	},
	"jobName": "ThirdJob",
	"jobTitle": "This a sample Job",
	"state": "Completed",
	"progress": 100,
	"plannedStartUtc": "2015-03-11T11:23:15.74",
	"auctualStartUtc": "2015-03-11T11:23:16.52",
	"auctualEndUtc": "2015-03-11T11:23:34.48"
}
```
### Getting Artefacts of a JobRun
If there are any artefacts for a specific run, they are available under.

	GET http://localhost:8765/api/jobruns/446/artefacts/{filename}

## Static Typed Client
There is also a static typed client available which you can use to interact with any Jobbr Rest Api. Install the client by using the following commands

	Install-Package Jobbr.Client

After installation, you might provide the base url where the api can be found. See example below

```c#
using Jobbr.Client;
using Jobbr.WebAPI.Common.Models;

// ...

var jobbrClient = new JobbrClient("http://localhost:1337");

var allJobs = jobbrClient.GetAllJobs();

```

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
* Michael Schnyder
* Oliver Zürcher

[webapi-link-build]:            https://ci.appveyor.com/project/Jobbr/jobbr-webapi         
[webapi-link-nuget]:            https://www.nuget.org/packages/Jobbr.Server.WebAPI

[webapi-badge-build-develop]:   https://img.shields.io/appveyor/ci/Jobbr/jobbr-webapi/develop.svg?label=develop
[webapi-badge-build-master]:    https://img.shields.io/appveyor/ci/Jobbr/jobbr-webapil/master.svg?label=master
[webapi-badge-nuget]:           https://img.shields.io/nuget/v/Jobbr.Server.WebAPI.svg?label=NuGet%20stable
[webapi-badge-nuget-pre]:       https://img.shields.io/nuget/vpre/Jobbr.Server.WebAPI.svg?label=NuGet%20pre


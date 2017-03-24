# Jobbr Web API Extension [![Build status](https://ci.appveyor.com/api/projects/status/akvsehv0wvwbo08a?svg=true)](https://ci.appveyor.com/project/Jobbr/jobbr-webapi)
Adds Rest-style api to a Jobbr-Server and a strong typed .NET Client

## Installation
### Package Download
Install the NuGet "Jobbr.Server.WebAPI" to the project where you host you Jobbr-Server. The extension already comes with a small webserver based on OWIN/Katana. You don't need to install any additional HttpListenrs, etc.

``
PM> Install-Package Jobbr.Server.WebAPI
``

### Registration
The Library comes with an extension method for the `JobbrBuilder` (which is explained [[here]]). To add the Web API to a Jobbr-Server you need to register it prior start as you see below.

```c#

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
There is a default configuration which binds the API-Endpoint to `http://localhost:80/jobbr/api`. This can be adjusted by using the fluent-syntax on the configuration object when one of the various overloads of the builder.AddWebApi()-Method. See example below:

```c#
builder.AddWebApi(config => 
   {
      config.BaseUrl = "http://localhost:8765/api";
   });
```
Please note that the configuration options reflects the current feature set of the server directly.

## Rest API Reference (incomplete)

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

	{
    "triggerType": "scheduled",
		"startDateTimeUtc": "2015-03-12 11:00"
    "isActive": true,
		"userName": "test"
    "parameters": { "Param1": "test", "Param2" : 42 }
	}

#### 3. Recurring

	{
    "triggerType": "recurring",
		"startDateTimeUtc": "2015-03-12 11:00
		"endDateTimeUtc": "2015-03-19 18:00"
		"definition: "* 15 * * *",
	  "isActive": true,
		"userName": "test"
		"parameters": { "Param1": "test", "Param2" : 42 }
	}

A definition is a cron definition as specified on wikipedia [http://en.wikipedia.org/wiki/Cron](http://en.wikipedia.org/wiki/Cron).

### Activate / Deactivate Trigger
A Trigger and all scheduled JobRuns getting deactivated

	PATCH http://localhost:8765/api/triggers/1234
	
		{
	        "triggerType": "scheduled",
	        "isActive": false,
		}

### Update Trigger
Causes a trigger to be updated. Right now, only the Definition (for RecurringJobs) or the StartDateTimeUtc (for ScheduledJobs) can be updated.

	PATCH http://localhost:8765/api/triggers/1234
	
		{
	        "triggerType": "scheduled",
	        "isActive": true,
		}


### Watch the Run-Status
To get a detailed view for a jobrun you have to know the JobRunId

	GET http://localhost:8765/api/jobruns/{JobRunId}

Sample Response

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

### Getting Artefacts of a JobRun
If there are any artefacts for a specific run, they are available under.

	GET http://localhost:8765/api/jobruns/446/artefacts/{filename}

## Client
There is also a static typed client available which you can use to interact with any Jobbr Rest Api. Install the client by using the following commands

`` 
PM> Install-Package Jobbr.Client
``

# License
This software is licenced under GPLv3. See [LICENSE](LICENSE), please see the related licences of 3rd party libraries below.

# Credits

## Based On
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

## Authors
This application was built by the following awesome developers:
* Michael Schnyder
* Oliver Zürcher
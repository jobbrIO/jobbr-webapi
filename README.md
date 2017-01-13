# Jobbr Web API Extension
Adds Rest-style api to a Jobbr-Server and a strong typed .NET Client

## Server Extension
### Installation
Install the NuGet "Jobbr.Server.WebAPI" to the project where you host you Jobbr-Server. Because the Web Api builds up on an OWIN Pipeline, you need to install a appropriate Host, for instance a HttpListener.

**Restful Web API**<br/>
``
PM> Install-Package Jobbr.Server.WebAPI
``

**OWIN Http Listener**<br/>
`` 
PM> Install-Package Microsoft.Owin.Host.HttpListener
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
There is a default configuration which binds the API-Endpoint to `http://localhost:80`. This can be adjusted by using the fluent-syntax on the configuration object when one of the various overloads of the builder.AddWebApi()-Method. See example below

```C#
builder.AddWebApi(config => {
  config.BaseUrl = "localhost:8765";
  }
```
Please note that the configuration options reflects the current feature set of the server directly.

## Client
There is also a static typed client available which you can use to interact with any Jobbr Rest Api. Install the client by using the following commands

`` 
PM> Install-Package Jobbr.Client
``

# Unity3D-Raven-CS

## Installation
Directly import the package into your project. The example folder can be omitted.

## How To Use
``` csharp
// Create a new Unity3DRavenCS instance before using it.
// A gameobject will be created in scene and won't be destroyed until the game ends.
// All messages captured by Unity3DRavenCS instance are sent to sentry server asynchronously without blocking the main thread.
Unity3DRavenCS.Unity3DRavenCS.NewInstance(/*DSN*/);

Unity3DRavenCS.Unity3DRavenCS client = Unity3DRavenCS.Unity3DRavenCS.instance;

// Send message to sentry server.
client.CaptureMessage("Hello, world!", LogType.Log);

// Capture exception
try
{
    /*
    * throws exception
    */
}
catch (Exception e)
{
    client.CaptureException(e);
}
```


You can also send all log messages including unhandled exceptions to sentry automatically by providing a log handler.
``` csharp
public void LogHandler(string condition, string stackTrace, LogType type)
{
    if (type == LogType.Exception)
    {
        client.CaptureException(condition, stackTrace);
    }
    else
    {
        client.CaptureMessage(condition, type);
    }
}

void OnEnable()
{
    Application.logMessageReceived += LogHandler;
}

void OnDisable()
{
    Application.logMessageReceived -= LogHandler;
}
```

Please check the /Assets/Example/example.cs for full example.

using System;
using UnityEngine;
using Newtonsoft.Json;


namespace Unity3DRavenCS
{
    public abstract class Packet
    {
        public struct SDK
        {
            public string name;
            public string version;
        }

        public SDK sdk = new SDK();
        
        public struct Device
        {
            public string name;
            public string version;
            public string build;
        }

        public Device device = new Device();

        public string event_id;
        public string message;
        public string timestamp;
        public string platform;

        public Packet()
        {
            this.event_id = System.Guid.NewGuid().ToString("N");
            this.platform = "csharp";
            this.sdk.name = "Unity3D-Raven-CS";
            this.sdk.version = Version.VERSION;
            this.timestamp = DateTime.UtcNow.ToString("s");
            this.device.name = SystemInfo.operatingSystem;
            this.device.version = "0";
            this.device.build = "";
        }

        public virtual string ToJson()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

	public class MessagePacket: Packet
	{
		public string level;
		public string logger;

		public MessagePacket(string message, LogType logType = LogType.Error)
		{
            this.message = message;
			this.level = ToLogLevelFromLogType(logType);
		}

		private string ToLogLevelFromLogType(LogType logType)
		{
			string logLevel;
			switch (logType) 
			{
			case LogType.Log:
				logLevel = "info";
				break;
			case LogType.Warning:
				logLevel = "warning";
				break;
			case LogType.Error:
			case LogType.Assert:
			case LogType.Exception:
				logLevel = "error";
				break;
			default:
				logLevel = "error";
				break;
			}
			return logLevel;
		}
	}

 
    public class ExceptionPacket: Packet
    {
        public RavenException exception;

        public ExceptionPacket(Exception exception)
        {
            this.exception = new RavenException(exception);
            this.message = exception.Message;
        }
    }

    
	public struct ResponsePacket
	{
		public string id;
	}
}

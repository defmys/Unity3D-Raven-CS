using System;
using System.Collections.Generic;
using UnityEngine;


namespace Unity3DRavenCS
{
    [Serializable]
    public abstract class Packet
    {
        public virtual string ToJson()
        {
            return JsonUtility.ToJson(this);
        }
    }

	public class MessagePacket: Packet
	{
		public string event_id;
		public string message;
		public string timestamp;
		public string level;
		public string logger;
		public string platform;

		[Serializable]
		public struct SDK
		{
			public string name;
			public string version;
		}
		public SDK sdk = new SDK();

		[Serializable]
		public struct Device
		{
			public string name;
			public string version;
			public string build;
		}
		public Device device = new Device();

		public MessagePacket(LogType logType = LogType.Error)
		{
			this.event_id = System.Guid.NewGuid().ToString("N");
			this.sdk.name = "Unity3D-Raven-CS";
			this.sdk.version = Version.VERSION;
			this.platform = "csharp";
			this.level = ToLogLevelFromLogType(logType);
			this.timestamp = DateTime.UtcNow.ToString("s");
			this.device.name = SystemInfo.operatingSystem;
			this.device.version = "0";
			this.device.build = "";
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
        public string message;

        public ExceptionPacket(Exception exception)
        {
            this.exception = new RavenException(exception);
            this.message = exception.Message;
        }
    }


	[Serializable]
	public struct ResponsePacket
	{
		public string id;
	}
}

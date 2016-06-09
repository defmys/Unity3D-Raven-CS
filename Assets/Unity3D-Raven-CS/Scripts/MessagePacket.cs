using System;
using System.Collections.Generic;
using UnityEngine;


namespace Unity3DRavenCS
{
	[Serializable]
	public class MessagePacket
	{

		public string eventID;
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

		public MessagePacket()
		{
		}

		public string ToJson()
		{
			return JsonUtility.ToJson(this);
		}
	}
}

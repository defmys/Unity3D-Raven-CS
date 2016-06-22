using System;
using Newtonsoft.Json;

namespace Unity3DRavenCS
{
	public class RavenException
	{
        public RavenStackTrace stacktrace;
        public string value;
        public string type;
		
		public RavenException(Exception exception)
		{
            this.stacktrace = new RavenStackTrace(exception);
            this.value = exception.Message;
            this.type = exception.GetType().ToString();
        }

        public RavenException(string message, string stackTrace)
        {
            this.stacktrace = new RavenStackTrace(stackTrace);
            this.value = message;
            this.type = message;
        }

        public RavenException(string message, System.Diagnostics.StackTrace stackTrace)
        {
            this.stacktrace = new RavenStackTrace(stackTrace);
            this.value = message;
            this.type = message;
        }
    }
}
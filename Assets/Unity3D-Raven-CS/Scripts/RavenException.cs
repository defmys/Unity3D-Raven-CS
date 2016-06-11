using System;

namespace Unity3DRavenCS
{
	public class RavenException
	{
		private string m_message = "";
		public string message 
		{
			get { return m_message; }
		}

		private RavenStackTrace m_stackTrace;
		
		public RavenException(Exception exception)
		{
			m_message = exception.Message;
			m_stackTrace = new RavenStackTrace(exception);
		}
	}
}
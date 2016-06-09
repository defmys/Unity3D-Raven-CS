using System.Text.RegularExpressions;
using UnityEngine;

namespace Unity3DRavenCS 
{
	public class DSN 
	{
		private bool m_isValid;
		public bool isValid { get { return m_isValid; } }

		private string m_uri;
		private string m_protocol;
		private string m_publicKey;
		private string m_secretKey;
		private string m_host;
		private int m_projectID;

		public DSN(string uri)
		{
			m_uri = uri;

			m_isValid = Parse();
		}

		private bool Parse()
		{
			if (string.IsNullOrEmpty(m_uri))
			{
				return false;
			}

			Regex reg = new Regex(@"^(?<protocol>[\w]+)://(?<publicKey>[\w]+):(?<secretKey>[\w]+)@(?<host>[\w\d.:-_]+)/(?<projectID>[\d]+)[/]?$", RegexOptions.IgnoreCase);
			Match match = reg.Match(m_uri);
			if (match.Groups.Count < 5) 
			{
				return false;
			}
				
			m_protocol = match.Groups["protocol"].Value;
			m_publicKey = match.Groups["publicKey"].Value;
			m_secretKey = match.Groups["secretKey"].Value;
			m_host = match.Groups["host"].Value;
			m_projectID = System.Convert.ToInt32(match.Groups["projectID"].Value);

			return true;
		}
	}
}

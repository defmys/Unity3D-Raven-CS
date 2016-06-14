using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Unity3DRavenCS {


	public class RavenOptionType
	{
		public int timeout = 5000;
	}


	public class Unity3DRavenCS {
		private DSN m_dsn;
		private bool m_valid;
		private RavenOptionType m_option;

		public Unity3DRavenCS(string dsnUri, RavenOptionType option = null)
		{
			m_dsn = new DSN(dsnUri);
			if (!m_dsn.isValid) {
				m_valid = false;
				Debug.Log ("Unity3DRavenCS is disabled because the DSN is invalid.");
			} else {
				m_valid = true;

				m_option = option == null ? new RavenOptionType() : option;
			}
		}

		public string CaptureMessage(string message, LogType logType=LogType.Error, Dictionary<string, string> tags=null)
		{
			string resultId = "";

			if (m_valid) 
			{
				MessagePacket packet = new MessagePacket(message, logType, tags);

                Send(packet.ToJson());
			}

			return resultId;
		}

		public string CaptureException(Exception exception, Dictionary<string, string> tags = null)
		{
			string resultId = "";

            if (m_valid)
            {
                ExceptionPacket paket = new ExceptionPacket(exception, tags);

                Send(paket.ToJson());
            }

			return resultId;
		}

        public string CaptureException(string message, string stackTrace, Dictionary<string, string> tags = null)
        {
            string resultId = "";

            if (m_valid)
            {
                ExceptionPacket paket = new ExceptionPacket(message, stackTrace, tags);

                Send(paket.ToJson());
            }

            return resultId;
        }

        private string Send(string payload)
        {
            string resultId = "";

            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(m_dsn.sentryUri);
            request.Method = "POST";
            request.Timeout = m_option.timeout;
            request.ReadWriteTimeout = m_option.timeout;
            request.Accept = "application/json";
            request.ContentType = "application/json; charset=utf-8";
            request.Headers.Add("X-Sentry-Auth", m_dsn.XSentryAuthHeader());
            request.UserAgent = m_dsn.UserAgent();

            using (Stream requestStream = request.GetRequestStream())
            {
                using (StreamWriter streamWriter = new StreamWriter(requestStream))
                {
                    streamWriter.Write(payload);
                }
            }

            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                using (Stream responseStream = response.GetResponseStream())
                {
                    if (responseStream != null)
                    {
                        using (StreamReader streamReader = new StreamReader(responseStream))
                        {
                            string responseContent = streamReader.ReadToEnd();
                            ResponsePacket responsePacket = JsonConvert.DeserializeObject<ResponsePacket>(responseContent);
                            resultId = responsePacket.id;
                        }
                    }
                }
            }

            return resultId;
        }
	}
}

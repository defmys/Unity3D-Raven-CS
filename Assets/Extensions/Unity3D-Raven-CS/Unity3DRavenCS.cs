using UnityEngine;
using System.Net;
using System.IO;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using ICSharpCode.SharpZipLib.GZip;

namespace Unity3DRavenCS {


	public class RavenOptionType
	{
		public int timeout = 5000;
        public bool compression = true;
	}


	public class Unity3DRavenCS {
		private DSN m_dsn;
		private bool m_valid;
		private RavenOptionType m_option;

        /// <summary>
        /// Construct raven client. The client is disabled if the DSN is invalid.
        /// </summary>
        /// <param name="dsnUri">The DSN of the project</param>
        /// <param name="option">The option of the client. Leave it null for default values.</param>
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

        /// <summary>
        /// Capture a message and send it to sentry server.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="tags"></param>
        /// <param name="stackTrace"></param>
        /// <returns>result id</returns>
		public string CaptureMessage(string message, LogType logType=LogType.Error, Dictionary<string, string> tags=null, string stackTrace=null)
		{
			string resultId = "";

			if (m_valid) 
			{
				MessagePacket packet = new MessagePacket(message, logType, tags, stackTrace);

				resultId = Send(packet.ToJson());
			}

			return resultId;
		}

        /// <summary>
        /// Capture a message with System.Diagnostics.StackTrace
        /// </summary>
        /// <param name="message"></param>
        /// <param name="logType"></param>
        /// <param name="tags"></param>
        /// <param name="stackTrace"></param>
        /// <returns>result id</returns>
        public string CaptureMessageWithSysStack(string message, LogType logType = LogType.Error, Dictionary<string, string> tags = null, System.Diagnostics.StackTrace stackTrace = null)
        {
            string resultId = "";

            if (m_valid)
            {
                MessagePacket packet = new MessagePacket(message, logType, tags, stackTrace);

                resultId = Send(packet.ToJson());
            }

            return resultId;
        }

        /// <summary>
        /// Send the captured exception to sentry server.
        /// </summary>
        /// <param name="exception">Captured exception</param>
        /// <param name="tags"></param>
        /// <returns>result id</returns>
		public string CaptureException(Exception exception, Dictionary<string, string> tags = null)
		{
            return CaptureException(exception.Message, new System.Diagnostics.StackTrace(exception, true) , tags);
		}

        /// <summary>
        /// Send an exception message with stack trace to sentry server.
        /// </summary>
        /// <param name="message">The description of the captured exception</param>
        /// <param name="stackTrace">The stack trace of the captured exception</param>
        /// <param name="tags"></param>
        /// <returns></returns>
        public string CaptureException(string message, string stackTrace, Dictionary<string, string> tags = null)
        {
            string resultId = "";

            if (m_valid)
            {
                ExceptionPacket packet = new ExceptionPacket(message, stackTrace, tags);

                resultId = Send(packet.ToJson());
            }

            return resultId;
        }

        /// <summary>
        /// Send an exception message with stack trace to sentry server.
        /// </summary>
        /// <param name="message">The description of the captured exception</param>
        /// <param name="stackTrace">The stack trace of the captured exception</param>
        /// <param name="tags"></param>
        /// <returns>result id</returns>
        public string CaptureException(string message, System.Diagnostics.StackTrace stackTrace, Dictionary<string, string> tags = null)
        {
            string resultId = "";

            if (m_valid)
            {
                ExceptionPacket packet = new ExceptionPacket(message, stackTrace, tags);

                resultId = Send(packet.ToJson());
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

            if (m_option.compression)
            {
                request.Headers.Add("Content-Encoding", "gzip");
            }

            using (Stream requestStream = request.GetRequestStream())
            {
                if (m_option.compression)
                {
                    byte[] payloadBuffer = System.Text.Encoding.UTF8.GetBytes(payload);
                    using (GZipOutputStream gzipStream = new GZipOutputStream(requestStream))
                    {
                        gzipStream.Write(payloadBuffer, 0, payloadBuffer.Length);
                    }
                }
                else
                {
                    using (StreamWriter streamWriter = new StreamWriter(requestStream))
                    {
                        streamWriter.Write(payload);
                    }
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

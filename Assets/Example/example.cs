using UnityEngine;
using System.Collections.Generic;
using System;

public class example : MonoBehaviour {
    private Unity3DRavenCS.Unity3DRavenCS m_client;
    private Dictionary<string, string> m_tags = null;

    void Start()
    {
        // Create a raven client with DSN uri.
		m_client = new Unity3DRavenCS.Unity3DRavenCS("http://ab02bafeb811496c825b4f22631f3ea3:81efdf5ff4f34aeb8e4bb20b27d286eb@192.168.1.109:9000/2");

        // Create some tags that need to be sent with log messages.
        m_tags = new Dictionary<string, string>();
        m_tags.Add("Device-Model", SystemInfo.deviceModel);
        m_tags.Add("Device-Name", SystemInfo.deviceName);
        m_tags.Add("OS", SystemInfo.operatingSystem);
        m_tags.Add("MemorySize", SystemInfo.systemMemorySize.ToString());


        // =========================================================================================================
        // Capture message
        m_client.CaptureMessage("Hello, world!", LogType.Log, m_tags);

        // Capture exception
        try
        {
            ExceptionCall();
        }
        catch (Exception e)
        {
            m_client.CaptureException(e);
        }


        // =========================================================================================================
        // You can register a log handler to handle all logs including exceptions.
        // See LogHandler(), OnEnable() and OnDisable().
        // The following logs will be sent to sentry server automatically.
        Debug.Log("Info log");
        Debug.LogWarning("Warning log");
        Debug.LogError("Error log");
        // The following function call throws an exception. An error log with stack trace will be sent
        // to sentry server automatically.
        ExceptionCall();
    }

    void OnEnable()
    {
        Application.logMessageReceived += LogHandler;
    }

    void OnDisable()
    {
        Application.logMessageReceived -= LogHandler;
    }

    public void LogHandler(string condition, string stackTrace, LogType type)
    {
        if (m_client == null)
        {
            return;
        }

        if (type == LogType.Exception)
        {
            m_client.CaptureException(condition, stackTrace, m_tags);
        }
        else
        {
            m_client.CaptureMessage(condition, type, m_tags);
        }
    }

	private List<int> l = new List<int>();
    private void ExceptionCall()
    {
		l = null;
        ExceptionCall1();
    }

    private void ExceptionCall1()
    {
        ExceptionCall2();
    }

    private void ExceptionCall2()
    {
		l.Add(10);
    }
}

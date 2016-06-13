using UnityEngine;
using System.Collections.Generic;
using System;

public class example : MonoBehaviour {
    private Unity3DRavenCS.Unity3DRavenCS m_client;
    private Dictionary<string, string> m_tags = null;

    void Start()
    {
        // Create a raven client with DSN uri.
        m_client = new Unity3DRavenCS.Unity3DRavenCS("http://5aa275ef70b0416f80a405b258d20a15:801b6e0d9cbf46508560b9ca0b320c6a@192.168.188.128:9000/2");

        // Create some tags that need to be sent with log messages.
        m_tags = new Dictionary<string, string>();
        m_tags.Add("playTime", Time.realtimeSinceStartup.ToString());
        m_tags.Add("time", System.DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));

        m_tags.Add("ProcessorType", SystemInfo.processorType);
        m_tags.Add("ProcessorCount", SystemInfo.processorCount.ToString());

        m_tags.Add("Device-Uid", SystemInfo.deviceUniqueIdentifier);
        m_tags.Add("Device-Model", SystemInfo.deviceModel);
        m_tags.Add("Device-Name", SystemInfo.deviceName);
        m_tags.Add("OS", SystemInfo.operatingSystem);
        m_tags.Add("MemorySize", SystemInfo.systemMemorySize.ToString());

        m_tags.Add("GPU-Memory", SystemInfo.graphicsMemorySize.ToString());
        m_tags.Add("GPU-Name", SystemInfo.graphicsDeviceName);
        m_tags.Add("GPU-Vendor", SystemInfo.graphicsDeviceVendor);
        m_tags.Add("GPU-VendorID", SystemInfo.graphicsDeviceVendorID.ToString());
        m_tags.Add("GPU-id", SystemInfo.graphicsDeviceID.ToString());
        m_tags.Add("GPU-Version", SystemInfo.graphicsDeviceVersion);
        m_tags.Add("GPU-ShaderLevel", SystemInfo.graphicsShaderLevel.ToString());



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

    private void ExceptionCall()
    {
        ExceptionCall1();
    }

    private void ExceptionCall1()
    {
        ExceptionCall2();
    }

    private void ExceptionCall2()
    {
        int a = 1;
        int b = 0;
        int x = a / b;
    }
}

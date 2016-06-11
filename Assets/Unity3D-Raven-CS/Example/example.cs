using UnityEngine;
using System.Collections;
using System;

public class example : MonoBehaviour {
	private Unity3DRavenCS.Unity3DRavenCS client;

	// Use this for initialization
	void Start () {
		Application.stackTraceLogType = StackTraceLogType.ScriptOnly;
		client = new Unity3DRavenCS.Unity3DRavenCS ("http://ab02bafeb811496c825b4f22631f3ea3:81efdf5ff4f34aeb8e4bb20b27d286eb@192.168.1.109:9000/2");

		//client.CaptureMessage ("Hello, world!");
		try
		{
			int a = 1;
			int b = 0;
			int x = a / b;
		}
		catch (Exception e) 
		{
			client.captureException(e);
			Debug.Log (e.Message);
			throw;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

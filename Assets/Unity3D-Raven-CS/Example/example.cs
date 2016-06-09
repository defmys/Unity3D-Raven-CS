using UnityEngine;
using System.Collections;

public class example : MonoBehaviour {
	private Unity3DRavenCS.Unity3DRavenCS client;

	// Use this for initialization
	void Start () {
		client = new Unity3DRavenCS.Unity3DRavenCS ("http://ab02bafeb811496c825b4f22631f3ea3:81efdf5ff4f34aeb8e4bb20b27d286eb@192.168.1.109:9000/2");

		client.CaptureMessage ("Hello, world!");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

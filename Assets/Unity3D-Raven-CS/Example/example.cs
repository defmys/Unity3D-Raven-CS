using UnityEngine;
using System.Collections;

public class example : MonoBehaviour {
	private Unity3DRavenCS.Unity3DRavenCS client;

	// Use this for initialization
	void Start () {
		client = new Unity3DRavenCS.Unity3DRavenCS ("https://public:secret@host.com/1");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

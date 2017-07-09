using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bloodSuicide : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Invoke ("killSelf", 2f);
	}
	
	void killSelf()
	{
		Destroy (gameObject);
	}
}

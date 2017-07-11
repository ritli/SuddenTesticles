using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuCode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.KeypadEnter) || Input.GetKeyDown (KeyCode.Return) || Input.GetAxis("Accept") == 1) 
		{
			UnityEngine.SceneManagement.SceneManager.LoadScene ("game");
		}
	}
}

﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Selectable> ().Select ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void TaskOnClick(){
		Debug.Log ("Clicked");
		UnityEngine.SceneManagement.SceneManager.LoadScene ("game");
	}
}

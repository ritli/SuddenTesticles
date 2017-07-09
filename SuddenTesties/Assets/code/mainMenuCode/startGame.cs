using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class startGame : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Selectable> ().Select ();
	}

	public void start (){
		UnityEngine.SceneManagement.SceneManager.LoadScene ("game");
	}
}

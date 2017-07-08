using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class exitGame : MonoBehaviour {

	void TaskOnClick () {
		Debug.Log ("Exit");
		Application.Quit ();
	}
}

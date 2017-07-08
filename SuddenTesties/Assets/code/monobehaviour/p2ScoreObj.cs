using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p2ScoreObj : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		gameHandler.setScoreText (gameObject.GetComponent<Text> (), 2);
	}
}

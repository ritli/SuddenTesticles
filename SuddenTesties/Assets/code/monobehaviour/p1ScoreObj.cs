using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class p1ScoreObj : MonoBehaviour {

	// Use this for initialization
	void Awake(){
		gameHandler.setScoreText (gameObject.GetComponent<TMPro.TextMeshProUGUI> (), 1);
	}
}

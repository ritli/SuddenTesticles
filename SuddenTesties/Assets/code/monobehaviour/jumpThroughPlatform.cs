using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class jumpThroughPlatform : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			col.gameObject.layer = LayerMask.NameToLayer ("PlayerJumpthrough");
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject.tag == "Player") {
			Debug.Log ("Left");
			col.gameObject.layer = LayerMask.NameToLayer ("Player");
		}
	}
}

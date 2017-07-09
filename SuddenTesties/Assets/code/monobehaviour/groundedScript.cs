using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedScript : MonoBehaviour {

	// Use this for initialization
	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Grounded");
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") || col.gameObject.layer == LayerMask.NameToLayer ("WorldBox"))
			transform.GetComponentInParent<playerMovement> ().setGrounded ();
	}
}

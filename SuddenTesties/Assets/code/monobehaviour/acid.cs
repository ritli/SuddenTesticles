﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class acid : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D col)
	{
		if(col.tag == "Player")
		{
			col.gameObject.GetComponent<playerMovement> ().getHit ();
		}
	}
}

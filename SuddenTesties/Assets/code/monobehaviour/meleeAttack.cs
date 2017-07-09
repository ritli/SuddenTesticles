using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class meleeAttack : MonoBehaviour {

	bool active;

	public void setActive(bool dmgState)
	{
		active = dmgState;
	}

	void OnTriggerStay2D(Collider2D col)
	{
		if (active && col.tag == "Player" && col.transform != transform.parent) 
		{
			col.gameObject.GetComponent<playerMovement> ().getHit ();
		}
	}
}

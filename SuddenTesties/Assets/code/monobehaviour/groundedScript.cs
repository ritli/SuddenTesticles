using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundedScript : MonoBehaviour {

	// Needs counter to avoid being stuck when moving on multi segment platforms
	int platformsTouched = 0;

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") || col.gameObject.layer == LayerMask.NameToLayer ("WorldBox")) 
		{
			transform.GetComponentInParent<playerMovement> ().setGrounded (true);
			if(++platformsTouched == 1)
			{
				GetComponentInParent<Animator>().SetBool("isGrounded", true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") || col.gameObject.layer == LayerMask.NameToLayer ("WorldBox")) 
		{
			if(--platformsTouched == 0)
			{
				transform.GetComponentInParent<playerMovement> ().setGrounded (false);
				GetComponentInParent<Animator>().SetBool("isGrounded", false);
			}

			if (platformsTouched < 0)
				Debug.Log ("Error: Touching negative amount of platforms");
		}
	}
}

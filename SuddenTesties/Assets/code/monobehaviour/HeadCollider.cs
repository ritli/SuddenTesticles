using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour {

    LayerMask collisonMask;
    Collider2D collider;
    bool isColliding;
	bool isFiring;
    ContactPoint2D[] contactPoints;

    void Start()
    {
        if (transform.parent.CompareTag("Player"))
        {
            collisonMask = GetComponentInParent<playerMovement>().GetBallCollisionMask();
        }

        if (GetComponent<Collider2D>())
        {
            collider = GetComponent<Collider2D>();
        }
    }

    void OnCollisionStay2D(Collision2D col)
    {
        if (collider.IsTouchingLayers(collisonMask)){
            contactPoints = col.contacts;
        }
    }

	void OnTriggerEnter2D(Collider2D col)
	{
		if (isFiring) 
		{
			if (col.tag == "Player") 
			{
				Debug.Log ("Hit a player");
				if (col.transform != transform.parent) 
				{
					Debug.Log ("Player hit is not self");
					col.gameObject.GetComponent<playerMovement> ().getHit ();
					Debug.Log ("Player " + GetComponentInParent<playerMovement> ().getID ().ToString () + " got points");
				}
			}
		}
	}

    void Update()
    {
        if (collider.IsTouchingLayers(collisonMask))
        {
            isColliding = true;
        }
        else
        {
            contactPoints = null;
            isColliding = false;
        }
    }

    public bool IsColliding()
    {
        return isColliding;
    }

    public ContactPoint2D[] GetContactPoints()
    {
        return contactPoints;
    }

	public void SetFireState(bool state)
	{
		isFiring = state;
	}
}

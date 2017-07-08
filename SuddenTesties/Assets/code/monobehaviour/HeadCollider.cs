using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadCollider : MonoBehaviour {

    LayerMask collisonMask;
    Collider2D collider;
    bool isColliding;
    ContactPoint2D[] contactPoints;

    void Start()
    {
        if (transform.parent.CompareTag("Player"))
        {
            collisonMask = GetComponentInParent<playerMovement>().GetCollisionMask();
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
}

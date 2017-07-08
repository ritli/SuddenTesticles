﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    active, firing, paused
}

[System.Serializable]
public class Inputs{
    [Range(1, 2)]
    [SerializeField] int playerId = 1;

    string fire = "Fire";
    string strike = "Strike";
    string jump = "Jump";
    string horizontal = "Horizontal";
    string vertical = "Vertical";

    string prefix()
    {
        return "P" + playerId.ToString();
    }

    public string Fire{
        get{ return prefix() + fire;}
    }
    public string Strike
    {
        get { return prefix() + strike; }
    }
    public string Jump
    {
        get { return prefix() + jump; }
    }
    public string Horizontal
    {
        get { return prefix() + horizontal; }
    }
    public string Vertical
    {
        get { return prefix() + vertical; }
    }
}

public class playerMovement : MonoBehaviour {
    #region EditorVariables

    [Header("Movement Variables")]

    [SerializeField] float maxSpeed;
	[SerializeField] float acceleration;
	[SerializeField] float jumpForce;
	[SerializeField] Transform groundCheck;

    [Header("Fire Variables")]

    [SerializeField] float fireCooldownMax;
	[SerializeField] float fireForce = 2.5f;
    [SerializeField] float fireBounceForce = 2f;

    [Header("Layermasks")]

    [SerializeField] LayerMask collisionMask;

    [SerializeField] Inputs inputs;
    #endregion
    
    //Movement vars
    bool facingRight = true;
	bool grounded = false;
	bool jump = false;
	bool fallthrough = false;

	int xDir = 0; // Left: -1, Standstill: 0, Right: 1

    PlayerState state;

    //Fire vars
    float fireCooldown;
    Vector2 fireVector;
    Quaternion originalRotation;
    
    //Component vars
    HeadCollider headCollider;
    Rigidbody2D rigidbody;

	void Start () {
        fireCooldown = fireCooldownMax;

        headCollider = GetComponentInChildren<HeadCollider>();
        rigidbody = GetComponent<Rigidbody2D> ();
	}
	
	void Update () {

		grounded = Physics2D.Linecast (transform.position, groundCheck.position, collisionMask);

        if (state == PlayerState.active)
        {
            InputUpdate();
        }
    }

	void FixedUpdate() {

        switch (state)
        {
		case PlayerState.active:
			if (xDir != 0 && (rigidbody.velocity.x * (float)xDir) < maxSpeed) {
				rigidbody.AddForce (Vector2.right * xDir * acceleration);

				if (rigidbody.velocity.x > maxSpeed) {
					rigidbody.velocity = new Vector2 (Mathf.Sign (rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
				}
			}

			if (jump) {
				rigidbody.AddForce (Vector2.up * jumpForce, ForceMode2D.Impulse);
				jump = false;
			}

			if (fallthrough) {
				gameObject.layer = LayerMask.NameToLayer ("PlayerJumpthrough");
				fallthrough = false;
			}
                break;
            case PlayerState.firing:

                if (headCollider.IsColliding())
                {
                    EndFire();
                    break;
                }

                rigidbody.AddForce(transform.up * fireForce, ForceMode2D.Impulse);

                break;
            case PlayerState.paused:
                break;
            default:
                break;
        }

        fireCooldown += Time.fixedDeltaTime;
    }

    void InputUpdate()
    {
        if (Input.GetButtonDown(inputs.Fire) && fireCooldown > fireCooldownMax)
        {
            StartFire();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            xDir = 1;
        }

        else if (Input.GetKeyDown(KeyCode.A))
        {
            xDir = -1;
        }

		if (Input.GetKeyDown (KeyCode.S) && grounded) {
			fallthrough = true;
		}

        if ((Input.GetKeyUp(KeyCode.A) && xDir == -1) || (Input.GetKeyUp(KeyCode.D) && xDir == 1))
        {
            xDir = 0;
        }

        if (Input.GetButtonDown(inputs.Jump) && grounded)
        {
            jump = true;
        }
    }

    void StartFire()
    {
        fireCooldown = 0;
        rigidbody.gravityScale = 0;

        fireVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        transform.rotation = Quaternion.FromToRotation(transform.up, fireVector);

        state = PlayerState.firing;
    }

    void EndFire()
    {
        Vector2 normal = Vector2.zero;

        for (int i = 0; i < 3; i++)
        {
            Vector2 offset = transform.right * (i - 1) * 0.5f;
            Vector2 origin = transform.position - transform.right * 0.5f + (Vector3)offset;
            Vector2 direction = transform.up;

            RaycastHit2D hit;

            if (hit = Physics2D.Linecast(origin, origin + direction, collisionMask))
            {
                normal = hit.normal;
                break;
            }
        }

        rigidbody.velocity = Vector2.zero;

        Vector2 reflectVector = Vector2.Reflect(fireVector, normal).normalized;

        rigidbody.gravityScale = 1;
        transform.rotation = Quaternion.identity;

        rigidbody.AddForce(reflectVector * fireBounceForce, ForceMode2D.Impulse);

        state = PlayerState.active;
    }

    Vector2 GetFireNormal()
    {
        Vector2 point = headCollider.GetContactPoints()[0].point;
        Vector2 origin = headCollider.transform.position;

        point += (point - origin).normalized;

        return Physics2D.Linecast(origin, point, collisionMask).normal;
    }

    public LayerMask GetCollisionMask()
    {
        return collisionMask;
    }
}
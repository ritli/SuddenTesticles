using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerState
{
    active, firing, paused
}

public class playerMovement : MonoBehaviour {
    #region EditorVariables

    [Header("Movement Variables")]

    [SerializeField] float maxSpeed;
	[SerializeField] float acceleration;
	[SerializeField] float jumpForce;
	[SerializeField] Transform groundCheck;
    [SerializeField] float fireCooldownMax;
	[SerializeField] float fireForce = 2.5f;

    [Header("Layermasks")]

    [SerializeField]LayerMask collisionMask;

    #endregion
    
    //Movement vars
    bool facingRight = true;
	bool grounded = false;
	bool jump = false;

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
                if (xDir != 0 && (rigidbody.velocity.x * (float)xDir) < maxSpeed)
                {
                    rigidbody.AddForce(Vector2.right * xDir * acceleration);

                    if (rigidbody.velocity.x > maxSpeed)
                    {
                        rigidbody.velocity = new Vector2(Mathf.Sign(rigidbody.velocity.x) * maxSpeed, rigidbody.velocity.y);
                    }
                }

                if (jump)
                {
                    rigidbody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                    jump = false;
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
        if (Input.GetButtonDown("Fire1") && fireCooldown > fireCooldownMax)
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

        if ((Input.GetKeyUp(KeyCode.A) && xDir == -1) || (Input.GetKeyUp(KeyCode.D) && xDir == 1))
        {
            xDir = 0;
        }

        if (Input.GetButtonDown("Jump") && grounded)
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

       // headCollider.gameObject.SetActive(true);
        state = PlayerState.firing;
    }

    void EndFire()
    {
        rigidbody.gravityScale = 1;
        transform.rotation = Quaternion.identity;

       // rigidbody.AddForce( Vector2.Reflect(fireVector, GetFireNormal()).normalized * 30, ForceMode2D.Impulse);

        //headCollider.gameObject.SetActive(false);
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

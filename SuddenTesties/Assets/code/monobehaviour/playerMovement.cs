using System.Collections;
using System.Collections.Generic;
using UnityEngine;


enum PlayerState
{
    active, firing, paused
}

[System.Serializable]
public class Inputs{
    [Range(1, 2)]
    [SerializeField] int playerId;

    string fire = "Fire";
    string strike = "Strike";
    string jump = "Jump";
    string horizontal = "Horizontal";
    string vertical = "Vertical";
    string aimX = "HorizontalAim";
    string aimY = "VerticalAim";

	public Inputs(int pID)
	{
		playerId = pID;
	}

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
    public string AimX
    {
        get { return prefix() + aimX; }
    }
    public string AimY
    {
        get { return prefix() + aimY; }
    }
	public int ID 
	{
		get { return playerId; }
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

	float xDir = 0; // Left: -1, Standstill: 0, Right: 1

    PlayerState state;

    //Fire vars
    float fireCooldown;
    Vector2 fireVector;
    Quaternion originalRotation;
    
    //Component vars
    HeadCollider headCollider;
    Rigidbody2D rigidbody;
    BallHandler ballHandler;

    private bool XboxController = false;
    private bool PS4Controller = false;

	//Animator
	private Animator mAnimator;

	void Start () {
        fireCooldown = fireCooldownMax;

        headCollider = GetComponentInChildren<HeadCollider>();
        rigidbody = GetComponent<Rigidbody2D> ();
        ballHandler = GetComponentInChildren<BallHandler>();

        KeyValuePair<int, Vector3> spawnData = gameHandler.spawn ();
		transform.position = spawnData.Value;
		inputs = new Inputs (spawnData.Key);

        InitControllers();

		mAnimator = GetComponent<Animator>();
    }
	
	void Update () {

        if (state == PlayerState.active)
        {
            InputUpdate();
        }
    }

	void FixedUpdate() {
		mAnimator.SetFloat("speed", Mathf.Abs(rigidbody.velocity.x));

		if (mAnimator.GetCurrentAnimatorStateInfo(0).IsName("player_fall"))
		{
			mAnimator.ResetTrigger("jumpTrigger");
		}

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
				mAnimator.SetTrigger("jumpTrigger");
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
		if ((float)(facingRight ? 1 : -1) != Mathf.Sign (Input.GetAxis (inputs.Horizontal)) && Input.GetAxis(inputs.Horizontal) != 0) {
			facingRight = !facingRight;
			transform.localScale = new Vector2 (-transform.localScale.x, transform.localScale.y);
		}
        xDir = Input.GetAxis(inputs.Horizontal);
        float yVel = Input.GetAxis(inputs.Vertical);

        if (XboxController)
        {
            fireVector = new Vector2(Input.GetAxisRaw(inputs.AimX), -Input.GetAxisRaw(inputs.AimY));

            Debug.DrawLine(transform.position, transform.position + (Vector3)fireVector);

            if (fireVector.magnitude > 0.85f && fireCooldown > fireCooldownMax)
            {
                StartCoroutine(StartFire());
            }
        }
        else
        {
            if (Input.GetButtonDown(inputs.Fire) && fireCooldown > fireCooldownMax)
            {
                StartCoroutine(StartFire());
            }
        }

		if (Input.GetButtonDown (inputs.Strike)) 
		{
			
		}

		if (yVel > 0.5f && grounded) {
			fallthrough = true;
		}

		if (Input.GetAxis(inputs.Jump) == 1 && grounded && !jump && !(rigidbody.velocity.y > 5))
        {
            jump = true;
        }

        /*
        if ((Input.GetKeyUp(KeyCode.A) && xDir == -1) || (Input.GetKeyUp(KeyCode.D) && xDir == 1))
        {
            xDir = 0;
        }

        if (xVel > 0)
        {
            xDir = 1;
        }

        else if (xVel < 0)
        {
            xDir = -1;
        }
        */
    }

    IEnumerator StartFire()
    {
        fireCooldown = 0;
        rigidbody.gravityScale = 0;

        if (!XboxController)
        {
            fireVector = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        }

        transform.rotation = Quaternion.FromToRotation(transform.up, fireVector);

        ballHandler.StartAnimation();

        state = PlayerState.paused;

        yield return new WaitForSeconds(0.3f);

        GetComponent<SpriteRenderer>().enabled = false;

        state = PlayerState.firing;
		headCollider.SetFireState (true);
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

        ballHandler.StopAnimation();
        GetComponent<SpriteRenderer>().enabled = true;

        state = PlayerState.active;
		headCollider.SetFireState (false);
    }

    Vector2 GetFireNormal()
    {
        Vector2 point = headCollider.GetContactPoints()[0].point;
        Vector2 origin = headCollider.transform.position;

        point += (point - origin).normalized;

        return Physics2D.Linecast(origin, point, collisionMask).normal;
    }


    void InitControllers()
    {
        string[] names = Input.GetJoystickNames();
        for (int x = 0; x < names.Length; x++)
        {
            print(names[x]);
            if (names[x].Length == 19)
            {
                print("PS4 CONTROLLER IS CONNECTED");
                PS4Controller = true;
                XboxController = false;
            }
            if (names[x].Length == 20)
            {
                print("XBOX CONTROLLER IS CONNECTED");
                //set a controller bool to true
                PS4Controller = false;
                XboxController = true;

            }
        }

        if (XboxController)
        {
            //do something
        }
        else if (PS4Controller)
        {
            //do something
        }
        else
        {
            // there is no controllers
        }
    }

    public LayerMask GetCollisionMask()
    {
        return collisionMask;
    }

	public void setGrounded (bool groundedStatus)
	{
		grounded = groundedStatus;
	}

	public void getHit()
	{
		// Run death animation
		transform.position = gameHandler.playerDeath(inputs.ID);
	}

	public int getID()
	{
		return inputs.ID;
	}
}

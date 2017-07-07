using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
	#region EditorVariables
	[SerializeField] float maxSpeed;
	[SerializeField] float acceleration;
	[SerializeField] float jumpForce;
	[SerializeField] Transform groundCheck;
	#endregion

	bool facingRight = true;
	bool grounded = false;
	bool jump = false;
	int xDir = 0; // Left: -1, Standstill: 0, Right: 1

	Rigidbody2D playerRB;

	// Use this for initialization
	void Start () {
		playerRB = gameObject.GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {

		grounded = (Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("Ground")) ||
		Physics2D.Linecast (transform.position, groundCheck.position, 1 << LayerMask.NameToLayer ("WorldBox")));

		if (Input.GetKeyDown (KeyCode.D)) {
			xDir = 1;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			xDir = -1;
		}
		if ((Input.GetKeyUp (KeyCode.A) && xDir == -1) || (Input.GetKeyUp (KeyCode.D) && xDir == 1)) {
			xDir = 0;
		}
		if (Input.GetKeyDown (KeyCode.W) && grounded) {
			jump = true;
		}	
	}

	void FixedUpdate() {
		if (xDir != 0 && (gameObject.GetComponent<Rigidbody2D> ().velocity.x * (float)xDir) < maxSpeed) {
			gameObject.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * xDir * acceleration);
			if (playerRB.velocity.x > maxSpeed) {
				playerRB.velocity = new Vector2 (Mathf.Sign (playerRB.velocity.x) * maxSpeed, playerRB.velocity.y);
			}
		}
		if (jump) {
			playerRB.AddForce (Vector2.up * jumpForce);
			jump = false;
		}
	}
}

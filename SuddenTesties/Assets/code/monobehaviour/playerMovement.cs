using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
	#region EditorVariables
	[SerializeField] int maxSpeed;
	[SerializeField] float acceleration;
	#endregion

	bool facingRight = true;
	bool grounded = false;
	bool jump = false;
	int xDir = 0; // Left: -1, Standstill: 0, Right: 1

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.D)) {
			xDir = 1;
		} else if (Input.GetKeyDown (KeyCode.A)) {
			xDir = -1;
		}
		if (Input.GetKeyDown (KeyCode.W) && grounded) {
			jump = true;
		}
	}

	void FixedUpdate() {
		
	}
}

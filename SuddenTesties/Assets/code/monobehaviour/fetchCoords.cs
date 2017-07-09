using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fetchCoords : MonoBehaviour {

	void OnGUI()
	{
		Vector3 p = new Vector3();
		Camera  c = Camera.main;
		Event   e = Event.current;
		Vector2 mousePos = new Vector2();

		// Get the mouse position from Event.
		// Note that the y position from Event is inverted.
		mousePos.x = e.mousePosition.x;
		mousePos.y = c.pixelHeight - e.mousePosition.y;

		p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.nearClipPlane));

		GUILayout.BeginArea(new Rect(20, 20, 250, 120));
		GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
		GUILayout.Label("Mouse position: " + mousePos);
		GUILayout.Label("World position: " + p.ToString("F3"));
		GUILayout.EndArea();
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log (Input.mousePosition.x.ToString () + ", " + Input.mousePosition.y.ToString ());
		}
	}
}

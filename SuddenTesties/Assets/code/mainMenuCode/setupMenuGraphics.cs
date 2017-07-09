using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class setupMenuGraphics : MonoBehaviour {

	// Use this for initialization
	void Start () {
		float minSide = GetComponent<Canvas> ().pixelRect.height > GetComponent<Canvas> ().pixelRect.width ? GetComponent<Canvas> ().pixelRect.height : GetComponent<Canvas> ().pixelRect.width;
		foreach (Transform child in transform) 
		{
			if (child.name == "Image") 
			{
				child.transform.TransformVector (minSide, minSide, 0f);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

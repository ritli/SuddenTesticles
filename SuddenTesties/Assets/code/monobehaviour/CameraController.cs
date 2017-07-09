using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] GameObject[] players;
    [SerializeField] float maxAlphaDistance = 0.5f;
    [SerializeField] bool moveToPlayer = true;
    [SerializeField] float zoomSpeed = 0.2f;
    [SerializeField] float minZoomLevel = 5;
    [SerializeField] float maxZoomLevel = 8;

    const float zPos = -10f;
    Vector3 lookAtPosition;
    Camera camera;

    float lastZoom;

    bool shakeScreen = false;
    float screenShakeMagnitude = 0f;

    void Start () {
        camera = GetComponent<Camera>();

        if (players.Length == 0)
        {
			this.Invoke("findPlayers", .1f);
        }	
	}
		
	void findPlayers()
	{
		players = GameObject.FindGameObjectsWithTag ("Player");
	}
	
	void FixedUpdate () {

        Vector2 moveToPos = Vector2.zero;

        if (moveToPlayer)
        {
            moveToPos = GetPlayerPos();
        }


        Move(moveToPos);
	}

    void Move(Vector3 toPosition)
    {
        toPosition.z = zPos;

        transform.position = Vector3.MoveTowards(transform.position, toPosition, maxAlphaDistance);

        if (shakeScreen)
        {
            transform.position += (Vector3)GetRandomVector(screenShakeMagnitude);
        }

    }

    public void LookAtPosition(Vector2 position, float time)
    {
        lookAtPosition = position;

        StartCoroutine(LookAt(time));
    }

    IEnumerator LookAt(float time)
    {
        moveToPlayer = false;

        yield return new WaitForSeconds(time);

        moveToPlayer = true;
    }

    Vector2 GetPlayerPos()
    {
        Vector2[] playerpositions = new Vector2[players.Length];

        for (int i = 0; i < players.Length; i++)
        {
            playerpositions[i] = players[i].transform.position;
        }

        ZoomToFit(playerpositions);

        return CenterOfVectors(playerpositions);
    }

    Vector2 CenterOfVectors(Vector2[] vectors)
    {
        Vector2 sum = Vector2.zero;
        if (vectors == null || vectors.Length == 0)
        {
            return sum;
        }

        foreach (Vector2 vec in vectors)
        {
            sum += vec;
        }
        return sum / vectors.Length;
    }

    void ZoomToFit(Vector2[] positions)
    {
        float greatestDistance = 0;

        for (int i = 0; i < positions.Length; i++)
        {
            float dist = Vector2.Distance(camera.WorldToScreenPoint(positions[i]), new Vector3(Screen.width * 0.5f, Screen.height * 0.5f));

            if (dist > greatestDistance)
            {
                greatestDistance = dist;
            }
        }

        float alpha = greatestDistance / new Vector2(Screen.width * 0.5f, Screen.height * 0.5f).magnitude;

        camera.orthographicSize = Mathf.Lerp(lastZoom, Mathf.Lerp(minZoomLevel, maxZoomLevel, alpha), zoomSpeed);

        lastZoom = camera.orthographicSize;
    }

    Vector2 GetRandomVector(float magnitude)
    {
        return Random.insideUnitCircle * magnitude;
    }

    public void ShakeScreen(float time, float magnitude)
    {
        screenShakeMagnitude = magnitude; 

        StartCoroutine(StartScreenShake(time));
    }

    IEnumerator StartScreenShake(float time)
    {
        shakeScreen = true;

        yield return new WaitForSeconds(time);

        shakeScreen = false;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField] GameObject player;
    [SerializeField] float maxAlphaDistance = 0.5f;
    [SerializeField] bool moveToPlayer = true;

    float zPos = -10f;
    Vector3 lookAtPosition;

    void Start () {
        if (!player)
        {
            player = GameObject.FindGameObjectWithTag("Player");
        }	
	}
	
	void FixedUpdate () {

        if (Input.GetKeyDown(KeyCode.I))
        {
            LookAtPosition(new Vector2(1, 10), 2f);
        }

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
        return player.transform.position;
    }

}

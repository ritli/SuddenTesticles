using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour {

    void DestroyObject()
    {
        Destroy(gameObject);
    }

    public void DestroyAfterSeconds(float time)
    {
        Invoke("DestroyObject", time);
    }
}

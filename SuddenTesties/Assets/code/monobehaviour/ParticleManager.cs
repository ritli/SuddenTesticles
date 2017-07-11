using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

    public static void SpawnParticlesAtPosition(GameObject system, Vector2 position, Quaternion rotation, float deathTime)
    {
        GameObject g = Instantiate(system, position, rotation);

        g.AddComponent<ParticleDestroyer>().DestroyAfterSeconds(deathTime);
    }
}

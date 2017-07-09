using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallHandler : MonoBehaviour {

    ParticleSystem ballSystem;
    ParticleSystem chargeUp;
    ParticleSystem trail;
    public GameObject end;
    GameObject endObject;

    float initialEmission;
    float initialTrailEmission;
    bool firing;

	void Start () {
        ballSystem = GetComponent<ParticleSystem>();
        chargeUp = transform.Find("ChargeUp").GetComponent<ParticleSystem>();
        trail = transform.Find("Trail").GetComponent<ParticleSystem>();

        initialEmission = ballSystem.emission.rateOverTimeMultiplier;
        var emissionModule = ballSystem.emission;

        emissionModule.rateOverTimeMultiplier = 0;

        var trailEmission = trail.emission;
        initialTrailEmission = trailEmission.rateOverTimeMultiplier;
        trailEmission.rateOverTime = 0;

        chargeUp.gameObject.SetActive(false);
    }


    public void StartAnimation()
    {
        StartCoroutine(StartRoutine());
    }

    IEnumerator StartRoutine()
    {
        firing = true;

        chargeUp.gameObject.SetActive(true);
        chargeUp.Clear();
        chargeUp.time = 0f;

        yield return new WaitForSeconds(0.4f);

        var emissionModule = ballSystem.emission;

        emissionModule.rateOverTimeMultiplier = initialEmission;

        var trailEmission = trail.emission;
        trailEmission.rateOverTime = initialTrailEmission;

    }

    public void PlayEndAnim()
    {
        endObject = Instantiate(end, transform.position, transform.rotation);

        endObject.transform.Rotate(transform.right, 180);
        Invoke("DestroyEndObject", 2f);
    }

    void DestroyEndObject()
    {
        Destroy(endObject);
    }

    public void StopAnimation()
    {
        var emissionModule = ballSystem.emission;
        emissionModule.rateOverTimeMultiplier = 0;

        var trailEmission = trail.emission;
        trailEmission.rateOverTime = 0;
        

        firing = false;
        
    }
    void Update () {
		if (!firing)
        {
            StopAnimation();
        }
	}
}

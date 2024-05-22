using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed;

    private float random;
    private Coroutine flickerCoroutine;

    void Start()
    {
        random = Random.Range(0.0f, 65535.0f);
    }

    public void LightFlickerStart()
    {
        // Start the flicker coroutine
        if (flickerCoroutine == null)
        {
            flickerCoroutine = StartCoroutine(Flicker());
        }
    }

    public void LightFlickerStop()
    {
        // Stop the flicker coroutine
        if (flickerCoroutine != null)
        {
            flickeringLight.intensity = maxIntensity;
            StopCoroutine(flickerCoroutine);
            flickerCoroutine = null;

        }
    }

    private IEnumerator Flicker()
    {
        while (true)
        {
            float noise = Mathf.PerlinNoise(random, Time.time * flickerSpeed);
            flickeringLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
            yield return null; // Wait for the next frame
        }
    }
}


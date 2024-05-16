using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightFlicker : MonoBehaviour
{
    public Light flickeringLight;
    public float minIntensity = 0.5f;
    public float maxIntensity = 1.5f;
    public float flickerSpeed = 1f;

    private float random;

    public void Start()
    {
        random = Random.Range(0.0f, 65535.0f);
    }

    public void Update()
    {
        float noise = Mathf.PerlinNoise(random, Time.time * flickerSpeed);
        flickeringLight.intensity = Mathf.Lerp(minIntensity, maxIntensity, noise);
    }

}

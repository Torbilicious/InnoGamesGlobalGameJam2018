using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseController : MonoBehaviour
{

    public float noiseIntensity = 2.0f;
    public float noiseReductionSpeed = 0.7f;
    public float noiseBaseRange = 2.0f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Implement noise reduction
        if (transform.localScale.x > 0.25)
        {
            if (transform.localScale.x < 0.25)
            {
                return;
            }
            else
            {
                transform.localScale *= noiseReductionSpeed;
            }
        }

    }

    public void applyNoise(float magnitude)
    {
        transform.localScale += new Vector3(noiseBaseRange, noiseBaseRange, noiseBaseRange) * noiseIntensity * magnitude;
    }

}

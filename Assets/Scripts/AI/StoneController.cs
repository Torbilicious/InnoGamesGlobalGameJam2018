using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneController : MonoBehaviour {

    public NoiseController noiseArea;

    public float noise = 2.0f;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnCollisionEnter(Collision collision)
    {
        noiseArea.applyNoise(collision.impulse.magnitude * noise);
    }
}

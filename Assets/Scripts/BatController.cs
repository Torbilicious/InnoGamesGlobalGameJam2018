using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
	private float x = 0.5f, y = 0.5f;
	private float sound;
	
	// Use this for initialization
	void Start () {
		Random.InitState(83495903);
		sound = Random.Range(5, 20);
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Random.Range(0, 100) <= 1)
		{
			x *= -1;
		}
		if (Random.Range(0, 100) <= 1)
		{
			y *= -1;
		}
		
		transform.Translate(x * Time.fixedDeltaTime, y * Time.fixedDeltaTime, 0);

		sound -= Time.fixedDeltaTime;

		if (sound <= 0)
		{
			sound = Random.Range(5, 20);
		
			GetComponent<AudioSource>().Play();
		}
	}
}

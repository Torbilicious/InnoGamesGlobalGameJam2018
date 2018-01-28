using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatController : MonoBehaviour
{
	private float x = 0.5f, y = 0.5f;
	
	// Use this for initialization
	void Start () {
		Random.InitState(83495903);
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
	}
}

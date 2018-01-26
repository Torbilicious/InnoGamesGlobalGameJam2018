using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamplightController : MonoBehaviour
{
	public Rigidbody enemy;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	private void OnTriggerStay(Collider other)
	{
//		Destroy(other.gameObject);
		Debug.Log("Collision with lamplight");
	}
}

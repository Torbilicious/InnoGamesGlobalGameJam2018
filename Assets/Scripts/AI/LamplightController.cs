using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LamplightController : MonoBehaviour
{
	public Rigidbody enemyBody;
	public EnemyController enemy;
	
	public bool hasColl = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private void OnTriggerExit(Collider other)
	{
		hasColl = false;
	}

	private void OnTriggerStay(Collider other)
	{
		hasColl = true;
	}
}

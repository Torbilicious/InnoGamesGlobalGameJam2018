using UnityEngine;

public class SpawnController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SpawnController spawnController = FindObjectOfType<SpawnController>();
		
//		Create
		
		transform.position = spawnController.transform.position;
	}
}

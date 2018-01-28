using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ChestController : MonoBehaviour
{

	public bool IsOpen = false;

	public Transform Open;
	public Transform Close;

	private bool StonesGiven = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if (IsOpen && !StonesGiven)
		{
			StonesGiven = true;
			//TODO: 3 Steine dem Spieler geben
		}
		
		Close.gameObject.SetActive(!IsOpen);
		Open.gameObject.SetActive(IsOpen);
	}
}

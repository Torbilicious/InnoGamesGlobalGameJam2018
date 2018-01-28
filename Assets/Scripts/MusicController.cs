using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.AI;
using UnityEngine;

public class MusicController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		var alarm = GetComponents<AudioSource>()[1];
		alarm.Play();
		alarm.Pause();
	}
	
	// Update is called once per frame
	void Update () {
		
		var enemyControllers = FindObjectsOfType<EnemyController>();

		var isAlarm = enemyControllers.FirstOrDefault(e => e.isFollowing) != null;

		var alarm = GetComponents<AudioSource>()[1];
		var main =  GetComponents<AudioSource>()[0];
		
		if (isAlarm && !alarm.isPlaying)
		{
			main.Pause();
			alarm.UnPause();
		}
		else if(!isAlarm && alarm.isPlaying)
		{
			main.UnPause();
			alarm.Pause();
		}
	}
}

using System;
using System.Security.Cryptography;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyController : MonoBehaviour
	{
		public LamplightController lampLight;
		
		public Animator animator;
		
		public bool isDead = false;
		public float baseSpeed = 1.0f;

		public float spawnPosX = 0.0f;
		
		public bool followNoise = false;
		public float followPosX = 0.0f;
		public int followTimeout = 0;
	
		// Use this for initialization
		void Start () {
			animator.Play("Enemy_Idle");
			spawnPosX = transform.position.x;
		}
	
		// Update is called once per frame
		void Update ()
		{
			// can be re-used if a death animation is used
			if (isDead) 
			{
				UpdateDead();
				return; // don't do anything if the enemy is dead
			}
		
			if (lampLight.hasColl || followNoise || followTimeout > 0)
			{
				if (followTimeout > 0)
				{
					--followTimeout;
					if (followTimeout == 0)
					{
						followNoise = true;
						followPosX = spawnPosX;
					}
				}
				
				if (lampLight.hasColl)
				{
					followNoise = false;
					updatePlayerPos();
				}

				animator.Play("Enemy_Walking");
				FollowPos(followPosX);
			} else {
				animator.Play("Enemy_Idle");
			}
		}

		void updatePlayerPos()
		{
			followPosX = FindObjectOfType<PlayerController>().transform.position.x;
		}
		
		void UpdateDead()
		{
			//Destroy(gameObject);
		}
		
		void Die()
		{
			isDead = true;
			GetComponent<Collider>().isTrigger = true;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			transform.Translate(new Vector3(0.0f, 0.0f, -0.2f));
		}
	
		void FollowPos(float posX)
		{
			float lightSizeX = lampLight.GetComponent<Collider>().bounds.size.x;
			float distance = Mathf.Abs((float)transform.position.x - posX);
			float distanceNorm = Mathf.Clamp(1.0f - (distance / lightSizeX), 0.0f, 1.0f);
			
			if (followNoise)
			{
				distanceNorm = 0.25f;
			}
			
			float realSpeed = baseSpeed * distanceNorm;
			
			transform.Translate(new Vector3(realSpeed, 0.0f, 0.0f));
			animator.speed = (Math.Max(0.1f, distanceNorm)) * 4.0f;

			if (distance <= 0.001)
			{
				followNoise = false;
				
				if(spawnPosX != followPosX)
					followTimeout = 60 * 2;
			}
		}
	
		void OnCollisionEnter(Collision other)
		{
			if (isDead) return;

			if (other.gameObject.CompareTag("Player"))
			{
				if (lampLight.hasColl)
				{
					FindObjectOfType<PlayerController>().Die();
				} else {
					Die();
				}
			}
		}

		private void OnTriggerEnter(Collider other)
		{
			if (other.gameObject.CompareTag("NoiseArea"))
			{
				updatePlayerPos();
				followNoise = true;
			}
		}
	}
}

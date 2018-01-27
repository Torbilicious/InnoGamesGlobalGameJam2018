using System;
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
	
	
		// Use this for initialization
		void Start () {
			animator.Play("Enemy_Idle");
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
		
			if (lampLight.hasColl)
			{
				animator.Play("Enemy_Walking");
				FollowPlayer();
			}
			else
			{
				animator.Play("Enemy_Idle");
			}
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
	
		void FollowPlayer()
		{
			float lightSizeX = lampLight.GetComponent<Collider>().bounds.size.x;
			float distance = Mathf.Abs(transform.position.x - FindObjectOfType<PlayerController>().transform.position.x);
			float distanceNorm = Mathf.Clamp(1.0f - (distance / lightSizeX), 0.0f, 1.0f);
			
			float realSpeed = baseSpeed * distanceNorm;
			
			transform.Translate(new Vector3(realSpeed, 0.0f, 0.0f));
			animator.speed = (Math.Max(0.1f, distanceNorm)) * 4.0f;
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
	}
}

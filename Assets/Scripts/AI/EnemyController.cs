using System;
using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.AI
{
	[RequireComponent(typeof(Rigidbody))]
	public class EnemyController : MonoBehaviour
	{
		public enum Direction
		{
			LEFT,
			RIGHT
		}
		
		public LamplightController lampLight;
		public Animator animator;
		
		public bool isDead = false;
		public float baseSpeed = 1.0f;

		public float spawnPosX = 0.0f;
		
		public bool followNoise = false;
		public float followPosX = 0.0f;
		public int followTimeout = 0;

		public Direction direction;
		
		// Use this for initialization
		void Start ()
		{
			SetDirection(direction, false, true);
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
				else
				{
					if (lampLight.hasColl)
					{
						followNoise = false;
						updatePlayerPos();
					}

					//animator.Play((direction == Direction.RIGHT) ? "Enemy_Walking" : "Enemy_Walking_Left");
					SetDirection(direction, true, true);
					FollowPos(followPosX);
				}
			} else
			{
				SetDirection(direction, followNoise, true);
			}
		}

		void updatePlayerPos()
		{
			followPosX = FindObjectOfType<PlayerController>().transform.position.x;
		}
		
		void UpdateDead()
		{
			//Destroy(gameObject);
			animator.Play(direction == Direction.LEFT ? "Enemy_Die_Left" : "Enemy_Die");
		}
		
		void Die()
		{
			isDead = true;
			GetComponent<Collider>().isTrigger = true;
			GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
			transform.Translate(new Vector3(0.0f, 0.0f, -0.2f));
		}

		void SetDirection(Direction dir, bool walking = false, bool force = false)
		{
			if (force || direction != dir)
			{
				direction = dir;

				Vector3 lampPos = lampLight.transform.position;
				float lampWidth = lampLight.GetComponent<Collider>().bounds.extents.x;
				float lampOffset = 0.6f;
				
				if (direction == Direction.LEFT)
				{
					animator.Play(walking ? "Enemy_Walking_Left" : "Enemy_Idle_Left");
					lampPos.x = transform.position.x - (lampWidth + lampOffset);
				} else {
					animator.Play(walking ? "Enemy_Walking" : "Enemy_Idle");
					lampPos.x = transform.position.x + (lampWidth + lampOffset);
				}
				
				lampLight.transform.position = lampPos;
			}
		}
		
		void FollowPos(float posX)
		{
			float lightSizeX = lampLight.GetComponent<Collider>().bounds.size.x;
			float distance = Mathf.Abs((float)transform.position.x - posX);
			float distanceNorm = Mathf.Clamp(1.0f - (distance / lightSizeX), 0.0f, 1.0f);
			distanceNorm = Math.Max(0.125f, distanceNorm);
			
			if (followNoise)
			{
				distanceNorm = 0.25f;
			}
			
			float realSpeed = baseSpeed * distanceNorm;
			float walkDirection = Math.Sign(posX - transform.position.x);
			
			SetDirection(walkDirection > 0 ? Direction.RIGHT : Direction.LEFT, true);
			
			transform.Translate(new Vector3(realSpeed * walkDirection, 0.0f, 0.0f));
			animator.speed = (Math.Max(0.1f, distanceNorm)) * 4.0f;
			
			if (distance <= 0.1)
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
				followPosX = other.gameObject.transform.position.x;		
				followNoise = true;
			} 
		}
	}
}

using UnityEngine;

namespace Assets.Scripts.AI
{
	public class EnemyController : MonoBehaviour
	{
		public LamplightController lampLight;
	
		public bool isDead = false;
		public float baseSpeed = 1.0f;
	
		private const int deadAnimTimeTotal = 60; // total time the death animation takes
		private int deadAnimTime = 0; // current death animation time
	
		// Use this for initialization
		void Start () {
		
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (isDead) 
			{
				UpdateDead();
				return; // don't do anything if the enemy is dead
			}
		
			if (lampLight.hasColl)
			{
				FollowPlayer();
			}	
		}

		void UpdateDead()
		{
			if (deadAnimTime > 0)
			{
				--deadAnimTime;
				float deadTimerNormal = ((float) deadAnimTime / (float) deadAnimTimeTotal);
				transform.localScale *= deadTimerNormal;
			}
			else
			{
				Destroy(gameObject);
			}
		}
		
		void Die()
		{
			isDead = true;
			deadAnimTime = deadAnimTimeTotal;
			GetComponent<Collider>().enabled = false;
		}
	
		void FollowPlayer()
		{
			float lightSizeX = lampLight.GetComponent<Collider>().bounds.size.x;
			float distance = Mathf.Abs(transform.position.x - FindObjectOfType<PlayerController>().transform.position.x);
			float distanceNorm = Mathf.Clamp(1.0f - (distance / lightSizeX), 0.0f, 1.0f);
			
			float realSpeed = baseSpeed * distanceNorm;
			transform.Translate(new Vector3(realSpeed, 0.0f, 0.0f));
		}
	
		void OnCollisionEnter(Collision other)
		{
			if (isDead) return;
		
			if (other.gameObject.name == "Player")
			{
				if (lampLight.hasColl)
				{
					FindObjectOfType<PlayerController>().die();
				} else {
					Die();
				}
			}
		}
	}
}

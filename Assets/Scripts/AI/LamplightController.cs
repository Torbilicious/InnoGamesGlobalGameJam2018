using UnityEngine;

namespace Assets.Scripts.AI
{
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
			if (other.gameObject.CompareTag("Player"))
			{
				hasColl = false;
			}
		}

		private void OnTriggerStay(Collider other)
		{
			if (other.gameObject.CompareTag("Player"))
			{
				hasColl = true;
			}
		}
	}
}

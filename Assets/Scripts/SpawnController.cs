using UnityEngine;

namespace Assets.Scripts
{
	public class SpawnController : MonoBehaviour
	{
		public Transform player;

		// Use this for initialization
		void Start () {			
//			var player = GameObject.FindGameObjectWithTag("Player");
			Instantiate(player);
		}
	}
}

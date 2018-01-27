using UnityEngine;

namespace Assets.Scripts
{
	public class CameraController : MonoBehaviour {
		
		public float camSmoothVal = 0.25f;
		public Vector3 posOffset = new Vector3(0.0f, -1.2f, 0.0f);
	
		private Vector3 lastCamPos;	
	
		// Use this for initialization
		void Start () {
			lastCamPos = transform.position;
		}

		// Update is called once per frame
		void Update () {
			var player = GameObject.FindGameObjectWithTag("Player").transform;

			// get player pos. limited to X/Y
			var newCamPos = player.position;
			newCamPos.z = lastCamPos.z;
		
			// follow only a fraction of the distance
			transform.position += (newCamPos - lastCamPos) * camSmoothVal;

			lastCamPos = transform.position + posOffset;
		}
	}
}

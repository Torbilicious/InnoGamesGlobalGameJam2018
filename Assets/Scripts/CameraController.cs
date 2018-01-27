using UnityEngine;

namespace Assets.Scripts
{
	public class CameraController : MonoBehaviour {

		public Camera playerCamera;
		public Rigidbody playerBody;
	
		// Camera settings
		public float camSmoothVal = 0.25f;
	
		private Vector3 lastCamPos;
	
	
		// Use this for initialization
		void Start () {
			lastCamPos = transform.position;
		}

		// Update is called once per frame
		void Update () {
		
			// get player pos. limited to X/Y
			var newCamPos = playerBody.position;
			newCamPos.z = lastCamPos.z;
		
			// follow only a fraction of the distance
			transform.position += (newCamPos - lastCamPos) * camSmoothVal;
		
			lastCamPos = transform.position;
		}
	}
}

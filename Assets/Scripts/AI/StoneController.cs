using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class StoneController : MonoBehaviour
    {
        public NoiseController noiseArea;
        public float noise = 2.0f;

        private bool isActive = true;
        
        private void OnCollisionEnter(Collision collision)
        {
            if (isActive)
            {   
                noiseArea.applyNoise(collision.impulse.magnitude * noise);

                if (collision.gameObject.CompareTag("Ground"))
                {
                    GetComponent<Collider>().isTrigger = true;
                    GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    transform.Translate(new Vector3(0.0f, 0.0f, -0.2f));
                    isActive = false;
                }
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                this.gameObject.SetActive(false);
            }

        }
    }
}
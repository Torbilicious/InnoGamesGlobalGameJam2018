using Assets.Scripts.Player;
using UnityEngine;

namespace Assets.Scripts.AI
{
    public class StoneController : MonoBehaviour
    {
        public NoiseController noiseArea;
        public float noise = 2.0f;

        private void OnCollisionEnter(Collision collision)
        {
            noiseArea.applyNoise(collision.impulse.magnitude * noise);
        }
    }
}
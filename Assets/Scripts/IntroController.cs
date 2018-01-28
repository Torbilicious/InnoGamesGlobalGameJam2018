using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

namespace Assets.Scripts
{
    public class IntroController : MonoBehaviour
    {
        private float waitTime = 1.0f;

        void Update()
        {
            waitTime -= Time.fixedDeltaTime;

            if (waitTime > 0.0f) return;

            if (!FindObjectOfType<VideoPlayer>().isPlaying)
            {
                SceneManager.LoadScene("Level1", LoadSceneMode.Single);
            }
        }
    }
}
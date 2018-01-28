using Assets.Scripts.Player;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class StonesController : MonoBehaviour
    {
        private PlayerController playerController;

        public Sprite stonesZero;
        public Sprite stonesOne;
        public Sprite stonesTwo;
        public Sprite stonesThree;

        // Update is called once per frame
        void Update()
        {
            playerController = FindObjectOfType<PlayerController>();

            var images = gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                Sprite currentImage;

                switch (playerController.countStones)
                {
                    case 0:
                        currentImage = stonesZero;
                        break;
                    case 1:
                        currentImage = stonesOne;
                        break;
                    case 2:
                        currentImage = stonesTwo;
                        break;
                    case 3:
                        currentImage = stonesThree;
                        break;

                    default:
                        currentImage = stonesZero;
                        break;
                }

                image.sprite = currentImage;
            }
        }
    }
}
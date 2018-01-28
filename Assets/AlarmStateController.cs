using System.Linq;
using Assets.Scripts.AI;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class AlarmStateController : MonoBehaviour
    {
        public Sprite undetected;
        public Sprite alarm;


        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            var enemyControllers = FindObjectsOfType<EnemyController>();

            var isAlarm = enemyControllers.FirstOrDefault(e => e.isFollowing) != null;

            var images = gameObject.GetComponentsInChildren<Image>();
            foreach (Image image in images)
            {
                var currentImage = isAlarm ? alarm : undetected;
                image.sprite = currentImage;
            }
        }
    }
}
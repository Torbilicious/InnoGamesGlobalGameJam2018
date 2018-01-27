using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        private enum Direction
        {
            LEFT,
            RIGHT
        }

        private Direction direction;
        public NoiseController noiseArea;

        private float noiseLevel = 1.0f;

        public float movementSpeed;
        public Vector3 jump;
        public float deathBarrierY = -40.0f;

        public Animator animator;

        public float jumpForce = 2.0f;
        private float distToGround;
        private Rigidbody rb;

        private bool lastDirection = false;
        private bool isSneaking = false;
        private bool isGrounded = false;
        private bool isOnLadder = false;

        public Transform stone;
        public float throwRange = 8.0f;

        private float mass = 0;

        void Start()
        {
            distToGround = GetComponent<Collider>().bounds.extents.y;

            rb = GetComponent<Rigidbody>();
            jump = new Vector3(0.0f, 2.0f, 0.0f);

            spawn();
        }

        void Update()
        {
            if (transform.position.y < deathBarrierY)
            {
                die();
            }

            var x = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
            var y = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * movementSpeed / 2;

            if (isOnLadder)
            {
                rb.Sleep();
            }
            else
            {
                y = 0;
            }

            //Animation
            if (!isGrounded && !isOnLadder)
            {
                animator.Play("Player_Jump");
            }
            else if (Math.Abs(x) > 0)
            {
                if (isSneaking)
                {
                    animator.Play("Player_Sneaking"); //TODO: Animation fehlt noch
                }
                else
                {
                    animator.Play("Player_Walking");
                }

                var isLeftDirection = x < 0;
                direction = isLeftDirection ? Direction.LEFT : Direction.RIGHT;
                
                if (lastDirection != isLeftDirection)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                    lastDirection = isLeftDirection;
                }
            }
            else
            {
                if (isSneaking)
                {
                    animator.Play("Player_Duck");
                }
                else
                {
                    animator.Play("Player_Right");
                }
            }

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movementSpeed /= 2;
                noiseArea.noiseIntensity /= 2;
                isSneaking = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                movementSpeed *= 2;
                noiseArea.noiseIntensity *= 2;
                isSneaking = false;
            }

            if ((Input.GetButton("Jump") || CrossPlatformInputManager.GetButtonDown("Jump")) && isGrounded)
            {
                rb.velocity = new Vector3(0f, 0f, 0f);
                rb.angularVelocity = new Vector3(0f, 0f, 0f);
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            transform.Translate(x, y, 0);

            if (CrossPlatformInputManager.GetButtonDown("Fire"))
            {
                Debug.Log("test");
                var temp = Instantiate(stone,
                    new Vector3(transform.position.x, transform.position.y, transform.position.z), transform.rotation);

                temp.GetComponent<Rigidbody>().AddForce(throwRange, throwRange / 2, 0, ForceMode.Impulse);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }

            if (other.gameObject.CompareTag("Item"))
            {
                other.gameObject.SetActive(false);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }

            if (other.gameObject.CompareTag("Ladder"))
            {
                Debug.Log("LADDER");
                isOnLadder = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = false;
            }


            if (other.gameObject.CompareTag("Ladder"))
            {
                isOnLadder = false;
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            var colProps = collision.gameObject.GetComponent<CollisionProperties>();

            if (colProps != null)
            {
                noiseLevel = colProps.noiselevel;
            }
            else
            {
                noiseLevel = 1.0f;
            }

            noiseArea.applyNoise(collision.impulse.magnitude * noiseLevel);
        }

        private void OnCollisionStay(Collision collision)
        {
            var colProps = collision.gameObject.GetComponent<CollisionProperties>();
            if (colProps != null)
            {
                noiseArea.applyNoise(Math.Abs(Input.GetAxis("Horizontal")));
            }
        }

        public void spawn()
        {
            var spawn = FindObjectOfType<SpawnController>();
            transform.position = spawn.transform.position;
        }

        public void die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
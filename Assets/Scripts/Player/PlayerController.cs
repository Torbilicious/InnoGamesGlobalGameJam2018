using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public NoiseController noiseArea;

        public float noiseReductionSpeed = 0.7f;
        public float noiseBaseRange = 2.0f;

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
                spawn();
            }

            var x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
            var y = Input.GetAxis("Vertical") * Time.deltaTime * movementSpeed / 2;
            
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
                
                bool direction = x < 0;
                if (lastDirection != direction)
                {
                    Vector3 scale = transform.localScale;
                    scale.x *= -1;
                    transform.localScale = scale;
                    lastDirection = direction;
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
                movementSpeed = movementSpeed / 2;
                noiseArea.noiseIntensity = noiseArea.noiseIntensity / 2;
                isSneaking = true;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                movementSpeed = movementSpeed * 2;
                noiseArea.noiseIntensity = noiseArea.noiseIntensity * 2;
                isSneaking = false;

            }
            if (Input.GetButton("Jump") && isGrounded)
            {
                rb.velocity = new Vector3(0f,0f,0f); 
                rb.angularVelocity = new Vector3(0f,0f,0f);
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
                isGrounded = false;
            }

            if(noiseArea.transform.localScale.x > 0.5)
            {
                if(noiseArea.transform.localScale.x < 0.5 )
                {
                    return;
                }
                else
                {
                    //noiseArea.transform.localScale -= new Vector3(noiseReductionSpeed, noiseReductionSpeed, noiseReductionSpeed);
                    noiseArea.transform.localScale *= noiseReductionSpeed;
                }
            }
            
            transform.Translate(x, y, 0);
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
            if(colProps != null)
            {
                noiseLevel = colProps.noiselevel;        Debug.Log(colProps.noiselevel);
            }
            else
            {
                noiseLevel = 1.0f;
            }

            applyNoise(collision.impulse.magnitude);
              //  noiseArea.transform.localScale += new Vector3(noiseBaseRange, noiseBaseRange, noiseBaseRange) * noiseArea.noiseIntensity * collision.impulse.magnitude* noiseLevel;
        }

        private void OnCollisionStay(Collision collision)
        {
            var colProps = collision.gameObject.GetComponent<CollisionProperties>();
            if (colProps != null)
            {
              
                    applyNoise(Math.Abs(Input.GetAxis("Horizontal")));

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

        private void applyNoise (float magnitude)
        {
            noiseArea.transform.localScale += new Vector3(noiseBaseRange, noiseBaseRange, noiseBaseRange) * noiseArea.noiseIntensity * magnitude * noiseLevel;
        }
    }
}
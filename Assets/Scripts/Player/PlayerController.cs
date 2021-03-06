﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityStandardAssets.CrossPlatformInput;

namespace Assets.Scripts.Player
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
        public Transform StoneThrowingPosition;

        private float noiseLevel = 1.0f;

        public float movementSpeed;
        public Vector3 jump;
        public float deathBarrierY = -40.0f;

        public Animator animator;

        public float jumpForce = 2.0f;
        private Rigidbody rb;

        private bool isLanded = false;
        private bool lastDirection;
        private bool isSneaking;
        private bool isGrounded;
        private bool isOnLadder;
        private bool isHanging = false;
        private bool isThrowing = false;
        private float throwT = 0.5f;
        private float soundP = 0.0f;

        public Transform stone;
        public float throwRange = 8.0f;
        public int countStones;

        private float mass = 0;

        void Start()
        {
            rb = GetComponent<Rigidbody>();
            jump = new Vector3(0.0f, 2.0f, 0.0f);
            countStones = 0;
            Spawn();
            
            animator.Play("Player_Fall");
        }

        void Update()
        {            
            if (!isLanded && !isGrounded)
            {
                return;
            }
            else
            {
                isLanded = true;
            }
            
            if (transform.position.y < deathBarrierY)
            {
                Die();
            }

            var x = CrossPlatformInputManager.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
            var y = CrossPlatformInputManager.GetAxis("Vertical") * Time.deltaTime * movementSpeed / 2;

            if (isOnLadder || isHanging)
            {
                rb.Sleep();
            }
            else
            {
                y = 0;
            }
            if (isThrowing)
            {
                x = 0;
                y = 0;
            }
            
            transform.Translate(x, y, 0);

            //Richtung nur updaten wenn man sich bewegt, denn x < 0 ist true wenn x = 0
            //d.h. Wenn man dies immer updated dreht man sich nach Rechts sobald man stehen bleibt
            if (Math.Abs(x) > 0)
            {
                lastDirection = x < 0;
                direction = lastDirection ? Direction.LEFT : Direction.RIGHT;
            }

            Animate(x, y);

            if (CrossPlatformInputManager.GetButtonDown("Sneak"))
            {
                movementSpeed /= 2;
                noiseArea.noiseIntensity /= 2;
                isSneaking = true;
            }

            if (CrossPlatformInputManager.GetButtonUp("Sneak"))
            {
                movementSpeed *= 2;
                noiseArea.noiseIntensity *= 2;
                isSneaking = false;
            }
//            if ((Input.GetButton("Back") || CrossPlatformInputManager.GetButtonDown("Back")) && isGrounded)
//            {
//                SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
//            }


            if ((Input.GetButton("Jump") || CrossPlatformInputManager.GetButtonDown("Jump")) && isGrounded)
            {
                Jump();
            }


            if (CrossPlatformInputManager.GetButtonDown("Fire"))
            {
                if (countStones > 0 && !isThrowing)
                {
                    countStones = countStones - 1;
                    isThrowing = true;
                    throwT = 0.75f;
                }
            }
        }

        private void Animate(float x, float y)
        {
            animator.enabled = true;

            if (isThrowing && throwT > 0)
            {
                throwT -= Time.fixedDeltaTime;

                if (throwT <= 0)
                {
                    isThrowing = false;
                    ThrowStone();
                }
                
                if (lastDirection)
                {
                    
                    animator.Play("Player_Throw_Left");
                }
                else
                {
                    animator.Play("Player_Throw");
                }
            }
            else if (isOnLadder)
            {
                if (Math.Abs(y) < 0.01f)
                {
                    animator.enabled = false;
                }
                else if(lastDirection)
                {
                    animator.Play("Player_Climb_Left");
                }
                else
                {
                    animator.Play("Player_Climb");
                }
            }
            else if (isHanging)
            {
                animator.Play("Player_Hanging");
                if (Math.Abs(x) < 0.01f)
                {
                    animator.enabled = false;
                }
            }
            else if (Math.Abs(x) > 0)
            {
                if (x > 0)
                {
                    if (!isGrounded && !isOnLadder)
                    {
                        animator.Play("Player_Jump");
                    }                      
                    else if (isSneaking)
                    {
                        animator.Play("Player_Sneaking");
                        PlayWalking(true);
                    }
                    else
                    {
                        animator.Play("Player_Walking");
                        PlayWalking(false);
                    }
                }
                else
                {
                    if (!isGrounded && !isOnLadder)
                    {
                        animator.Play("Player_Jump_Left");
                    }
                    else if (isSneaking)
                    {
                        animator.Play("Player_Sneaking_Left");
                        PlayWalking(true);
                    }
                    else
                    {
                        animator.Play("Player_Walking_Left");
                        PlayWalking(false);
                    }
                }
            }
            else
            {
                if (!lastDirection)
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
                else
                {
                    if (isSneaking)
                    {
                        animator.Play("Player_Duck_Left");
                    }
                    else
                    {
                        animator.Play("Player_Left");
                    }
                }
            }
        }

        private void PlayWalking(bool sneaking)
        {
            soundP -= Time.fixedDeltaTime;

            if (soundP <= 0)
            {
                var list = GetComponents<AudioSource>();
                int r = UnityEngine.Random.Range(0, list.Length);
                list[r].Play();
                soundP = sneaking ? 0.5f : 0.3f;
            }
        }

        private void ThrowStone()
        {
            Debug.Log("test");
            var temp = Instantiate(stone, StoneThrowingPosition.position, transform.rotation);

            temp.GetComponent<Rigidbody>().AddForce(direction == Direction.LEFT ? -throwRange : throwRange, throwRange / 2, 0, ForceMode.Impulse);
       
        }

        private void Jump()
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
            rb.angularVelocity = new Vector3(0f, 0f, 0f);
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Ground"))
            {
                isGrounded = true;
            }
            
            if (other.gameObject.CompareTag("Chest"))
            {
                ChestController c = other.gameObject.GetComponent<ChestController>();
                if (!c.IsOpen)
                {
                    c.IsOpen = true;
                    countStones += 3;
                }
            }

            if (other.gameObject.CompareTag("Item"))
            {      
                if (countStones < 3)
                {
                    other.gameObject.SetActive(false);
                    countStones = countStones + 1;
                }
                
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
                isOnLadder = true;
            }
            
            if (other.gameObject.CompareTag("Hang"))
            {
                isHanging = true;
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
            
            if (other.gameObject.CompareTag("Hang"))
            {
                isHanging = false;
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

        private void Spawn()
        {
            var spawn = FindObjectOfType<SpawnController>();
            transform.position = spawn.transform.position;
        }

        public void Die()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
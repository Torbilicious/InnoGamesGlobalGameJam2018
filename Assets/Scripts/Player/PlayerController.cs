﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : MonoBehaviour
    {
        public NoiseController noiseArea;

        public float movementSpeed;
        public Vector3 jump;
        public float deathBarrierY = -40.0f;
    
        public float jumpForce = 2.0f;
        private float distToGround;
        private Rigidbody rb;


        void Start()
        {
            distToGround = GetComponent<Collider>().bounds.extents.y;
        
            rb = GetComponent<Rigidbody>();
            jump = new Vector3(0.0f, 2.0f, 0.0f);

            spawn();
        }

        Boolean isGrounded() {
            return Physics.Raycast(transform.position, -Vector3.up, distToGround + 0.1f);
        }

        void Update()
        {
            if (transform.position.y < deathBarrierY)
            {
                spawn();
            }

            var x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;
            transform.Translate(x, 0, 0);

            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                movementSpeed = movementSpeed / 2;
            }

            if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                movementSpeed = movementSpeed * 2;
            }

            if (Input.GetButton("Jump") && isGrounded())
            {
                rb.AddForce(jump * jumpForce, ForceMode.Impulse);
//            isGrounded = false;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Item"))
            {
                other.gameObject.SetActive(false);
            }
        }

        private void OnCollisionEnter(Collision collision)
        {
            //if (collision.gameObject.CompareTag("Ground"))
            //{
            //if(noiseArea.gameObject.name != noiseArea.name)
           
                noiseArea.transform.localScale += new Vector3(0.1F, 0.1F, 0.1F) * noiseArea.noiseIntensity;
            
                Debug.Log(noiseArea.gameObject.name);

            //}
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
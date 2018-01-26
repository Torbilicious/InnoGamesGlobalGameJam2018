using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
	public float movementSpeed;
	
	public Vector3 jump;
	public float jumpForce = 2.0f;
     
	public bool isGrounded;
	Rigidbody rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody>();
		jump = new Vector3(0.0f, 2.0f, 0.0f);
	}
	
	void OnCollisionStay()
	{
		isGrounded = true;
	}
	
	// Update is called once per frame
	void Update () {
		var x = Input.GetAxis("Horizontal") * Time.deltaTime * movementSpeed;

		transform.Translate(x, 0, 0);
		
		if(Input.GetKeyDown(KeyCode.Space) && isGrounded){
			rb.AddForce(jump * jumpForce, ForceMode.Impulse);
			isGrounded = false;
		}
	}
}

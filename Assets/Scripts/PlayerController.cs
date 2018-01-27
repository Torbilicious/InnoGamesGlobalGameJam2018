using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public float movementSpeed;

    public Vector3 jump;
    public float jumpForce = 2.0f;

    public bool isGrounded;

    Rigidbody rb;

    public float deathBarrierY = -40.0f;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        jump = new Vector3(0.0f, 2.0f, 0.0f);

        respawn();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void Update()
    {
        if (transform.position.y < deathBarrierY)
        {
            respawn();
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

        if (Input.GetButton("Jump") && isGrounded)
        {
            rb.AddForce(jump * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Item"))
        {
            other.gameObject.SetActive(false);
        }
    }

    public void respawn()
    {
        var spawn = FindObjectOfType<SpawnController>();
        transform.position = spawn.transform.position;
    }
}
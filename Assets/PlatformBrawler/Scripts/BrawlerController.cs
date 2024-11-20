using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public Vector3 respawnPosition = new Vector3(0f, 3f, 0f);
    private Rigidbody rb;
    public Rigidbody rbPusher;

    private Animator player_animator;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        player_animator = GetComponent<Animator>();

        rb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        float movHorizontal = Input.GetAxis("Horizontal");
        float movVertical = Input.GetAxis("Vertical");

        //Player Movement
        Vector3 movement = new Vector3(movHorizontal, 0.0f, movVertical);
        transform.Translate(movement * movSpeed * Time.deltaTime, Space.World);
        
        //Player Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        //Player Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player_animator.SetTrigger("Attack");
        }
    }

    public void AddForce()
    {
        rbPusher.AddForce(10, 0, 0, ForceMode.Impulse);
    }

    void OnTriggerEnter(Collider other)
    {
        //Respawn Function
        if (other.CompareTag("Death"))
        {
            transform.position = respawnPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public Vector3 respawnPosition = new Vector3(-5f, 0.5f, 0f);
    private Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        rb.useGravity = true;
    }

    // Update is called once per frame
    void Update()
    {
        float movHorizontal = Input.GetAxis("Horizontal");
        float movVertical = Input.GetAxis("Vertical");

        //Cube Movement
        Vector3 movement = new Vector3(movHorizontal, 0.0f, movVertical);
        transform.Translate(movement * movSpeed * Time.deltaTime, Space.World);
        
        //rb.velocity = new Vector3 (movHorizontal * movSpeed, 0, movVertical * movSpeed);

        //Cube Rotation
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
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

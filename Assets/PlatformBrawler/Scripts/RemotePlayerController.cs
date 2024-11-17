using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerController : MonoBehaviour
{
    public RemoteInputs finalRemoteInputs;  

    private float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public Vector3 respawnPosition = new Vector3(5f, 0.5f, 0f);
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        finalRemoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
    }

    void Update()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector3 movement = Vector3.zero;

        //Remote Cube Movement
        if (finalRemoteInputs.Apressed) movement += Vector3.left;
        if (finalRemoteInputs.Dpressed) movement += Vector3.right;
        if (finalRemoteInputs.Wpressed) movement += Vector3.forward;
        if (finalRemoteInputs.Spressed) movement += Vector3.back;

        //Remote Cube Rotation
        if (finalRemoteInputs.Qpressed)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        if (finalRemoteInputs.Epressed)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        rb.MovePosition(rb.position + movement * movSpeed * Time.deltaTime);
    }
    /*
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
    */
} 


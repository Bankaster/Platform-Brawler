using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerController : MonoBehaviour
{
    public RemoteInputs finalRemoteInputs;  

    private float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public Vector3 respawnPosition = new Vector3(0f, 3f, 0f);
    private Rigidbody rb;
    public Rigidbody rbPusher;

    private Animator remote_player_animator;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        finalRemoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
        remote_player_animator = GetComponent<Animator>();
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

        //Player Attack
        if (finalRemoteInputs.SpacePressed)
        {
            remote_player_animator.SetTrigger("Attack");
        }

        rb.MovePosition(rb.position + movement * movSpeed * Time.deltaTime);
    }

    public void AddForce()
    {
        float forceMagnitude = 20f;
        Vector3 forceDirection = transform.right;
        Vector3 force = forceDirection * forceMagnitude;
        rbPusher.AddForce(force, ForceMode.Impulse);
    }
    public void ResetForce()
    {
        rbPusher.AddForce(0, 0, 0);
    }

    void OnTriggerEnter(Collider other)
    {
        //Respawn Function
        if (other.CompareTag("Death") & this.CompareTag("Player2"))
        {
            transform.position = respawnPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rbPusher.velocity = Vector3.zero;
            rbPusher.angularVelocity = Vector3.zero;
        }
    }
    
} 


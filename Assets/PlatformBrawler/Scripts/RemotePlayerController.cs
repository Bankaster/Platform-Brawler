using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemotePlayerController : MonoBehaviour
{
    public RemoteInputs finalRemoteInputs;

    private float movSpeed = 10f; //Movement speed of the remote player
    public float rotationSpeed = 100f; //Rotation speed of the remote player

    public Vector3 respawnPosition = new Vector3(0f, 3f, 0f);
    private Rigidbody rb;
    public Rigidbody rbPusher;

    private Animator remote_player_animator;
    private string playerID; //Unique ID for the remote player

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        //Get RemoteInputs from the OnlineManager
        finalRemoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
        remote_player_animator = GetComponent<Animator>();

        //Assign a unique ID for replication
        playerID = gameObject.name;
    }

    void Update()
    {
        MovePlayer();
        SendPlayerState();
    }

    private void MovePlayer()
    {
        Vector3 movement = Vector3.zero;

        //Remote Player Movement
        if (finalRemoteInputs.Apressed) movement += Vector3.left;
        if (finalRemoteInputs.Dpressed) movement += Vector3.right;
        if (finalRemoteInputs.Wpressed) movement += Vector3.forward;
        if (finalRemoteInputs.Spressed) movement += Vector3.back;

        //Remote Player Rotation
        if (finalRemoteInputs.Qpressed)
        {
            transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);
        }

        if (finalRemoteInputs.Epressed)
        {
            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }

        //Remote Player Attack
        if (finalRemoteInputs.SpacePressed)
        {
            remote_player_animator.SetTrigger("Attack");
        }

        rb.MovePosition(rb.position + movement * movSpeed * Time.deltaTime);
    }

    private void SendPlayerState()
    {
        //Send the player's state to the ReplicationManager
        Vector3 rotation = transform.eulerAngles;
        string action = GetCurrentAction();
        ReplicationManager.instance.UpdatePlayerState(playerID, transform.position, rotation, action);
    }

    private string GetCurrentAction()
    {
        //Determine current action based on RemoteInputs
        if (finalRemoteInputs.Apressed) return "MoveLeft";
        if (finalRemoteInputs.Dpressed) return "MoveRight";
        if (finalRemoteInputs.Wpressed) return "MoveForward";
        if (finalRemoteInputs.Spressed) return "MoveBackward";
        if (finalRemoteInputs.Qpressed) return "RotateLeft";
        if (finalRemoteInputs.Epressed) return "RotateRight";
        if (finalRemoteInputs.SpacePressed) return "Attack";
        return "Idle";
    }

    //Remote Player Attack Event
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

    public void UpdateFromReplication(WorldState.PlayerState state)
    {
        //Update the position, rotation, and inputs from replication data
        transform.position = state.position;
        transform.rotation = Quaternion.Euler(state.rotation);
        finalRemoteInputs.UpdateFromReplication(state.action);
    }
}

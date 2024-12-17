using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f; //Movement speed of the player
    public float rotationSpeed = 100f; //Rotation speed of the player

    public Vector3 respawnPosition = new Vector3(0f, 3f, 0f);
    private Rigidbody rb;
    public Rigidbody rbPusher;
    private Animator player_animator;

    private string playerID; //Unique ID for the player

    //Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (rb == null)
        {
            rb = gameObject.AddComponent<Rigidbody>();
        }

        player_animator = GetComponent<Animator>();

        rb.useGravity = true;

        //Assign playerID based on GameObject name
        playerID = gameObject.name;
    }

    //Update is called once per frame
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

        //Send updated player state to ReplicationManager
        SendPlayerState();
    }

    //Send player state to the ReplicationManager for synchronization
    private void SendPlayerState()
    {
        Vector3 rotation = transform.eulerAngles;
        string action = GetCurrentAction();

        ReplicationManager.instance.UpdatePlayerState(playerID, transform.position, rotation, action);
    }

    //Determine current player action
    private string GetCurrentAction()
    {
        if (Input.GetKey(KeyCode.W)) return "MoveForward";
        if (Input.GetKey(KeyCode.A)) return "MoveLeft";
        if (Input.GetKey(KeyCode.S)) return "MoveBackward";
        if (Input.GetKey(KeyCode.D)) return "MoveRight";
        if (Input.GetKey(KeyCode.Q)) return "RotateLeft";
        if (Input.GetKey(KeyCode.E)) return "RotateRight";
        if (Input.GetKeyDown(KeyCode.Space)) return "Attack";
        return "Idle";
    }

    // Player Attack Event
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
        if (other.CompareTag("Death") & this.CompareTag("Player1"))
        {
            transform.position = respawnPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rbPusher.velocity = Vector3.zero;
            rbPusher.angularVelocity = Vector3.zero;
        }
    }
}

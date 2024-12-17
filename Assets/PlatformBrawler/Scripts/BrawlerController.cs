using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f;
    public float rotationSpeed = 100f;

   // public RemoteInputs remoteInputs;

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
      /*  if (remoteInputs == null)
        {
            remoteInputs = new RemoteInputs(); 
        }
      */

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

       /* // Actualizar las entradas
        remoteInputs.Apressed = Input.GetKey(KeyCode.A);
        remoteInputs.Wpressed = Input.GetKey(KeyCode.W);
        remoteInputs.Spressed = Input.GetKey(KeyCode.S);
        remoteInputs.Dpressed = Input.GetKey(KeyCode.D);
        remoteInputs.Qpressed = Input.GetKey(KeyCode.Q);
        remoteInputs.Epressed = Input.GetKey(KeyCode.E);
        remoteInputs.SpacePressed = Input.GetKey(KeyCode.Space);
       */

        //Player Attack
        if (Input.GetKeyDown(KeyCode.Space))
        {
            player_animator.SetTrigger("Attack");
        }
    }

    //Player Attack Event
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

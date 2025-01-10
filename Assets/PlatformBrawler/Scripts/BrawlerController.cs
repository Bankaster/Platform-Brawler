using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f;
    public float rotationSpeed = 100f;
    //public float deathCount = 0;

    public GameObject player1;
    public GameObject player2;

    public Vector3 respawnPosition = new Vector3(0f, 4f, 0f);
    public Rigidbody rb;
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
        if (Input.GetKey(KeyCode.Space))
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
        rbPusher.velocity = Vector3.zero; 
        rbPusher.angularVelocity = Vector3.zero;
    }
    /*
    void OnTriggerEnter(Collider other)
    {
        //Respawn Function
        if (other.CompareTag("Death") & player1.CompareTag("Player1"))
        {
            transform.position = respawnPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rbPusher.velocity = Vector3.zero;
            rbPusher.angularVelocity = Vector3.zero;

            //Blue Player death counter
            OnlineManager.instance.blueDeathCount++;
            OnlineManager.instance.blueResult.text = OnlineManager.instance.blueDeathCount.ToString();
        }

        if (other.CompareTag("Death") & player2.CompareTag("Player2"))
        {
            transform.position = respawnPosition;
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rbPusher.velocity = Vector3.zero;
            rbPusher.angularVelocity = Vector3.zero;

            //Blue Player death counter
            OnlineManager.instance.redDeathCount++;
            OnlineManager.instance.redResult.text = OnlineManager.instance.redDeathCount.ToString();
        }
    } 
    */
}

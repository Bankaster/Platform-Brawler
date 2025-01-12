using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BrawlerController : MonoBehaviour
{
    public float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public GameObject player1;
    public GameObject player2;

    public Vector3 respawnPosition = new Vector3(0f, 4f, 0f);
    public Rigidbody rb;
    public Rigidbody rbPusher;
    private Animator player_animator;

    public AudioSource sfxAudioSource;
    public AudioClip attackSound;

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
        sfxAudioSource.PlayOneShot(attackSound);

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
}

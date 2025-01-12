using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Windows;

public class RemotePlayerController : MonoBehaviour
{
    public RemoteInputs finalRemoteInputs;  

    private float movSpeed = 10f;
    public float rotationSpeed = 100f;

    public GameObject player1;
    public GameObject player2;

    public Vector3 respawnPosition = new Vector3(0f, 4f, 0f);
    public Rigidbody rb;
    public Rigidbody rbPusher;
    private Animator remote_player_animator;

    public AudioSource sfxAudioSource;
    public AudioClip attackSound;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        finalRemoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
        remote_player_animator = GetComponent<Animator>();
        StartCoroutine(UpdatePositionCo());
    }

    void Update()
    {
        MovePlayer();
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

    //Remote Player Attack Event
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

    IEnumerator UpdatePositionCo()
    {
        while (true)
        {
            yield return new WaitForSeconds(1.0f);

            while (Vector3.Distance(transform.position, finalRemoteInputs.pos) > 0.01f)
            {
                transform.position = Vector3.Lerp(transform.position, finalRemoteInputs.pos, 10 * Time.deltaTime);
                yield return null;
            }

            transform.position = finalRemoteInputs.pos;

            float rotY = transform.rotation.y;
            rotY = finalRemoteInputs.rot;
            //Debug.Log("PositionUpdated");
        }
    }
} 


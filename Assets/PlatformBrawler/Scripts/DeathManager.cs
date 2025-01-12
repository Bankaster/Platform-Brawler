using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathManager : MonoBehaviour
{
    public GameObject player1;
    public GameObject player2;

    public AudioSource sfxAudioSource;
    public AudioClip deathSound;

    [SerializeField] BrawlerController Player1Controller;
    [SerializeField] BrawlerController Player2Controller;
    [SerializeField] RemotePlayerController Player1RemoteController;
    [SerializeField] RemotePlayerController Player2RemoteController;

    RemoteInputs remoteInputs = new RemoteInputs();


    void OnTriggerEnter(Collider other)
    {
        //Respawn Function
        if (this.CompareTag("Death") && other.CompareTag("Player1"))
        {
            if(Player1Controller != null && !Player1RemoteController.isActiveAndEnabled)
            {
                player1.transform.position = Player1Controller.respawnPosition;
                Player1Controller.rb.velocity = Vector3.zero;
                Player1Controller.rb.angularVelocity = Vector3.zero;
                Player1Controller.rbPusher.velocity = Vector3.zero;
                Player1Controller.rbPusher.angularVelocity = Vector3.zero;
            }
            else if (Player1RemoteController != null && !Player1Controller.isActiveAndEnabled)
            {
                player1.transform.position = Player1RemoteController.respawnPosition;
                Player1RemoteController.rb.velocity = Vector3.zero;
                Player1RemoteController.rb.angularVelocity = Vector3.zero;
                Player1RemoteController.rbPusher.velocity = Vector3.zero;
                Player1RemoteController.rbPusher.angularVelocity = Vector3.zero;
            }

            //Blue Player death counter
            OnlineManager.instance.blueDeathCount++;
            OnlineManager.instance.blueResultText.text = OnlineManager.instance.blueDeathCount.ToString();
        }

        if (this.CompareTag("Death") && other.CompareTag("Player2"))
        {
            if (Player2Controller != null && !Player2RemoteController.isActiveAndEnabled)
            {
                player2.transform.position = Player2Controller.respawnPosition;
                Player2Controller.rb.velocity = Vector3.zero;
                Player2Controller.rb.angularVelocity = Vector3.zero;
                Player2Controller.rbPusher.velocity = Vector3.zero;
                Player2Controller.rbPusher.angularVelocity = Vector3.zero;
            }
            else if (Player2RemoteController != null && !Player2Controller.isActiveAndEnabled)
            {
                player2.transform.position = Player2RemoteController.respawnPosition;
                Player2RemoteController.rb.velocity = Vector3.zero;
                Player2RemoteController.rb.angularVelocity = Vector3.zero;
                Player2RemoteController.rbPusher.velocity = Vector3.zero;
                Player2RemoteController.rbPusher.angularVelocity = Vector3.zero;
            }

            //Red Player death counter
            OnlineManager.instance.redDeathCount++;
            OnlineManager.instance.redResultText.text = OnlineManager.instance.redDeathCount.ToString();
        }



        //Death sound Trigger
        if (this.CompareTag("Space") && other.CompareTag("Player1"))
        {
            if (Player1Controller != null && !Player1RemoteController.isActiveAndEnabled)
            {
                sfxAudioSource.PlayOneShot(deathSound);
            }
            else if (Player1RemoteController != null && !Player1Controller.isActiveAndEnabled)
            {
                sfxAudioSource.PlayOneShot(deathSound);
            }
        }

        if (this.CompareTag("Space") && other.CompareTag("Player2"))
        {
            if (Player2Controller != null && !Player2RemoteController.isActiveAndEnabled)
            {
                sfxAudioSource.PlayOneShot(deathSound);
            }
            else if (Player2RemoteController != null && !Player2Controller.isActiveAndEnabled)
            {
                sfxAudioSource.PlayOneShot(deathSound);
            }
        }
    }
}

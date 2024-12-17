using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    //Singleton instance
    public static OnlineManager instance;

    [SerializeField] BrawlerController Player1Controller;
    [SerializeField] BrawlerController Player2Controller;
    [SerializeField] RemotePlayerController Player1RemoteController;
    [SerializeField] RemotePlayerController Player2RemoteController;

    private void Awake()
    {
        //Ensure only one instance exists
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //Check if this is running on the server or client
        if (GameObject.FindGameObjectWithTag("Server"))
        {
            //Enable local Player1 and remote Player2 on the server
            if (Player1Controller) Player1Controller.enabled = true;
            if (Player2RemoteController) Player2RemoteController.enabled = true;
        }
        else
        {
            //Enable local Player2 and remote Player1 on the client
            if (Player2Controller) Player2Controller.enabled = true;
            if (Player1RemoteController) Player1RemoteController.enabled = true;
        }
    }
}

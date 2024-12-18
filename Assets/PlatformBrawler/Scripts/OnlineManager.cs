using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnlineManager : MonoBehaviour
{
    static public OnlineManager instance;

    [SerializeField] BrawlerController Player1Controller;
    [SerializeField] BrawlerController Player2Controller;
    [SerializeField] RemotePlayerController Player1RemoteController;
    [SerializeField] RemotePlayerController Player2RemoteController;

    private void Awake()
    {
        instance = this; 

    }

    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("Server"))
        {
            Player1Controller.enabled = true;
            Player2RemoteController.enabled = true;
        }
        else
        {
            Player2Controller.enabled = true;
            Player1RemoteController.enabled = true;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

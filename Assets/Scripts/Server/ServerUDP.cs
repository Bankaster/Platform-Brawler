using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;

public class ServerUDP : MonoBehaviour
{
    Socket socket;
    EndPoint RemoteClient;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string serverText;
    RemoteInputs remoteInputs;

    public bool goToGame = false;
    bool asignInputClass = false;
    bool exitGameLoop = false;


    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
      //  remoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
    }

    public void startServer()
    {
        serverText = "Starting UDP Server *bee bop bee bop*...";

        //TO DO 1
        //UDP doesn't keep track of our connections like TCP
        //This means that we "can only" reply to other endpoints,
        //since we don't know where or who they are
        //We want any UDP connection that wants to communicate with 9050 port to send it to our socket.
        //So as with TCP, we create a socket and bind it to the 9050 port. 
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        //TO DO 3
        //Our client is sending a handshake, the server has to be able to receive it
        //It's time to call the Receive thread
        Thread newConnection = new Thread(Receive);
        newConnection.Start();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        UItext.text = serverText;

        if (goToGame)
        {
            goToGame = false;
            SceneManager.LoadScene(1);
        }
        else if (asignInputClass)
        {
            asignInputClass = false;
            remoteInputs = GameObject.FindGameObjectWithTag("OnlineManager").GetComponent<RemoteInputs>();
        }
    }

    void Receive()
    {
        int recv;
        byte[] data = new byte[1024];

        serverText = serverText + "\n" + "Waiting for new Client...";

        //TO DO 3
        //We don't know who may be communicating with this server, so we have to create an
        //endpoint with any address and an IPEndPoint from it to reply to it later.
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        RemoteClient = (EndPoint)sender;

        //Loop the whole process, and start receiving messages directed to our socket
        //(the one we binded to a port before)
        //When using socket.ReceiveFrom, be sure to send our remote as a reference so we can keep
        //this address (the client) and reply to it on TO DO 4
        while (true)
        {
            recv = socket.ReceiveFrom(data, ref RemoteClient);
            serverText = serverText + "\n" + "Message received from " + RemoteClient.ToString();
            serverText = serverText + "\n" + Encoding.ASCII.GetString(data, 0, recv);

            //TO DO 4
            //When our UDP server receives a message from a random remote, it has to send a ping,
            //Call a send thread
            Thread sendThread = new Thread(() => SendData(RemoteClient));
            sendThread.Start();
            goToGame = true;
        }
    }

    void SendData(EndPoint RemoteClient)
    {
        //TO DO 4
        //Use socket.SendTo to send a ping using the remote we stored earlier.
        //byte[] data = Encoding.ASCII.GetBytes("Ping");
        while (!exitGameLoop)
        {
            byte[] data = Serialize.instance.SerializeJson().GetBuffer();
            socket.SendTo(data, RemoteClient);
        }
    }

    void RecieveData()
    {
        while (!exitGameLoop)
        {
            if (!remoteInputs)
            {
                asignInputClass = true;
                continue;
            }

            byte[] receiveData = new byte[1024];
            socket.ReceiveFrom(receiveData, ref RemoteClient);
            Serialize.instance.DeserializeJson(receiveData, ref remoteInputs);
        }

    }

    private void OnApplicationQuit()
    {
        exitGameLoop = true;
    }
}

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
    TextMeshProUGUI UItext;
    RemoteInputs remoteInputs;

    public bool goToGame = false;
    bool asignInputClass = false;
    bool exitGameLoop = false;


    void Start()
    {

    }

    public void startServer()
    {
        //UDP doesn't keep track of our connections like TCP
        //This means that we "can only" reply to other endpoints,
        //since we don't know where or who they are
        //We want any UDP connection that wants to communicate with 9050 port to send it to our socket.
        //So as with TCP, we create a socket and bind it to the 9050 port. 
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        //Our client is sending a handshake, the server has to be able to receive it
        //It's time to call the Receive thread
        Thread newConnection = new Thread(Receive);
        newConnection.Start();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
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

        //We don't know who may be communicating with this server, so we have to create an
        //endpoint with any address and an IPEndPoint from it to reply to it later.
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        RemoteClient = (EndPoint)sender;

        recv = socket.ReceiveFrom(data, ref RemoteClient);
        goToGame = true;

        //When our UDP server receives a message from a random remote, it has to send a ping,
        //Call a send thread
        Thread sendThread = new Thread(() => SendData());
        sendThread.Start();
        Thread recieveThread = new Thread(() => RecieveData());
        recieveThread.Start();

        //Loop the whole process, and start receiving messages directed to our socket
        //(the one we binded to a port before)
        //When using socket.ReceiveFrom, be sure to send our remote as a reference so we can keep
        while (true)
        {

        }

    }

    void SendData()
    {

        //Use socket.SendTo to send a ping using the remote we stored earlier.
        //byte[] data = Encoding.ASCII.GetBytes("Ping");
        while (!exitGameLoop)
        {
            if (OnlineManager.instance == null || Serialize.instance == null) continue;
            byte[] data = new byte[1024]; 
            data = Serialize.instance.SerializeJson().GetBuffer();
            socket.SendTo(data, data.Length, SocketFlags.None, RemoteClient);
        }
    }

    void RecieveData()
    {
        while (!exitGameLoop)
        {
            if (OnlineManager.instance == null || Serialize.instance == null) continue;
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

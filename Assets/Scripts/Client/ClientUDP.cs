using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    EndPoint RemoteServer;
    IPEndPoint ipep;
    public GameObject UItextObj;
    TextMeshProUGUI UItext;
    string clientText;
    RemoteInputs remoteInputs;

    bool goToGame = false;
    bool asignInputClass = false;
    bool exitGameLoop = false;

    // Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
    }
    public void StartClient()
    {
        Thread mainThread = new Thread(Send);
        mainThread.Start();
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        UItext.text = clientText;

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

    void Send()
    {
        //TO DO 2
        //Unlike with TCP, we don't "connect" first,
        //we are going to send a message to establish our communication so we need an endpoint
        //We need the server's IP and the port we've binded it to before
        //Again, initialize the socket
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        //TO DO 2.1 
        //Send the Handshake to the server's endpoint.
        //This time, our UDP socket doesn't have it, so we have to pass it
        //as a parameter on its SendTo() method
        byte[] data = Encoding.ASCII.GetBytes("OMG");
        socket.SendTo(data, ipep);

        //TO DO 5
        //We'll wait for a server response,
        //so you can already start the receive thread
        Thread receive = new Thread(Receive);
        receive.Start();
        goToGame = true;
    }

    //TO DO 5
    //Same as in the server, in this case the remote is a bit useless
    //since we already know it's the server who's communicating with us
    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        RemoteServer = (EndPoint)sender;
        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref RemoteServer);

        clientText = $"Message received from {RemoteServer.ToString()}:";
        clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);
    }

    void SendData()
    {
        while (!exitGameLoop)
        {
            byte[] sendData = new byte[1024];
            sendData = Serialize.instance.SerializeJson().GetBuffer();
            socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipep);
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
            socket.ReceiveFrom(receiveData, ref RemoteServer);
            Serialize.instance.DeserializeJson(receiveData, ref remoteInputs);
        }
    }

    private void OnApplicationQuit()
    {
        exitGameLoop = true;
    }
}

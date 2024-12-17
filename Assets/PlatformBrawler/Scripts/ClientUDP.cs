using System.Net;
using System.Net.Sockets;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using TMPro;
using UnityEngine.SceneManagement;

public class ClientUDP : MonoBehaviour
{
    Socket socket;
    EndPoint RemoteServer;
    IPEndPoint ipep;
    public GameObject UItextObj;
    public InputField ipInputField;
    TextMeshProUGUI UItext;
    string clientText;

    bool goToGame = false;
    bool exitGameLoop = false;

    private ReplicationManager replicationManager; //Reference to the ReplicationManager

    public Socket GetSocket()
    {
        return socket;
    }

    //Start is called before the first frame update
    void Start()
    {
        UItext = UItextObj.GetComponent<TextMeshProUGUI>();
        DontDestroyOnLoad(gameObject);
    }

    public void StartClient()
    {
        string serverIP = ipInputField.text;

        if (string.IsNullOrEmpty(serverIP))
        {
            Debug.LogError("Please, write a valid IP.");
            return;
        }

        try
        {
            //Configure the endpoint with the provided IP
            ipep = new IPEndPoint(IPAddress.Parse(serverIP), 9050);
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //Send the handshake
            byte[] data = Encoding.ASCII.GetBytes("OMG");
            socket.SendTo(data, data.Length, SocketFlags.None, ipep);

            //Start the thread to receive data
            Thread receive = new Thread(Receive);
            receive.Start();

            goToGame = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error connecting to server: {e.Message}");
        }
    }

    void Update()
    {
        UItext.text = clientText;

        if (goToGame)
        {
            goToGame = false;
            SceneManager.LoadScene(1);
        }
    }

    void Receive()
    {
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        RemoteServer = (EndPoint)sender;

        byte[] data = new byte[1024];
        int recv = socket.ReceiveFrom(data, ref RemoteServer);

        clientText = $"Message received from {RemoteServer.ToString()}:";
        clientText += "\n" + Encoding.ASCII.GetString(data, 0, recv);

        //Start threads to send and receive game data
        Thread threadSend = new Thread(() => SendData());
        threadSend.Start();

        Thread threadReceive = new Thread(() => ReceiveData());
        threadReceive.Start();
    }

    void SendData()
    {
        while (!exitGameLoop)
        {
            if (ReplicationManager.instance == null) continue;

            //Serialize the local WorldState and send it to the server
            string jsonData = ReplicationManager.instance.GetReplicationData();
            byte[] sendData = Encoding.ASCII.GetBytes(jsonData);
            socket.SendTo(sendData, sendData.Length, SocketFlags.None, ipep);
        }
    }

    void ReceiveData()
    {
        while (!exitGameLoop)
        {
            if (ReplicationManager.instance == null) continue;

            //Receive the WorldState from the server
            byte[] receiveData = new byte[1024];
            socket.ReceiveFrom(receiveData, ref RemoteServer);

            string jsonData = Encoding.ASCII.GetString(receiveData);
            ReplicationManager.instance.HandleReplicationData(jsonData);
        }
    }

    private void OnApplicationQuit()
    {
        exitGameLoop = true;
    }
}

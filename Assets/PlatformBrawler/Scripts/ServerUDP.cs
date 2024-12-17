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
    RemoteInputs remoteInputs;
    public TextMeshProUGUI UItext;

    string serverIP;
    public bool goToGame = false;
    bool asignInputClass = false;
    bool exitGameLoop = false;

    private ReplicationManager replicationManager;

    public string GetLocalIPAddress()
    {
        string localIP = "No IP found";

        try
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Error: {ex.Message}");
        }

        return localIP;
    }

    void Start()
    {
        serverIP = GetLocalIPAddress();
        Debug.Log($"IP address: {serverIP}");
        replicationManager = FindObjectOfType<ReplicationManager>();
    }

    public void startServer()
    {
        //UDP doesn't keep track of connections like TCP
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 9050);
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        socket.Bind(ipep);

        UItext.text = $"Server IP address: {serverIP}";

        //Start the Receive thread
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
        byte[] data = new byte[1024];
        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        RemoteClient = (EndPoint)sender;

        Debug.Log("Waiting for client connection...");
        socket.ReceiveFrom(data, ref RemoteClient);
        goToGame = true;

        //Start the send and receive threads
        Thread sendThread = new Thread(() => SendWorldState());
        sendThread.Start();

        Thread receiveThread = new Thread(() => ReceiveData());
        receiveThread.Start();
    }

    void SendWorldState()
    {
        while (!exitGameLoop)
        {
            if (replicationManager == null) continue;

            //Serialize WorldState and send to client
            string worldStateData = replicationManager.GetReplicationData();
            byte[] sendData = Encoding.ASCII.GetBytes(worldStateData);
            socket.SendTo(sendData, RemoteClient);
        }
    }

    void ReceiveData()
    {
        while (!exitGameLoop)
        {
            if (replicationManager == null) continue;

            byte[] receiveData = new byte[1024];
            socket.ReceiveFrom(receiveData, ref RemoteClient);

            string jsonData = Encoding.ASCII.GetString(receiveData);
            replicationManager.HandleReplicationData(jsonData);
        }
    }

    private void OnApplicationQuit()
    {
        exitGameLoop = true;
        socket.Close();
    }
}

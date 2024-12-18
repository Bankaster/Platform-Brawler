using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;

public class ReplicationManager : MonoBehaviour
{
    public struct ReplicationPacket
    {
        public int NetworkID;
        public string Action;
        public Vector3 Position;
        public float Health;
    }

    private Dictionary<int, ReplicationPacket> stateTable = new Dictionary<int, ReplicationPacket>();
    private List<ReplicationPacket> pendingReplication = new List<ReplicationPacket>();

    private Socket serverSocket;
    private List<EndPoint> clients = new List<EndPoint>();
    private const int PORT = 9050;

    private UDPJitter udpJitter;

    void Start()
    {
        SetupServer();
        udpJitter = new UDPJitter();
    }

    void SetupServer()
    {
        serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, PORT);
        serverSocket.Bind(endPoint);
        Debug.Log("Server is running on port: " + PORT);

        //Start listening for clients

        ReceiveMessages();
    }

    void ReceiveMessages()
    {
        byte[] buffer = new byte[1024];
        EndPoint clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
        serverSocket.BeginReceiveFrom(buffer, 0, buffer.Length, SocketFlags.None, ref clientEndPoint, (ar) =>
        {
            int received = serverSocket.EndReceiveFrom(ar, ref clientEndPoint);
            string receivedData = Encoding.ASCII.GetString(buffer, 0, received);

            //Handle replication packet

            HandleReplicationData(receivedData, clientEndPoint);

            //Continue receiving

            ReceiveMessages();
        }, null);
    }

    void HandleReplicationData(string data, EndPoint clientEndPoint)
    {
        Debug.Log("Received data: " + data + " from " + clientEndPoint.ToString());

        //Add client to the list if new

        if (!clients.Contains(clientEndPoint))
        {
            clients.Add(clientEndPoint);
            Debug.Log("New client connected: " + clientEndPoint.ToString());
        }

        //Deserialize and process data

        ReplicationPacket packet = JsonUtility.FromJson<ReplicationPacket>(data);

        if (stateTable.ContainsKey(packet.NetworkID))
        {
            //Update existing object

            stateTable[packet.NetworkID] = packet;
        }
        else
        {
            //Add new object to state table

            stateTable.Add(packet.NetworkID, packet);
        }

        //Queue replication to all clients

        foreach (var client in clients)
        {
            if (!client.Equals(clientEndPoint))
            {
                pendingReplication.Add(packet);
            }
        }

        //Send replication packets

        SendReplicationPackets();
    }

    void SendReplicationPackets()
    {
        foreach (var packet in pendingReplication)
        {
            string jsonData = JsonUtility.ToJson(packet);
            byte[] data = Encoding.ASCII.GetBytes(jsonData);

            foreach (var client in clients)
            {
                udpJitter.sendMessage(() => serverSocket.SendTo(data, client));
            }
        }

        pendingReplication.Clear();
    }

    void OnApplicationQuit()
    {
        if (serverSocket != null)
        {
            serverSocket.Close();
        }
    }
}

public class UDPJitter
{
    private float lossRate = 0.1f; //Simulate 10% packet loss
    private float minDelay = 0.05f; //Minimum delay in seconds
    private float maxDelay = 0.2f; //Maximum delay in seconds

    private System.Random random = new System.Random();

    public void sendMessage(Action sendAction)
    {
        float chance = (float)random.NextDouble();

        if (chance > lossRate)
        {
            float delay = (float)(minDelay + (random.NextDouble() * (maxDelay - minDelay)));
            new Thread(() =>
            {
                Thread.Sleep((int)(delay * 1000));
                sendAction.Invoke();
            }).Start();
        }
        else
        {
            Debug.Log("Packet lost");
        }
    }
}

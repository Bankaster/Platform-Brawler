using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;


public class RemotePlayerController : MonoBehaviour
{
    public RemoteInputs remoteInputs;  

    //Paràmetres de moviment del jugador
    [SerializeField] private float movSpeed = 10f;
    private Rigidbody rb;

    //Variables de xarxa
    private Socket udpSocket;
    private EndPoint remoteEndPoint;
    private IPEndPoint serverEndPoint;
    private bool connected = false;
    private Thread receiveThread;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        //Configuració del socket UDP
        udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        serverEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000);
        remoteEndPoint = (EndPoint)serverEndPoint;

        //Iniciar el fil per rebre dades
        receiveThread = new Thread(new ThreadStart(ReceiveData));
        receiveThread.Start();
    }

    void Update()
    {
        if (connected)
        {
            //Enviar inputs actuals al servidor
            SendInputData();

            //Actualitzar moviment del jugador
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        Vector3 movement = Vector3.zero;

        if (remoteInputs.Apressed) movement += Vector3.left;
        if (remoteInputs.Dpressed) movement += Vector3.right;
        if (remoteInputs.Wpressed) movement += Vector3.forward;

        rb.MovePosition(rb.position + movement * movSpeed * Time.deltaTime);
    }

    private void SendInputData()
    {
        //Serialitzar inputs a JSON
        string jsonData = JsonUtility.ToJson(remoteInputs);
        byte[] data = Encoding.ASCII.GetBytes(jsonData);

        //Enviar dades al servidor
        udpSocket.SendTo(data, remoteEndPoint);
    }

    private void ReceiveData()
    {
        byte[] buffer = new byte[1024];
        while (true)
        {
            int recv = udpSocket.ReceiveFrom(buffer, ref remoteEndPoint);
            string jsonData = Encoding.ASCII.GetString(buffer, 0, recv);

            //Deserialitzar dades a remoteInputs
            JsonUtility.FromJsonOverwrite(jsonData, remoteInputs);
        }
    }

    private void OnApplicationQuit()
    {
        receiveThread.Abort();
        udpSocket.Close();
    }
} 


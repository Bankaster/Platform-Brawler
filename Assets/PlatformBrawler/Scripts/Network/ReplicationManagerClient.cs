using System.Collections.Generic;
using UnityEngine;
using System.Net;
using System.Net.Sockets;

public class ReplicationManagerClient : MonoBehaviour
{
    private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    private Socket socket;  // No creamos el socket aquí

    void Start()
    {
        // El socket ya se ha creado en ClientUDP, solo lo asignamos
        socket = GameObject.FindObjectOfType<ClientUDP>().GetSocket(); // Obtener el socket desde ClientUDP
    }

    void Update()
    {
        // Recibir datos de replicación
        ReceiveReplicationData();
    }

    void ReceiveReplicationData()
    {
        byte[] receiveData = new byte[1024];

        // Crear un IPEndPoint que almacenará la dirección del servidor remoto
        EndPoint RemoteServer = new IPEndPoint(IPAddress.Any, 0); // Usamos cualquier dirección IP y puerto 0 para recibir

        try
        {
            // Recibe datos desde el servidor (actualiza RemoteServer con la dirección de quien envía los datos)
           // int bytesReceived = socket.ReceiveFrom(receiveData, ref RemoteServer);  // Recibe datos y guarda el endpoint
            //if (bytesReceived > 0)
            {
                Debug.Log("Datos recibidos desde: " + RemoteServer.ToString());
                // Deserializar los datos recibidos
                Packet packet = new Packet(receiveData);
                ProcessReceivedData(packet);
            }
        }
        catch (SocketException ex)
        {
            Debug.LogError("Error al recibir datos: " + ex.Message);
        }
    }

    void ProcessReceivedData(Packet packet)
    {
        int netId = packet.ReadInt();            // Leer NetworkID del objeto
        Vector3 position = packet.ReadVector3(); // Leer posición del objeto

        // Leer las entradas de los botones del jugador
        bool Apressed = packet.ReadBool();
        bool Wpressed = packet.ReadBool();
        bool Spressed = packet.ReadBool();
        bool Dpressed = packet.ReadBool();
        bool Qpressed = packet.ReadBool();
        bool Epressed = packet.ReadBool();
        bool SpacePressed = packet.ReadBool();

        // Si el objeto ya existe, simplemente actualiza su estado
        if (!objects.ContainsKey(netId))
        {
            GameObject player = FindPlayerByNetworkID(netId);
            if (player != null)
            {
                objects[netId] = player;
            }
        }

        // Si se encuentra el jugador, actualizamos su posición y entradas
        if (objects.ContainsKey(netId))
        {
            GameObject playerObj = objects[netId];
            playerObj.transform.position = position;

            // Actualizar las entradas del jugador
            RemoteInputs remoteInputs = playerObj.GetComponent<BrawlerController>().remoteInputs;
            remoteInputs.Apressed = Apressed;
            remoteInputs.Wpressed = Wpressed;
            remoteInputs.Spressed = Spressed;
            remoteInputs.Dpressed = Dpressed;
            remoteInputs.Qpressed = Qpressed;
            remoteInputs.Epressed = Epressed;
            remoteInputs.SpacePressed = SpacePressed;

            // Procesar la acción de ataque (Space)
            if (remoteInputs.SpacePressed)
            {
                playerObj.GetComponent<BrawlerController>().AddForce();
            }
        }
    }

    GameObject FindPlayerByNetworkID(int netId)
    {
        // Buscar un jugador por su etiqueta, nombre o NetworkID
        return GameObject.FindGameObjectWithTag("Player" + netId + 1); // Busca por etiqueta
    }
}





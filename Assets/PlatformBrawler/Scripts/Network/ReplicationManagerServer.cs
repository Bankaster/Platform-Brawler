using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.Net;
using System.Net.Sockets;



public class Packet
{
    private List<byte> data = new List<byte>();

    public Packet() { }

    public Packet(byte[] data)
    {
        this.data.AddRange(data);
    }

    // Método para escribir un entero en el paquete
    public void Write(int value)
    {
        data.AddRange(BitConverter.GetBytes(value));
    }
    public void Write(float value)
    {
        data.AddRange(BitConverter.GetBytes(value)); 
    }

    // Método para escribir un Vector3 (posición)
    public void Write(Vector3 value)
    {
        Write(value.x);
        Write(value.y);
        Write(value.z);
    }

    // Método para escribir un booleano
    public void Write(bool value)
    {
        data.Add((byte)(value ? 1 : 0));
    }

    // Obtener todos los bytes del paquete
    public byte[] GetBytes()
    {
        return data.ToArray();
    }

    // Leer un entero desde el paquete
    public int ReadInt()
    {
        int value = BitConverter.ToInt32(data.ToArray(), 0);
        data.RemoveRange(0, 4);  // Elimina los primeros 4 bytes
        return value;
    }

    // Leer un Vector3 desde el paquete
    public Vector3 ReadVector3()
    {
        float x = BitConverter.ToSingle(data.ToArray(), 0);
        data.RemoveRange(0, 4);
        float y = BitConverter.ToSingle(data.ToArray(), 0);
        data.RemoveRange(0, 4);
        float z = BitConverter.ToSingle(data.ToArray(), 0);
        data.RemoveRange(0, 4);
        return new Vector3(x, y, z);
    }

    // Leer un booleano desde el paquete
    public bool ReadBool()
    {
        bool value = data[0] == 1;
        data.RemoveAt(0); // Elimina el primer byte
        return value;
    }
}

public class ReplicationManagerServer : MonoBehaviour
{
    private Dictionary<int, GameObject> objects = new Dictionary<int, GameObject>();
    private int nextNetId = 0; // ID único para cada objeto
    private Socket socket;
    private IPEndPoint ipep;
    private EndPoint RemoteServer;  // Guarda la dirección del cliente remoto

    void Start()
    {
        // Inicializar el socket UDP para enviar y recibir
        socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

        // Configurar el puerto en el que el servidor escucha (puede ser diferente para el servidor)
        socket.Bind(new IPEndPoint(IPAddress.Any, 9050));

        // Definir la IP y el puerto de destino (por ejemplo, el cliente local en 127.0.0.1)
        ipep = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9050);  // Cambia la IP si es necesario
    }

    void Update()
    {
        foreach (var obj in objects.Values)
        {
            if (HasObjectChanged(obj))
            {
                SendReplicationPacket(obj);
            }
        }
    }

    bool HasObjectChanged(GameObject obj)
    {
        // Compara la posición actual con la anterior para determinar si ha cambiado
        return true; // Simplificado para este ejemplo
    }

    void SendReplicationPacket(GameObject obj)
    {
        // Crear un paquete de replicación
        Packet packet = new Packet();

        // Usamos el NetworkID almacenado en la clase ReplicationManagerServer
        packet.Write(obj.GetComponent<NetworkIdentity>().NetworkID);

        // Escribir la posición del objeto
        packet.Write(obj.transform.position);

        // Obtener las entradas del jugador (movimiento, rotación, ataque)
        RemoteInputs remoteInputs = obj.GetComponent<BrawlerController>().remoteInputs;
        packet.Write(remoteInputs.Apressed);
        packet.Write(remoteInputs.Wpressed);
        packet.Write(remoteInputs.Spressed);
        packet.Write(remoteInputs.Dpressed);
        packet.Write(remoteInputs.Qpressed);
        packet.Write(remoteInputs.Epressed);
        packet.Write(remoteInputs.SpacePressed);

        // Enviar el paquete a los clientes (usando la IP del cliente remoto)
        socket.SendTo(packet.GetBytes(), RemoteServer);  // Enviar el paquete al cliente
    }

    public void RegisterObject(GameObject obj)
    {
        int networkID = nextNetId++;
        objects[networkID] = obj;
    }

    public void UnregisterObject(GameObject obj)
    {
        // Eliminar el objeto del servidor cuando ya no sea necesario
        foreach (var item in objects)
        {
            if (item.Value == obj)
            {
                objects.Remove(item.Key);
                break;
            }
        }
    }

    // Método para recibir datos de los clientes
    void ReceiveData()
    {
        byte[] receiveData = new byte[1024];
        socket.ReceiveFrom(receiveData, ref RemoteServer);  // Al recibir, actualizamos la dirección del cliente remoto
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serialize : MonoBehaviour
{
    public static Serialize instance;
    static MemoryStream stream;
    byte[] receiveInfo;
    RemoteInputs remoteInputs = new RemoteInputs();

    private void Awake()
    {
        if (instance == null)
              instance = this;

        stream = new MemoryStream();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        remoteInputs.Apressed = false;
        remoteInputs.Wpressed = false;
        remoteInputs.Spressed = false;
        remoteInputs.Dpressed = false;
        remoteInputs.Qpressed = false;
        remoteInputs.Epressed = false;

        //Update Movement
        if (Input.GetKeyDown(KeyCode.A))
        {
            remoteInputs.Apressed = true;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            remoteInputs.Wpressed = true;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            remoteInputs.Spressed = true;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            remoteInputs.Dpressed = true;
        }

        //Update Rotation
        if (Input.GetKeyDown(KeyCode.Q))
        {
            remoteInputs.Qpressed = true;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            remoteInputs.Epressed = true;
        }
    }

    public MemoryStream SerializeJson()
    {
        string json = JsonUtility.ToJson(remoteInputs);
        stream = new MemoryStream();
        BinaryWriter writer = new BinaryWriter(stream);
        writer.Write(json);

        return stream;
    }

    public void DeserializeJson(byte[] sendInfo, ref RemoteInputs serializedThings)
    {     
        MemoryStream stream = new MemoryStream(sendInfo);
        BinaryReader reader = new BinaryReader(stream);
        stream.Seek(0, SeekOrigin.Begin);
        string json = reader.ReadString();
        JsonUtility.FromJsonOverwrite(json, serializedThings);
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Serialize : MonoBehaviour
{
    public static Serialize instance; //Singleton instance
    static MemoryStream stream;       //Stream for serialization
    RemoteInputs remoteInputs = new RemoteInputs();

    private void Awake()
    {
        if (instance == null)
            instance = this;

        stream = new MemoryStream();
    }

    //Update is called once per frame
    void Update()
    {
        CaptureInputs();
    }

    //Capture the current player inputs
    private void CaptureInputs()
    {
        remoteInputs.ResetMovement();

        //Update Movement
        if (Input.GetKey(KeyCode.A)) remoteInputs.Apressed = true;
        if (Input.GetKey(KeyCode.W)) remoteInputs.Wpressed = true;
        if (Input.GetKey(KeyCode.S)) remoteInputs.Spressed = true;
        if (Input.GetKey(KeyCode.D)) remoteInputs.Dpressed = true;

        //Update Rotation
        if (Input.GetKey(KeyCode.Q)) remoteInputs.Qpressed = true;
        if (Input.GetKey(KeyCode.E)) remoteInputs.Epressed = true;

        //Update Attack
        if (Input.GetKey(KeyCode.Space)) remoteInputs.SpacePressed = true;
    }

    //Serialize RemoteInputs to JSON
    public MemoryStream SerializeJson()
    {
        string json = JsonUtility.ToJson(remoteInputs);
        stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(json);
        }
        return stream;
    }

    //Deserialize JSON into RemoteInputs
    public void DeserializeJson(byte[] sendInfo, ref RemoteInputs serializedInputs)
    {
        using (MemoryStream stream = new MemoryStream(sendInfo))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            stream.Seek(0, SeekOrigin.Begin);
            string json = reader.ReadString();
            JsonUtility.FromJsonOverwrite(json, serializedInputs);
        }
    }

    //Serialize WorldState to JSON
    public MemoryStream SerializeWorldState(WorldState worldState)
    {
        string json = JsonUtility.ToJson(worldState);
        stream = new MemoryStream();
        using (BinaryWriter writer = new BinaryWriter(stream))
        {
            writer.Write(json);
        }
        return stream;
    }

    //Deserialize JSON into WorldState
    public WorldState DeserializeWorldState(byte[] sendInfo)
    {
        using (MemoryStream stream = new MemoryStream(sendInfo))
        using (BinaryReader reader = new BinaryReader(stream))
        {
            stream.Seek(0, SeekOrigin.Begin);
            string json = reader.ReadString();
            return JsonUtility.FromJson<WorldState>(json);
        }
    }
}

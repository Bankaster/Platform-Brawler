using System.Collections.Generic;
using UnityEngine;

//Manages replication of the world state between server and clients
public class ReplicationManager : MonoBehaviour
{
    public static ReplicationManager instance; // Singleton instance
    public WorldState worldState = new WorldState();

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    //Receive and apply replicated data from the server
    public void HandleReplicationData(string json)
    {
        WorldState receivedState = WorldState.DeserializeState(json);
        worldState = receivedState;
        ApplyWorldState();
    }

    //Apply the received world state to all relevant players
    private void ApplyWorldState()
    {
        foreach (var playerState in worldState.players)
        {
            GameObject player = GameObject.Find(playerState.playerID);
            if (player)
            {
                RemotePlayerController controller = player.GetComponent<RemotePlayerController>();
                if (controller != null)
                {
                    controller.UpdateFromReplication(playerState);
                }
            }
        }
    }

    //Retrieve the current world state for replication
    public string GetReplicationData()
    {
        return worldState.SerializeState();
    }

    //Update a player's state based on its position, rotation, and action
    public void UpdatePlayerState(string id, Vector3 pos, Vector3 rot, string action)
    {
        worldState.UpdatePlayerState(id, pos, rot, action);
    }
}

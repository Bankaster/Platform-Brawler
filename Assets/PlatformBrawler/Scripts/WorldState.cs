using System.Collections.Generic;
using UnityEngine;

// Class to manage the overall game state for all players
[System.Serializable]
public class WorldState
{
    public List<PlayerState> players = new List<PlayerState>();

    // Individual player state: position, rotation, and current action
    [System.Serializable]
    public class PlayerState
    {
        public string playerID;
        public Vector3 position;
        public Vector3 rotation;
        public string action;
    }

    // Update or add a player's state
    public void UpdatePlayerState(string id, Vector3 pos, Vector3 rot, string act)
    {
        PlayerState player = players.Find(p => p.playerID == id);
        if (player == null)
        {
            player = new PlayerState() { playerID = id };
            players.Add(player);
        }

        player.position = pos;
        player.rotation = rot;
        player.action = act;
    }

    // Convert the current state to JSON
    public string SerializeState()
    {
        return JsonUtility.ToJson(this);
    }

    // Convert JSON back into a WorldState object
    public static WorldState DeserializeState(string json)
    {
        return JsonUtility.FromJson<WorldState>(json);
    }
}


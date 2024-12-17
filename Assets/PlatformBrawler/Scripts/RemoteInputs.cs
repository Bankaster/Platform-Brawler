using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoteInputs : MonoBehaviour
{
    public bool Apressed = false;
    public bool Wpressed = false;
    public bool Spressed = false;
    public bool Dpressed = false;
    public bool Qpressed = false;
    public bool Epressed = false;
    public bool SpacePressed = false;

    //Reset all movement inputs
    public void ResetMovement()
    {
        //Reset Movement
        Apressed = false;
        Wpressed = false;
        Spressed = false;
        Dpressed = false;

        //Reset Rotation
        Qpressed = false;
        Epressed = false;

        //Reset Attack
        SpacePressed = false;
    }

    // Update inputs from replicated data
    public void UpdateFromReplication(string action)
    {
        ResetMovement();

        //Apply inputs based on replicated action
        switch (action)
        {
            case "MoveLeft": Apressed = true; break;
            case "MoveRight": Dpressed = true; break;
            case "MoveForward": Wpressed = true; break;
            case "MoveBackward": Spressed = true; break;
            case "RotateLeft": Qpressed = true; break;
            case "RotateRight": Epressed = true; break;
            case "Attack": SpacePressed = true; break;
        }
    }
}


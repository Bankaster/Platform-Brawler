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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Trap : RandomEvent
{
    void Start() {
        //Set probability values
        period = 0.33f;
        probability = 0.25f; 
    }
    /*
    Triggers the trap
    */
    protected override void OnRandomEventTriggered() {
        Debug.Log("Trap triggered");
    }
}

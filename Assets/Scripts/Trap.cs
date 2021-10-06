using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Trap : RandomEvent
{
    protected override void Start() {
        base.Start();
    }

    //Ensure that the trap does not trigger while on screen
    protected override void OnEnable() {
        base.OnEnable();
        eventEnabled = false;
    }

    protected override void OnDisable() {
        base.OnDisable();
        eventEnabled = true;
    }

    /*
    Triggers the trap
    */
    protected override void OnRandomEventTriggered() {
        Debug.Log("Trap triggered");
        Destroy(this.gameObject);
    }
}

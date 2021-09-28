using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTick : MonoBehaviour
{
    private TimeManager time;
    private Animator anim;
    private int tick;

    //This will determine where the tick will start before counting up.
    //This will allow saves of the current tick in future so the game can be started at a specific time.
    private void Awake()
    {
        time = GameObject.Find("GameManager").GetComponentInChildren<TimeManager>();

        tick = time.currentTick;

        anim = GetComponent<Animator>();
        //TODO: Add Tick to hours and hours variables to start from where they left off at some point.

        if (tick >= 426 && tick <= 1135)
            anim.SetTrigger("IdleDaySelect");
        else if (tick >= 1136 && tick <= 1145)
            anim.SetTrigger("DayToNightSelect");
        else if (tick >= 1146 || tick <= 414)
            anim.SetTrigger("IdleNightSelect");
        else if (tick >= 415 && tick <= 425)
            anim.SetTrigger("NightToDaySelect");
    }

    private void FixedUpdate()
    { 
        //Will determine what time of day/ night it is and play the corresponding animations to change/ stay at said time.
        if (tick == 1140)
            anim.SetTrigger("DayToNightSelect");
        else if (tick == 420)
            anim.SetTrigger("NightToDaySelect");
    }
}

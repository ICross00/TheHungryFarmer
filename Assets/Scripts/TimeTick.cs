using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTick : MonoBehaviour
{
    private const float tickTimerMax = 1.0f;
    private int tick;
    private float tickTimer;
    public int maxTicks;
    public int currentTick;
    private Animator anim;

    //This will determine where the tick will start before counting up.
    //This will allow saves of the current tick in future so the game can be started at a specific time.
    private void Awake()
    {
        tick = currentTick;
        anim = GetComponent<Animator>();

        if (tick >= 1 && tick <= 15)
            anim.SetTrigger("IdleDaySelect");
        else if (tick >= 16 && tick <= 75)
            anim.SetTrigger("DayToNightSelect");
        else if (tick >= 76 && tick <= 90)
            anim.SetTrigger("IdleNightSelect");
        else if (tick >= 91 && tick <= 150)
            anim.SetTrigger("NightToDaySelect");
    }

    private void Update()
    {
        //Thill will reset the ticks backs to zero after they reach maxTick value.
        if (tick == maxTicks)
        {
            tick = 1;
        }

        //This will update the tick every second (to change time they are updated, update the tickTimer value).
        tickTimer += Time.deltaTime;
        if(tickTimer > tickTimerMax)
        {
            tickTimer -= tickTimerMax;
            tick++;

            //Will determine what time of day/ night it is and play the corresponding animations to change/ stay at said time.
            if (tick == 10)
                anim.SetTrigger("DayToNightSelect");
            else if (tick == 20)
                anim.SetTrigger("NightToDaySelect");

            //This will print the ticks on the player (for testing purposes only, this will be removed).
            GameManager.instance.ShowText("Tick " + tick, 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
        }
    }
}

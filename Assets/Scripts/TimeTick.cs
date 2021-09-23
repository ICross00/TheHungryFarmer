using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTick : MonoBehaviour
{
    private const float tickTimerMax = 1.0f;
    private const int maxTicks = 1440;
    private int tick;
    private float tickTimer;
    public int currentTick;
    public int seconds;
    public int hours;
    public int days;
    private Animator anim;

    //This will determine where the tick will start before counting up.
    //This will allow saves of the current tick in future so the game can be started at a specific time.
    private void Awake()
    {
        tick = currentTick;

        seconds = tick % 60;

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

    private void Update()
    {
        //Thill will reset the ticks backs to zero after they reach maxTick value.
        if (tick == maxTicks)
        {
            tick = 0;
            days++;
            hours = 0;
            GameManager.instance.ShowText("Days: " + days, 60, Color.blue, transform.position, Vector3.up * 50, 3.0f);
        }

        //This will update the tick every second (to change time they are updated, update the tickTimer value).
        tickTimer += Time.deltaTime;
        if(tickTimer > tickTimerMax)
        {
            if (seconds == 60)
            {
                hours++;
                seconds = 0;
                GameManager.instance.ShowText("Hours: " + hours, 60, Color.green, transform.position, Vector3.up * 50, 3.0f);
            }

            tickTimer -= tickTimerMax;
            tick++;
            seconds++;

            //Will determine what time of day/ night it is and play the corresponding animations to change/ stay at said time.
            if (tick == 1140)
                anim.SetTrigger("DayToNightSelect");
            else if (tick == 420)
                anim.SetTrigger("NightToDaySelect");

            //This will print the ticks on the player (for testing purposes only, this will be removed).
            GameManager.instance.ShowText("Seconds: " + seconds, 20, Color.red, transform.position, Vector3.up * 50, 3.0f);
        }
    }
}

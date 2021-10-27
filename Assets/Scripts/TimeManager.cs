using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public GameManager gameManager;

    private const float tickTimerMax = 1.0f;
    private const int maxTicks = 1440;
    private int tick;
    private float tickTimer;
    public int currentTick;
    public int seconds;
    public int hours;
    public int days;
    public int dungeonRemaining;

    //This will determine where the tick will start before counting up.
    //This will allow saves of the current tick in future so the game can be started at a specific time.
    void Awake()
    {
        gameManager = GetComponent<GameManager>();

        tick = currentTick;

        dungeonRemaining = 1;
        seconds = tick % 60;
        hours = (currentTick - (currentTick % 60)) / 60;
    }

    void FixedUpdate()
    {
        //This will reset the ticks backs to zero after they reach maxTick value.
        if (tick == maxTicks)
        {
            NextDay();
            gameManager.ResetPlayer();
        }

        //This will update the tick every second (to change time they are updated, update the tickTimer value).
        tickTimer += Time.deltaTime;
        if (tickTimer > tickTimerMax)
        {
            if (seconds == 59)
            {
                hours++;
                seconds = 0;
            }

            tickTimer -= tickTimerMax;
            tick++;
            seconds++;
        }
    }

    public void NextDay()
    {
        tick = 480;
        hours = 8;
        seconds = 0;
        days++;
        dungeonRemaining = 1;
    }
}

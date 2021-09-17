using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeTick : MonoBehaviour
{
    private const float tickTimerMax = 1.0f;
    private int tick;
    private float tickTimer;
    public int maxTicks;

    private void Awake()
    {
        tick = 0;
    }

    private void Update()
    {
        //Thill will reset the ticks backs to zero after they reach maxTick value.
        if (tick == maxTicks)
        {
            tick = 0;
        }

        //This will update the tick every second (to change time they are updated, update the tickTimer value).
        tickTimer += Time.deltaTime;
        if(tickTimer > tickTimerMax)
        {
            tickTimer -= tickTimerMax;
            tick++;

            //This will print the ticks on the player (for testing purposes only, this will be removed).
            GameManager.instance.ShowText("Tick " + tick, 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class allows you to associate behaviour with a randomly triggered event by overriding the OnRandomEventTriggered function.

    The frequency at which OnRandomEventTriggered is called depends on the 'probability' and 'period' variables.

    For example, if 'period' is set to 0.1f and 'probability' is set to 0.5f, every 0.1 seconds there is a 50% chance that OnRandomEventTriggered is called.
    Override this method in a child class to define behaviour for when this random event is triggered, and assign values to
    the probability and period values to change how often this random event occurs.
*/
public abstract class RandomEvent : MonoBehaviour
{
    public float probability = 0.1f; //The probability that reach random check will pass, where 0.0 = never and 1.0 = certain
    public float period = 0.33f; //The number of seconds that must pass before another random check is made
    private float nextTrialTime = 0.0f; //Time of the next random trial

    protected abstract void OnRandomEventTriggered();

    void Start() {
        nextTrialTime = Time.time + period;
    }

    void Update()
    {
        if(Time.time > nextTrialTime) {
            float trial = Random.Range(0.0f, 1.0f);
            if(trial < probability) {
                OnRandomEventTriggered();
            }

            //Schedule next random trial
            nextTrialTime = Time.time + period;
        }
    }
}

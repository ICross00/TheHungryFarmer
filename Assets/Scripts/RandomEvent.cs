using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class allows you to associate behaviour with a randomly triggered event by overriding the OnRandomEventTriggered function.

    The frequency at which OnRandomEventTriggered is called depends on the 'eventProbability' and 'period' variables.

    For example, if 'period' is set to 0.1f and 'eventProbability' is set to 0.5f, every 0.1 seconds there is a 50% chance that OnRandomEventTriggered is called.
    Override this method in a child class to define behaviour for when this random event is triggered, and assign values to
    the eventProbability and period values to change how often this random event occurs.
*/
public abstract class RandomEvent : MonoBehaviour
{
    public static Dictionary<int, float> randomEventMap = new Dictionary<int, float>(); //Keeps track of times at which objects were disabled by using their instance ID as the key

    public float eventProbability = 0.1f;       //The probability that reach random check will pass, where 0.0 = never and 1.0 = certain
    public float period = 0.33f;                //The number of seconds that must pass before another random check is made
    public bool eventEnabled = true;            //Whether or not the random event should be triggered
    public bool simulateWhileDisabled = true;   //Whether or not random events should continue to be simulated while the object is disabled

    private float nextTrialTime = 0.0f;         //Time of the next random trial
    private float timeOnDisable = 0.0f;         //Last time the object was disabled

    protected abstract void OnRandomEventTriggered();


    //Virtual MonoBehaviour methods that can be overridden - make sure to call base.Start()
    protected virtual void Start() {
        nextTrialTime = Time.time + period;
        randomEventMap[GetInstanceID()] = Time.time;
        timeOnDisable = Time.time;
    }

    protected virtual void Update() {
        if(Time.time > nextTrialTime) {
            Trial();
            nextTrialTime = Time.time + period; //Schedule next random trial
        }
    }

    protected virtual void OnDisable() {
        randomEventMap[GetInstanceID()] = Time.time; //To be used later when object is re-enabled
    }

    protected virtual void OnEnable() {
        if(!randomEventMap.ContainsKey(GetInstanceID()))
            return;

        float deltatime = Time.time - randomEventMap[GetInstanceID()];
        SimulateTrials(deltatime); //Simulate trials based on how long the object was disabled
    }

    protected virtual void OnDestroy() {
        randomEventMap.Remove(GetInstanceID());
    }

    /*
    Performs a random trial with a chance of success defined by the eventProbability variable
    If successful, calls OnRandomEventTriggered
    If the RandomEvent instance is not enabled, this function does nothing
    */
    private void Trial() {
        if(!eventEnabled)
            return;

        float trial = Random.Range(0.0f, 1.0f);
        if(trial < eventProbability) {
            OnRandomEventTriggered();
        }
    }

    /*
    Estimates the number of random trials that would have occurred over a given time interval, and runs them using
    Trial(). This allows a longer period of random trials to be performed instantaneously. This can be used to simulate trials while
    the object has been disabled, e.g. a crop can continue to grow off screen.

    If the RandomEvent instance is not enabled, this function does nothing.

    @param deltatime The time interval over which to simulate trials
    */
    private void SimulateTrials(float deltatime) {
        if(!eventEnabled)
            return;

        if(deltatime > period) {
            int numPeriods = (int)Mathf.Floor(deltatime / period);
            for(int i = 0; i < numPeriods; i++)
                Trial(); //Run a trial for each period that passed during the time interval
        }
    }
}

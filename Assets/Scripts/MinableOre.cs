using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinableOre : RandomEvent
{
    private const float SHAKE_DURATION = 0.4f;        //Time in seconds to shake when struck
    private const float SHAKE_FREQUENCY = 4f;       //Number of shake oscillations to complete over SHAKE_DURATION
    private const float SHAKE_INTENSITY = 0.06f;     //Maximum displacement from origin during shake
    private float shakeTime = 0.0f;
    private int shakeDirection = 1;

    public Transform spriteObject; //The transform containing the ore sprite
    private SpriteRenderer sr;

    protected override void Start() {
        sr = spriteObject.GetComponent<SpriteRenderer>();
        base.Start();

        Shake();
    }

    //Re-enable the ore if it has been destroyed
    protected override void OnRandomEventTriggered() {

    }

    //Triggers a shake effect on the ore. This should be used when it is struck by a pickaxe
    public void Shake() {
        shakeDirection = Random.value > 0.5f ? 1 : -1;
        shakeTime = SHAKE_DURATION;
    }

    // Update is called once per frame
    protected override void Update()
    {
        if(shakeTime > 0.0f) { //Shake animation
            shakeTime = Mathf.Max(0.0f, shakeTime - Time.deltaTime); //Ensure time does not reach below 0
            float frac = 1.0f - (shakeTime / SHAKE_DURATION);
            float shakeOffset = Mathf.Sin(SHAKE_FREQUENCY * frac * 2.0f * Mathf.PI) * Mathf.Pow(1.0f - frac, 2);

            Vector3 vecOffset = new Vector3(shakeOffset * SHAKE_INTENSITY * shakeDirection, 0, 0);
            spriteObject.transform.localPosition = vecOffset;
        }

        base.Update();
    }
}

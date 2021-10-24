using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Class repressenting an object that can be mined with a tool, such as a tree or an ore containing an item such as an emerald or iron
*/
public class MinableObject : RandomEvent
{
    private const float SHAKE_DURATION = 0.35f;      //Time in seconds to shake when struck
    private const float SHAKE_FREQUENCY = 4.5f;      //Number of shake oscillations to complete over SHAKE_DURATION
    private const float SHAKE_INTENSITY = 0.06f;     //Maximum displacement from origin during shake
    private float shakeTime = 0.0f;
    private int shakeDirection = 1;

    public Transform spriteObject; //The transform containing the object sprite
    public ItemTemplate dropType;
    public float dropProbability = 0.06f; //The chance that when mined, this object will drop an item
    public int numDropsBeforeDestroy = 6; //The maximum number of times that this object can drop an item

    private int remainingDrops;
    private SpriteRenderer sr;
    private int numMisses = 0;
    private float missIncrement; //How much the probability of mining should increment by with each miss

    /*
    Returns the first instance of a MinableObject object that overlaps a provided position
    @param position The position to check for a minable object at
    */
    public static MinableObject GetObjectAtPosition(Vector2 position) {
        Collider2D colliderAtPoint = Physics2D.OverlapPoint(position);
        if(colliderAtPoint != null) {
            MinableObject obj = colliderAtPoint.GetComponent<MinableObject>();
            if(obj != null)
                return obj;
        }

        return null;
    }

    protected override void Start() {
        sr = spriteObject.GetComponent<SpriteRenderer>();
        remainingDrops = numDropsBeforeDestroy;
        missIncrement = dropProbability / 8;
        base.Start();
    }

    private void SetEnabled(bool enabled) {
        GetComponent<BoxCollider2D>().enabled = enabled;
        sr.enabled = enabled;
    }

    /*
    Spawns the set drop item in the world
    */
    private void SpawnDrop() {
        Item drop = new Item { itemTemplate = dropType, amount = 1, activeItem = !dropType.isEquippable };
        Collectable spawnedDrop = Collectable.Spawn(transform.position, drop, 0.75f);
        spawnedDrop.ApplyRandomForce(6.0f + Random.value * 3.0f);
    }

    /*
    Rolls a random chance to drop the item and decrements the remaining hits if successful
    */
    public void Mine() {
        if(!enabled) //Do nothing if the ore has been mined
            return;

        bool success = Random.value < (dropProbability + numMisses * missIncrement);
        numMisses = success ? 0 : (numMisses + 1); //Increase the probability with each miss to prevent long streaks of nothing being collected
        Debug.Log(numMisses);
        if(success) {
            SpawnDrop();

            remainingDrops--;
            if(remainingDrops == 0) {
                SetEnabled(false);
            }
        }

        shakeDirection = Random.value > 0.5f ? 1 : -1;
        shakeTime = SHAKE_DURATION;
    }



    //Re-enable the ore if it has been destroyed
    protected override void OnRandomEventTriggered() {
        SetEnabled(true);
        remainingDrops = numDropsBeforeDestroy;
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Trap : RandomEvent
{
    //The items that can be dropped from a trap, where each consecutive entry corresponds to a tier level
    private static string[] dropList = { "Chicken", "Steak", "Bacon" };

    private int tier = 1;
    private const int MIN_DROP = 2;
    private const int MAX_DROP = 4;

    protected override void Start() {
        //Assign unique name
        this.name =  "trap_t" + tier + "_" + GetInstanceID();

        this.period = 1.0f; //Check every 30 seconds
        this.eventProbability = 0.05f + tier * 0.05f; //Increasing probability of success with higher tier traps

        base.Start();
    }

    //Set the tier of the trap
    public void SetTier(int newTier) {
        this.tier = newTier;
    }

    //Set the new sprite
    public void SetSprite(Sprite newSprite) {
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        sr.sprite = newSprite;
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
        string dname = dropList[tier - 1];
        int amount = Random.Range(2, 4);
        Collectable spawnedItem = Collectable.Spawn(transform.position, dname, amount, 1.5f);
        Destroy(this.gameObject);
    }
}

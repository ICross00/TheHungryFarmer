using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Mover
{
    //xpValue determines how much XP is dropped from killing an enemy.
    public int xpValue = 1;

    //Trigger length determines the distance before the enemy starts chasing the player.
    //Chase lengths determines how far the enemy will chase the player.
    public float triggerLength = 1;
    public float chaseLength = 5;

    private bool chasing;
    private bool collidingWithPlayer;
    private Transform playerTransform;
    private Vector3 startingPosition;

    // Hitbox. This will duplicate collideable class due to not being able to inherit multiple classes.
    private BoxCollider2D hitBox;
    private Collider2D[] hits = new Collider2D[10];
    public ContactFilter2D filter;

    protected override void Start()
    {
        base.Start();
        playerTransform = GameObject.Find("Player_01").transform;
        startingPosition = transform.position;
        hitBox = transform.GetChild(0).GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //Is the player within range.
        if(Vector3.Distance(playerTransform.position,startingPosition) < chaseLength)
        {
            chasing = Vector3.Distance(playerTransform.position, startingPosition) < triggerLength;

            if (chasing)
            {
                if (!collidingWithPlayer)
                {
                    UpdateMotor((playerTransform.position - transform.position).normalized);
                }
            }
            else
            {
                UpdateMotor(startingPosition - transform.position);
            }
        }
        else
        {
            UpdateMotor(startingPosition - transform.position);
            chasing = false;
        }

        // Checking for any overlaps
        collidingWithPlayer = false;
        //Work for collision
        boxCollider.OverlapCollider(filter, hits);
        for (int num = 0; num < hits.Length; num++)
        {
            if (hits[num] == null)
                continue;

            if(hits[num].tag == "Fighter" && hits[num].name == "Player_01")
            {
                collidingWithPlayer = true;
            }

            //Clean array after process
            hits[num] = null;
        }

        UpdateMotor(Vector3.zero);
    }

    protected override void Death()
    {
        Destroy(gameObject);
        //TODO: Add ability to give player XP when enemy is killed. Video point (4:00 hours in).
        //TODO: Add text showing player got XP after the enemy dies. Previous TODO relies on this one.
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Mover
{
    //xpValue determines how much XP is dropped from killing an enemy.
    public int xpValue = 1;
    //Below are the variables required to make the Nav Mesh work and the enemies starting position.
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private Vector3 startingPosition;
    public float chaceDistance;

    protected override void Start()
    {
        base.Start();
        target = GameObject.Find("Player").transform;
        startingPosition = transform.position;

        //This will stop a bug with Nav Mesh. These settings are required to be true with 3D games only.
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        float distane = Vector3.Distance(transform.position, target.position);

        Debug.Log(distane);

        //This will stop the enemy chancing you off the bat and have it only chase when the player enter range.
        if (distane > chaceDistance)
        {
            agent.SetDestination(startingPosition);
        }
        else
        {
            agent.SetDestination(target.position);
        }

        //agent.SetDestination(target.position);
    }

    //This will destroy the enemy object and drop gold in its place
    protected override void Death()
    {
        //TODO: Have the enemy drop XP when it dies.

        int numSpawnedCoins = Random.Range(3, 7);

        Collectable.Spawn(transform.position, "GoldCoin", numSpawnedCoins, 1.0f);
        Destroy(gameObject);
    }
}
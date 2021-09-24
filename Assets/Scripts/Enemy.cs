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
        agent.SetDestination(target.position);
        
    }

    protected override void Death()
    {
        Destroy(gameObject);
        //TODO: Add ability to give player XP when enemy is killed. Video point (4:00 hours in).
        //TODO: Add text showing player got XP after the enemy dies. Previous TODO relies on this one.
    }
}

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
    public Vector3 directionDelta;
    public float chaseDistance = 20.0f;
    [SerializeField] string enemyName;
    GameObject knife;
    int health;
    Player player;
    private int lootMuliplier;

    protected override void Start()
    {
        base.Start();
        knife = GameObject.Find("ThrowingWeapon");
        target = GameObject.Find("Player").transform;
        startingPosition = transform.position;

        //This will stop a bug with Nav Mesh. These settings are required to be true with 3D games only.
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void FixedUpdate()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        directionDelta = transform.position - target.position;

        //This will stop the enemy chancing you off the bat and have it only chase when the player enter range.
        if (distance > chaseDistance)
        {
            agent.SetDestination(startingPosition);
        }
        else
        {
            agent.SetDestination(target.position);
        }

        if (hitPoint <= 0)
        {
            Death();
        }
    }

    //This will destroy the enemy object and drop loot in its place.
    protected override void Death()
    {
        lootMuliplier = GameObject.Find("ThrowingWeapon").GetComponent<ThrowingWeapon>().lootDropChance;

        int randNum = Random.Range(3, 7);

        //This will drop enemy loot based on what 'enemyName' thay have
        //NOTE: Using new tags seemed to cause issues with several other scripts
        //Slime
        if (enemyName == "Slime")
        {
            Collectable.Spawn(transform.position, "SlimeResidue", 1 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "GoldCoin", Random.Range(5, 10) * lootMuliplier, 1.0f);
            Destroy(gameObject);
        }
        //Bat
        else if (enemyName == "Bat")
        {
            Collectable.Spawn(transform.position, "BatWings", 1 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "GoldCoin", Random.Range(1, 3) * lootMuliplier, 1.0f);
            Destroy(gameObject);
        }
        //Ghost
        else if (enemyName == "Ghost")
        {
            Collectable.Spawn(transform.position, "Fish", 1 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "GoldCoin", Random.Range(1, 3) * lootMuliplier, 1.0f);
            Destroy(gameObject);
        }
        //No name
        else
        {
            Debug.Log("Enemy name invalid. Please type a name in the 'Enemy' script ('Slime', 'Bat', or 'Ghost')");
        }
        //Activates the life steal weapon perk when an enemy is killed.
        GameObject.Find("ThrowingWeapon").GetComponent<ThrowingWeapon>().LifeSteal();
    }
}
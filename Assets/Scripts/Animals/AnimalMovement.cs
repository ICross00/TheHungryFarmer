using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AnimalMovement : Fighter
{
    public int xpValue = 1;
    [SerializeField] Transform target;
    NavMeshAgent agent;
    private int lootMuliplier;
    public float EnemyDistance = 4.0f;
    public string animalName;
    public bool activeChase = true;
    private Animator anim;
    Vector2 direction;
    float distance;

    void Start()
    {
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();

        //This will stop a bug with Nav Mesh. These settings are required to be true with 3D games only.
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        FollowTarget();

        distance = Vector3.Distance(transform.position, target.transform.position);

        if (hitPoint < 10)
        {
            EnemyDistance = 15;
        }

        if (distance > EnemyDistance)
        {
            //Has the animal stop moving
            agent.SetDestination(transform.position);
            activeChase = false;
        }
        else
        {
            //Has the animal run from the player
            Vector3 dirToPlayer = transform.position - target.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            agent.SetDestination(newPos);
            activeChase = true;
        }

        if (hitPoint <= 0)
        {
            hitPoint = 0;
            Death();
        }
    }

    private void Death()
    {
        lootMuliplier = GameObject.Find("ThrowingWeapon").GetComponent<ThrowingWeapon>().lootDropChance;

        int randNum = Random.Range(3, 7);

        //This will drop animal loot based on what 'animalName' thay have
        //NOTE: Using new tags seemed to cause issues with several other scripts
        //Rabbit
        if (animalName == "Rabbit")
        {
            Collectable.Spawn(transform.position, "Leather", 1 * lootMuliplier, 1.0f);
            Destroy(gameObject);
        }
        //Pig
        else if (animalName == "Pig")
        {
            Collectable.Spawn(transform.position, "Bacon", 2 * lootMuliplier, 1.0f);
            Destroy(gameObject);
        }
        //No name
        else
        {
            Debug.Log("Animal name invalid.");
        }
        //Activates the life steal weapon perk when an animal is killed.
        GameObject.Find("ThrowingWeapon").GetComponent<ThrowingWeapon>().LifeSteal();
    }

    void FollowTarget()
    {
        direction = (target.transform.position - transform.position).normalized;

        if (distance < EnemyDistance)
        {
            //Controls movement for up and down
            if (direction.y > 0 && (direction.x > -0.25 && direction.x < 0.25))
            {
                anim.SetInteger("Idle", 3);
                anim.SetTrigger("Down");
            }
            else if (direction.y < 0 && (direction.x > -0.25 && direction.x < 0.25))
            {
                anim.SetInteger("Idle", 1);
                anim.SetTrigger("Up");
            }
            //Controls left and right movement animations
            else if (direction.x > 0.25)
            {
                anim.SetInteger("Idle", 4);
                anim.SetTrigger("Left");
            }
            else if (direction.x < -0.25)
            {
                anim.SetInteger("Idle", 2);
                anim.SetTrigger("Right");
            }
        }
        //Sets animal animation back to idle.
        else
        {
            anim.SetInteger("Idle", 0);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gaurdian : MonoBehaviour
{
    //Public fields
    public int hitPoint = 150;
    public float pushRecoverySpeed = 0.2f;
    Throwable knife;
    Pike pike1;
    Pike pike2;
    Pike pike3;
    Pike pike4;
    protected float immuneTime = 1.0f;
    protected float lastImmune;
    public Vector3 directionDelta;
    Animator anim;
    float distance;
    private Transform target;
    public float activatDistance;
    private int lootMuliplier;
    private bool gaurdianDead = false;
    private float lastUse;
    public int coolDown;
    public string gaurdianName;

    private bool switchTimer = true;

    private void Start()
    {
        pike1 = GameObject.Find("Pike").GetComponent<Pike>();
        pike2 = GameObject.Find("Pike2").GetComponent<Pike>();
        pike3 = GameObject.Find("Pike3").GetComponent<Pike>();
        pike4 = GameObject.Find("Pike4").GetComponent<Pike>();

        anim = GetComponent<Animator>();
        knife = GameObject.Find("ThrowingWeapon2").GetComponent<Throwable>();
        target = GameObject.Find("Player").transform;
    }

    //Push
    protected Vector3 pushDirection;

    //Making sure the gaurdian can be damaged.
    protected virtual void ReceiveDamage(Damage dmg)
    {
        if (Time.time - lastImmune > immuneTime)
        {
            if (hitPoint > 0)
            {
                lastImmune = Time.time;
                hitPoint -= dmg.damageAmount;
                pushDirection = (transform.position - dmg.origin).normalized * dmg.pushForce;

                //Shows the effect of damage being taken via text on screen.
                GameManager.instance.ShowText(dmg.damageAmount.ToString(), 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
            }
        }
    }

    //Controls the gaudians animations based on health.
    private void Update()
    {
        if (hitPoint > (hitPoint / 2))
        {
            anim.SetBool("Full", true);
            anim.SetBool("Half", false);
            anim.SetBool("None", false);
        }
        else if (hitPoint > 0 && hitPoint < ((hitPoint / 2) + 1))
        {
            anim.SetBool("Full", false);
            anim.SetBool("Half", true);
            anim.SetBool("None", false);
        }
        else if (hitPoint <= 0)
        {
            anim.SetBool("Full", false);
            anim.SetBool("Half", false);
            anim.SetBool("None", true);
        }
    }

    //Controls the gaurdians functions including health and death.
    private void FixedUpdate()
    {
        distance = Vector3.Distance(transform.position, target.position);
        directionDelta = transform.position - target.position;

        //This will stop the enemy chancing you off the bat and have it only chase when the player enter range.
        if (distance < activatDistance)
        {
            GetComponent<BoxCollider2D>().enabled = true;

            if (hitPoint > 0)
            {
                Timer();
            }

            if (switchTimer == false && hitPoint > 0)
            {
                BlockModeEnabled();
            }
            else if (switchTimer == true && hitPoint > 0)
            {
                BlockModeDisabled();
            }
            else if (hitPoint <= 0 && gaurdianDead == false)
            {
                BlockModeDisabled();
                LootDrop();
                gaurdianDead = true;
            }
        }
        else
        {
            BlockModeDisabled();
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }

    //Pikes changing to attack mode (player cannot attack)
    private void BlockModeEnabled()
    {
        pike1.PikesOut();
        pike2.PikesOut();
        pike3.PikesOut();
        pike4.PikesOut();

        knife.enableFire = true;
    }

    //Pikes changing to idle mode (player can attack)
    private void BlockModeDisabled()
    {
        pike1.PiklesIdle();
        pike2.PiklesIdle();
        pike3.PiklesIdle();
        pike4.PiklesIdle();

        knife.enableFire = false;
    }

    //Time between gaurdian phases
    private void Timer()
    {
        if (Time.time - lastUse > coolDown)
        {
            lastUse = Time.time;

            if (switchTimer == true)
                switchTimer = false;
            else if (switchTimer == false)
                switchTimer = true;
        }
    }

    private void LootDrop()
    {
        lootMuliplier = GameObject.Find("ThrowingWeapon").GetComponent<ThrowingWeapon>().lootDropChance;
        int randNum = Random.Range(0, 100);

        if (gaurdianName == "One")
        {
            //TODO: Add loot to the first gaurdian as seen below

            Collectable.Spawn(transform.position, "Ruby", 16 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "Emerald", 8 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "Diamond", 4 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "GoldCoin", 200 * lootMuliplier, 1.0f);

            if (randNum > 98 || randNum < 100)
            {
                Collectable.Spawn(transform.position, "BlackSwordHandle", 1, 1.0f);
            }
        }
        else if (gaurdianName == "Two")
        {
            //TODO: Add loot to the second gaurdian as seen below

            Collectable.Spawn(transform.position, "Ruby", 20 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "Emerald", 10 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "Diamond", 6 * lootMuliplier, 1.0f);
            Collectable.Spawn(transform.position, "GoldCoin", 300 * lootMuliplier, 1.0f);

            if (randNum > 93 || randNum < 100)
            {
                Collectable.Spawn(transform.position, "BlackSwordBlade", 1, 1.0f);
            }
        }
    }
}

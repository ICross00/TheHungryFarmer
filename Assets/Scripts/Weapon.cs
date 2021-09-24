using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : Collideable
{
    private Player player;

    // Damage structure
    public int damagePoint = 1;
    public float pushForce = 2.0f;

    // Upgrade section
    public int weaponLevel = 0;
    private SpriteRenderer spriteRenderer;

    //Swinging the sword
    private Animator anim;
    private float cooldown = 1.0f;
    private float lastSwing;

    protected override void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    protected override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Mouse0) && player.isInvOpen == false)
        {
            if(Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                anim.SetTrigger("SwordAttack");
            }
        }

        //TODO: Add another swing in the opposite direction you are facing. This is so you can attack while running away.
        //This will swing in the opposite direction to where the player usually attacks.
        //P.s. Mouse1 == Right clicking mouse.
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (Time.time - lastSwing > cooldown)
            {
                lastSwing = Time.time;
                Swing();
            }
        }
    }

    protected override void OnCollide(Collider2D coll)
    {
        if(coll.tag == "Fighter")
        {
            if(coll.name == "Player")
                return;

            // Creates a new damage object and sends it to the fighter using hit.
            Damage dmg = new Damage
            {
                damageAmount = damagePoint,
                origin = transform.position,
                pushForce = pushForce
            };

            coll.SendMessage("ReceiveDamage", dmg);
        }

        
        if(coll.tag == "Crop") {
            coll.SendMessage("HarvestCrop");
        }
    }


    private void Swing()
    {
        anim.SetTrigger("Swing");
    }
}

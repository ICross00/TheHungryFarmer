using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ThrowingWeapon : MonoBehaviour
{
    Player player;
    public GameObject target;
    public Vector3 offset = new Vector3(0, 0, -1);

    public Rigidbody2D rb;
    private Camera sceneCamera;
    private GameObject ThrowingKnife;
    public Transform FirePoint;
    private Vector2 mousePosition;
    public float fireForce;
    private float lastSwing;
    public int lifeSteal;
    public int lootDropChance;

    string swordCheck;

    //Array to check if a sword is equipped in a future if statement
    private string[] swordTypes = {"Sword", "Sword2", "Sword3", "Sword4", "Sword5", "Sword6"};

    private void Start()
    {
        ThrowingKnife = Resources.Load<GameObject>("Prefabs/ThrowingKnife");
        sceneCamera = Camera.main;
        player = GameObject.Find("Player").GetComponent<Player>();
        target = GameObject.Find("Player");
    }

    void Update()
    {
        sceneCamera = Camera.main;
        ProcessInputs();
        //Calls the AimDirection method.
        AimDirection();
    }

    void FixedUpdate()
    {
        //Has the throwingweapon object follow the player assuming the target exists.
        if (target)
        {
            transform.position = new Vector3(
                target.transform.position.x + offset.x,
                target.transform.position.y + offset.y,
                target.transform.position.z + offset.z);
        }
    }

    public void Fire()
    {
        GameObject projectile = Instantiate(ThrowingKnife, FirePoint.position, FirePoint.rotation);
        //Instantiate(ThrowingKnife, FirePoint.position, FirePoint.rotation);
        projectile.GetComponent<Rigidbody2D>().AddForce(FirePoint.up * fireForce, ForceMode2D.Impulse);
    }

    void ProcessInputs()
    {
        mousePosition = sceneCamera.ScreenToWorldPoint(Input.mousePosition);

        //Throws the knife assuming the player has the sword equiped and the cooldown is over.
        //Try catch has been added to avoid Null Reference if nothing is equipped.
        try
        {
            swordCheck = player.GetSelectedItem().GetItemType().ToString();
            if (swordTypes.Contains(player.GetSelectedItem().GetItemType().ToString()) && Time.time - lastSwing > CoolDownTime(swordCheck))
            {
                if (Input.GetMouseButtonDown(1) && (player.isInvOpen == false))
                {
                    lastSwing = Time.time;
                    Fire();
                }
            }
        }
        catch (System.NullReferenceException) { }
    }

    // Update is called once per frame
    void AimDirection()
    {
        //Sets the objects position on the player.

        //Rotate the direction of weapon
        //NOTE: The gun is there to avoid having to use player direction. Doing this makes it easier to aim in game.
        Vector2 aimDirection = mousePosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }

    //Sets the sword cooldown based on what sword is equiped and sets the damage.
    //Will also set bonus perks such as life steal and extra loot for the sword.
    private int CoolDownTime(string swordType)
    {
        if (swordType == "Sword")
        {
            lifeSteal = 0;
            lootDropChance = 1;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 4;
            return 3;
        }
        else if (swordType == "Sword2")
        {
            lifeSteal = 0;
            lootDropChance = 1;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 4;
            return 3;
        }
        else if (swordType == "Sword3")
        {
            lifeSteal = 0;
            lootDropChance = 1;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 6;
            return 3;
        }
        else if (swordType == "Sword4")
        {
            lifeSteal = 1;
            lootDropChance = 1;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 6;
            return 2;
        }
        else if (swordType == "Sword5")
        {
            lifeSteal = 2;
            lootDropChance = 2;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 10;
            return 2;
        }
        else if (swordType == "Sword6")
        {
            lifeSteal = 3;
            lootDropChance = 3;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 10;
            return 1;
        }
        else
        {
            lifeSteal = 0;
            lootDropChance = 1;
            ThrowingKnife.GetComponent<KnifeScript>().knifeDamage = 4;
            return 3;
        }
    }

    public void LifeSteal()
    {
        GameObject.Find("Player").GetComponent<Player>().hitPoint = 5;
    }
}

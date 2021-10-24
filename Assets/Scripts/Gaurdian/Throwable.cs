using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Throwable : MonoBehaviour
{
    GameObject player;
    public GameObject target;
    public Vector3 offset = new Vector3(0, 0, -1);

    public Rigidbody2D rb;
    private GameObject throwingObject;
    public Transform FirePoint;
    private Vector2 playerPosition;
    public float fireForce;
    private float lastSwing;
    public int coolDown;

    public bool enableFire = false;

    private void Start()
    {
        throwingObject = Resources.Load<GameObject>("Prefabs/GaurdianKnife");
        player = GameObject.Find("Player");
        target = GameObject.Find("GaurdianOne");
    }

    void Update()
    {
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
        if (enableFire == true)
        {
            GameObject projectile = Instantiate(throwingObject, FirePoint.position, FirePoint.rotation);
            //Instantiate(ThrowingKnife, FirePoint.position, FirePoint.rotation);
            projectile.GetComponent<Rigidbody2D>().AddForce(FirePoint.up * fireForce, ForceMode2D.Impulse);
        }
    }

    void ProcessInputs()
    {
        //Throws the knife assuming the player has the sword equiped and the cooldown is over.
        //Try catch has been added to avoid Null Reference if nothing is equipped.
        if (Time.time - lastSwing > coolDown)
        {
            lastSwing = Time.time;
            Fire();
        }
    }

    // Update is called once per frame
    void AimDirection()
    {
        //Sets the objects position on the player.

        //Rotate the direction of weapon
        //NOTE: The gun is there to avoid having to use player direction. Doing this makes it easier to aim in game.
        playerPosition = player.transform.position;
        Vector3 aimDirection = playerPosition - rb.position;
        float aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = aimAngle;
    }
}

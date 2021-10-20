using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowingWeapon : MonoBehaviour
{
    Player player;
    public GameObject target;
    public Vector3 offset = new Vector3(0, 0, -1);

    public Rigidbody2D rb;
    private Camera sceneCamera;
    public GameObject ThrowingKnife;
    public Transform FirePoint;
    private Vector2 mousePosition;
    public float fireForce;
    public float cooldown;
    private float lastSwing;
    Item selectedItem;

    private void Start()
    {
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
            if (player.GetSelectedItem().GetItemType() == ItemType.Sword)
            {
                if (Input.GetMouseButtonDown(1) && (Time.time - lastSwing > cooldown))
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
}

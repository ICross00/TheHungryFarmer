using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover : Fighter
{
    public int experience;
    public int energy;

    protected Vector3 playerLocation;
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    //Controls movement speed.
    protected float ySpeed = 4f;
    protected float xSpeed = 5f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        Collectable itemWorld = collider.GetComponent<Collectable>();
        //If we collided with an item, add it to the inventory
        if (itemWorld != null)
        {
            //Get the player's inventory
            Inventory playerInventory = gameObject.GetComponent(typeof(Inventory)) as Inventory;
            playerInventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

        //Handle other 2D trigger events here
    }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //Rest movedealta.
        playerLocation = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Changing direction sprite is facing (left or right)
        if (playerLocation.x > 0)
            transform.localScale = Vector3.one;
        else if (playerLocation.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Add push vector if there are any.
        playerLocation += pushDirection;

        //Reduce push force every frame to prevent what has been pushed from going on FOREVER AND EVER AND EVER AN.....
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //Checking if a box is in the direction the payer is moving. If a box is there, the player will not be allowed to move there.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, playerLocation.y), Mathf.Abs(playerLocation.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Allowing the sprite to move
            transform.Translate(0, playerLocation.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(playerLocation.x, 0), Mathf.Abs(playerLocation.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Allowing the sprite to move
            transform.Translate(playerLocation.x * Time.deltaTime, 0, 0);
        }
    }
}

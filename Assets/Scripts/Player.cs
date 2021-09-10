using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    public int experience;
    public int energy;
    public Vector3 playerLocation;
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;

    //Will be set in the fighter class in future.
    private float xSpeed = 5.0f;
    private float ySpeed = 4.01f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        WorldItem.Spawn(new Vector3(1, 1), new Item(Item.ItemType.Heart, 1));
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        WorldItem itemWorld = collider.GetComponent<WorldItem>();
        //If we collided with an item, add it to the inventory
        if(itemWorld != null) {
            //Add it to the inventory here
            Debug.Log("1");
            itemWorld.DestroySelf();
        }
    }
    private void FixedUpdate()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        //Rest movedealta.
        playerLocation = new Vector3(x * xSpeed, y * ySpeed, 0);

        //Changing direction sprite is facing (left or right)
        if (playerLocation.x > 0)
            transform.localScale = Vector3.one;
        else if (playerLocation.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

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

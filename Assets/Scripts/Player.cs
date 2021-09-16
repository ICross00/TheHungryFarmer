using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Fighter
{
    public int experience;
    public int energy;
    public UI_Inventory inventoryUI;

    public Vector3 playerLocation;
    private BoxCollider2D boxCollider;
    private RaycastHit2D hit;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;

    //Will be set in the fighter class in future.
    private float xSpeed = 5.0f;
    private float ySpeed = 4.01f;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        inventory = GetComponent<Inventory>();
        inventoryUI.SetInventory(inventory);
        inventoryUI.SetOwner(this);

        //Setup callback functions for interacting with the inventory UI
        inventoryUI.onButtonLeftClicked.AddListener((int slotIndex) => {
            Item clickedItem = inventory.GetItemList()[slotIndex];
            UseItem(clickedItem);
        });

        inventoryUI.onButtonRightClicked.AddListener((int slotIndex) => {
            Collectable spawnedItem = inventory.DropItem(transform.position, slotIndex);
            //Apply a force to the spawned item
            Rigidbody2D rb2d = spawnedItem.GetComponent<Rigidbody2D>();
            rb2d.AddForce(Random.insideUnitCircle.normalized * 11, ForceMode2D.Impulse);
        });
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Collectable itemWorld = collider.GetComponent<Collectable>();
        //If we collided with an item, add it to the inventory
        if(itemWorld != null) {
            //Get the player's inventory
            Inventory playerInventory = GetComponent<Inventory>();
            playerInventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }
        //Handle other 2D trigger events here
    }

    private void FixedUpdate()
    {
        //Movement code
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

    void Update() {
        //UI Keybinds

        //Show/hide inventory
        if(Input.GetKeyDown(KeyCode.F)) {
            if(inventoryUI) {
                inventoryUI.ToggleVisible();
            }
        }
    }

    /*
    This function will be called by the UI_Inventory class in the OnButtonLeftClicked function when the user left-clicks an inventory item.
    It can also be called when the player left-clicks while the inventory is closed and they have an item selected in their hotbar.

    The received parameter is the item that the user used.

    Define behaviours for item types being used inside the function.
    */
    public void UseItem(Item item)  {
        switch(item.GetItemType()) {
            case ItemType.Seeds_Tomato:
                Debug.Log("Planted tomato seeds");
            break;

            case ItemType.Seeds_Carrot:
                Debug.Log("Planted carrot seeds");
            break;

            case ItemType.Seeds_Strawberry:
                Debug.Log("Planted strawberry seeds");
            break;
        }
    }
}

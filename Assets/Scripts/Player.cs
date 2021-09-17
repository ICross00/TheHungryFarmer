using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public int experience;
    public int energy;
    public UI_Inventory inventoryUI;

    
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        inventory = GetComponent<Inventory>();
        inventoryUI.SetInventory(inventory);

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
        //Enter a shop trigger
        Shop worldShop = collider.GetComponent<Shop>();
        if(worldShop != null) {
            worldShop.OpenShop(this);
        }
    }

    private void OnTriggerExit2D(Collider2D collider) {
                //Exit a shop trigger
        Shop worldShop = collider.GetComponent<Shop>();
        if(worldShop != null) {
            worldShop.CloseShop();
        }
    }

    private void FixedUpdate()
    {
        //Movement code
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector3(x, y, 0));
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

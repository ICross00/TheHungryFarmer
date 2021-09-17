using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Mover
{
    public UI_Inventory inventoryUI;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;
    private GameManager gameManager;

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();

        inventoryUI.SetInventory(inventory);

        //Setup callback functions for interacting with the inventory UI

        //This function will run when the player left clicks on an inventory slot in their own inventory
        inventoryUI.onButtonLeftClicked.AddListener((int slotIndex) => {
            Item clickedItem = inventory.GetItemList()[slotIndex];
            UseItem(clickedItem);
        });

        //This function will run when the player right clicks on an inventory slot in their own inventory
        inventoryUI.onButtonRightClicked.AddListener((int slotIndex) => {
            Collectable spawnedItem = inventory.DropItem(transform.position, slotIndex);
            //Apply a force to the spawned item
            Rigidbody2D rb2d = spawnedItem.GetComponent<Rigidbody2D>();
            rb2d.AddForce(Random.insideUnitCircle.normalized * 11, ForceMode2D.Impulse);
        });
    }

    //Returns the player's inventory
    public Inventory GetInventory() {
        return inventory;
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Entered trigger");

        Collectable itemWorld = collider.GetComponent<Collectable>();
        //If we collided with an item, add it to the inventory
        if(itemWorld != null) {
            //Get the player's inventory
            inventory.AddItem(itemWorld.GetItem());
            itemWorld.DestroySelf();
        }

        //Handle other 2D trigger events here
    }

    protected override void OnTriggerExit2D(Collider2D collider) {
        Debug.Log("Exited trigger");
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

        //Interactable keybinds
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Find all objects the player can interact with at this position
            List<Interactable> interactableObjects = Interactable.GetInteractablesAtPoint(transform.position);

            foreach(Interactable i in interactableObjects) {
                i.Interact(this);
            }
        }
    }

    /*
    This function will be called by the UI_Inventory class in the OnButtonLeftClicked function when the user left-clicks an inventory item.
    This behaviour is established by the callback functions assigned in the Start() function of this class.

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

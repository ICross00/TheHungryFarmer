using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Mover
{
    public UI_Inventory inventoryUI;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;
    private GameManager gameManager;

    //Actions to store and use items. These may be temporarily overridden by other classes, so are stored so they may be reset
    private UnityAction<int> dropItem;
    private UnityAction<int> useItem;

    protected override void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();

        inventoryUI.SetInventory(inventory);

        //Setup callback functions for interacting with the inventory UI

        //This function will run when the player right clicks on an inventory slot in their own inventory
        dropItem = (int slotIndex) => {
            Collectable spawnedItem = inventory.DropItem(transform.position, slotIndex);
            //Apply a force to the spawned item
            Rigidbody2D rb2d = spawnedItem.GetComponent<Rigidbody2D>();
            rb2d.AddForce(Random.insideUnitCircle.normalized * 11, ForceMode2D.Impulse);
        };

        //This function will run when the player left clicks on an inventory slot in their own inventory
        useItem = (int slotIndex) => {
            Item clickedItem = inventory.GetItemList()[slotIndex];
            UseItem(clickedItem);
        };

        //Attach the listeners
        SetDefaultInventoryListeners();
    }

    //Sets the default actions for when the player interacts with inventory slots
    public void SetDefaultInventoryListeners() {
        inventoryUI.onButtonLeftClicked.RemoveAllListeners();
        inventoryUI.onButtonLeftClicked.AddListener(useItem);
        inventoryUI.onButtonRightClicked.RemoveAllListeners();
        inventoryUI.onButtonRightClicked.AddListener(dropItem);
    }

    //Returns the player's inventory
    public Inventory GetInventory() {
        return inventory;
    }

    //Returns the amount of gold held by the player
    public int GetGold() {
        return gameManager.GetGold();
    }

    //Wrapper function for the GameManager's ChangeGold function.
    //This allows objects that the player interacts with (such as shops) to change the player's gold without needing to acquire a reference to the GameManager
    public void ChangeGold(int amount) {
        gameManager.ChangeGold(amount);
    }

    protected override void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Entered trigger");

        Collectable itemWorld = collider.GetComponent<Collectable>();
        //If we collided with a valid collectable...
        if(itemWorld != null) {
            Item collectedItem = itemWorld.GetItem();

            if(collectedItem.GetItemType() == ItemType.Coin_Gold) {
                 //Special case for gold, it should not be added to the inventory but used to increment the gold stored in the game manager
                ChangeGold(collectedItem.amount);
            } else {
                //Otherwise, add the item to the inventory
                inventory.AddItem(collectedItem);
            }

            //Destroy the collectable
            itemWorld.DestroySelf();
        }

        //Handle other 2D trigger events here
    }

    protected override void OnTriggerExit2D(Collider2D collider) {
        Debug.Log("Exited trigger");
        //Exit a interactable trigger, close any dialogs or UI elements
        Interactable worldInteractable = collider.GetComponent<Interactable>();
        if(worldInteractable != null) {
            worldInteractable.Close(this);
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

            case ItemType.Heart:
                Debug.Log("Used a heart to heal");
                hitPoint += 5; //Increase health by 5

                inventory.RemoveItemSingle(item); //Remove one heart from the inventory
            break;
        }
    }
}

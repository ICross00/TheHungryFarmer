using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Mover
{
    public float interactionRadius = 1.2f;
    public UI_Inventory inventoryUI;
    private SpriteRenderer spriteRenderer;
    private Inventory inventory;
    private GameManager gameManager;
    public bool isInvOpen = false;

    public Item selectedItem;

    //Actions to store and select items. These may be temporarily overridden by other classes, so are stored so they may be reset
    private UnityAction<int> dropItem;
    private UnityAction<int> selectItem;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(gameObject);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("GameManager").GetComponent<Inventory>();

        //inventoryUI.SetInventory(inventory);
        //Setup callback functions for interacting with the inventory UI.

        //This function will run when the player right clicks on an inventory slot in their own inventory.
        dropItem = (int slotIndex) => {
            Item clickedItem = inventory.GetItem(slotIndex);

            if(clickedItem == selectedItem) {
                selectedItem.Equip(this, false); //Trigger unequip behaviour
                selectedItem = null;
                UpdateItemAnimations();
            }

            Collectable spawnedItem = inventory.DropItem(transform.position, slotIndex);
            spawnedItem.ApplyRandomForce(11.0f);
        };

        //This function will run when the player left clicks on an inventory slot in their own inventory

        selectItem = (int slotIndex) => {
            Item clickedItem = inventory.GetItem(slotIndex);

            //Handle equipped item logic
            Item previousItem = selectedItem;
            selectedItem = (clickedItem == previousItem) ? null : clickedItem; //Update selected item

            if(selectedItem != null)
                selectedItem.Equip(this, true); //Trigger equip behaviour for the newly selected item
            if(previousItem != null)
                previousItem.Equip(this, false); //Trigger unequip behaviour for the previously selected item

            UpdateItemAnimations(); //Update animations
        };

        inventory.OnItemListChanged += (object sender, System.EventArgs e) => {
            if(selectedItem != null)  {
                if(selectedItem.amount == 0) {
                    selectedItem.Equip(this, false); //Trigger unequip behaviour
                    selectedItem = null;
                }
            }
            UpdateItemAnimations();
        };

        //Attach the listeners
        SetDefaultInventoryListeners();
    }

    /*
    Updates the held and equipped item animation states
    */
    public void UpdateItemAnimations() {
        this.transform.Find("HeldItem").GetComponent<HeldItem>().updateHeldItem(selectedItem);
        this.transform.Find("EquippedItem").GetComponent<EquippedItem>().updateEquippedItem(selectedItem);
        this.GetComponent<PlayerAnimator>().UpdateItemAnim();
    }

    //Sets the default actions for when the player interacts with inventory slots
    public void SetDefaultInventoryListeners() {
        inventoryUI.onButtonLeftClicked.RemoveAllListeners();
        inventoryUI.onButtonLeftClicked.AddListener(selectItem);
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

    private void FixedUpdate()
    {
        //Movement code
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector2(x, y));
    }

    void Update() {
        //UI Keybinds

        //Show/hide inventory
        if(Input.GetKeyDown(KeyCode.F)) {
            if(inventoryUI) {
                isInvOpen = !isInvOpen;
                inventoryUI.ToggleVisible();
            }
        }

        //Use selected item
        if(Input.GetKeyDown(KeyCode.E)) {
            if(selectedItem != null) {
                if(selectedItem.amount <= 0) //Deselect the item
                    selectedItem = null;
                else
                    selectedItem.Use(this);
            }
        }

        //Interactable keybinds
        if(Input.GetKeyDown(KeyCode.Space)) {
            //Find all objects the player can interact with at this position
            List<Interactable> interactableObjects = Interactable.GetInteractablesInRadius(transform.position, interactionRadius);

            foreach(Interactable i in interactableObjects) {
                i.Interact(this);
            }
        }
    }
}

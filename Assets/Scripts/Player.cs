using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : Mover
{
    public float interactionRadius = 1.2f;
    public UI_Inventory inventoryUI;
    public UI_Hotbar hotbarUI;
    private SpriteRenderer spriteRenderer;
    private PlayerInventory inventory;
    private InventoryUIController inventoryController;
    private GameManager gameManager;
    public bool isInvOpen = false;


    //Actions to store and select items. These may be temporarily overridden by other classes, so are stored so they may be reset
    private UnityAction<Item, int> dropItem;
    private UnityAction<Item, int> selectItem;
    private UnityAction<Item, int> selectHotbarItem;
    private UnityAction<Item, int> transferHotbarItem;

    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(gameObject);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("GameManager").GetComponent<PlayerInventory>();

        hotbarUI.Initialize(inventory);
        //Initialize inventory UI clicking behaviour
        inventoryController = new InventoryUIController {
            player = this,
            inventory = inventory,
            inventoryUI = inventoryUI,
            hotbarUI = hotbarUI
        };

        dropItem = inventoryController.OnInventoryRightClick;           //Inventory slot right clicked
        selectItem = inventoryController.OnInventoryLeftClick;          //Inventory slot left clicked
        selectHotbarItem = inventoryController.OnHotbarLeftClick;       //Hotbar slot left clicked
        transferHotbarItem = inventoryController.OnHotbarRightClick;    //Hotbar slot right clicked

        SetDefaultInventoryListeners(); //Attach listeners
    }

    /*
    Updates the held and equipped item animation states
    */
    public void UpdateItemAnimations() {
        Item selectedItem = inventoryController.selectedItem;

        GetComponentInChildren<HeldItem>().updateHeldItem(selectedItem);
        GetComponentInChildren<EquippedItem>().updateEquippedItem(selectedItem);
        GetComponent<PlayerAnimator>().UpdateItemAnim();
    }

    //Sets the default actions for when the player interacts with inventory slots
    public void SetDefaultInventoryListeners() {
        inventoryUI.SetClickListeners(selectItem, dropItem);
        hotbarUI.SetClickListeners(selectHotbarItem, transferHotbarItem);
    }

    //Returns the selected item
    public Item GetSelectedItem() {
        return inventoryController.selectedItem;
    }

    //Returns the player's inventory
    public Inventory GetInventory() {
        return (Inventory)inventory;
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
        //Item/inventory keybinds

        if(Input.GetKeyDown(KeyCode.F))
            inventoryController.ShowHideInventory();

        if(Input.GetKeyDown(KeyCode.E))
            inventoryController.UseSelectedItem();

         for(int i = 1; i <= UI_Inventory.ROW_SIZE; i++) //Keys 1-8 on the keyboard are used to access the hotbar
             if(Input.GetKeyDown(i.ToString())) {
                inventoryController.SelectHotbarItem(i - 1);
                break; //Only allow selecting one item
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

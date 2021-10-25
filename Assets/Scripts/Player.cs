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
    private PlayerSkills playerSkills;

    public ItemBehaviour activeBehaviour; //Behaviour associated with the current item

    //Actions to store and select items. These may be temporarily overridden by other classes, so are stored so they may be reset
    private UnityAction<Item, int> dropItem;
    private UnityAction<Item, int> selectItem;
    protected override void Start()
    {
        base.Start();
        spriteRenderer = GetComponent<SpriteRenderer>();

        DontDestroyOnLoad(gameObject);

        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        inventory = GameObject.Find("GameManager").GetComponent<PlayerInventory>();

        hotbarUI.Initialize(inventory);

        //Initialize inventory UI clicking behaviour
        inventoryController = new InventoryUIController
        {
            player = this,
            inventory = inventory,
            inventoryUI = inventoryUI,
            hotbarUI = hotbarUI
        };

        inventoryController.SetUIListeners(); //Attach listeners
    }

    private void Awake()
    {
        playerSkills = new PlayerSkills();
        playerSkills.OnSkillUnlocked += PlayerSkills_OnSkillUnlocked;
    }

    private void PlayerSkills_OnSkillUnlocked(object sender, PlayerSkills.OnSkillUnlockedEventArgs e)
    {
        //Add skills to unlock
        switch(e.skillType)
        {
            case PlayerSkills.SkillType.healthMax_1:
                
                break;
            case PlayerSkills.SkillType.healthMax_2:

                break;
            case PlayerSkills.SkillType.healthMax_3:

                break;
            case PlayerSkills.SkillType.MoveSpeed_1:

                break;
            case PlayerSkills.SkillType.MoveSpeed_2:

                break;
    

        }

    }

    /*
    Updates the held and equipped item animation states
    */
    public void UpdateItemAnimations()
    {
        Item selectedItem = inventoryController.selectedItem;

        GetComponentInChildren<HeldItem>().updateHeldItem(selectedItem);
        GetComponentInChildren<EquippedItem>().updateEquippedItem(selectedItem);
        GetComponent<PlayerAnimator>().UpdateItemAnim();
    }

    //Sets the default actions for when the player interacts with inventory slots
    public void SetDefaultInventoryListeners()
    {
        inventoryController.SetUIListeners();
    }

    public PlayerSkills GetPlayerSkills()
    {
        return playerSkills;
    }


    //Returns the selected item
    public Item GetSelectedItem()
    {
        return inventoryController.selectedItem;
    }

    //Returns the player's inventory
    public Inventory GetInventory()
    {
        return (Inventory)inventory;
    }

    //Returns the amount of gold held by the player
    public int GetGold()
    {
        return gameManager.GetGold();
    }

    //Wrapper function for the GameManager's ChangeGold function.
    //This allows objects that the player interacts with (such as shops) to change the player's gold without needing to acquire a reference to the GameManager
    public void ChangeGold(int amount)
    {
        gameManager.ChangeGold(amount);
    }

    /*
    Sets the speed at which the player mines as a multiplier, where 1 = default
    @param speed The new mining speed
    */
    public void SetMiningSpeed(float speed) {
        GetComponent<PlayerAnimator>().SetMiningSpeed(speed);
    }

    //This function will be called by the mining animation whenever the pickaxe strikes the ground
    public void OnMine(int direction) {
        if(activeBehaviour != null & activeBehaviour is Behaviour_Pickaxe) {
            Behaviour_Pickaxe pickaxe = (Behaviour_Pickaxe)activeBehaviour;
            pickaxe.OnMine(this, direction);
        }
    }

    private void FixedUpdate()
    {
        //Movement code
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");

        UpdateMotor(new Vector2(x, y));
    }

    void Update()
    {
        //Item/inventory keybinds

        if (Input.GetKeyDown(KeyCode.F))
            inventoryController.ShowHideInventory();

        if (Input.GetKeyDown(KeyCode.E))
            inventoryController.UseSelectedItem();

        for (int i = 1; i <= UI_Inventory.ROW_SIZE; i++) //Keys 1-8 on the keyboard are used to access the hotbar
            if (Input.GetKeyDown(i.ToString()))
            {
                inventoryController.SelectHotbarItem(i - 1);
                break; //Only allow selecting one item
            }


        /*Check for scrolling through hotbar slots
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if(scroll != 0f)
            inventoryController.ChangeHotbarItem(scroll);
        */

        //Interactable keybinds
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Find all objects the player can interact with at this position
            List<Interactable> interactableObjects = Interactable.GetInteractablesInRadius(transform.position, interactionRadius);

            foreach (Interactable i in interactableObjects)
            {
                i.Interact(this);
            }
        }
    }
}

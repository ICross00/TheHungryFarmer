using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Inventory : MonoBehaviour
{
    public static readonly float CELL_SIZE = 21.0f;
    public static readonly int ROW_SIZE = 8;

    public Inventory m_inventory; //The inventory object that this will display a UI for
    protected Transform itemSlotContainer;
    protected Transform itemSlotTemplate;
    protected Tooltip tooltip;

    protected bool isVisible;

    public EventSystem eventSystem;

    public UnityEvent<Item, int> onButtonRightClicked; //Callback function run whenever the player left-clicks an item in the inventory
    public UnityEvent<Item, int> onButtonLeftClicked; //Callback function run whenever the player right-clicks an item in the inventory

    protected void Start() {
        itemSlotContainer = transform.Find("InventoryImage");
        itemSlotTemplate = itemSlotContainer.Find("SlotTemplate");

        tooltip = itemSlotContainer.Find("Tooltip").GetComponent<Tooltip>();
        isVisible = false;

        //Set the render camera
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Canvas uiCanvas = GetComponent<Canvas>();
        uiCanvas.worldCamera = mainCamera;
        Refresh();
    }

    /*
    Sets the inventory that this UI inventory object will display from
    */
    public void SetInventory(Inventory inventory) {
        m_inventory = inventory;
        m_inventory.OnItemListChanged += UI_OnItemListChanged;
    }

    /*
    Sets actions for when an item is left and right clicked
    */
    public void SetClickListeners(UnityAction<Item, int> leftClickListener, UnityAction<Item, int> rightClickListener) {
        onButtonLeftClicked.RemoveAllListeners();
        onButtonRightClicked.RemoveAllListeners();

        onButtonLeftClicked.AddListener(leftClickListener);
        onButtonRightClicked.AddListener(rightClickListener);
    }

    //Refresh when the inventory data structure is changed
    protected void UI_OnItemListChanged(object sender, System.EventArgs e) {
        Refresh();
    }

    /*
    Toggles the inventory gameobject between visible and not visible
    */
    public void ToggleVisible()  {
        SetVisible(!isVisible);
    }

    /*
    Makes the inventory gameobject appear visible
    If visible is true, the gameobject will appear. If visible is false, the game object will be hidden
    */
    public void SetVisible(bool visible) {
        isVisible = visible;
        GetComponent<Canvas>().enabled = isVisible;
        tooltip.ShowTooltip(false);
        Refresh();
    }

    /**
    Callback function run whenever the player left-clicks an item in the inventory
    */
    protected virtual void OnButtonLeftClicked(Item item, int slotIndex) {
        onButtonLeftClicked.Invoke(item, slotIndex);
        tooltip.ShowTooltip(false);
        Refresh();
    }

    /**
    Callback function run whenever the player right-clicks an item in the inventory
    */
    protected virtual void OnButtonRightClicked(Item item, int slotIndex) {
        onButtonRightClicked.Invoke(item, slotIndex);
        tooltip.ShowTooltip(false);
        Refresh();
    }

    /**
    Callback function run whenever the player's mouse hovers over an item in the inventory
    */
    protected virtual void OnButtonHover(int slotIndex) {
        Item hoveredItem = m_inventory.GetItemList()[slotIndex];
        string text = hoveredItem.GetName() + "\nUnit Price: " + hoveredItem.GetUnitSellPrice().ToString();

        tooltip.ShowTooltip(true);
        tooltip.SetText(text);
    }


    /**
    Callback function run whenever the player's mouse stops hovering over an item in the inventory
    */
    protected virtual void OnButtonHoverExit(int slotIndex) {
        tooltip.ShowTooltip(false);
        tooltip.SetText("");
    }

    /*
    Refreshes all of the event listeners to default for a provided item slot
    @param slotItem The ClickableObject associated with the item slot for which event listeners will be reset
    */
    protected void RefreshListeners(ClickableObject slotButton) {
        slotButton.RemoveAllListeners();
        slotButton.onRight.AddListener(OnButtonRightClicked);
        slotButton.onLeft.AddListener(OnButtonLeftClicked);
        slotButton.onHover.AddListener(OnButtonHover);
        slotButton.onExit.AddListener(OnButtonHoverExit);
    }

    private void DrawSlots() {
        int x = 0;
        int y = 0;

        int index = 0; //Index in the item list of the current slot

        foreach (Item item in m_inventory.GetItemList()) {
            RectTransform slotTf = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = itemSlotTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * CELL_SIZE, y * CELL_SIZE);

            Image image = slotTf.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();
            image.preserveAspect = true;
            image.rectTransform.sizeDelta = new Vector2(16, 16);

            //Update quantity text
            TextMeshProUGUI amountText = slotTf.Find("AmountText").GetComponent<TextMeshProUGUI>();
            amountText.text = item.amount > 1 ? item.amount.ToString() : "";

            //Update event listeners
            ClickableObject slotButton = slotTf.Find("ItemButton").GetComponent<ClickableObject>();
            slotButton.index = index;
            slotButton.item = item;
            RefreshListeners(slotButton);

            //Wrap around rows
            x++;
            if(x > ROW_SIZE) {
                x = 0;
                y--;
            }

            index++;
        }
    }

    /*
    Redraws the inventory by destroying all the slot gameobjects and recreating them from the current inventory state
    Can be overridden to draw additional information
    */
    private void Refresh() {
        if(m_inventory == null) 
            return;

        //Destroy old gameobjects
        tooltip.ShowTooltip(false);
        foreach(Transform child in itemSlotContainer) {
            if(child == itemSlotTemplate || child == tooltip.transform) continue;
            Destroy(child.gameObject);
        }

        //Redraw item slots
        DrawSlots();
    }
}

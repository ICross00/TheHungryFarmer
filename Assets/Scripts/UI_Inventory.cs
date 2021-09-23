using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Inventory : MonoBehaviour
{
    private Inventory m_inventory; //The inventory object that this will display a UI for
    private Transform itemSlotContainer;
    private Transform itemSlotTemplate;
    private Tooltip tooltip;
    private int selectedIndex;

    private bool isVisible;

    public EventSystem eventSystem;

    public UnityEvent<int> onButtonRightClicked; //Callback function run whenever the player left-clicks an item in the inventory
    public UnityEvent<int> onButtonLeftClicked; //Callback function run whenever the player right-clicks an item in the inventory

    private void Start() {
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

    //Refresh when the inventory data structure is changed
    private void UI_OnItemListChanged(object sender, System.EventArgs e) {
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
    private void OnButtonLeftClicked(int slotIndex) {
        onButtonLeftClicked.Invoke(slotIndex);

        tooltip.SetText(m_inventory.GetItemList()[slotIndex].GetName());
        tooltip.ShowTooltip(false);

        //Select the item, if it was already selected then deselect it
        selectedIndex = (slotIndex == selectedIndex) ? -1 : slotIndex;
        Refresh();
    }

    /**
    Callback function run whenever the player right-clicks an item in the inventory
    */
    private void OnButtonRightClicked(int slotIndex) {
        onButtonRightClicked.Invoke(slotIndex);
        tooltip.ShowTooltip(false);

        //Deselect the item
        selectedIndex = -1;

        Refresh();
    }

    /**
    Callback function run whenever the player's mouse hovers over an item in the inventory
    */
    private void OnButtonHover(int slotIndex) {
        Item hoveredItem = m_inventory.GetItemList()[slotIndex];
        string text = hoveredItem.GetName() + "\nUnit Price: " + hoveredItem.GetUnitSellPrice().ToString();

        tooltip.ShowTooltip(true);
        tooltip.SetText(text);
    }


    /**
    Callback function run whenever the player's mouse stops hovering over an item in the inventory
    */
    private void OnButtonHoverExit(int slotIndex) {
        tooltip.ShowTooltip(false);
        tooltip.SetText("");
    }

    /*
    Refreshes all of the event listeners to default for a provided item slot
    @param slotItem The ClickableObject associated with the item slot for which event listeners will be reset
    */
    private void RefreshListeners(ClickableObject slotButton) {
        slotButton.RemoveAllListeners();
        slotButton.onRight.AddListener(OnButtonRightClicked);
        slotButton.onLeft.AddListener(OnButtonLeftClicked);
        slotButton.onHover.AddListener(OnButtonHover);
        slotButton.onExit.AddListener(OnButtonHoverExit);
    }

    /*
    Redraws the inventory by destroying all the slot gameobjects and recreating them from the current inventory state
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

        int x = 0;
        int y = 0;
        float cellSize = 21f;
        float cellMargin = 0f;

        int index = 0; //Index in the item list of the current slot

        foreach (Item item in m_inventory.GetItemList()) {
            RectTransform slotTf = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = itemSlotTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * cellSize + cellMargin, y * cellSize + cellMargin);

            //Update sprite
            Image image = slotTf.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();

            //Scale up the image if it is selected
            if(selectedIndex >= 0 && selectedIndex == index) {
                image.rectTransform.sizeDelta = new Vector2(16, 16);
            } else {
                image.rectTransform.sizeDelta = new Vector2(12, 12);
            }

            //Update quantity text
            Text amountText = slotTf.Find("AmountText").GetComponent<Text>();
            amountText.text = item.amount > 1 ? item.amount.ToString() : "";

            //Update event listeners
            ClickableObject slotButton = slotTf.Find("ItemButton").GetComponent<ClickableObject>();
            slotButton.index = index;
            RefreshListeners(slotButton);

            //Wrap around rows
            x++;
            if(x > 8) {
                x = 0;
                y--;
            }

            index++;
        }
    }
}

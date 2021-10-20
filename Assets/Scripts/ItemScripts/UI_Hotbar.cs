using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_Hotbar : UI_Inventory
{
    private PlayerInventory hotbarInventory;
    private bool hasInitialized = false;
    private int selectedIndex = -1;

    public void Initialize(PlayerInventory inventory) {
        hotbarInventory = inventory;
        hotbarInventory.OnHotbarChanged += UI_OnHotbarChanged;

        hasInitialized = true;
    }

    public int GetSelectedIndex() {
        return selectedIndex;
    }

    public void SetSelectedIndex(int index) {
        selectedIndex = index;
        Refresh();
    }

    private void UI_OnHotbarChanged(object sender, System.EventArgs e) {
        Refresh();
    }

    /**
    Callback function run whenever the player left-clicks an item in the hotbar
    */
    protected override void OnButtonLeftClicked(Item item, int slotIndex) {
        onButtonLeftClicked.Invoke(item, slotIndex);
        tooltip.ShowTooltip(false);
        //Select the item, if it was already selected then deselect it
        selectedIndex = (slotIndex == selectedIndex) ? -1 : slotIndex;
        Refresh();
    }

    /**
    Callback function run whenever the player right-clicks an item in the hotbar
    */
    protected override void OnButtonRightClicked(Item item, int slotIndex) {
        onButtonRightClicked.Invoke(item, slotIndex);
        tooltip.ShowTooltip(false);

        //Deselect the item
        if(selectedIndex > slotIndex)
            selectedIndex--;
        else
            selectedIndex = -1;

        Refresh();
    }

    /**
    Callback function run whenever the player's mouse hovers over an item in the hotbar
    */
    protected override void OnButtonHover(int slotIndex) {
        Item hoveredItem = hotbarInventory.GetHotbarList()[slotIndex];
        string text = hoveredItem.GetName() + "\nUnit Price: " + hoveredItem.GetUnitSellPrice().ToString();

        tooltip.ShowTooltip(true);
        tooltip.SetText(text);
    }

    private void Refresh() {
        if(!hasInitialized)
            return;

        //Destroy old gameobjects
        tooltip.ShowTooltip(false);
        foreach(Transform child in itemSlotContainer) {
            if(child == itemSlotTemplate || child == tooltip.transform) continue;
            Destroy(child.gameObject);
        }

        int x = 0;
        int y = 0;

        int index = 0; //Index in the item list of the current slot

        //Redraw hotbar slots
        foreach (Item item in hotbarInventory.GetHotbarList()) {
            RectTransform slotTf = Instantiate(itemSlotTemplate, itemSlotContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = itemSlotTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * 41.0f, y * 41.0f);

            Image image = slotTf.Find("ItemIcon").GetComponent<Image>();
            image.sprite = item.GetSprite();
            image.preserveAspect = true;

            if(index == selectedIndex) {
                image.rectTransform.sizeDelta = new Vector2(36, 36);
                Shadow outline = image.gameObject.AddComponent(typeof(Shadow)) as Shadow;
                outline.effectDistance = new Vector2(3.0f, -3.0f);
            } else {
                image.rectTransform.sizeDelta = new Vector2(30, 30);
            }

            //Update quantity text
            Text amountText = slotTf.Find("AmountText").GetComponent<Text>();
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
}

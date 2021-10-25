using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UI_CraftingStation : MonoBehaviour
{
    public static readonly float CELL_SIZE = 120.0f;
    public static readonly float ITEM_SIZE = 100.0f;
    public static readonly int ROW_SIZE = 6;

    private CraftingStation craftingStation;
    protected Transform craftingContainer;
    protected Transform craftingTemplate;
    protected Tooltip tooltip;

    protected bool isVisible;
    public EventSystem eventSystem;

    protected void Start() {
        craftingContainer = transform.Find("CraftingBackground");
        craftingTemplate = craftingContainer.Find("CraftSlotTemplate");

        tooltip = craftingContainer.Find("Tooltip").GetComponent<Tooltip>();
        isVisible = false;

        //Set the render camera
        Camera mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Canvas uiCanvas = GetComponent<Canvas>();
        uiCanvas.worldCamera = mainCamera;
        Refresh();
    }

    public void SetCraftingStation(CraftingStation craftingStation) {
        this.craftingStation = craftingStation;
        Refresh();
    }

    /*
    Toggles the crafting inventory gameobject between visible and not visible
    */
    public void ToggleVisible()  {
        SetVisible(!isVisible);
    }

    /*
    Makes the crafting inventory gameobject appear visible
    If visible is true, the gameobject will appear. If visible is false, the game object will be hidden
    */
    public void SetVisible(bool visible) {
        isVisible = visible;
        GetComponent<Canvas>().enabled = isVisible;
        tooltip.ShowTooltip(false);
        Refresh();
    }

    /**
    Callback function run whenever the player left-clicks an item in the crafting inventory
    The item parameter is unused
    */
    protected virtual void OnButtonLeftClicked(Item item, int slotIndex) {
        CraftingRecipe clickedRecipe = craftingStation.craftableItems[slotIndex];
        clickedRecipe.Craft(craftingStation.GetInventory());
    }

    /**
    Callback function run whenever the player's mouse hovers over an item in the crafting inventory
    */
    protected virtual void OnButtonHover(int slotIndex) {
        CraftingRecipe hoveredRecipe = craftingStation.craftableItems[slotIndex];
        Inventory inv = craftingStation.GetInventory();
        string text =  "<b>" + hoveredRecipe.craftingOutput.itemName + "</b>\nIngredients:\n" + hoveredRecipe.FormatIngredientString(inv);
        tooltip.ShowTooltip(true);
        tooltip.SetText(text);
    }


    /**
    Callback function run whenever the player's mouse stops hovering over an item in the crafting inventory
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
        slotButton.onLeft.AddListener(OnButtonLeftClicked);
        slotButton.onHover.AddListener(OnButtonHover);
        slotButton.onExit.AddListener(OnButtonHoverExit);
    }

    private void DrawSlots() {
        int x = 0;
        int y = 0;

        int index = 0; //Index in the item list of the current slot

        foreach (CraftingRecipe recipe in craftingStation.craftableItems) {
            RectTransform slotTf = Instantiate(craftingTemplate, craftingContainer).GetComponent<RectTransform>();
            slotTf.gameObject.SetActive(true);
            RectTransform templateTf = craftingTemplate.GetComponent<RectTransform>();
            slotTf.anchoredPosition = templateTf.anchoredPosition + new Vector2(x * CELL_SIZE, y * CELL_SIZE);

            Image image = slotTf.Find("ItemIcon").GetComponent<Image>();
            image.sprite = recipe.craftingOutput.sprite;
            image.preserveAspect = true;
            image.rectTransform.sizeDelta = new Vector2(ITEM_SIZE, ITEM_SIZE);

            //Update quantity text
            TextMeshProUGUI amountText = slotTf.Find("AmountText").GetComponent<TextMeshProUGUI>();
            amountText.text = recipe.outputAmount > 1 ? recipe.outputAmount.ToString() : "";

            //Update event listeners
            ClickableObject slotButton = slotTf.Find("ItemButton").GetComponent<ClickableObject>();
            slotButton.index = index;
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
    Redraws the crafting inventory by destroying all the slot gameobjects and recreating them from the current inventory state
    */
    private void Refresh() {
        if(craftingStation == null) 
            return;

        //Update width
        RectTransform rect = craftingContainer.GetComponent<RectTransform>();
        int nrecipes = craftingStation.GetNumRecipes();
        int nwidth = 190 + 120 * (nrecipes - 1);
        int nheight = 190 + 120 * (nrecipes % ROW_SIZE);
        rect.sizeDelta = new Vector2(nwidth, rect.sizeDelta.y);

        //Destroy old gameobjects
        tooltip.ShowTooltip(false);
        foreach(Transform child in craftingContainer) {
            if(child == craftingTemplate || child == tooltip.transform) continue;
            Destroy(child.gameObject);
        }

        //Redraw item slots
        DrawSlots();
    }
}

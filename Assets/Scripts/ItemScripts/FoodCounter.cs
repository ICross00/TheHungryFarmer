using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCounter : Interactable
{
    //References
    SpriteRenderer placedFood;
    private Item storedItem;
    public int dayTracker = 0;
    public TimeManager timeManager;

    public static int moneyEarned;

    public bool test;

    void Start()
    {
        if (!test)
        {
            placedFood = transform.Find("placed_food").GetComponent<SpriteRenderer>();
            timeManager = GameObject.Find("GameManager").GetComponent<TimeManager>();
        }
        storedItem = null;
        moneyEarned = 0;
    }

    private void Update()
    {
        if (dayTracker != timeManager.days)
        {
            dayTracker = timeManager.days;
            ClearStoredItem();
            moneyEarned = 0;
        }
    }

    /*
     * This function is an override from the Interactable class, and takes the item from the players
     * selected item slot, if it is a valid cooked food item. 
     * 
     * @param triggerPlayer The player that is interacting with the food counter.
     */
    protected override void OnInteract(Player triggerPlayer)
    {
        if (storedItem == null)
        {
            if (triggerPlayer.GetSelectedItem() != null)
            {
                if (triggerPlayer.GetSelectedItem().GetItemTagValue("food_type") == "cooked")
                {
                    storedItem = triggerPlayer.GetSelectedItem();
                    placedFood.sprite = storedItem.GetSprite();
                    ((PlayerInventory)triggerPlayer.GetInventory()).RemoveItemFromHotbar(triggerPlayer.GetSelectedItem());
                    AddMoneyEarned();
                }
            }
        }
    }

    public Item GetStoredItem()
    {
        if (this.storedItem != null)
            return this.storedItem;
        else
            return null;
    }

    public void SetStoredItem(Item item)
    {
        this.storedItem = item;
    }

    public void AddMoneyEarned()
    {
        moneyEarned += storedItem.itemTemplate.sellPrice;
    }

    public void ClearStoredItem()
    {
        this.storedItem = null;
        if (!test)
            placedFood.sprite = null;
    }
}

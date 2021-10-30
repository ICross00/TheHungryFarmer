using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodCounter : Interactable
{
    //References
    SpriteRenderer placedFood;
    Item storedItem;

    void Awake()
    {
        placedFood = transform.Find("placed_food").GetComponent<SpriteRenderer>();
        storedItem = null;
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
                }
            }
        }
    }
}

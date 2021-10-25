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
        placedFood = GameObject.Find("placed_food").GetComponent<SpriteRenderer>();
    }

    protected override void OnInteract(Player triggerPlayer)
    {
        if (triggerPlayer.GetSelectedItem() != null)
        {
            if (triggerPlayer.GetSelectedItem().GetItemTagValue("food_type") == "cooked")
            {
                storedItem = triggerPlayer.GetSelectedItem();
                placedFood.sprite = storedItem.GetSprite();
                //triggerPlayer.GetInventory().RemoveItem(triggerPlayer.GetSelectedItem());
            }
        }
    }
}

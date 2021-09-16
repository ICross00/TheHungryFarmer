using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Class representing an instance of an item that is stored in an inventory or attached to a collectable
    This class encapsulates the template that this item derives from as well as the stack amount
*/
[System.Serializable]
public class Item
{
    public ItemTemplate item;
    public int amount;

    /*
    Returns the type of this item, as one of the enumerations of the ItemType enum in the ItemTemplate class
    */
    public ItemType GetItemType() {
        return item.type;
    }

    /* Gets the name of the item as a string */
    public string GetName() {
        return item.itemName;
    }

    public Sprite GetSprite() {
        return item.sprite;
    }

    public int GetMaxStack() {
        return item.maxStack;
    }

    public int GetSellPrice() {
        return item.sellPrice;
    }
}

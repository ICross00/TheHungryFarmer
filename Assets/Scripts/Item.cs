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

    /**
    Creates an instance of an item that can be added to an inventory.
    If you want to spawn an item in the world physically as a Collectable, see the Spawn() function in the Collectable class.

    @param name The name of the item to create. This must match the name of the item as found in the Resources/Items folder
    @param itemAmount The amount of item to include in the stack
    */

    public static Item CreateItem(string name, int itemAmount) {
        ItemTemplate template = Resources.Load<ItemTemplate>("Items/"+name);
        return new Item { item = template, amount = itemAmount };
    }

    /**
    Creates a copy of an item. This is useful when purchasing an item from a shop. Items in shop inventories should not be copied by reference to the player's inventory,
    as any modification to that reference will have unintended side effects the shop's inventory.

    @param itemToCopy The item to copy
    @return A copy of the item
    */
    public static Item CopyItem(Item itemToCopy) {
        return new Item { item = itemToCopy.item, amount = itemToCopy.amount };
    }

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

    public int GetUnitSellPrice() {
        return item.sellPrice;
    }

    public int GetTotalSellPrice() {
        return item.sellPrice * amount;
    }
}

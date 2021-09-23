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
    public ItemTemplate itemTemplate;
    public int amount;
    public bool activeItem;

    /**
    Creates an instance of an item that can be added to an inventory.
    If you want to spawn an item in the world physically as a Collectable, see the Spawn() function in the Collectable class.

    @param name The name of the item to create. This must match the name of the item as found in the Resources/Items folder
    @param itemAmount The amount of item to include in the stack
    */

    public static Item CreateItem(string name, int itemAmount) {
        ItemTemplate template = Resources.Load<ItemTemplate>("Items/"+name);
        return new Item { itemTemplate = template, amount = itemAmount };
    }

    /**
    Creates a copy of an item. This is useful when purchasing an item from a shop. Items in shop inventories should not be copied by reference to the player's inventory,
    as any modification to that reference will have unintended side effects the shop's inventory.

    @param itemToCopy The item to copy
    @return A copy of the item
    */
    public static Item CopyItem(Item itemToCopy) {
        return new Item { itemTemplate = itemToCopy.itemTemplate, amount = itemToCopy.amount };
    }

    /*
    Returns the type of this item, as one of the enumerations of the ItemType enum in the ItemTemplate class
    */
    public ItemType GetItemType() {
        return itemTemplate.type;
    }

    /* Gets the name of the item as a string */
    public string GetName() {
        return itemTemplate.itemName;
    }

    public Sprite GetSprite() {
        return itemTemplate.sprite;
    }

    public int GetMaxStack() {
        return itemTemplate.maxStack;
    }

    public int GetUnitSellPrice() {
        return itemTemplate.sellPrice;
    }

    public int GetTotalSellPrice() {
        return itemTemplate.sellPrice * amount;
    }

    public string GetInternalName() {
        return itemTemplate.name;
    }

    //Access the set value of a tag from a given key
    public string GetItemTagValue(string key) {
        return itemTemplate.GetTagValue(key);
    }

    /*
        Uses an item. The player should be passed in so that additional information is available.
        For example, the position of the player can be used in this function, or the player's
        inventory can be accessed if you want to remove an item to make it a consumable.

        To define new item behaviour, add a new switch case corresponding to the enum in ItemType (see ItemTemplate.cs) you want to define behaviour for.

        @param item The item to use
        @param user The player who used this item
    */
    public static void UseItem(Item item, Player user)  {
        Inventory inventory = user.GetInventory();

        switch(item.GetItemType()) {
            case ItemType.Seeds_Tomato:
            case ItemType.Seeds_Carrot:
            case ItemType.Seeds_Strawberry:
                Debug.Log("Planted crop: " + item.GetInternalName());
                GameObject pfCrop = Resources.Load<GameObject>("Prefabs/pfPlantedCrop");
                PlantedCrop crop = GameObject.Instantiate(pfCrop, user.transform.position, Quaternion.identity).GetComponent<PlantedCrop>();
                crop.cropTemplate = item.itemTemplate;

                //Consume the item
                inventory.RemoveItemSingle(item);
            break;

            case ItemType.Heart:
                Debug.Log("Used a heart to heal");
                user.hitPoint += 5; //Increase health by 5

                inventory.RemoveItemSingle(item); //Remove one heart from the inventory
            break;
        }
    }
}

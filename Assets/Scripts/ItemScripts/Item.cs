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
    public bool activeItem = true;

    /**
    Creates an instance of an item that can be added to an inventory.
    If you want to spawn an item in the world physically as a Collectable, see the Spawn() function in the Collectable class.

    @param name The name of the item to create. This must match the name of the item as found in the Resources/Items folder
    @param itemAmount The amount of item to include in the stack
    @param activeItem
    */
    public static Item CreateItem(string name, int itemAmount, bool activeItem = false) {
        ItemTemplate template = Resources.Load<ItemTemplate>("Items/"+name);
        return new Item { itemTemplate = template, amount = itemAmount, activeItem = !template.isEquippable };
    }

    /**
    Creates a copy of an item. This is useful when purchasing an item from a shop. Items in shop inventories should not be copied by reference to the player's inventory,
    as any modification to that reference will have unintended side effects on the shop's inventory.

    @param itemToCopy The item to copy
    @return A copy of the item
    */
    public static Item CopyItem(Item itemToCopy) {
        return new Item { itemTemplate = itemToCopy.itemTemplate, amount = itemToCopy.amount, activeItem = itemToCopy.itemTemplate.isEquippable };
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

    public string GetDescription() {
        return itemTemplate.GetTagValue("Description");
    }

    //Access the set value of a tag from a given key
    public string GetItemTagValue(string key) {
        return itemTemplate.GetTagValue(key);
    }

    /*
    Triggers the equip/uenquip behaviour for this item.

    @param ply The player who equipped or unequipped this item
    @param equipped Whether the item was equipped (true) or unequipped (false)
    */
    public void Equip(Player ply, bool equipped) {
        Inventory inventory = ply.GetInventory();
        ply.activeBehaviour = null;

        //Invoke the item's OnEquipped/OnUnequipped behaviour
        if(itemTemplate != null) {
            ItemBehaviour behaviour = itemTemplate.GetItemBehaviour();
            if(behaviour != null) {
                if(equipped) {
                    behaviour.OnItemEquipped(this, ply, inventory);
                    ply.activeBehaviour = behaviour;
                } else {
                    behaviour.OnItemUnequipped(this, ply, inventory);
                }
            }
        }
    }

    /*
    Uses an item. The player should be passed in so that additional information is available.
    For example, the position of the player can be used in this function, or the player's
    inventory can be accessed if you want to remove an item to make it a consumable.

    @param ply The player who used this item
    */
    public void Use(Player ply)  {
        PlayerInventory inventory = ply.GetInventory() as PlayerInventory;
        //Invoke the item's OnUse behaviour
        if(itemTemplate != null) {
            ItemBehaviour behaviour = itemTemplate.GetItemBehaviour();

            if(behaviour != null) {
                behaviour.OnItemUsed(this, ply, inventory);
            }

            //Refresh the inventory in case an item was consumed
            inventory.TriggerRefresh();
        }
        /*
        switch(item.GetItemType()) {
            case ItemType.Seeds_Tomato:
            case ItemType.Seeds_Carrot:
            case ItemType.Seeds_Strawberry:
                GameObject pfCrop = Resources.Load<GameObject>("Prefabs/pfPlantedCrop");
                Transform plantableGrid = GameObject.Find("Grid").transform;

                if(PlantedCrop.CanPlant(user.transform.position)) {
                    GameObject crop = GameObject.Instantiate(pfCrop, user.transform.position, Quaternion.identity, plantableGrid);
                    crop.GetComponent<PlantedCrop>().cropTemplate = item.itemTemplate; //Set the crop type after spawning it
                    //Consume the item
                    inventory.RemoveItemSingle(item);
                }
            break;

            case ItemType.Heart:
                Debug.Log("Used a heart to heal");
                user.hitPoint += 5; //Increase health by 5

                inventory.RemoveItemSingle(item); //Remove one heart from the inventory
            break;
        }
        */
    }
}

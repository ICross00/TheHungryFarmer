using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
HOW TO USE THE ITEM SYSTEM:
For items to be visible in a scene, an empty gameobject with an 'ItemAssets' component attached to it must exist.
Then, in the inspector, set the sprite for each item individually

To add an entirely new item, add a new line to the 'ItemType' enum corresponding to the item's name.
Then, in the ItemAssets script file, add a new public variable with type Sprite to store the sprite for this item.

Then, add a new case to the switch statement in GetSprite to return the item's sprite from ItemAssets using the name you gave the variable in the previous step.

For example, if you wanted to add a "Sword" item, you would add "Sword" to the ItemType enum, and you named the sword sprite "SwordSprite",
then the new switch case would look as follows:

case ItemType.Sword: return ItemAssets.Instance.SwordSprite;
*/

//Class representing an item
[Serializable]
public class Item
{
    public enum ItemType {
        Heart,
        Star
    }

    public ItemType type;
    public int amount;

    public Item(ItemType type, int amount) {
        this.type = type;
        this.amount = amount;
    }

    public Sprite GetSprite() {
        switch(type) {
            default:
            case ItemType.Heart: return ItemAssets.Instance.HeartSprite;
            case ItemType.Star: return ItemAssets.Instance.StarSprite;
        }
    }
}

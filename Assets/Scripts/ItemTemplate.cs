using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
HOW TO USE THE ITEM SYSTEM:

To add an entirely new item, navigate to the 'Items' folder under 'Assets/Resources'. Right click within the folder and go to Create > Items > Item Template.
Add the name of the new item type to the 'ItemType' enum in the class below on a new line.

This will create a new template item for which you must specify a sprite, type, maximum stack size, and a user-friendly name.
*/

//Class representing an instance of an item
[Serializable]
[CreateAssetMenu(fileName = "New Item Template", menuName = "Items/Item Template")]
public class ItemTemplate : ScriptableObject
{
    public string itemName;
    public ItemType type;
    public int maxStack;
    public int sellPrice;
    public Sprite sprite;
}

    public enum ItemType {
        Heart,
        Star,
        Gear,
        Diamond,
        Ruby,
        Emerald,
        Coin_Gold,
        Coin_Silver,
        Coin_Copper,
        Tomato,
        Strawberry,
        Carrot,
        Seeds_Tomato,
        Seeds_Strawberry,
        Seeds_Carrot
    }
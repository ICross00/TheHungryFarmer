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

[Serializable]
[CreateAssetMenu(fileName = "New Item Template", menuName = "Items/Item Template")]
public class ItemTemplate : ScriptableObject
{
    //Dictionary to map behaviour names as strings to ItemBehaviour scripts
    public static Dictionary<string, ItemBehaviour> itemBehaviourMap = new Dictionary<string, ItemBehaviour>();

    public string itemName; //User-friendly name. Access the internal name using the name field
    public string behaviourClassName; //Name of the behaviour class, if the item has any behaviour. This class must inherit from ItemBehaviour

    public ItemType type;

    public int maxStack;
    public int sellPrice;
    public Sprite sprite;
    public bool isEquippable;

    [Serializable]
    public struct ItemTag {
        public string key;
        public string value;
    }

    //Optional field allowing you to assign key/value pairs of tags to an item
    public ItemTag[] tags;

    /*
    Gets the associated behaviour for this template. This function will be called by the Item class whenever the item is used.
    When this function is called for the first time, it will create an instance of the ItemBehaviour child class provided by
    behaviourClassName, then store it for successive calls.

    If the item has no specified behaviour, null is returned
    */
    public ItemBehaviour GetItemBehaviour() {
        if(!String.IsNullOrEmpty(behaviourClassName)) {
            if(itemBehaviourMap.ContainsKey(behaviourClassName)) { //Return existing behaviour
                return itemBehaviourMap[behaviourClassName];
            } else {
                try {
                    //Find the associated class from the class name
                    Type behaviourClassType = Type.GetType(behaviourClassName);
                    ItemBehaviour behaviour = (ItemBehaviour)Activator.CreateInstance(behaviourClassType); //Instantiate and cast to parent class
                    itemBehaviourMap[behaviourClassName] = behaviour; //Assign behaviour

                    return behaviour;
                } catch(Exception e) {
                    //If these exceptions are thrown, then the provided behaviourClassName did not match any behaviour class or behaviourClassName became null at runtime
                    if (e is TypeLoadException || e is ArgumentNullException) {
                        Debug.Log("[ItemTemplate] Failed to load behaviour in item \"" + itemName+"\".");
                        Debug.LogException(e, this);
                        return null;
                    }
                }
            }
        }

        //No behaviour specified
        return null;
    }
    //Access the set value of a tag from a given key
    public string GetTagValue(string key) {
        foreach(ItemTag tag in tags) {
            if(tag.key == key) {
                return tag.value;
            }
        }

        return null;
    }

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
    Pumpkin,
    Carrot,
    Seeds_Tomato,
    Seeds_Strawberry,
    Seeds_Carrot,
    Seeds_Pumpkin,
    Sword,
    Katana,
    Fish,
    Trap1,
    Trap2,
    Trap3,
    Gold,
    Iron,
    Copper,
    SlimeResidue,
    GhostDust,
    BatWings, 
    FishingRod
}
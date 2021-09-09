using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class representing an item
//Author: Ishaiah Cross
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

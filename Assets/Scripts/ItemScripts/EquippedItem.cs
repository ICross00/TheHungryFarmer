using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquippedItem : MonoBehaviour
{
    private SpriteRenderer equippedSprite;
    private Item equippedItem;

    void Start()
    {
        equippedSprite = this.GetComponent<SpriteRenderer>();
    }

    public void updateEquippedItem(Item newItem)
    {
        if (newItem != null)
        {
            if ((newItem.itemTemplate.isEquippable))
            {
                equippedSprite.sprite = newItem.GetSprite();
                equippedItem = newItem;
            }
        }
        else
        {
            equippedSprite.sprite = null;
            equippedItem = null;
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldItem : MonoBehaviour
{
    private SpriteRenderer heldSprite;
    private Item heldItem;

    void Start()
    {
        heldSprite = GetComponent<SpriteRenderer>();
    }

    public void updateHeldItem(Item newItem)
    {
        if (newItem != null)
        {
            if (!(newItem.itemTemplate.isEquippable))
            {
                heldSprite.sprite = newItem.GetSprite();
                heldItem = newItem;
                return;
            }
        }

        heldSprite.sprite = null;
        heldItem = null;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class represents an item in the world that can be walked over by the player and picked up.
In the 'prefabs' folder is a prefab for a collectable storing an item, named pfCollectable.

You can use the provided static function Spawn() to spawn items in the world from any other class programmatically.
To do this, call it using Collectable.Spawn(position, item).

If you want to place items in the world from the inspector, please see the ItemSpawner class.
*/
public class Collectable : MonoBehaviour
{
    public Item item;

    //Spawns an item in the world at the provided position and with the given item type
    public static Collectable Spawn(Vector3 position, Item itemType) {
        GameObject tf = Instantiate(ItemAssets.Instance.pfWorldItem, position, Quaternion.identity);
        Collectable item = tf.GetComponent<Collectable>();
        SpriteRenderer renderer = tf.GetComponent<SpriteRenderer>();
        renderer.sprite = itemType.GetSprite();
        renderer.sortingOrder = 1;

        item.SetItem(itemType);

        return item;
    }

    public void SetItem(Item item) {
        this.item = item;
    }

    public Item GetItem() {
        return this.item;
    }

    public void DestroySelf() {
        Destroy(this.gameObject);
    }

}

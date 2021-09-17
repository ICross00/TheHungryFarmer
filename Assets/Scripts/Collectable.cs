using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*
This class represents an item in the world that can be walked over by the player and picked up.

You can use the provided static function Spawn() to spawn items in the world from any other class programmatically.

If you want to place items in the world from the editor, please see the ItemSpawner class.
*/
public class Collectable : MonoBehaviour
{
    public Item item;
    public float pickupCooldown = 0; //How long before this item can be picked up.
    public bool shouldCheckCooldown = false;

    /**
    Spawns an item in the world

    @param position The location to spawn the item at in the world
    @param item The item to spawn
    @param cooldown The time in seconds before this item can be picked up by the player
    */
    public static Collectable Spawn(Vector3 position, Item item, float cooldown = 0) {
        GameObject tf = Instantiate(ItemAssets.Instance.pfWorldItem, position, Quaternion.identity);
        Collectable collectableItem = tf.GetComponent<Collectable>();
        SpriteRenderer renderer = tf.GetComponent<SpriteRenderer>();

        collectableItem.pickupCooldown = cooldown;
        collectableItem.SetItem(item);


        //Disable collisions if a pickup cooldown was set
        if(cooldown > 0) {
            collectableItem.GetComponent<BoxCollider2D>().enabled = false;
            collectableItem.shouldCheckCooldown = true;
        }

        return collectableItem;
    }

    /**
    Spawns an item in the world

    @param position The location to spawn the item at in the world
    @param name The name of the item to create. This must match the name of the item as found in the Resources/Items folder
    @param amount The amount of item to include in the stack
    @param cooldown The time in seconds before this item can be picked up by the player
    */
    public static Collectable Spawn(Vector3 position, string name, int amount, float cooldown = 0) {
        Item spawnedItem = Item.CreateItem(name, amount);
        return Spawn(position, spawnedItem, cooldown);
    }

    public void SetItem(Item item) {
        this.item = item;

        //Update visuals
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        Text amountText = transform.Find("AmountText").GetComponent<Text>();

        renderer.sprite = item.GetSprite();
        renderer.sortingOrder = 1;

        amountText.text = item.amount > 1 ? item.amount.ToString() : "";
    }

    public Item GetItem() {
        return this.item;
    }

    public void DestroySelf() {
        Destroy(this.gameObject);
    }

    void Update() {
        //Decrement a cooldown timer that can be used to determine if this item can be picked up.
        //This is useful for allowing players to drop items without them immediately being picked up again.

        if(shouldCheckCooldown) {
            if(pickupCooldown > 0) {
                pickupCooldown -= Time.deltaTime;
            } else {
                shouldCheckCooldown = false; //Stop future checking if collisions need to be re-enabled
                GetComponent<BoxCollider2D>().enabled = pickupCooldown <= 0;
            }
        }
    }

}

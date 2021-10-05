using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
This class spawns a collectable at its location, then destroys itself when the scene is loaded.
This is useful if you want to add a collectable to a scene without writing a script to spawn it in

To use this class, drag a "pfItemSpawner" prefab from the Prefabs folder into the scene.
You then need to select which item this spawner will spawn through the inspector.

In the scene view, the item spawner will appear as a wrapped gift. This is just a placeholder. When the game starts,
the item spawner will appear as the item selected in the inspector.
*/

public class ItemSpawner : MonoBehaviour
{
    public ItemTemplate item; //The item that this spawner will spawn
    public int amount = 1; //The amount of the item that will be spawned

    void Start()
    {
        //Spawn the item
        //Set the z-coordinate to 0 so the item doesn't appear behind the background
        Collectable.Spawn(new Vector3(transform.position.x, transform.position.y, 0), new Item { itemTemplate = this.item, amount = this.amount });
        Destroy(gameObject);
    }
}

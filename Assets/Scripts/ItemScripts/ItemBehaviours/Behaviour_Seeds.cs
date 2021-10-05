using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Seeds : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        GameObject pfCrop = Resources.Load<GameObject>("Prefabs/pfPlantedCrop");
        Transform plantableGrid = GameObject.Find("Grid").transform;

        if(PlantedCrop.CanPlant(player.transform.position)) {
            PlantedCrop crop = GameObject.Instantiate(pfCrop, player.transform.position, Quaternion.identity, plantableGrid).GetComponent<PlantedCrop>();
            crop.cropTemplate = item.itemTemplate; //Set the crop type after spawning it

            //Consume the item
            inventory.RemoveItemSingle(item);
        }
    }
}

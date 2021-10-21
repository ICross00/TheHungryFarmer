using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Pickaxe : ItemBehaviour
{
    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        MonoBehaviour plyAsMono = (MonoBehaviour)player; //Start a coroutine on the player's monobehaviour
        plyAsMono.StartCoroutine(SetMiningAnimation(player));
    }

    public override void OnItemEquipped(Item item, Player player, Inventory inventory) {

    }

    public override void OnItemUnequipped(Item item, Player player, Inventory inventory) {

    }

    IEnumerator SetMiningAnimation(Player player) {
        while(Input.GetKey(KeyCode.E)) {
            Animator animator = player.GetComponent<Animator>();
            animator.SetBool("isMining", true);

            yield return new WaitForSeconds(0.1f); //Stop the animation if the key is released
            animator.SetBool("isMining", Input.GetKey(KeyCode.E));
        }
    }
}

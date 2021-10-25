using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Behaviour_Pickaxe : ItemBehaviour
{
    private const float PICKAXE_RANGE = 2f;

    public override void OnItemUsed(Item item, Player player, Inventory inventory) {
        MonoBehaviour plyAsMono = (MonoBehaviour)player; //Start a coroutine on the player's monobehaviour
        plyAsMono.StartCoroutine(SetMiningAnimation(player));
    }

    public override void OnItemEquipped(Item item, Player player, Inventory inventory) {

    }

    public override void OnItemUnequipped(Item item, Player player, Inventory inventory) {

    }

    /*
    Called by the Player class whenever the mining frame of the pickaxe is reached
    //Direction: 0 = left, 1 = up, 2 = right, 3 = down
    */
    public void OnMine(Player player, int direction) {
        //Calculate angle to ore
        float angle = direction * Mathf.PI / 2.0f;
        Vector3 offset = new Vector3(-Mathf.Cos(angle) * PICKAXE_RANGE, Mathf.Sin(angle) * PICKAXE_RANGE - 0.75f, 0);

        //Find ore in front of player
        MinableObject targetObj = MinableObject.GetObjectAtPosition(player.transform.position + offset);
        if(targetObj != null)
            targetObj.Mine();
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

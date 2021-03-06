using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour 
{
    private Animator anim;
    public Player player;
    //The move number was put here to allow the weapon class to check which direction the player is facing.

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("idleKeys", true);
        if (Input.GetKey(KeyCode.D))
        {
            anim.SetBool("isMoveRight", true);
            anim.SetBool("idleKeys", false);
        }
        else
        {
            anim.SetBool("isMoveRight", false);
        }

        if (Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoveLeft", true);
            anim.SetBool("idleKeys", false);
        }
        else
        {
            anim.SetBool("isMoveLeft", false);
        }

        if (Input.GetKey(KeyCode.S) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            anim.SetBool("isMoveDown", true);
            anim.SetBool("idleKeys", false);
        }
        else
        {
            anim.SetBool("isMoveDown", false);
        }
        
        if (Input.GetKey(KeyCode.W) && !(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D)))
        {
            anim.SetBool("isMoveUp", true);
            anim.SetBool("idleKeys", false);
        }
        else
        {
            anim.SetBool("isMoveUp", false);
        }

        if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.A))
        {
            anim.SetBool("isMoveRight", false);
            anim.SetBool("isMoveLeft", false);
        }

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            anim.SetBool("isMoveUp", false);
            anim.SetBool("isMoveDown", false);
        }
    }

    /*
    Sets the speed at which the player mines as a multiplier, where 1 = default
    @param speed The new mining speed
    */
    public void SetMiningSpeed(float speed) {
        anim.SetFloat("miningSpeed", speed);
    }

    /*
    Updates the state of the mining animation
    @param enabled Whether or not the mining animation should be enabled
    */
    public void TriggerMiningAnim(bool enabled) {
        anim.SetBool("isMining", enabled);
    }

    public void UpdateItemAnim()
    {
        Item selectedItem = player.GetSelectedItem();
        if (selectedItem != null)
        {
            if (!selectedItem.activeItem)
            {
                anim.SetBool("isHoldItem", true);
            }
            else
            {
                anim.SetBool("isEquipItem", false);
                anim.SetBool("isHoldItem", false);
            }
        }
        else
        {
            anim.SetBool("isEquipItem", false);
            anim.SetBool("isHoldItem", false);
        }
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;

    private void Awake()
    {
        //add skill button functions
        //transform.Find("Health1").gameObject.GetComponent<Button>.onClick = () =>
        {
            playerSkills.TryUnlockSkill(PlayerSkills.SkillType.healthMax_1);
        };



    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }

    
}

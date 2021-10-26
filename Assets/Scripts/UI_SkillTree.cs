using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SkillTree : MonoBehaviour
{
    private PlayerSkills playerSkills;
    public Player player;
    public Canvas skillCanvas;

    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();

        //add skill button functions
        transform.Find("Health_1").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.healthMax_1));
        transform.Find("Health_2").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.healthMax_2));
        transform.Find("Health_3").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.healthMax_3));
        transform.Find("Movement_1").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MoveSpeed_1));
        transform.Find("Movement_2").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MoveSpeed_2));
        transform.Find("Movement_3").GetComponentInChildren<Button>().onClick.AddListener(() => playerSkills.TryUnlockSkill(PlayerSkills.SkillType.MoveSpeed_3));


    }

    public void SetPlayerSkills(PlayerSkills playerSkills)
    {
        this.playerSkills = playerSkills;
    }

    public void ShowHideSkillTree()
    {
        if(skillCanvas)
        {
            player.isSkillOpen = !player.isSkillOpen;
            skillCanvas.enabled = player.isSkillOpen;
        }
    }



}

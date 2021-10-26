using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills {
    public class OnSkillUnlockedEventArgs : EventArgs
    {
        public SkillType skillType;
    }

    public enum SkillType //The player skills
    {
        None,
        healthMax_1,
        healthMax_2,
        healthMax_3,
        MoveSpeed_1,
        MoveSpeed_2,
        MoveSpeed_3,
    }

    private List<SkillType> unlockedSkillTypeList; //List of unlocked skills
    private Player player = GameObject.Find("Player").GetComponent<Player>();
    private XpManager xp;

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }


    private void UnlockSkill(SkillType skillType)
    {
        xp = XpManager.instance;
        if (!IsSkillUnlocked(skillType) && xp.currentSkillpoint > 0)
        {
            unlockedSkillTypeList.Add(skillType);
            player.PlayerSkills_OnSkillUnlocked(skillType.ToString());
        }
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    //skill requirements
    public SkillType GetSkillRequirement (SkillType skillType) //skill path to unlock.
    { 
        switch (skillType)
        {
            case SkillType.healthMax_3: return SkillType.healthMax_2;
            case SkillType.healthMax_2: return SkillType.healthMax_1;
            case SkillType.MoveSpeed_3: return SkillType.MoveSpeed_2;
            case SkillType.MoveSpeed_2: return SkillType.MoveSpeed_1;
        }
        return SkillType.None;
    }

    public bool TryUnlockSkill(SkillType skillType){
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if(skillRequirement != SkillType.None) {
            if (IsSkillUnlocked(skillRequirement)) {
                UnlockSkill(skillType);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            UnlockSkill(skillType);
            return true;
        }
    }
}

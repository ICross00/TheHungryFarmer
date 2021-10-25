using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills {

    public event EventHandler <OnSkillUnlockedEventArgs> OnSkillUnlocked; //event to handle when skill is unlocked
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

    public PlayerSkills()
    {
        unlockedSkillTypeList = new List<SkillType>();
    }


    private void UnlockSkill(SkillType skillType)
    {
        if(!IsSkillUnlocked(skillType))
        unlockedSkillTypeList.Add(skillType);
        OnSkillUnlocked?.Invoke(this, new OnSkillUnlockedEventArgs { skillType = skillType});
    }

    public bool IsSkillUnlocked(SkillType skillType)
    {
        return unlockedSkillTypeList.Contains(skillType);
    }

    public SkillType GetSkillRequirement (SkillType skillType) //This skill path where skills requirements are set
    {
        //add skill requirements
    }

    public bool TryUnlockSkill(SkillType skillType){
        SkillType skillRequirement = GetSkillRequirement(skillType);

        if(skillRequirement != SkillType.None) {
            if (IsSkillUnlocked(skillRequirement)) {
                UnlockSkill(skillType);
                return true;
            } else {
                return false;
            }
        } else {
            UnlockSkill(skillType);
            return true;
        }
    }
 
    
   
}





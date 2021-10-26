using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class XpManager : MonoBehaviour
{
    public TextMeshProUGUI currentXPtext, leveltext, currentSkillpointtext;
    public int currentXP, XPcap, level, currentSkillpoint;

    public static XpManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public void Start()
    {
        currentXPtext.text = currentXP.ToString() + XPcap.ToString();
        leveltext.text = level.ToString();
        currentSkillpointtext.text = currentSkillpoint.ToString();
    }

    //Adding xp togeather and level up
    public void addXP(int xp) //xpManageer.instance.addXP(amount);
    {
        currentXP += xp;

        //level up
        while (currentXP >= XPcap)
        {
            currentXP = currentXP - XPcap;
            level++;
            currentSkillpoint++;

            XPcap += XPcap / 30; //How much the level exp increments each time player levels up.
            leveltext.text = "Level: " + level.ToString();
            currentSkillpointtext.text = "SP: " + currentSkillpoint.ToString();
        }
        currentXPtext.text = currentXP.ToString() + " / " + XPcap.ToString();
    }



    


}

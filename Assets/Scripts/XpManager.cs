using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;



public class XpManager : MonoBehaviour
{

    public bool testing = false;
    public TextMeshProUGUI currentXPtext, leveltext, currentSkillpointtext;
    public int currentXP, XPcap, level, currentSkillpoint;

    public static XpManager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        currentXP = 0;
        XPcap = 100;
        level = 1;
        currentSkillpoint = 0;
    }

    public void Start()
    {
      if(testing == false)
        {
            currentXPtext.text = "Experience: " + currentXP.ToString() + " / " + XPcap.ToString();
            leveltext.text = "Level: " + level.ToString();
            currentSkillpointtext.text = "SP: " + currentSkillpoint.ToString();
        }
    }

    //Adding xp togeather and level up
    public void addXP(int xp) //xpManager.instance.addXP(amount);
    {
        currentXP += xp;

        //level up
        while (currentXP >= XPcap)
        {
            currentXP = currentXP - XPcap;
            level++;
            currentSkillpoint++;

            XPcap += XPcap / 30; //How much the level exp increments each time player levels up.
        }
    }

    private void Update()
    {
        if (testing == false)
        {
            leveltext.text = "Level: " + level.ToString();
            currentSkillpointtext.text = "SP: " + currentSkillpoint.ToString();
            currentXPtext.text = "Experience: " + currentXP.ToString() + " / " + XPcap.ToString();
        }
    }
}

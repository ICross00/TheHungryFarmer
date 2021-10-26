using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class XP_Test
{

    GameObject experienceObject;
    XpManager xp;
    private int level;
    private int skillPoint;
    private int experience;

    [SetUp]
    public void Setup()
    {
        experienceObject = new GameObject();

        xp = experienceObject.AddComponent<XpManager>();
        xp.testing = true;
        xp.XPcap = 100;
        xp.currentXP = 0;
        //xp.currentSkillpoint = 0;
    }

    [TearDown]
    public void TearDown()
    {

    }

    [UnityTest]
    public IEnumerator LevelUp()
    {
        xp.addXP(100);
        level = xp.level;
        Assert.AreEqual(2,level);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SkillPoints()
    {
        skillPoint = xp.currentSkillpoint;
        Assert.AreEqual(1, skillPoint);

        yield return null;
    }

    


}

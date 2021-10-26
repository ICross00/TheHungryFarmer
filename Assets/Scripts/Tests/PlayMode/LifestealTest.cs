using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class LifestealTest
{
    GameObject weaponObject;
    ThrowingWeapon weapon;

    private int lifestealAmount;
    private int playerHealthAmount;
    //Setting up the needed objects required to run the test.
    [SetUp]
    public void Setup()
    {
        weaponObject = new GameObject();
        weapon = weaponObject.AddComponent<ThrowingWeapon>();
        weapon.isTesting = true;
    }

    [TearDown]
    public void TearDown()
    {
        weapon.isTesting = true;
    }

    [UnityTest]
    public IEnumerator NoLifestealSword()
    {
        //Sending 'Sword2' to the script because 'Sword2' should not have life steal
        weapon.CoolDownTime("Sword2");
        //0 should be returned.
        lifestealAmount = weapon.lifeSteal;

        //Adding the lifesteal amount to the players health.

        Assert.AreEqual(0, lifestealAmount);

        yield return null;
    }

    [UnityTest]
    public IEnumerator SomeLifestealSword()
    {
        //Sending 'Sword2' to the script because 'Sword2' should not have life steal
        weapon.CoolDownTime("Sword5");
        //0 should be returned.
        lifestealAmount = weapon.lifeSteal;

        //Adding the lifesteal amount to the players health.

        Assert.AreEqual(2, lifestealAmount);

        yield return null;
    }

    [UnityTest]
    public IEnumerator MaxLifestealSword()
    {
        //Sending 'Sword2' to the script because 'Sword2' should not have life steal
        weapon.CoolDownTime("Sword7");
        //0 should be returned.
        lifestealAmount = weapon.lifeSteal;

        //Adding the lifesteal amount to the players health.

        Assert.AreEqual(5, lifestealAmount);

        yield return null;
    }
}

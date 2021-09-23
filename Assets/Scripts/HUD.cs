using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //This class contains the methods used in the HUD information

    public Image healthBar;

    float health;
    float maxHealth;
    float lerdSpeed;

    private void Start()
    {
        health = (float) GameManager.instance.player.maxHitPoint;
        maxHealth = health;
    }

    private void Update()
    {
        if (health > maxHealth) health = maxHealth;

        lerdSpeed = 3f * Time.deltaTime;

        HealthBarFiller();
    }

    void HealthBarFiller()
    {
        healthBar.fillAmount = Mathf.Lerp(healthBar.fillAmount, health / maxHealth, lerdSpeed);
    }
    


}

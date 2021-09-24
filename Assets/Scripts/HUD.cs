using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //This class contains the methods used in the HUD information

    public Text timeText ,daysText, hoursText;

    public Image healthBar;

    private TimeTick time;

    float health = GameManager.instance.player.maxHitPoint;
    float maxHealth;
    //float fillSpeed;


    private void Start()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        time = mainCamera.transform.GetChild(0).GetComponent<TimeTick>(); 

        //Health Bar
        maxHealth = health;
    }

    private void Update()
    {

        //Health Bar
       
        //fillSpeed = 3f * Time.deltaTime;
        HealthBarFiller();
        

        //Clock
        timeText.text = time.hours.ToString() + ":" + time.seconds.ToString();
        daysText.text = time.days.ToString();
    }

    public void HealthBarFiller() // function to control health bar fill
    {
        healthBar.fillAmount = health / maxHealth;
    }
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //This class contains the methods used in the HUD information

    public Text timeText, dayText, Day_Night_Text, numOfDays;

    public Image healthBar;

    private TimeManager time;

    float health;
    float maxHealth;
    //float fillSpeed;


    private void Start()
    {
        time = GetComponentInParent<TimeManager>();

        //Health Bar
        health = GameManager.instance.player.maxHitPoint;
        maxHealth = health;
    }

    private void Update()
    {
        //Health Bar
        health = GameManager.instance.player.hitPoint;

        //fillSpeed = 3f * Time.deltaTime;
        HealthBarFiller();


        //Clock
        timeText.text = time.hours.ToString() + ":" + string.Format("{0:00}", time.seconds);
        numOfDays.text = (time.days + 1).ToString();

        switch ((time.days % 7) + 1)
        {
            case 1:
                dayText.text = "MON";
                break;
            case 2:
                dayText.text = "TUE";
                break;
            case 3:
                dayText.text = "WED";
                break;
            case 4:
                dayText.text = "THU";
                break;
            case 5:
                dayText.text = "FRI";
                break;
            case 6:
                dayText.text = "SAT";
                break;
            case 7:
                dayText.text = "SUN";
                break;
        }

        if (time.currentTick <= 1140)
        {
            Day_Night_Text.text = "AM";
        }
        else
        {
            Day_Night_Text.text = "PM";
        }
    }

    public void HealthBarFiller() // function to control health bar fill
    {
        healthBar.fillAmount = health / maxHealth;
    }
    


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    //This class contains the methods used in the HUD information

    public Text timeText, dayText, Day_Night_Text;

    public Image healthBar;

    private TimeTick time;

    public enum day
    {
        



    }

    float health;
    float maxHealth;
    //float fillSpeed;


    private void Start()
    {
        GameObject mainCamera = GameObject.Find("Main Camera");
        time = mainCamera.transform.GetChild(0).GetComponent<TimeTick>();

        //Health Bar
        health = GameManager.instance.player.maxHitPoint;
        maxHealth = health;
    }

    private void Update()
    {

        //Health Bar

        //fillSpeed = 3f * Time.deltaTime;
        HealthBarFiller();


        //Clock
        timeText.text = time.hours.ToString() + ":" + time.seconds.ToString();
        dayText.text = time.days.ToString();

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

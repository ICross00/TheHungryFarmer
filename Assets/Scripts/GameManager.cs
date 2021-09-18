using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //References
    public Player player;
    public Inventory playerInventory;
    public int restaurantRating;
    public FloatingTextManager floatingTextManager;
    public int gold;
    private string previousScene;

    public static GameManager instance;
    Inventory playerInventory;

    //For now these values are zero or uninitialized. They will need to be loaded from a save state in the future
    int gameInt;
    int restaurantRating;
    int playerGold = 0;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }

        instance = this;
        playerInventory = GetComponent<Inventory>();
    }

    public int GetGold() {
        return playerGold;
    }

    public void ChangeGold(int amount) {
        playerGold += amount;

        Text playerGoldLabel = GameObject.Find("PlayerGold").GetComponent<Text>();
        playerGoldLabel.text = "Gold: " + playerGold.ToString();
    }

    //Referencing floating text for later use
    public FloatingTextManager floatingTextManager;

        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);
    }

    //Ability to display floating text after being called with parameters.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Save State

    //STRING previousScene
    //INT gold
    public void SaveState()
    {
        string s = "";

        //Attach saved values to the string here, separated by '|'
        s += (SceneManager.GetActiveScene().name + "|");
        s += (gold + "|");

        PlayerPrefs.SetString("SaveState", s);
        Debug.Log("SaveState");
    }

    public void LoadState(Scene s, LoadSceneMode mode)
    {
        if (!PlayerPrefs.HasKey("SaveState"))
            return;

        string[] data = PlayerPrefs.GetString("SaveState").Split('|');

        // Set previous scene name
        previousScene = data[0];

        // Set player gold
        gold = int.Parse(data[1]);

        player.transform.position = GameObject.Find("SpawnPoint").transform.position;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //References
    public Player player;
    //public Inventory playerInventory;
    public int restaurantRating;
    public FloatingTextManager floatingTextManager;  //Referencing floating text for later use
    public TimeManager timeManager;

    //Local Variables
    private string previousScene;
    private bool initialLoad;
    public int gold = 0;
    private bool resetLoad = false;

    public static GameManager instance;

    private void Awake()
    {
        if (GameManager.instance != null)
        {
            Destroy(gameObject);
            Destroy(player.gameObject);
            Destroy(floatingTextManager.gameObject);
            return;
        }
        initialLoad = true;

        SceneManager.sceneLoaded += LoadState;
        DontDestroyOnLoad(gameObject);

        instance = this;
        //Attach the inventory ui to the inventory component
        UI_Inventory playerInventoryUI = GameObject.Find("Player Inventory UI").GetComponent<UI_Inventory>();
        playerInventoryUI.SetInventory(GetComponent<Inventory>());

        timeManager = GetComponent<TimeManager>();

        GameObject.Find("Main Camera").transform.position = player.transform.position;
        ChangeGold(0);

        previousScene = "Bed"; //To prevent a null pointer exception when respawning the player if they haven't left home by middnight
    }

    public int GetGold() {
        return gold;
    }

    /**
    Updates the player's gold, will also update a PlayerGold text label if it exists in the scene
    */
    public void ChangeGold(int amount) {
        gold += amount;
        GameObject playerGoldLabel = GameObject.Find("PlayerGold");
        if(playerGoldLabel != null) {
            playerGoldLabel.GetComponent<Text>().text = "Gold: " + gold.ToString();
        }
    }

    //Ability to display floating text after being called with parameters.
    public void ShowText(string msg, int fontSize, Color color, Vector3 position, Vector3 motion, float duration)
    {
        floatingTextManager.Show(msg, fontSize, color, position, motion, duration);
    }

    //Save State

    //STRING previousScene
    //INT gold
    //STRING playerInventory
    public void SaveState()
    {
        string s = "";

        //Attach saved values to the string here, separated by '|'
        s += (SceneManager.GetActiveScene().name + "|");
        s += (gold + "|");

        ChangeGold(0);

        PlayerPrefs.SetString("SaveState", s);
        initialLoad = false;
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

        //Set Player Inventory
        List<Item> tempInventory = Inventory.FromString(data[2]);

        if (!initialLoad && !resetLoad)
        {
            SpawnPlayer();
        }
        else if (initialLoad || resetLoad)
        {
            SpawnPlayer("Bed");
        }
        resetLoad = false;
    }

    public void ResetPlayer()
    {
        resetLoad = true;
        SaveState();
        SceneManager.LoadScene("Home");
    }

    public void SpawnPlayer()
    {
        player.transform.position = GameObject.Find(previousScene + "SpawnPoint").transform.position;
        GameObject.Find("Main Camera").transform.position = player.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    public void SpawnPlayer(string spawnName)
    {
        player.transform.position = GameObject.Find(spawnName + "SpawnPoint").transform.position;
        GameObject.Find("Main Camera").transform.position = player.transform.position;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }
}

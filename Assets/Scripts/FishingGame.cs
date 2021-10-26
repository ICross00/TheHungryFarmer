using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGame : MonoBehaviour
{
    //References
    Popup popup;
    GameManager gameManager;

    [SerializeField] Transform Top; //Top position of the hook bar which make the hook and fish move up
    [SerializeField] Transform Bottom; //Bottom position of the hook bar which make the hook and fish move down
    [SerializeField] RectTransform fish; //Fish object
    [SerializeField] RectTransform hook; //hook object
  
    float fishPosition;
    float fishDestination;
    float fishTimer;
    float fishSpeed;    
    float hookProgress;
    float hookPullVelocity;
    float hookPosition;

    [SerializeField] private const float timerMultiplicator = 0.05f;
    [SerializeField] private const float smoothMotion = 1f;
    [SerializeField] private const float hookSize = 0.1f;
    [SerializeField] private const float hookPower = 0.5f;
    [SerializeField] private const float hookPullPower = 0.01f;
    [SerializeField] private const float hookGravityPower = 0.005f;
    [SerializeField] private const float hookProgressDegradationPower = 0.5f;

    [SerializeField] public SpriteRenderer hookSpriteRenderer; //Rendering the hook when trying to catch the fish.
    [SerializeField] public Transform ProgressBarContainer; //Progess bar holding.
    [SerializeField] public float failTimer; //Assign the fail time when the hook out of the fish.

    bool pause = false;

    private void Start() //Game starts if ..
    {
        popup = GameObject.Find("FishingCanvas Variant(Clone)").GetComponent<Popup>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        pause = false;
    }

    private void Update() 
    {
        if (pause) 
        {
            return;
        }

        Fish();
        Hook();
        Progress();
    }

    void Fish() //Fish object movement inside the hookbar.
    {
        fishTimer -= Time.unscaledDeltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion, Mathf.Infinity, Time.unscaledDeltaTime);
        fish.position = Vector3.Lerp(Bottom.position, Top.position, fishPosition);
    }

    void Hook() //Hook movement by player left-click
    {
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.unscaledDeltaTime;
        }

        hookPullVelocity -= hookGravityPower * Time.unscaledDeltaTime;

        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }

        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }
        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - (hookSize/2));
        hook.position = Vector3.Lerp(Bottom.position, Top.position, hookPosition);
    }

    private void Progress() //progress bar calculate the success of hook
    {
        Vector3 ls = ProgressBarContainer.localScale;
        ls.y = hookProgress;
        ProgressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.unscaledDeltaTime;
        }
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.unscaledDeltaTime;
            failTimer -= Time.unscaledDeltaTime;
            if (failTimer < 0f)
            {
                Lose();
            }

        }
        if (hookProgress >= 1f)
        {
            Win();
            Vector3 position = GameObject.Find("Player").transform.position;
            Collectable.Spawn(position, "Fish", 1);
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);

    }

    private void Win() //Print win methods
    {
        pause = true;
        popup.ExitPopup();
        string message = "You caught a fish!";
        gameManager.floatingTextManager.Show(message, 25, Color.blue, gameManager.player.transform.position, Vector3.zero, 1.0f);
    }

    private void Lose() //Print lose methods
    {
        pause = true;
        popup.ExitPopup();
        string message = "The fish got away!";
        gameManager.floatingTextManager.Show(message, 25, Color.blue, gameManager.player.transform.position, Vector3.zero, 1.0f);
    }
}
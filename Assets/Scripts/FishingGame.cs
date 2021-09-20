using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingGame : MonoBehaviour
{
    [SerializeField] Transform Top;
    [SerializeField] Transform Bottom;

    [SerializeField] Transform fish;

    float fishPosition;
    float fishDestination;

    float fishTimer;
    [SerializeField] private const float timerMultiplicator = 8f;

    float fishSpeed;
    [SerializeField] private const float smoothMotion = 2f;

    [SerializeField] Transform hook;
    float hookPosition;
    [SerializeField] private const float hookSize = 0.1f;
    [SerializeField] private const float hookPower = 0.5f;
    float hookProgress;
    float hookPullVelocity;
    [SerializeField] private const float hookPullPower = 0.01f;
    [SerializeField] private const float hookGravityPower = 0.005f;
    [SerializeField] private const float hookProgressDegradationPower = 0.1f;

    [SerializeField] public SpriteRenderer hookSpriteRenderer;

    [SerializeField] public Transform ProgressBarContainer;

    bool pause = false;
    [SerializeField] public float failTimer = 10f;

    private void Update()
    {
        if (pause) { return; }

        Fish();
        Hook();
        Progress();
    }

    void Fish()
    {
        fishTimer -= Time.deltaTime;
        if (fishTimer < 0f)
        {
            fishTimer = UnityEngine.Random.value * timerMultiplicator;

            fishDestination = UnityEngine.Random.value;
        }

        fishPosition = Mathf.SmoothDamp(fishPosition, fishDestination, ref fishSpeed, smoothMotion);
        fish.position = Vector3.Lerp(Bottom.position, Top.position, fishPosition);
    }

    void Hook()
    {
        if (Input.GetMouseButton(0))
        {
            hookPullVelocity += hookPullPower * Time.deltaTime;
        }

        hookPullVelocity -= hookGravityPower * Time.deltaTime;

        hookPosition += hookPullVelocity;

        if (hookPosition - hookSize / 2 <= 0f && hookPullVelocity < 0f)
        {
            hookPullVelocity = 0f;
        }

        if (hookPosition + hookSize / 2 >= 1f && hookPullVelocity > 0f)
        {
            hookPullVelocity = 0f;
        }
        hookPosition = Mathf.Clamp(hookPosition, hookSize / 2, 1 - hookSize / 2);
        hook.position = Vector3.Lerp(Bottom.position, Top.position, hookPosition);
    }

    private void Progress()
    {
        Vector3 ls = ProgressBarContainer.localScale;
        ls.y = hookProgress;
        ProgressBarContainer.localScale = ls;

        float min = hookPosition - hookSize / 2;
        float max = hookPosition + hookSize / 2;

        if (min < fishPosition && fishPosition < max)
        {
            hookProgress += hookPower * Time.deltaTime;
        }
        else
        {
            hookProgress -= hookProgressDegradationPower * Time.deltaTime;
            failTimer -= Time.deltaTime;
            if (failTimer < 0f)
            {
                Lose();
            }

        }
        if (hookProgress >= 1f)
        {
            Win();
        }

        hookProgress = Mathf.Clamp(hookProgress, 0f, 1f);

    }


    private void Win()
    {
        pause = true;
        Debug.Log("YOU WIN! CONGRATULATIONS, YOU CAUGHT THE FISH!");
    }

    private void Lose()
    {
        pause = true;
        Debug.Log("YOU LOST THE FISH! :( ");
    }
}
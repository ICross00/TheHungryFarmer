using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    private Transform target;
    Camera myCamera;
    public float speed = 0.1f;
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        boxCollider = GetComponent<BoxCollider2D>();
        target = GameObject.Find("Player").transform;
    }

    private void FixedUpdate()
    {
        //This will allow the game to run accross multiple devices and have the scale of the game work its self out.
        myCamera.orthographicSize = (Screen.height / 100f) / 0.8f;

        //This will allow the camera to move as the player moves.
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0, 0, -10);
        }
    }
}

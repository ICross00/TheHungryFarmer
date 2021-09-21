using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform target;
    Camera myCamera;
    public float speed;
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;

    private void Start()
    {
        myCamera = GetComponent<Camera>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        //This will allow the game to run accross multiple devices and have the scale of the game work its self out.
        myCamera.orthographicSize = (Screen.height / 100f) / 0.5f;

        //This will allow the camera to move as the player moves.
        if (target)
        {
            transform.position = Vector3.Lerp(transform.position, target.position, speed) + new Vector3(0, 0, -10);
        }
    }
}

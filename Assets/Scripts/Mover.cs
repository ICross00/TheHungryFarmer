using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{
    private Rigidbody2D rb2d;

    //Controls the Speed of the Object
    public float xSpeed = 1f;
    public float ySpeed = 1f;
    
    protected virtual void Start()
    {
        rb2d = gameObject.GetComponent<Rigidbody2D>();
    }

    //Must be Overridden in the child class FixedUpdate method
    protected virtual void UpdateMotor(Vector2 input)
    {
        if (input.x > 0 || input.x < 0)
        {
            rb2d.AddForce(new Vector2(input.x * xSpeed, 0), ForceMode2D.Impulse);
        }

        if (input.y > 0 || input.y < 0)
        {
            rb2d.AddForce(new Vector2(0, input.y * ySpeed), ForceMode2D.Impulse);
        }
    }
}

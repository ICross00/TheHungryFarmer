using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Mover_OLD : Fighter
{
    protected Vector3 moverLocation;
    protected BoxCollider2D boxCollider;
    protected RaycastHit2D hit;
    //Controls movement speed.
    public float ySpeed = 4f;
    public float xSpeed = 5f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    //Handle 2D trigger enter events in this function inside derived classes
    protected virtual void OnTriggerEnter2D(Collider2D collider) { }

    //Handle 2D trigger exit events in this function inside derived classes
    protected virtual void OnTriggerExit2D(Collider2D collider) { }

    protected virtual void UpdateMotor(Vector3 input)
    {
        //Reset movedealta.
        moverLocation = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        //Changing direction sprite is facing (left or right)
        if (moverLocation.x > 0)
            transform.localScale = Vector3.one;
        else if (moverLocation.x < 0)
            transform.localScale = new Vector3(-1, 1, 1);

        //Add push vector if there are any.
        moverLocation += pushDirection;

        //Reduce push force every frame to prevent what has been pushed from going on FOREVER AND EVER AND EVER AN.....
        pushDirection = Vector3.Lerp(pushDirection, Vector3.zero, pushRecoverySpeed);

        //Checking if a box is in the direction the payer is moving. If a box is there, the mover will not be allowed to move there.
        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moverLocation.y), Mathf.Abs(moverLocation.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Allowing the sprite to move
            transform.Translate(0, moverLocation.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moverLocation.x, 0), Mathf.Abs(moverLocation.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking"));
        if (hit.collider == null)
        {
            //Allowing the sprite to move
            transform.Translate(moverLocation.x * Time.deltaTime, 0, 0);
        }
    }
}

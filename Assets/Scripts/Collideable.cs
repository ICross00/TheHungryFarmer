using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collideable : Interactable
{
    public BoxCollider2D[] filter = new BoxCollider2D[10];

    protected virtual void OnCollide()
    {

    }
}

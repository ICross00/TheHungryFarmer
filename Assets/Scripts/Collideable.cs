using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collideable : Interactable
{
    public ContactFilter2D filter;
    private BoxCollider2D boxCollider;
    private Collider2D[] hits = new Collider2D[10];

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void Update()
    {
        //Work for collision
        boxCollider.OverlapCollider(filter, hits);
        for (int num = 0; num < hits.Length; num++)
        {
            if (hits[num] == null)
                continue;

            OnCollide(hits[num]);

            //Clean array after process
            hits[num] = null;
        }
    }

    protected virtual void OnCollide(Collider2D coll)
    {
        Debug.Log("OnCollide was not implimented in " + this.name);
    }
}

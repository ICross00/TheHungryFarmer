using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pike : MonoBehaviour
{
    public Rigidbody2D rb;
    public int knifeDamage;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
            collision.gameObject.GetComponent<Player>().hitPoint = collision.gameObject.GetComponent<Player>().hitPoint - knifeDamage;
            GameManager.instance.ShowText(knifeDamage.ToString(), 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
            break;
        }
    }

    public void PikesOut()
    {
        anim.SetBool("PikeIn", false);
        anim.SetBool("PikeOut", true);
    }

    public void PiklesIdle()
    {
        anim.SetBool("PikeOut", false);
        anim.SetBool("PikeIn", true);
    }
}

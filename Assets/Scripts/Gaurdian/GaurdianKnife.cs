using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaurdianKnife : MonoBehaviour
{
    public Rigidbody2D rb;
    Player player;
    public int knifeDamage;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Collidable":
            Destroy(gameObject);
            break;

            case "Player":
            collision.gameObject.GetComponent<Player>().hitPoint = collision.gameObject.GetComponent<Player>().hitPoint - knifeDamage;
            GameManager.instance.ShowText(knifeDamage.ToString(), 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
            break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
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

            case "Fighter":
                collision.gameObject.GetComponent<Enemy>().hitPoint = collision.gameObject.GetComponent<Enemy>().hitPoint - knifeDamage;
                GameManager.instance.ShowText(knifeDamage.ToString(), 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
                break;

            case "Crop":
                collision.SendMessage("HarvestCrop");
                break;

            case "Gaurdian":
                collision.gameObject.GetComponent<Gaurdian>().hitPoint = collision.gameObject.GetComponent<Gaurdian>().hitPoint - knifeDamage;
                GameManager.instance.ShowText(knifeDamage.ToString(), 25, Color.red, transform.position, Vector3.up * 50, 3.0f);
                break;
        }
    }
}

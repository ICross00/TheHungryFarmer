using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeScript : MonoBehaviour
{
    public int damage;
    public Rigidbody2D rb;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.gameObject.tag)
        {
            case "Collidable":
            Destroy(gameObject);
            break;

            case "Fighter":
                collision.gameObject.GetComponent<Enemy>().hitPoint = collision.gameObject.GetComponent<Enemy>().hitPoint - damage;
                Destroy(gameObject);
                break;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAnimation : MonoBehaviour
{
    private Animator anim;
    Vector2 direction;
    [SerializeField] Transform target;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("Player").transform;
        anim = GetComponent<Animator>();
    }
    
    // Update is called once per frame
    void Update()
    {
        FollowTarget();
    }

    void FollowTarget()
    {
        direction = (target.transform.position - transform.position).normalized;

        //Controls movement for up and down
        if (direction.y > 0 && (direction.x > -0.25 && direction.x < 0.25))
        {
            anim.SetInteger("Idle", 1);
            anim.SetTrigger("Up");
        }
        else if (direction.y < 0 && (direction.x > -0.25 && direction.x < 0.25))
        {
            anim.SetInteger("Idle", 3);
            anim.SetTrigger("Down");
        }
        //Controls left and right movement animations
        else if (direction.x > 0.25)
        {
            anim.SetInteger("Idle", 2);
            anim.SetTrigger("Right");
        }
        else if (direction.x < -0.25)
        {
            anim.SetInteger("Idle", 4);
            anim.SetTrigger("Left");
        }
        else
        {
            anim.SetInteger("Idle", 0);
        }
    }
}

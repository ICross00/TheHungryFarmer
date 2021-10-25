using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
<<<<<<< HEAD
    public float rotationSpeed = 15.0f;
=======
    public float rotationSpeed = 10.0f;
>>>>>>> parent of aed4538 (Revert "Merge branch 'main' into Liams_branch_take3")
    public Transform bladesTransform;

    void Update()
    {
        bladesTransform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
    }
}

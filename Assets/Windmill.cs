using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Windmill : MonoBehaviour
{
    public float rotationSpeed = 15.0f;
    public Transform bladesTransform;

    void Update()
    {
        bladesTransform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
    }
}

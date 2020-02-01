using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallingObjectController : MonoBehaviour
{
    public float FallingSpeed = 1.0f;
    private Rigidbody rb;
    private Transform trans;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.down * FallingSpeed;
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        trans.Rotate(0, 7, 0);
    }
}

using UnityEngine;

public class FallingObjectController : MonoBehaviour
{

    public float FallingSpeed = 1.0f;

    public const float YSpeed = 2.0f;
    public const float RotatingSpeed = 15.0f;
    private Rigidbody rb;
    private Transform trans;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = Vector3.down * YSpeed * FallingSpeed;
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        trans.Rotate(0, RotatingSpeed * Time.deltaTime * FallingSpeed, 0);
    }
}

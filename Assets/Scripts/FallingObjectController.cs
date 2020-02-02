using UnityEngine;

public class FallingObjectController : MonoBehaviour
{
    public float FallingSpeed = 1.0f;
    public float YSpeed = 2.0f;
    public float RotatingSpeed = 15.0f;

    private Rigidbody rb;
    private Transform trans;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        trans = GetComponent<Transform>();
    }

    private void Update()
    {
        if (GameEngine.GameState == GameState.Started)
        {
            rb.velocity = Vector3.down * YSpeed * FallingSpeed;
            trans.Rotate(0, RotatingSpeed * Time.deltaTime * FallingSpeed, 0);
        }
    }
}

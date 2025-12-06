using UnityEngine;


public class Obstacle : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    public float minSize = 1;
    public float maxSize = 2f;

    public float minSpeed = 1f;
    public float maxSpeed = 2f;

    public float maxSpinSpeed = 5f;

    private void Start()
    {
        // Random size
        var size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);

        _rb = GetComponent<Rigidbody2D>();

        // Random direction + impulse
        Vector2 direction = Random.insideUnitCircle.normalized;
        var speed = Random.Range(minSpeed, maxSpeed);

        _rb.AddForce(direction * speed, ForceMode2D.Impulse);

        // Spin
        _rb.AddTorque(Random.Range(-maxSpinSpeed, maxSpinSpeed), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        // Prevent stopping
        const float minVelocity = 3f;
        if (_rb.linearVelocity.magnitude < minVelocity)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * minVelocity;
        }
    }
}
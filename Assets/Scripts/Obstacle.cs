using UnityEngine;


public class Obstacle : MonoBehaviour
{
    private Rigidbody2D _rb;
    
    public float minSize = 1f;
    public float maxSize = 2f;

    public float minSpeed = 0.5f;
    public float maxSpeed = 1f;

    public float maxSpinSpeed = 0.7f;

    private void Start()
    {
        var size = Random.Range(minSize, maxSize);
        transform.localScale = new Vector3(size, size, 1);

        _rb = GetComponent<Rigidbody2D>();

        Vector2 direction = Random.insideUnitCircle.normalized;
        var speed = Random.Range(minSpeed, maxSpeed);

        _rb.AddForce(direction * speed, ForceMode2D.Impulse);

        _rb.AddTorque(Random.Range(-maxSpinSpeed, maxSpinSpeed), ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        const float minVelocity = 3f;
        if (_rb.linearVelocity.magnitude < minVelocity)
        {
            _rb.linearVelocity = _rb.linearVelocity.normalized * minVelocity;
        }
    }
}
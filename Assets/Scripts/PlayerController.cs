using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    private Camera _camera;
    private Rigidbody2D _rb;
    
    public float maxSpeed = 5f;
    public float thrustForce = 7f;
    public float rotationSpeed = 15f;
    
    private Vector3 _mouseWorldPos;
    private bool _isThrusting;
    
    public UIDocument uiDocument;
    
    private float _elapsedTime;
    
    private Label _scoreText;
    private Button _restartButton;
    public GameObject explosionEffect;

    private void Start()
    {
        _camera = Camera.main;
        _rb = GetComponent<Rigidbody2D>();
        
        // UI setup 
        var root = uiDocument.rootVisualElement;
        _scoreText = root.Q<Label>("ScoreLabel");
        _restartButton = root.Q<Button>("RestartButton");
        _restartButton.style.display = DisplayStyle.None;
        _restartButton.clicked += ReloadScene;
    }
    private void Update()
    {
        // Score update
        _elapsedTime += Time.deltaTime;
        _scoreText.text = ((int)((_elapsedTime + Time.deltaTime) * 10)).ToString();
        
        if (_restartButton.style.display == DisplayStyle.Flex &&
            Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            ReloadScene();
        }
        
        // handle input, get mouse world position
        _isThrusting = Mouse.current.leftButton.isPressed;

        if (!_isThrusting) return;
        
        Vector3 mousePos = Mouse.current.position.value;
        mousePos.z = 0f;
        _mouseWorldPos = _camera.ScreenToWorldPoint(mousePos);
    }
    
    private void FixedUpdate()
    {
        if (_isThrusting)
        {
            Vector2 diff = _mouseWorldPos - transform.position;
            
            if (diff.magnitude < 2f)
                return;

            var direction = diff.normalized;
            var targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
            var smoothed = Mathf.LerpAngle(_rb.rotation, targetAngle, rotationSpeed * Time.fixedDeltaTime);
            _rb.MoveRotation(smoothed);

            // Apply thrust
            _rb.AddForce(direction * thrustForce);
        }

        // speed clamp
        if (_rb.linearVelocity.magnitude > maxSpeed)
            _rb.linearVelocity = _rb.linearVelocity.normalized * maxSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Instantiate(explosionEffect, transform.position, Quaternion.identity);

        Destroy(gameObject);
        
        _restartButton.style.display = DisplayStyle.Flex;
        
        _scoreText.text = $"smashed: {_scoreText.text}";
    }

    private static void ReloadScene()
    {
        var current = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(current);
    }
}
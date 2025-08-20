using UnityEngine;
using UnityEngine.SceneManagement;

public class TopDownCharacterController : MonoBehaviour
{
    private const float RunningMultiplier = 1.4f;
    public float speed;

    private Animator _animator;
    private Vector2 _movementDirection;
    private Rigidbody2D _rigidbody;
    private SpriteRenderer _spriteRenderer;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        _rigidbody = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        var moveX = Input.GetAxisRaw("Horizontal");
        var moveY = Input.GetAxisRaw("Vertical");
        var isRunning = Input.GetKey(KeyCode.LeftShift);

        _movementDirection = new Vector2(moveX, moveY);
        _movementDirection.Normalize();

        if (IsMoving())
        {
            _animator.SetFloat("moveX", moveX);
            _animator.SetFloat("moveY", moveY);
            _spriteRenderer.flipX = moveX > 0f;
            _animator.speed = isRunning ? RunningMultiplier : 1f;
        }

        _animator.SetBool("IsMoving", IsMoving());

        _rigidbody.velocity = (isRunning ? RunningMultiplier : 1f) * speed * _movementDirection;

        if (Input.GetKeyDown(KeyCode.R)) RestartLevel();
    }

    private void RestartLevel()
    {
        var oldScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(oldScene.name, LoadSceneMode.Single);
    }

    private bool IsMoving()
    {
        return _movementDirection.magnitude > 0;
    }
}
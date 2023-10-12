#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerController : MonoBehaviour, IInventoryHolder
{
    #region SerializedFields

    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Transform jumpCheckTransform;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;

    [SerializeField, Range(0f, 1f)] private float jumpCheckRadius;

    [SerializeField] private LayerMask jumpLayerMask;

    [SerializeField, Range(1, 100)] private float fallingSpeed = 30f;

    #endregion


    #region Variables

    private const float maxFallSpeed = 30f;

    private Collider2D[] colliders = new Collider2D[3];

    private Vector2 _direction;

    private Rigidbody2D _playerRb;
    private bool _isGrounded;
    private bool _isWalking;

    #endregion

    #region Props

    public Inventory Inventory { get; private set; } = new();

    #endregion

    #region Unity Callbbacks
    // Start is called before the first frame update
    void Start()
    {
        PlayerActions inputActions = new PlayerActions();
        inputActions.Movement.Enable();
        inputActions.Movement.Walk.performed += Walk_performed;
        inputActions.Movement.Walk.canceled += Walk_canceled;

        inputActions.Movement.Jump.performed += Jump_performed;

        _playerRb = GetComponent<Rigidbody2D>();
        _playerRb.velocity = Vector2.zero;
        _direction = Vector2.zero;
    }


    private void FixedUpdate()
    {
        Move();

        CheckGrounded();

        FallFaster();

        if (_isGrounded && _isWalking == false)
        {
            _direction = Vector2.zero;
            _playerRb.velocity = new Vector2(0, _playerRb.velocity.y);
        }
    }

    private void FallFaster()
    {
        var velY = _playerRb.velocity.y;
        if (_isGrounded == false && velY < 0)
        {
            velY -= Time.deltaTime * fallingSpeed;

            velY = Mathf.Clamp(velY, -maxFallSpeed, 0);
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, velY);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var interactable = collision.gameObject.GetComponent<IInteractable>();
        interactable?.Interact(gameObject);
    }

    #endregion

    #region Methods

    private void Move()
    {
        _playerRb.AddForce(_direction * speed);

        var velX = Mathf.Clamp(_playerRb.velocity.x, -3, 3);
        _playerRb.velocity = new Vector2(velX, _playerRb.velocity.y);
    }

    private void CheckGrounded()
    {
        int count = Physics2D.OverlapCircleNonAlloc(jumpCheckTransform.position, jumpCheckRadius, colliders, jumpLayerMask);

        _isGrounded = count > 0;
    }


    #endregion


    #region InputActions

    private void Walk_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", false);
        var playerXVel = _playerRb.velocity.x;
        if (_isGrounded)
        {
            _direction = Vector2.zero;
            playerXVel = 0;
        }

        _playerRb.velocity = new Vector2(playerXVel, _playerRb.velocity.y);

        _isWalking = false;
    }

    private void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", true);
        _direction = obj.ReadValue<Vector2>();

        transform.localScale = new Vector2(_direction.x, transform.localScale.y);

        _isWalking = true;
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (_isGrounded)
        {
            _playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            _isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        var movingBlock = collision.collider.GetComponent<MovingBlock>();
        if (movingBlock)
        {
            transform.SetParent(movingBlock.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        var movingBlock = collision.collider.GetComponent<MovingBlock>();

        if (movingBlock && movingBlock.gameObject.activeInHierarchy)
        {
            transform.SetParent(null);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(jumpCheckTransform.position, jumpCheckRadius);
        Handles.Label(transform.position, $"walk:{_isWalking} ground:{_isGrounded}");
    }
#endif

    #endregion
}

#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class PlayerController : MonoBehaviour, IInventoryHolder
{
    #region SerializedFields
    [Header("Components")]
    [SerializeField] private Animator playerAnimator;

    [SerializeField] private Transform jumpCheckTransform;
    [SerializeField] private Transform boxOverlapTransform;

    [Header("Character Settings")]
    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;

    [SerializeField, Range(1, 100)] private float fallingSpeed;

    [SerializeField, Range(0f, 10f)] private float maxSpeed;

    [SerializeField, Range(1f, 100f)] private float MaxFallSpeed;

    [SerializeField, Range(0f, 1f)] private float jumpCheckRadius;

    [SerializeField] private LayerMask jumpLayerMask;


    [Header("Overlap Box")]

    [SerializeField] private Vector2 overlapBoxsize = Vector2.zero;

    [SerializeField] private Vector3 overlapBoxOffset;

    [SerializeField] private InventoryObject inventory;

    #endregion


    #region Variables

    private Collider2D[] colliders = new Collider2D[3];

    private Vector2 _direction;

    private Rigidbody2D _playerRb;
    private bool _isGrounded;
    private bool _isWalking;

    public InventoryObject Inventory { get { return inventory; } }

    #endregion

    #region Props


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

            velY = Mathf.Clamp(velY, -MaxFallSpeed, 0);
            _playerRb.velocity = new Vector2(_playerRb.velocity.x, velY);

        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var item = collision.gameObject.GetComponent<IInteractable>();
        item?.Interact(gameObject);
    }

    private void OnApplicationQuit()
    {
        Inventory.Container.Clear();
    }

    #endregion

    #region Methods

    private void Move()
    {
        _playerRb.velocity =
            new Vector2(Vector2.ClampMagnitude(_direction * speed, maxSpeed).x, _playerRb.velocity.y);

        // Check for collisions in the direction of movement
        var colliders = Physics2D.OverlapBoxAll(boxOverlapTransform.position + overlapBoxOffset, overlapBoxsize, 0, jumpLayerMask);

        if (colliders.Length > 0 && _isWalking == false && _playerRb.velocity.y < 0)
        {
            _playerRb.velocity = new Vector2(0f, _playerRb.velocity.y);

            _direction = Vector2.zero;
            playerAnimator.SetBool("Walk", false);
        }
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
        Gizmos.DrawWireSphere(jumpCheckTransform.position, jumpCheckRadius);
        Handles.Label(transform.position, $"walk:{_isWalking} ground:{_isGrounded}");

        Gizmos.DrawWireCube(boxOverlapTransform.position + overlapBoxOffset, overlapBoxsize);
    }
#endif

    #endregion
}

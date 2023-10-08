using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] Animator playerAnimator;

    [SerializeField] Transform jumpCheckTransform;

    [SerializeField] private float speed;

    [SerializeField] private float jumpForce;

    [SerializeField, Range(0f, 1f)] private float jumpCheckRadius;

    [SerializeField] LayerMask jumpLayerMask;

    #endregion

    #region Variables

    private Vector2 _direction;

    private Rigidbody2D _playerRb;

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
        if (Mathf.Abs(_playerRb.velocity.x) <= 3f)
            _playerRb.AddForce(_direction * speed);
    }

    #endregion


    #region InputActions

    private void Walk_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", false);
        _direction = Vector2.zero;
        _playerRb.velocity = Vector2.zero;

    }

    private void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", true);
        _direction = obj.ReadValue<Vector2>();

        transform.localScale = new Vector2(_direction.x, transform.localScale.y);
    }

    private void Jump_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        var colliders = Physics2D.OverlapCircleAll(jumpCheckTransform.position, jumpCheckRadius, jumpLayerMask);

        if (colliders != null && colliders.Length > 0)
        {
            _playerRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(jumpCheckTransform.position, jumpCheckRadius);
    }


    #endregion
}

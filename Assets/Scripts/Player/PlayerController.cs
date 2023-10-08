using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region SerializedFields
    [SerializeField] Animator playerAnimator;

    [SerializeField] private float speed;

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

        _playerRb = GetComponent<Rigidbody2D>();
    }


    private void FixedUpdate()
    {
        _playerRb.velocity = _direction * speed * Time.deltaTime;
    }

    #endregion


    #region InputActions

    private void Walk_canceled(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", false);
        _direction = Vector2.zero;

    }

    private void Walk_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        playerAnimator.SetBool("Walk", true);
        _direction = obj.ReadValue<Vector2>();

        transform.localScale = new Vector2(_direction.x,transform.localScale.y);
    }

    #endregion
}
